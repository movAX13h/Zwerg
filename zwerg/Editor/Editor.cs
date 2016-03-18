using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Editor.Inputs;
using Editor.Nodes;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Editor
{
    public class Editor
    {
        public List<DistancePrimitive> DistanceFieldTypes { get; private set; }
        public List<DistanceOperation> DistanceOperations { get; private set; }
        public List<DomainOperation> DomainOperations { get; private set; }
        //public SceneNode Scene { get; private set; }
        public PropertiesPanel PropertiesPanel { get; private set; }
        private TreeView tree;

        private string shaderSourceCode;
        public string SceneSourceCode { get; private set; }

        private List<string> errors = new List<string>();
        public string Errors { get { return string.Join(Environment.NewLine, errors.ToArray()); } }

        private int domainId = 0;
        private int primitiveCounter = 0; // used as id for click selection

        public Editor(TreeView treeView, PropertiesPanel propertiesPanel, string source)
        {
            tree = treeView;
            shaderSourceCode = source;
            PropertiesPanel = propertiesPanel;

            DistanceFieldTypes = new List<DistancePrimitive>();
            DistanceOperations = new List<DistanceOperation>();
            DomainOperations = new List<DomainOperation>();

            Reset();
        }

        public bool Load(string nodesXMLFilename)
        {
            try
            {
                XElement x = XElement.Load(nodesXMLFilename, LoadOptions.None);

                XElement settings = x.Element("settings");
                foreach (XElement n in x.Elements("node"))
                {
                    switch (n.Attribute("type").Value)
                    {
                        case "do":
                            DistancePrimitive dp = new DistancePrimitive(n.Attribute("name").Value, n.Attribute("caption").Value, n.Attribute("code").Value);
                            foreach (XElement p in n.Elements("prop"))
                            {
                                dp.Properties.Add(createInputProperty(p.Attribute("type").Value, p.Attribute("name").Value, p.Attribute("default").Value));
                            }
                            DistanceFieldTypes.Add(dp);
                            break;

                        case "distop":
                            DistanceOperation distop = new DistanceOperation(n.Attribute("name").Value, n.Attribute("caption").Value, n.Attribute("code").Value);
                            foreach (XElement p in n.Elements("prop"))
                            {
                                distop.Properties.Add(createInputProperty(p.Attribute("type").Value, p.Attribute("name").Value, p.Attribute("default").Value));
                            }
                            DistanceOperations.Add(distop);
                            break;

                        case "domop":
                            DomainOperation dop = new DomainOperation(n.Attribute("name").Value, n.Attribute("caption").Value, n.Attribute("code").Value);
                            foreach (XElement p in n.Elements("prop"))
                            {
                                dop.Properties.Add(createInputProperty(p.Attribute("type").Value, p.Attribute("name").Value, p.Attribute("default").Value));
                            }
                            DomainOperations.Add(dop);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                errors.Add("Failed to load nodes.xml ("+e.Message+")");
                return false;
            }

            return true;
        }

        private InputProperty createInputProperty(string type, string name, string value)
        {
            switch(type)
            {
                case "float":
                    return new InputFloat(name, value);

                case "vec2":
                    return new InputVec2(name, value);

                case "vec3":
                    return new InputVec3(name, value);

                case "vec4":
                    return new InputVec4(name, value);
            }

            return null;
        }
        
        public void Reset()
        {
            tree.Nodes.Clear();
            primitiveCounter = 0;
        }

        private DistancePrimitive insertDistancePrimitive(DistancePrimitive type, TreeNode parent)
        {
            DistancePrimitive n = new DistancePrimitive(type);
            primitiveCounter++;
            n.Id = primitiveCounter;

            if (parent == null) tree.Nodes.Add(n.TreeNode);
            else parent.Nodes.Add(n.TreeNode);
            return n;
        }

        private DistanceOperation insertDistanceOperation(DistanceOperation type, TreeNode parent)
        {
            DistanceOperation n = new DistanceOperation(type);
            if (parent == null) tree.Nodes.Add(n.TreeNode);
            else parent.Nodes.Add(n.TreeNode);
            return n;
        }

        private DomainOperation insertDomainOperation(DomainOperation type, TreeNode parent)
        {
            DomainOperation n = new DomainOperation(type);
            if (parent == null) tree.Nodes.Add(n.TreeNode);
            else parent.Nodes.Add(n.TreeNode);
            return n;
        }

        public SceneNode InsertNodeByType(string type, TreeNode parent)
        {
            foreach (DistancePrimitive d in DistanceFieldTypes)
            {
                if (d.Type == type) return insertDistancePrimitive(d, parent);
            }

            foreach (DistanceOperation d in DistanceOperations)
            {
                if (d.Type == type) return insertDistanceOperation(d, parent);
            }

            foreach (DomainOperation d in DomainOperations)
            {
                if (d.Type == type) return insertDomainOperation(d, parent);
            }

            return null;
        }

        //------------
        /*
        private void addNodesRecursive(TreeNode parent, TreeNode parentTreeNode)
        {
            foreach (TreeNode node in parent.Nodes)
            {
                TreeNode tn = createTreeNode(node);
                if (parentTreeNode == null) sceneTreeView.Nodes.Add(tn);
                else parentTreeNode.Nodes.Add(tn);

                addNodesRecursive(node, tn);
            }
        }
        */
        /*
        private void updateSceneListView()
        {
            sceneTreeView.Nodes.Clear();
            addNodesRecursive(editor.Scene, null);
            sceneTreeView.ExpandAll();
        }
        */
        //--------------


        public void SceneFromXElement(XElement x)
        {
            foreach(XElement cx in x.Elements())
            {
                addNodeFromXElement(cx, null);
            }
        }

        private bool addNodeFromXElement(XElement x, TreeNode parent)
        {
            SceneNode n = InsertNodeByType(x.Attribute("type").Value, parent);
            if (n == null) return false;

            n.ValuesFromXElement(x);

            foreach(XElement cx in x.Elements())
            {
                if (!addNodeFromXElement(cx, n.TreeNode)) return false;
            }

            return true;
        }

        public XElement SceneToXElement()
        {
            XElement scene = new XElement("scene");
            foreach(TreeNode node in tree.Nodes)
            {
                scene.Add(((SceneNode)node.Tag).ToXElement());
            }
            return scene;
        }

        public string SceneToShaderCode()
        {
            errors.Clear();
            domainId = 0;
            SceneSourceCode = nodeToShaderCode(tree.Nodes, "", 0);
            return shaderSourceCode.Replace("#ifdef SCENE", "#ifdef SCENE" + Environment.NewLine + SceneSourceCode);
        }

        private string nodeToShaderCode(TreeNodeCollection nodes, string code, int domain)
        {
            foreach (TreeNode node in nodes)
            {
                SceneNode sceneNode = (SceneNode)node.Tag;
                if (!sceneNode.Enabled) continue;

                // get color property from node
                string color = "vec4(1.0)";
                InputProperty c = sceneNode.GetPropertyByName("color");
                if (c != null) color = "vec4(" + c.Value + ",1.0)";

                // primitive
                if (sceneNode is DistancePrimitive)
                {
                    code += "d1 = " + sceneNode.ToCode(domain) + "; ";
                    code += "if (d1 < d) { d = d1; col = " + color + "; id = " + ((DistancePrimitive)sceneNode).Id + "; }" + Environment.NewLine;
                    if (node.Nodes.Count > 0) code = nodeToShaderCode(node.Nodes, code, domain);
                }
                // distance operation
                else if (sceneNode is DistanceOperation)
                {
                    string[] values = new string[2];

                    int i = 0;
                    foreach (TreeNode child in node.Nodes)
                    {
                        SceneNode childSceneNode = (SceneNode)child.Tag;
                        if (!childSceneNode.Enabled) continue;

                        if (childSceneNode is DistancePrimitive)
                        {
                            if (i > 1)
                            {
                                errors.Add("Distance operation '" + sceneNode.Name + "' contains more than 2 primitives." + Environment.NewLine + "Only the first two are used.");
                                break;
                            }

                            if (i == 0) // take color from first object
                            {
                                c = childSceneNode.GetPropertyByName("color");
                                if (c != null) color = "vec4(" + c.Value + ",1.0)";
                            }
                            values[i] = childSceneNode.ToCode(domain);
                            i++;
                        }
                    }

                    if (i == 2)
                    {
                        code += "d1 = " + String.Format(sceneNode.ToCode(domain), values) + "; ";
                        code += "if (d1 < d) { d = d1; col = " + color + "; }" + Environment.NewLine;
                    }
                    else errors.Add("Distance operation '" + sceneNode.Name + "' requires 2 primitives.");
                }
                // domain operation
                else if (sceneNode is DomainOperation)
                {
                    int nd = newDomain();
                    code += sceneNode.ToCode(domain, nd) + "; " + Environment.NewLine;
                    if (node.Nodes.Count > 0) code = nodeToShaderCode(node.Nodes, code, nd);
                    else errors.Add("Domain operation '" + sceneNode.Name + "' does not contain any nodes.");
                }
                else errors.Add("Node type '" + sceneNode.Type + "' not implemented.");
            }

            return code;
        }

        private int newDomain()
        {
            domainId++;
            return domainId;
        }
    }
}
