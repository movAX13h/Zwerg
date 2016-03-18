using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Editor
{
    public abstract class InputProperty
    {
        public string Name;
        public string Type;
        public string Value;

        public InputProperty(string type, string name, string value)
        {
            Type = type;
            Name = name;
            Value = value;
        }

        /*
        public XElement ToXElement()
        {
            return new XElement("property", 
                new XAttribute("name", Name), 
                new XAttribute("type", Type), 
                new XAttribute("value", Value)
                );
        }*/

        public abstract InputProperty Clone();

        protected Label CreateLabel(string name, string value, int pos)
        {
            Label label = new Label();
            label.Name = "propertiesPanelLabel" + name;
            label.Text = value;
            label.AutoSize = true;
            label.Top = pos;
            return label;
        }

        protected TextBox CreateNumericTextBox(string name, string value, int posX, int posY, int width)
        {
            TextBox txt = new TextBox();
            txt.Name = "propertiesPanelTextBox" + name;
            txt.Text = value;
            txt.Left = posX;
            txt.Top = posY;
            txt.Width = width;
            txt.GotFocus += txt_GotFocus;
            return txt;
        }

        void txt_GotFocus(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        protected string[] ValuesFromString(string input)
        {
            return input.Split(',');
        }

        public abstract int CreateControls(Control panel, int posY);
    }
}
