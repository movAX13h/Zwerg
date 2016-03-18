using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Core;
using Core.Assets;
using Utils;
using Editor;
using Editor.Nodes;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using OpenTK.Graphics;

namespace Zwerg
{
    public partial class MainForm : Form
    {
        #region list view fields
        private string NodeMap;
        private const int MAPSIZE = 128;
        private StringBuilder NewNodeMap = new StringBuilder(MAPSIZE);
        #endregion

        private Canvas canvas;
        private Shader shader;
        private List<Asset> assets;
        private Stopwatch stopwatch;

        private bool loaded = false;

        private bool mouseLeftDown = false;
        private bool mouseRightDown = false;
        private Point mousePos;
        private bool mouseMoved = false;
        private float mouseWheelDelta = 0;

        List<Keys> keys;

        private float time = 0;

        private Vector3 camPos;
        private Vector3 camTarget;

        private Vector3 lightPos;
        private Vector3 selectionPosition;

        private Editor.Editor editor;
        private PropertiesPanel propertiesPanel;

        private string nodesXmlPath = "nodes.xml";
        private string shaderPath = "raymarcher.fs";
        private string shaderSource = "";

        private ToolStripMenuItem deleteItem;
        private bool forceRender = false;

        public MainForm()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            if (File.Exists(shaderPath)) shaderSource = File.ReadAllText(shaderPath);
            else MessageBox.Show("Base shader not found.");

            Log.Filename = "log.txt";

            InitializeComponent();

            Text = "Zwerg " + Application.ProductVersion.Substring(0, 3) + " - Distance Field Editor by movAX13h";

            usageLabel.Text = "Use right mouse button context menu of the scene panel to add and remove nodes." + Environment.NewLine + Environment.NewLine;
            usageLabel.Text += "Click the 'Apply changes' button or press ENTER to update the preview at any time." + Environment.NewLine + Environment.NewLine;
            usageLabel.Text += "Use the wasd-keys or move your mouse while holding the left button to rotate the camera around the selected object." + Environment.NewLine + Environment.NewLine;
            usageLabel.Text += "In free fly mode use the wasd-keys to move the camera and left mouse button drag to look around." + Environment.NewLine + Environment.NewLine;
            usageLabel.Text += "A simple mouse click on a distance object in the preview selects the node in the scene view." + Environment.NewLine + Environment.NewLine;
            usageLabel.Text += "Mouse wheel increases/decreases the distance to the target." + Environment.NewLine + Environment.NewLine;
            usageLabel.Text += "Right mouse button controls light direction (rotates around y-axis).";

            propertiesPanel = new PropertiesPanel(propertiesPanelControl);
            editor = new Editor.Editor(sceneTreeView, propertiesPanel, shaderSource);
            if (!editor.Load(nodesXmlPath)) MessageBox.Show("Failed to load/parse nodes.xml\nNo nodes available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            cameraRotationModeBox.SelectedItem = cameraRotationModeBox.Items[0];

            // setup project tree view context menu 
            ToolStripMenuItem menuItem;
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.RenderMode = ToolStripRenderMode.System;
            contextMenu.ShowImageMargin = false;
            contextMenu.ShowCheckMargin = false;
            contextMenu.ItemClicked += contextMenu_ItemClicked;
            contextMenu.Opening += contextMenu_Opening;

            ToolStripMenuItem i = new ToolStripMenuItem();
            i.Text = "Primitives";
            i.DropDownItemClicked += contextMenu_ItemClicked;
            foreach (DistancePrimitive type in editor.DistanceFieldTypes)
            {
                menuItem = newToolStripMenuItem();
                menuItem.Text = type.Name;
                menuItem.Tag = type;
                i.DropDownItems.Add(menuItem);
            }
            contextMenu.Items.Add(i);

            i = new ToolStripMenuItem();
            i.Text = "Operations";
            i.DropDownItemClicked += contextMenu_ItemClicked;
            foreach (DistanceOperation type in editor.DistanceOperations)
            {
                menuItem = newToolStripMenuItem();
                menuItem.Text = type.Name;
                menuItem.Tag = type;
                i.DropDownItems.Add(menuItem);
            }
            contextMenu.Items.Add(i);

            i = new ToolStripMenuItem();
            i.Text = "Domain";
            i.DropDownItemClicked += contextMenu_ItemClicked;
            foreach (DomainOperation type in editor.DomainOperations)
            {
                menuItem = newToolStripMenuItem();
                menuItem.Text = type.Name;
                menuItem.Tag = type;
                i.DropDownItems.Add(menuItem);
            }
            contextMenu.Items.Add(i);

            deleteItem = newToolStripMenuItem();
            deleteItem.Text = "Delete [DEL]";
            deleteItem.Tag = "delete";
            contextMenu.Items.Add(deleteItem);

            sceneTreeView.ContextMenuStrip = contextMenu;
            resetView();

            keys = new List<Keys>();
            stopwatch = new Stopwatch();
            assets = new List<Asset>();
            canvas = new Canvas(glControl1.Width, glControl1.Height);
        }

        void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            deleteItem.Enabled = sceneTreeView.SelectedNode != null;
        }

        private ToolStripMenuItem newToolStripMenuItem()
        {
            ToolStripMenuItem i = new ToolStripMenuItem();
            return i;
        }

        private void resetView()
        {
            // init sensitive vars
            camPos = new Vector3(6.0f, 4.0f, -6.0f);
            camTarget = Vector3.Zero;
            lightPos = new Vector3(2.0f, 2.0f, 2.0f);
            selectionPosition = Vector3.Zero;
        }

        #region Project view nodes management

        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            if (item.Tag == null) return;

            TreeNode current = sceneTreeView.SelectedNode;

            if (item.Tag is string)
            {
                if (current == null)
                {
                    MessageBox.Show("Select a node first.", "Hint");
                    return;
                }

                switch((string)item.Tag)
                {
                    case "delete":
                        removeCurrentSelection();
                        break;

                    default:
                        break;
                }
                return;
            }

            SceneNode node = editor.InsertNodeByType(((SceneNode)item.Tag).Type, current);
            if (node != null) sceneTreeView.SelectedNode = node.TreeNode;
        }

        private void removeCurrentSelection()
        {
            if (sceneTreeView.SelectedNode == null) return;
            sceneTreeView.SelectedNode.Remove();
            loadShader();
        }

        #endregion

        private void loadShader()
        {
            loaded = false;

            string source = "";

            //try
            //{
                source = editor.SceneToShaderCode();
            //}
            //catch(Exception e)
            //{
                //MessageBox.Show("Code generation failed: " + e.Message);
              //  return;
            //}

            errorOutput.Text = editor.Errors;
            sourceOutput.Text = source;
            sceneOutput.Text = editor.SceneSourceCode;

            if (editor.Errors.Length > 0) MessageBox.Show(editor.Errors, "Scene", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            shader = new Shader("raymarcher");
            shader.LoadFromSource(source);

            if (!shader.Compile())
            {
                MessageBox.Show(shader.CompileLog.Replace("\n", "\r\n"));
                return;
            }

            //assets.Add(new TextureAsset("texture0", "tex03.jpg", TextureUnit.Texture0));

            if (!loadAssets()) return;

            canvas = new Canvas(glControl1.Width, glControl1.Height);
            canvas.Create(shader);

            mousePos = glControl1.PointToClient(Form.MousePosition);

            stopwatch.Reset();
            loaded = true;
            forceRender = true;
            start();
        }

        private void render()
        {
            if (!loaded) return;
            if (stopwatch.Elapsed.TotalSeconds > 1.0 && 
                keys.Count == 0 &&
                !forceRender && 
                !mouseLeftDown && 
                !mouseRightDown) return;

            //float dtime = (float)stopwatch.Elapsed.TotalSeconds - time;
            time = (float)stopwatch.Elapsed.TotalSeconds;

            float lookSpeed = 0.01f * (mouseSpeedBar.Value / 100.0f);
            float moveSpeed = 0.06f * (moveSpeedBar.Value / 100.0f);

            // mouse coords
            Point mp = glControl1.PointToClient(Form.MousePosition);
            Vector4 mouse = new Vector4(
                MathUtils.Clamp(0.0f, glControl1.Width, (float)mp.X),
                MathUtils.Clamp(0.0f, glControl1.Height, glControl1.Height - (float)mp.Y),
                mouseLeftDown ? 1.0f : 0.0f,
                mouseRightDown ? 1.0f : 0.0f);

            Vector2 mouseDelta = new Vector2(mp.X - mousePos.X, mp.Y - mousePos.Y);
            mousePos = mp;
            
            // camera control
            Vector3 camDir = Vector3.Normalize(camTarget - camPos);
            Vector3 camSide = Vector3.Normalize(Vector3.Cross(camDir, Vector3.UnitY));

            // selection - rotate around selection
            if (cameraRotationModeBox.SelectedIndex == 0) 
            {
                if (mouseLeftDown)
                {
                    // rotate around origin
                    camPos = Vector3.Transform(camPos, Matrix4.CreateFromAxisAngle(camSide, -mouseDelta.Y * lookSpeed));
                    camPos = Vector3.Transform(camPos, Matrix4.CreateFromAxisAngle(Vector3.UnitY, -mouseDelta.X * lookSpeed));
                }

                if (mouseWheelDelta != 0) camPos += mouseWheelDelta * camDir * moveSpeed;

                if (keys.Contains(Keys.W)) camPos += camDir * moveSpeed;
                if (keys.Contains(Keys.S)) camPos -= camDir * moveSpeed;
                if (keys.Contains(Keys.A)) camPos = Vector3.Transform(camPos, Matrix4.CreateFromAxisAngle(Vector3.UnitY, -lookSpeed));
                if (keys.Contains(Keys.D)) camPos = Vector3.Transform(camPos, Matrix4.CreateFromAxisAngle(Vector3.UnitY, lookSpeed));
            }
            else // free - camera mode
            {
                camDir = Vector3.Normalize(camTarget - camPos);
                camSide = Vector3.Normalize(Vector3.Cross(camDir, Vector3.UnitY));

                if (mouseWheelDelta != 0)
                {
                    Vector3 d = mouseWheelDelta * camDir * moveSpeed;
                    camPos += d;
                    camTarget += d;
                }

                if (keys.Contains(Keys.W))
                {
                    camPos += camDir * moveSpeed;
                    camTarget += camDir * moveSpeed;
                }

                if (keys.Contains(Keys.S))
                {
                    camPos -= camDir * moveSpeed;
                    camTarget -= camDir * moveSpeed;
                }

                if (keys.Contains(Keys.A))
                {
                    camPos -= camSide * moveSpeed;
                    camTarget -= camSide * moveSpeed;
                }

                if (keys.Contains(Keys.D))
                {
                    camPos += camSide * moveSpeed;
                    camTarget += camSide * moveSpeed;
                }

                if (mouseLeftDown)
                {
                    camTarget = Vector3.Transform(camTarget - camPos, Matrix4.CreateFromAxisAngle(camSide, -mouseDelta.Y * lookSpeed)) + camPos;
                    camTarget = Vector3.Transform(camTarget - camPos, Matrix4.CreateFromAxisAngle(Vector3.UnitY, -mouseDelta.X * lookSpeed)) + camPos;
                }
            }

            // move light
            if (mouseRightDown)
            {
                lightPos = Vector3.Transform(lightPos, Matrix4.CreateFromAxisAngle(Vector3.UnitY, mouseDelta.X * lookSpeed * 3.0f));
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Bind();
            shader.SetUniform("resolution", new Vector2((float)canvas.Width, (float)canvas.Height));
            //shader.SetUniform("mouse", mouse);
            shader.SetUniform("time", time);

            shader.SetUniform("showGrid", showGridBox.Checked ? (int)1 : (int)0);
            shader.SetUniform("showAxis", showAxisBox.Checked ? (int)1 : (int)0);
            shader.SetUniform("showSun", showSunBox.Checked ? (int)1 : (int)0);

            shader.SetUniform("camPos", camPos);
            shader.SetUniform("camTarget", camTarget);

            shader.SetUniform("focus", (float)focusBar.Value / 10.0f);
            shader.SetUniform("far", (float)farDistanceBar.Value);

            shader.SetUniform("lightPos", lightPos);
            shader.SetUniform("lightPosNormalized", Vector3.Normalize(lightPos));

            shader.SetUniform("diffuseIntensity", (float)diffuseIntensityBar.Value / 100.0f);
            shader.SetUniform("diffuseColor", diffuseColorButton.BackColor);
            shader.SetUniform("specularIntensity", (float)specularIntensityBar.Value / 100.0f);
            shader.SetUniform("specularColor", specularColorButton.BackColor);
            shader.SetUniform("specularFallOff", 7.0f * (float)specularFalloffBar.Value / 100.0f);

            shader.SetUniform("mouseDownPos", new Vector2((float)mp.X, glControl1.Height - (float)mp.Y));

            // assets uniforms
            List<float> floats = new List<float>();

            foreach (Asset asset in assets)
            {
                if (!asset.Ready()) continue;

                asset.Bind();
                shader.SetUniform(asset.Name, asset);
            }

            // redraw canvas and finish
            canvas.Draw();
            shader.UnBind();

            glControl1.SwapBuffers();

            parametersLabel.Text = "cam pos" + Environment.NewLine + camPos.ToString() + Environment.NewLine + Environment.NewLine +
                "cam target" + Environment.NewLine + camTarget.ToString() + Environment.NewLine + Environment.NewLine +
                "light pos" + Environment.NewLine + lightPos.ToString();

            mouseWheelDelta = 0;
            forceRender = false;
        }

        private void processClickSelection() // shader outputs 4 pixels in the bottom left corner that represent the df primitive ID
        {
            if (GraphicsContext.CurrentContext == null) MessageBox.Show("Failed to process click selection.");

            Bitmap bmp = new Bitmap(2, 2);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(new Rectangle(0, 0, 2, 2), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, 2, 2, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            selectTreeNodeById((int)Math.Round(bmp.GetPixel(0, 0).R / (255.0f * 0.04f)));
        }

        //TODO: move this into editor class
        private void selectTreeNodeById(int id)
        {
            foreach(TreeNode n in sceneTreeView.Nodes)
            {
                selectTreeNodeById(n, id);
            }
        }

        //TODO: move this into editor class
        private void selectTreeNodeById(TreeNode node, int id)
        {
            if (node.Tag is DistancePrimitive && ((DistancePrimitive)node.Tag).Id == id)
            {
                sceneTreeView.SelectedNode = node;
                return;
            }

            foreach(TreeNode n in node.Nodes)
            {
                selectTreeNodeById(n, id);
            }
        }

        private void selectionChanged()
        {
            updatePropertiesPanel();
            selectionPosition = getSelectedNodePosition();
            if (cameraRotationModeBox.SelectedIndex == 0) camTarget = selectionPosition;
            forceRender = true;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectionChanged();
        }

        private void updatePropertiesPanel()
        {
            if (sceneTreeView.SelectedNode == null) propertiesPanel.Update(null);
            else propertiesPanel.Update((SceneNode)sceneTreeView.SelectedNode.Tag);
        }

        //TODO: move this into editor class
        private Vector3 getSelectedNodePosition()
        {
            if (sceneTreeView.SelectedNode == null) return Vector3.Zero;
            return getSceneNodePositionFromTreeNode(sceneTreeView.SelectedNode);
        }

        //TODO: move this into editor class
        private Vector3 getSceneNodePositionFromTreeNode(TreeNode tnode) // SceneNode does not know its parent.
        {
            if (tnode == null) return Vector3.Zero;
            
            if (tnode.Tag is DistancePrimitive)
            {
                DistancePrimitive d = (DistancePrimitive)tnode.Tag;
                return d.Position();
            }
            
            return getSceneNodePositionFromTreeNode(tnode.Parent);
        }

        private bool loadAssets()
        {
            foreach (Asset asset in assets)
            {
                if (!asset.Load())
                {
                    MessageBox.Show("Failed to load asset: " + asset.Filename);
                    return false;
                }
            }

            return true;
        }

        private void start()
        {
            stopwatch.Start();
            Application.Idle += applicationIdle;
            glControl1.MouseWheel += glControl1_MouseWheel;
            render();
        }

        private void applicationIdle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                render();
            }
        }

        private void parametersLabel_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(parametersLabel.Text);
            MessageBox.Show("Values copied to clipboard.");
        }

        private void createCodeButton_Click(object sender, EventArgs e)
        {
            loadShader();
        }

        private void colorSelectorButton_Click(object sender, EventArgs e)
        {
            ColorDialog d = new ColorDialog();
            d.Color = ((Button)sender).BackColor;
            d.FullOpen = true;
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ((Button)sender).BackColor = d.Color;
                forceRender = true;
            }
        }

        private void cameraRotationModeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectionChanged();
        }
        
        #region GL stuff
        private void glClear()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            glControl1.SwapBuffers();
        }

        private void glSetupViewport()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            glSetupViewport();
            canvas.Resize(glControl1.Width, glControl1.Height);
            render();
            glControl1.SwapBuffers(); // required after resolution change
            forceRender = true;
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glClear();
            forceRender = true;
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            glSetupViewport();
            glClear();

            loadShader(); // show initial scene
        }

        private void glControl1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseWheelDelta = (float)e.Delta * 0.1f; // this is reset after use in render()
            forceRender = true;
        }

        private void glControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseLeftDown = true;
                mouseMoved = false;
                mousePos = glControl1.PointToClient(Form.MousePosition);
            }
            if (e.Button == MouseButtons.Right) mouseRightDown = true;
        }

        private void glControl1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseLeftDown = false;
                if (!mouseMoved) processClickSelection();
            }
            if (e.Button == MouseButtons.Right) mouseRightDown = false;
        }

        private void glControl1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (mouseLeftDown) mouseMoved = true;
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (Keys k in keys)
            {
                if (k.Equals(e.KeyCode)) return;
            }
            keys.Add(e.KeyCode);
        }

        private void glControl1_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (Keys k in keys)
            {
                if (k.Equals(e.KeyCode))
                {
                    keys.Remove(k);
                    return;
                }
            }
        }
        #endregion

        #region TreeView Drag/Drop

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false) && NodeMap != "")
            {
                TreeNode MovingNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                string[] NodeIndexes = NodeMap.Split('|');
                TreeNodeCollection InsertCollection = sceneTreeView.Nodes;
                for (int i = 0; i < NodeIndexes.Length - 1; i++)
                {
                    InsertCollection = InsertCollection[Int32.Parse(NodeIndexes[i])].Nodes;
                }

                if (InsertCollection != null)
                {
                    InsertCollection.Insert(Int32.Parse(NodeIndexes[NodeIndexes.Length - 1]), (TreeNode)MovingNode.Clone());
                    sceneTreeView.SelectedNode = InsertCollection[Int32.Parse(NodeIndexes[NodeIndexes.Length - 1])];

                    MovingNode.Remove();
                }
            }
        }

        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            TreeNode NodeOver = sceneTreeView.GetNodeAt(sceneTreeView.PointToClient(Cursor.Position));
            TreeNode NodeMoving = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

            // A bit long, but to summarize, process the following code only if the nodeover is null
            // and either the nodeover is not the same thing as nodemoving UNLESSS nodeover happens
            // to be the last node in the branch (so we can allow drag & drop below a parent branch)
            if (NodeOver != null && (NodeOver != NodeMoving || (NodeOver.Parent != null && NodeOver.Index == (NodeOver.Parent.Nodes.Count - 1))))
            {
                int OffsetY = sceneTreeView.PointToClient(Cursor.Position).Y - NodeOver.Bounds.Top;
                Graphics g = sceneTreeView.CreateGraphics();

                // Image index of 1 is the non-folder icon
                if (NodeOver.ImageIndex == 1)
                {
                    #region Standard Node
                    if (OffsetY < (NodeOver.Bounds.Height / 2))
                    {
                        #region If NodeOver is a child then cancel
                        TreeNode tnParadox = NodeOver;
                        while (tnParadox.Parent != null)
                        {
                            if (tnParadox.Parent == NodeMoving)
                            {
                                this.NodeMap = "";
                                return;
                            }

                            tnParadox = tnParadox.Parent;
                        }
                        #endregion
                        #region Store the placeholder info into a pipe delimited string
                        SetNewNodeMap(NodeOver, false);
                        if (SetMapsEqual() == true)
                            return;
                        #endregion
                        #region Clear placeholders above and below
                        this.Refresh();
                        #endregion
                        #region Draw the placeholders
                        this.DrawLeafTopPlaceholders(NodeOver);
                        #endregion
                    }
                    else
                    {
                        #region If NodeOver is a child then cancel
                        TreeNode tnParadox = NodeOver;
                        while (tnParadox.Parent != null)
                        {
                            if (tnParadox.Parent == NodeMoving)
                            {
                                this.NodeMap = "";
                                return;
                            }

                            tnParadox = tnParadox.Parent;
                        }
                        #endregion
                        #region Allow drag drop to parent branches
                        TreeNode ParentDragDrop = null;
                        // If the node the mouse is over is the last node of the branch we should allow
                        // the ability to drop the "nodemoving" node BELOW the parent node
                        if (NodeOver.Parent != null && NodeOver.Index == (NodeOver.Parent.Nodes.Count - 1))
                        {
                            int XPos = this.sceneTreeView.PointToClient(Cursor.Position).X;
                            if (XPos < NodeOver.Bounds.Left)
                            {
                                ParentDragDrop = NodeOver.Parent;

                                if (XPos < (ParentDragDrop.Bounds.Left - this.sceneTreeView.ImageList.Images[ParentDragDrop.ImageIndex].Size.Width))
                                {
                                    if (ParentDragDrop.Parent != null)
                                        ParentDragDrop = ParentDragDrop.Parent;
                                }
                            }
                        }
                        #endregion
                        #region Store the placeholder info into a pipe delimited string
                        // Since we are in a special case here, use the ParentDragDrop node as the current "nodeover"
                        SetNewNodeMap(ParentDragDrop != null ? ParentDragDrop : NodeOver, true);
                        if (SetMapsEqual() == true)
                            return;
                        #endregion
                        #region Clear placeholders above and below
                        this.Refresh();
                        #endregion
                        #region Draw the placeholders
                        DrawLeafBottomPlaceholders(NodeOver, ParentDragDrop);
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region Folder Node
                    if (OffsetY < (NodeOver.Bounds.Height / 3))
                    {
                        //this.lblDebug.Text = "folder top";

                        #region If NodeOver is a child then cancel
                        TreeNode tnParadox = NodeOver;
                        while (tnParadox.Parent != null)
                        {
                            if (tnParadox.Parent == NodeMoving)
                            {
                                this.NodeMap = "";
                                return;
                            }

                            tnParadox = tnParadox.Parent;
                        }
                        #endregion
                        #region Store the placeholder info into a pipe delimited string
                        SetNewNodeMap(NodeOver, false);
                        if (SetMapsEqual() == true)
                            return;
                        #endregion
                        #region Clear placeholders above and below
                        this.Refresh();
                        #endregion
                        #region Draw the placeholders
                        this.DrawFolderTopPlaceholders(NodeOver);
                        #endregion
                    }
                    else if ((NodeOver.Parent != null && NodeOver.Index == 0) && (OffsetY > (NodeOver.Bounds.Height - (NodeOver.Bounds.Height / 3))))
                    {
                        //this.lblDebug.Text = "folder bottom";

                        #region If NodeOver is a child then cancel
                        TreeNode tnParadox = NodeOver;
                        while (tnParadox.Parent != null)
                        {
                            if (tnParadox.Parent == NodeMoving)
                            {
                                this.NodeMap = "";
                                return;
                            }

                            tnParadox = tnParadox.Parent;
                        }
                        #endregion
                        #region Store the placeholder info into a pipe delimited string
                        SetNewNodeMap(NodeOver, true);
                        if (SetMapsEqual() == true)
                            return;
                        #endregion
                        #region Clear placeholders above and below
                        this.Refresh();
                        #endregion
                        #region Draw the placeholders
                        DrawFolderTopPlaceholders(NodeOver);
                        #endregion
                    }
                    else
                    {
                        //this.lblDebug.Text = "folder over";

                        if (NodeOver.Nodes.Count > 0)
                        {
                            NodeOver.Expand();
                            //this.Refresh();
                        }
                        else
                        {
                            #region Prevent the node from being dragged onto itself
                            if (NodeMoving == NodeOver)
                                return;
                            #endregion
                            #region If NodeOver is a child then cancel
                            TreeNode tnParadox = NodeOver;
                            while (tnParadox.Parent != null)
                            {
                                if (tnParadox.Parent == NodeMoving)
                                {
                                    this.NodeMap = "";
                                    return;
                                }

                                tnParadox = tnParadox.Parent;
                            }
                            #endregion
                            #region Store the placeholder info into a pipe delimited string
                            SetNewNodeMap(NodeOver, false);
                            NewNodeMap = NewNodeMap.Insert(NewNodeMap.Length, "|0");

                            if (SetMapsEqual() == true)
                                return;
                            #endregion
                            #region Clear placeholders above and below
                            this.Refresh();
                            #endregion
                            #region Draw the "add to folder" placeholder
                            DrawAddToFolderPlaceholder(NodeOver);
                            #endregion
                        }
                    }
                    #endregion
                }
            }
        }

        #region Helper Methods
        private void DrawLeafTopPlaceholders(TreeNode NodeOver)
        {
            Graphics g = sceneTreeView.CreateGraphics();

            int NodeOverImageWidth = 16; // this.treeView1.ImageList.Images[NodeOver.ImageIndex].Size.Width + 8;
            int LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
            int RightPos = sceneTreeView.Width - 4;

            Point[] LeftTriangle = new Point[5]{
												   new Point(LeftPos, NodeOver.Bounds.Top - 4),
												   new Point(LeftPos, NodeOver.Bounds.Top + 4),
												   new Point(LeftPos + 4, NodeOver.Bounds.Y),
												   new Point(LeftPos + 4, NodeOver.Bounds.Top - 1),
												   new Point(LeftPos, NodeOver.Bounds.Top - 5)};

            Point[] RightTriangle = new Point[5]{
													new Point(RightPos, NodeOver.Bounds.Top - 4),
													new Point(RightPos, NodeOver.Bounds.Top + 4),
													new Point(RightPos - 4, NodeOver.Bounds.Y),
													new Point(RightPos - 4, NodeOver.Bounds.Top - 1),
													new Point(RightPos, NodeOver.Bounds.Top - 5)};


            g.FillPolygon(System.Drawing.Brushes.Black, LeftTriangle);
            g.FillPolygon(System.Drawing.Brushes.Black, RightTriangle);
            g.DrawLine(new System.Drawing.Pen(Color.Black, 2), new Point(LeftPos, NodeOver.Bounds.Top), new Point(RightPos, NodeOver.Bounds.Top));

        }//eom

        private void DrawLeafBottomPlaceholders(TreeNode NodeOver, TreeNode ParentDragDrop)
        {
            Graphics g = sceneTreeView.CreateGraphics();

            int NodeOverImageWidth = 16; // treeView1.ImageList.Images[NodeOver.ImageIndex].Size.Width + 8;
            // Once again, we are not dragging to node over, draw the placeholder using the ParentDragDrop bounds
            int LeftPos, RightPos;
            if (ParentDragDrop != null) LeftPos = ParentDragDrop.Bounds.Left - (sceneTreeView.ImageList.Images[ParentDragDrop.ImageIndex].Size.Width + 8);
            else LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
            RightPos = sceneTreeView.Width - 4;

            Point[] LeftTriangle = new Point[5]{
												   new Point(LeftPos, NodeOver.Bounds.Bottom - 4),
												   new Point(LeftPos, NodeOver.Bounds.Bottom + 4),
												   new Point(LeftPos + 4, NodeOver.Bounds.Bottom),
												   new Point(LeftPos + 4, NodeOver.Bounds.Bottom - 1),
												   new Point(LeftPos, NodeOver.Bounds.Bottom - 5)};

            Point[] RightTriangle = new Point[5]{
													new Point(RightPos, NodeOver.Bounds.Bottom - 4),
													new Point(RightPos, NodeOver.Bounds.Bottom + 4),
													new Point(RightPos - 4, NodeOver.Bounds.Bottom),
													new Point(RightPos - 4, NodeOver.Bounds.Bottom - 1),
													new Point(RightPos, NodeOver.Bounds.Bottom - 5)};


            g.FillPolygon(System.Drawing.Brushes.Black, LeftTriangle);
            g.FillPolygon(System.Drawing.Brushes.Black, RightTriangle);
            g.DrawLine(new System.Drawing.Pen(Color.Black, 2), new Point(LeftPos, NodeOver.Bounds.Bottom), new Point(RightPos, NodeOver.Bounds.Bottom));
        }//eom

        private void DrawFolderTopPlaceholders(TreeNode NodeOver)
        {
            Graphics g = sceneTreeView.CreateGraphics();
            int NodeOverImageWidth = 16; // treeView1.ImageList.Images[NodeOver.ImageIndex].Size.Width + 8;

            int LeftPos, RightPos;
            LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
            RightPos = sceneTreeView.Width - 4;

            Point[] LeftTriangle = new Point[5]{
												   new Point(LeftPos, NodeOver.Bounds.Top - 4),
												   new Point(LeftPos, NodeOver.Bounds.Top + 4),
												   new Point(LeftPos + 4, NodeOver.Bounds.Y),
												   new Point(LeftPos + 4, NodeOver.Bounds.Top - 1),
												   new Point(LeftPos, NodeOver.Bounds.Top - 5)};

            Point[] RightTriangle = new Point[5]{
													new Point(RightPos, NodeOver.Bounds.Top - 4),
													new Point(RightPos, NodeOver.Bounds.Top + 4),
													new Point(RightPos - 4, NodeOver.Bounds.Y),
													new Point(RightPos - 4, NodeOver.Bounds.Top - 1),
													new Point(RightPos, NodeOver.Bounds.Top - 5)};


            g.FillPolygon(System.Drawing.Brushes.Black, LeftTriangle);
            g.FillPolygon(System.Drawing.Brushes.Black, RightTriangle);
            g.DrawLine(new System.Drawing.Pen(Color.Black, 2), new Point(LeftPos, NodeOver.Bounds.Top), new Point(RightPos, NodeOver.Bounds.Top));

        }//eom
        private void DrawAddToFolderPlaceholder(TreeNode NodeOver)
        {
            Graphics g = sceneTreeView.CreateGraphics();
            int RightPos = NodeOver.Bounds.Right + 6;
            Point[] RightTriangle = new Point[5]{
													new Point(RightPos, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) + 4),
													new Point(RightPos, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) + 4),
													new Point(RightPos - 4, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2)),
													new Point(RightPos - 4, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) - 1),
													new Point(RightPos, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) - 5)};

            Refresh();
            g.FillPolygon(System.Drawing.Brushes.Black, RightTriangle);
        }//eom

        private void SetNewNodeMap(TreeNode tnNode, bool boolBelowNode)
        {
            NewNodeMap.Length = 0;

            if (boolBelowNode)
                NewNodeMap.Insert(0, (int)tnNode.Index + 1);
            else
                NewNodeMap.Insert(0, (int)tnNode.Index);
            TreeNode tnCurNode = tnNode;

            while (tnCurNode.Parent != null)
            {
                tnCurNode = tnCurNode.Parent;

                if (NewNodeMap.Length == 0 && boolBelowNode == true)
                {
                    NewNodeMap.Insert(0, (tnCurNode.Index + 1) + "|");
                }
                else
                {
                    NewNodeMap.Insert(0, tnCurNode.Index + "|");
                }
            }
        }//oem

        private bool SetMapsEqual()
        {
            if (NewNodeMap.ToString() == NodeMap) return true;
            else
            {
                NodeMap = NewNodeMap.ToString();
                return false;
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            ((SceneNode)e.Node.Tag).Enabled = e.Node.Checked;
        }

        private void treeView1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (sceneTreeView.GetNodeAt(e.Location) == null)
            {
                sceneTreeView.SelectedNode = null;
                updatePropertiesPanel();
            }
        }
        #endregion

        #endregion

        //TODO: move this into editor class
        private XElement sceneXML()
        {
            return new XElement("dfe",
                new XAttribute("version", "1.0"),
                new XElement("settings",
                    new XElement("showGrid", showGridBox.Checked),
                    new XElement("showAxis", showAxisBox.Checked),
                    new XElement("showSun", showSunBox.Checked),

                    new XElement("camPos", vector3ToString(camPos)),
                    new XElement("camTarget", vector3ToString(camTarget)),

                    new XElement("lightPos", vector3ToString(lightPos)),

                    new XElement("diffuseIntensity", diffuseIntensityBar.Value),
                    new XElement("diffuseColor", diffuseColorButton.BackColor.ToArgb()),
                    new XElement("specularIntensity", specularIntensityBar.Value),
                    new XElement("specularColor", specularColorButton.BackColor.ToArgb()),
                    new XElement("specularFallOff", specularFalloffBar.Value),

                    new XElement("focus", focusBar.Value),
                    new XElement("far", farDistanceBar.Value)
                ),
                editor.SceneToXElement()
            );
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string filename = "scene.xml";

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = filename;
            dlg.Filter = "xml files (*.xml)|*.xml|all files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() != DialogResult.OK) return;
            sceneXML().Save(dlg.FileName);
        }

        //TODO: move this into editor class
        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "xml files (*.xml)|*.xml|all files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() != DialogResult.OK) return;

            editor.Reset();

            XElement x = XElement.Load(dlg.FileName, LoadOptions.None);

            XElement settings = x.Element("settings");
            showGridBox.Checked = bool.Parse(settings.Element("showGrid").Value);
            showAxisBox.Checked = bool.Parse(settings.Element("showAxis").Value);
            showSunBox.Checked = bool.Parse(settings.Element("showSun").Value);

            camPos = stringToVector3(settings.Element("camPos").Value);
            camTarget = stringToVector3(settings.Element("camTarget").Value);

            lightPos = stringToVector3(settings.Element("lightPos").Value);

            diffuseIntensityBar.Value = int.Parse(settings.Element("diffuseIntensity").Value);
            diffuseColorButton.BackColor = Color.FromArgb(int.Parse(settings.Element("diffuseColor").Value));
            specularIntensityBar.Value = int.Parse(settings.Element("specularIntensity").Value);
            specularColorButton.BackColor = Color.FromArgb(int.Parse(settings.Element("specularColor").Value));
            specularFalloffBar.Value = int.Parse(settings.Element("specularFallOff").Value);

            focusBar.Value = int.Parse(settings.Element("focus").Value);
            farDistanceBar.Value = int.Parse(settings.Element("far").Value);

            editor.SceneFromXElement(x.Element("scene"));

            loadShader();
        }

        private string vector3ToString(Vector3 v)
        {
            return v.X.ToString() + "," + v.Y.ToString() + "," + v.Z.ToString();
        }

        private Vector3 stringToVector3(string s)
        {
            string[] n = s.Split(',');
            if (n.Length == 3) return new Vector3(float.Parse(n[0]), float.Parse(n[1]), float.Parse(n[2]));
            else return Vector3.Zero;
        }

        private void Form1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                loadShader();
                e.Handled = true;
            }
        }

        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                removeCurrentSelection();
                e.Handled = true;
            }

        }

        private void gui_ValueChanged(object sender, EventArgs e)
        {
            forceRender = true;
        }

        private void xmlButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(sceneXML().ToString());
        }

    }
}
