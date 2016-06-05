using Editor.Inputs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

            y += 10 + newLabel("labelNodeName", "Node: " + node.Name, y);
            y += 10 + newLabel("labelNodeDescription", node.Comment, y, false);
            
            foreach (InputProperty prop in node.Properties)
            {
                y += prop.CreateControls(Control, y);
                properties.Add(prop);
            }
        }

        private int newLabel(string name, string value, int pos, bool autoSize = true)
        {
            Label label = new Label();
            label.Name = name;
            label.Top = pos;
            label.AutoSize = autoSize;
            label.Text = value;
            if (!autoSize)
            {
                Size maxSize = new Size(188, 100);
                
                int h = TextRenderer.MeasureText(label.Text, label.Font, maxSize, TextFormatFlags.WordBreak).Height;
                label.Size = new Size(188, h);
            }
            Control.Controls.Add(label);

            return label.Height;
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
