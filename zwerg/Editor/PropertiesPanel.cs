using Editor.Inputs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editor
{
    public class PropertiesPanel
    {
        public Control Control { get; private set; }
        private List<InputProperty> properties;

        public PropertiesPanel(Control c)
        {
            Control = c;

        }

        public void Update(SceneNode node)
        {
            Control.Controls.Clear();
            properties = new List<InputProperty>();

            int y = 10;

            if (node == null)
            {
                newLabel("labelNoNode", "No node selected.", y);
                return;
            }

            newLabel("labelNodeName", "Node name", y);
            newTextBox("textBoxNodeName", node.Name, y + 20, false);

            y += 50;
            
            foreach (InputProperty prop in node.Properties)
            {
                y += prop.CreateControls(Control, y);
                properties.Add(prop);
            }
        }

        private void newLabel(string name, string value, int pos)
        {
            Label label = new Label();
            label.Name = name;
            label.Text = value;
            label.AutoSize = true;
            label.Top = pos;
            Control.Controls.Add(label);
        }

        private void newTextBox(string name, string value, int pos, bool enabled)
        {
            TextBox txt = new TextBox();
            txt.Name = name;
            txt.Text = value;
            txt.Top = pos;
            txt.Left = 10;
            txt.Width = 170;
            txt.Enabled = enabled;
            Control.Controls.Add(txt);
        }

    }
}
