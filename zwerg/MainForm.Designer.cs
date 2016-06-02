namespace Zwerg
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.glControl1 = new OpenTK.GLControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.loadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.createCodeButton = new System.Windows.Forms.Button();
            this.propertiesPanelControl = new System.Windows.Forms.Panel();
            this.sceneTreeView = new System.Windows.Forms.TreeView();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.sceneOutput = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.sourceOutput = new System.Windows.Forms.TextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.errorOutput = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.usageLabel = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.showSunBox = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.specularFalloffBar = new System.Windows.Forms.TrackBar();
            this.specularIntensityBar = new System.Windows.Forms.TrackBar();
            this.diffuseIntensityBar = new System.Windows.Forms.TrackBar();
            this.specularColorButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.diffuseColorButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.showAxisBox = new System.Windows.Forms.CheckBox();
            this.showGridBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.moveSpeedBar = new System.Windows.Forms.TrackBar();
            this.cameraRotationModeBox = new System.Windows.Forms.ComboBox();
            this.mouseSpeedBar = new System.Windows.Forms.TrackBar();
            this.parametersLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.focusBar = new System.Windows.Forms.TrackBar();
            this.label10 = new System.Windows.Forms.Label();
            this.farDistanceBar = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.panel3.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.specularFalloffBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.specularIntensityBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffuseIntensityBar)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.moveSpeedBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouseSpeedBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.focusBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.farDistanceBar)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.glControl1.Location = new System.Drawing.Point(212, 13);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(673, 682);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = true;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl1_KeyDown);
            this.glControl1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.glControl1_KeyUp);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.loadButton);
            this.panel3.Controls.Add(this.saveButton);
            this.panel3.Controls.Add(this.createCodeButton);
            this.panel3.Controls.Add(this.propertiesPanelControl);
            this.panel3.Controls.Add(this.sceneTreeView);
            this.panel3.Location = new System.Drawing.Point(889, 7);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(219, 695);
            this.panel3.TabIndex = 4;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(3, 4);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 13);
            this.label11.TabIndex = 9;
            this.label11.Text = "Scene";
            // 
            // loadButton
            // 
            this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loadButton.Location = new System.Drawing.Point(109, 667);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(107, 23);
            this.loadButton.TabIndex = 8;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Location = new System.Drawing.Point(4, 667);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(99, 23);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // createCodeButton
            // 
            this.createCodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.createCodeButton.Location = new System.Drawing.Point(4, 606);
            this.createCodeButton.Name = "createCodeButton";
            this.createCodeButton.Size = new System.Drawing.Size(212, 55);
            this.createCodeButton.TabIndex = 6;
            this.createCodeButton.Text = "Apply changes [ENTER]";
            this.createCodeButton.UseVisualStyleBackColor = true;
            this.createCodeButton.Click += new System.EventHandler(this.createCodeButton_Click);
            // 
            // propertiesPanelControl
            // 
            this.propertiesPanelControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertiesPanelControl.Location = new System.Drawing.Point(6, 240);
            this.propertiesPanelControl.Name = "propertiesPanelControl";
            this.propertiesPanelControl.Size = new System.Drawing.Size(210, 360);
            this.propertiesPanelControl.TabIndex = 2;
            // 
            // sceneTreeView
            // 
            this.sceneTreeView.AllowDrop = true;
            this.sceneTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sceneTreeView.CheckBoxes = true;
            this.sceneTreeView.FullRowSelect = true;
            this.sceneTreeView.HideSelection = false;
            this.sceneTreeView.Location = new System.Drawing.Point(6, 20);
            this.sceneTreeView.Name = "sceneTreeView";
            this.sceneTreeView.Size = new System.Drawing.Size(210, 215);
            this.sceneTreeView.TabIndex = 1;
            this.sceneTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.sceneTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.sceneTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.sceneTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.sceneTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.sceneTreeView.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
            this.sceneTreeView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyUp);
            this.sceneTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Location = new System.Drawing.Point(12, 520);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(194, 176);
            this.tabControl2.TabIndex = 7;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.sceneOutput);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(186, 150);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Scene code";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // sceneOutput
            // 
            this.sceneOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sceneOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sceneOutput.Location = new System.Drawing.Point(0, 0);
            this.sceneOutput.Margin = new System.Windows.Forms.Padding(0);
            this.sceneOutput.Multiline = true;
            this.sceneOutput.Name = "sceneOutput";
            this.sceneOutput.ReadOnly = true;
            this.sceneOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.sceneOutput.Size = new System.Drawing.Size(186, 150);
            this.sceneOutput.TabIndex = 8;
            this.sceneOutput.WordWrap = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.sourceOutput);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(186, 150);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Complete";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // sourceOutput
            // 
            this.sourceOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sourceOutput.Location = new System.Drawing.Point(0, 0);
            this.sourceOutput.Margin = new System.Windows.Forms.Padding(0);
            this.sourceOutput.Multiline = true;
            this.sourceOutput.Name = "sourceOutput";
            this.sourceOutput.ReadOnly = true;
            this.sourceOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.sourceOutput.Size = new System.Drawing.Size(186, 133);
            this.sourceOutput.TabIndex = 7;
            this.sourceOutput.WordWrap = false;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.errorOutput);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(186, 150);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Errors";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // errorOutput
            // 
            this.errorOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.errorOutput.Location = new System.Drawing.Point(0, 0);
            this.errorOutput.Margin = new System.Windows.Forms.Padding(0);
            this.errorOutput.Multiline = true;
            this.errorOutput.Name = "errorOutput";
            this.errorOutput.ReadOnly = true;
            this.errorOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorOutput.Size = new System.Drawing.Size(186, 133);
            this.errorOutput.TabIndex = 8;
            this.errorOutput.WordWrap = false;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.usageLabel);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(186, 476);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Usage";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // usageLabel
            // 
            this.usageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usageLabel.Location = new System.Drawing.Point(15, 21);
            this.usageLabel.Name = "usageLabel";
            this.usageLabel.Size = new System.Drawing.Size(154, 439);
            this.usageLabel.TabIndex = 7;
            this.usageLabel.Text = "usage info here";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.showSunBox);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.specularFalloffBar);
            this.tabPage2.Controls.Add(this.specularIntensityBar);
            this.tabPage2.Controls.Add(this.diffuseIntensityBar);
            this.tabPage2.Controls.Add(this.specularColorButton);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.diffuseColorButton);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(186, 476);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Light";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // showSunBox
            // 
            this.showSunBox.AutoSize = true;
            this.showSunBox.Location = new System.Drawing.Point(15, 283);
            this.showSunBox.Name = "showSunBox";
            this.showSunBox.Size = new System.Drawing.Size(71, 17);
            this.showSunBox.TabIndex = 9;
            this.showSunBox.Text = "show sun";
            this.showSunBox.UseVisualStyleBackColor = true;
            this.showSunBox.CheckedChanged += new System.EventHandler(this.gui_ValueChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(174, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "intensity";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(174, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "intensity";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 222);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(174, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "falloff";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // specularFalloffBar
            // 
            this.specularFalloffBar.AutoSize = false;
            this.specularFalloffBar.BackColor = System.Drawing.SystemColors.Window;
            this.specularFalloffBar.Location = new System.Drawing.Point(6, 238);
            this.specularFalloffBar.Maximum = 100;
            this.specularFalloffBar.Minimum = 1;
            this.specularFalloffBar.Name = "specularFalloffBar";
            this.specularFalloffBar.Size = new System.Drawing.Size(174, 28);
            this.specularFalloffBar.TabIndex = 6;
            this.specularFalloffBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.specularFalloffBar.Value = 50;
            this.specularFalloffBar.ValueChanged += new System.EventHandler(this.gui_ValueChanged);
            // 
            // specularIntensityBar
            // 
            this.specularIntensityBar.AutoSize = false;
            this.specularIntensityBar.BackColor = System.Drawing.SystemColors.Window;
            this.specularIntensityBar.Location = new System.Drawing.Point(6, 191);
            this.specularIntensityBar.Maximum = 100;
            this.specularIntensityBar.Name = "specularIntensityBar";
            this.specularIntensityBar.Size = new System.Drawing.Size(174, 28);
            this.specularIntensityBar.TabIndex = 5;
            this.specularIntensityBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.specularIntensityBar.Value = 30;
            this.specularIntensityBar.ValueChanged += new System.EventHandler(this.gui_ValueChanged);
            // 
            // diffuseIntensityBar
            // 
            this.diffuseIntensityBar.AutoSize = false;
            this.diffuseIntensityBar.BackColor = System.Drawing.SystemColors.Window;
            this.diffuseIntensityBar.Location = new System.Drawing.Point(6, 77);
            this.diffuseIntensityBar.Maximum = 100;
            this.diffuseIntensityBar.Name = "diffuseIntensityBar";
            this.diffuseIntensityBar.Size = new System.Drawing.Size(174, 28);
            this.diffuseIntensityBar.TabIndex = 4;
            this.diffuseIntensityBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.diffuseIntensityBar.Value = 40;
            this.diffuseIntensityBar.ValueChanged += new System.EventHandler(this.gui_ValueChanged);
            // 
            // specularColorButton
            // 
            this.specularColorButton.BackColor = System.Drawing.Color.Gainsboro;
            this.specularColorButton.Location = new System.Drawing.Point(15, 140);
            this.specularColorButton.Name = "specularColorButton";
            this.specularColorButton.Size = new System.Drawing.Size(23, 23);
            this.specularColorButton.TabIndex = 3;
            this.specularColorButton.UseVisualStyleBackColor = false;
            this.specularColorButton.Click += new System.EventHandler(this.colorSelectorButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "specular";
            // 
            // diffuseColorButton
            // 
            this.diffuseColorButton.BackColor = System.Drawing.Color.White;
            this.diffuseColorButton.Location = new System.Drawing.Point(15, 27);
            this.diffuseColorButton.Name = "diffuseColorButton";
            this.diffuseColorButton.Size = new System.Drawing.Size(23, 23);
            this.diffuseColorButton.TabIndex = 1;
            this.diffuseColorButton.UseVisualStyleBackColor = false;
            this.diffuseColorButton.Click += new System.EventHandler(this.colorSelectorButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "diffuse";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.showAxisBox);
            this.tabPage1.Controls.Add(this.showGridBox);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.moveSpeedBar);
            this.tabPage1.Controls.Add(this.cameraRotationModeBox);
            this.tabPage1.Controls.Add(this.mouseSpeedBar);
            this.tabPage1.Controls.Add(this.parametersLabel);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.focusBar);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.farDistanceBar);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(186, 476);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "View";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // showAxisBox
            // 
            this.showAxisBox.AutoSize = true;
            this.showAxisBox.Checked = true;
            this.showAxisBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showAxisBox.Location = new System.Drawing.Point(9, 248);
            this.showAxisBox.Name = "showAxisBox";
            this.showAxisBox.Size = new System.Drawing.Size(72, 17);
            this.showAxisBox.TabIndex = 5;
            this.showAxisBox.Text = "show axis";
            this.showAxisBox.UseVisualStyleBackColor = true;
            this.showAxisBox.CheckedChanged += new System.EventHandler(this.gui_ValueChanged);
            // 
            // showGridBox
            // 
            this.showGridBox.AutoSize = true;
            this.showGridBox.Checked = true;
            this.showGridBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showGridBox.Location = new System.Drawing.Point(9, 271);
            this.showGridBox.Name = "showGridBox";
            this.showGridBox.Size = new System.Drawing.Size(94, 17);
            this.showGridBox.TabIndex = 4;
            this.showGridBox.Text = "show floor grid";
            this.showGridBox.UseVisualStyleBackColor = true;
            this.showGridBox.CheckedChanged += new System.EventHandler(this.gui_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "move speed";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "camera mode";
            // 
            // moveSpeedBar
            // 
            this.moveSpeedBar.AutoSize = false;
            this.moveSpeedBar.BackColor = System.Drawing.SystemColors.Window;
            this.moveSpeedBar.Location = new System.Drawing.Point(6, 125);
            this.moveSpeedBar.Maximum = 100;
            this.moveSpeedBar.Minimum = 10;
            this.moveSpeedBar.Name = "moveSpeedBar";
            this.moveSpeedBar.Size = new System.Drawing.Size(174, 27);
            this.moveSpeedBar.TabIndex = 2;
            this.moveSpeedBar.TickFrequency = 10;
            this.moveSpeedBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.moveSpeedBar.Value = 50;
            // 
            // cameraRotationModeBox
            // 
            this.cameraRotationModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cameraRotationModeBox.FormattingEnabled = true;
            this.cameraRotationModeBox.Items.AddRange(new object[] {
            "focus selection",
            "free fly/look (mouse+WASD)"});
            this.cameraRotationModeBox.Location = new System.Drawing.Point(9, 29);
            this.cameraRotationModeBox.Name = "cameraRotationModeBox";
            this.cameraRotationModeBox.Size = new System.Drawing.Size(171, 21);
            this.cameraRotationModeBox.TabIndex = 1;
            this.cameraRotationModeBox.SelectedIndexChanged += new System.EventHandler(this.cameraRotationModeBox_SelectedIndexChanged);
            // 
            // mouseSpeedBar
            // 
            this.mouseSpeedBar.AutoSize = false;
            this.mouseSpeedBar.BackColor = System.Drawing.SystemColors.Window;
            this.mouseSpeedBar.Location = new System.Drawing.Point(6, 81);
            this.mouseSpeedBar.Maximum = 100;
            this.mouseSpeedBar.Minimum = 10;
            this.mouseSpeedBar.Name = "mouseSpeedBar";
            this.mouseSpeedBar.Size = new System.Drawing.Size(174, 25);
            this.mouseSpeedBar.TabIndex = 1;
            this.mouseSpeedBar.TickFrequency = 10;
            this.mouseSpeedBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.mouseSpeedBar.Value = 50;
            // 
            // parametersLabel
            // 
            this.parametersLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parametersLabel.Location = new System.Drawing.Point(6, 310);
            this.parametersLabel.Name = "parametersLabel";
            this.parametersLabel.Size = new System.Drawing.Size(174, 163);
            this.parametersLabel.TabIndex = 0;
            this.parametersLabel.Text = "-";
            this.parametersLabel.Click += new System.EventHandler(this.parametersLabel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "mouse sensitivity";
            // 
            // focusBar
            // 
            this.focusBar.AutoSize = false;
            this.focusBar.BackColor = System.Drawing.SystemColors.Window;
            this.focusBar.Location = new System.Drawing.Point(6, 214);
            this.focusBar.Maximum = 100;
            this.focusBar.Minimum = 10;
            this.focusBar.Name = "focusBar";
            this.focusBar.Size = new System.Drawing.Size(174, 25);
            this.focusBar.TabIndex = 1;
            this.focusBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.focusBar.Value = 50;
            this.focusBar.ValueChanged += new System.EventHandler(this.gui_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 198);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(33, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "focus";
            // 
            // farDistanceBar
            // 
            this.farDistanceBar.AutoSize = false;
            this.farDistanceBar.BackColor = System.Drawing.SystemColors.Window;
            this.farDistanceBar.Location = new System.Drawing.Point(6, 170);
            this.farDistanceBar.Maximum = 100;
            this.farDistanceBar.Minimum = 2;
            this.farDistanceBar.Name = "farDistanceBar";
            this.farDistanceBar.Size = new System.Drawing.Size(174, 25);
            this.farDistanceBar.TabIndex = 1;
            this.farDistanceBar.TickFrequency = 10;
            this.farDistanceBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.farDistanceBar.Value = 25;
            this.farDistanceBar.ValueChanged += new System.EventHandler(this.gui_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 154);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "far distance";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(194, 502);
            this.tabControl1.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 708);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.glControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1020, 690);
            this.Name = "MainForm";
            this.Text = "Zwerg - Distance Field Editor by movAX13h ";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.specularFalloffBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.specularIntensityBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffuseIntensityBar)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.moveSpeedBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouseSpeedBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.focusBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.farDistanceBar)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TreeView sceneTreeView;
        private System.Windows.Forms.Panel propertiesPanelControl;
        private System.Windows.Forms.Button createCodeButton;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox sourceOutput;
        private System.Windows.Forms.TextBox sceneOutput;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TextBox errorOutput;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label usageLabel;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox showSunBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar specularFalloffBar;
        private System.Windows.Forms.TrackBar specularIntensityBar;
        private System.Windows.Forms.TrackBar diffuseIntensityBar;
        private System.Windows.Forms.Button specularColorButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button diffuseColorButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox showAxisBox;
        private System.Windows.Forms.CheckBox showGridBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TrackBar moveSpeedBar;
        private System.Windows.Forms.ComboBox cameraRotationModeBox;
        private System.Windows.Forms.TrackBar mouseSpeedBar;
        private System.Windows.Forms.Label parametersLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar focusBar;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TrackBar farDistanceBar;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabControl tabControl1;
    }
}

