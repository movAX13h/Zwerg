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

        protected NumericUpDown CreateNumericTextBox(string name, decimal value, int posX, int posY, int width, int decimalPlaces = 4)
        {
            //TextBox txt = new TextBox();
            NumericUpDown txt = new NumericUpDown();
            txt.Minimum = -9999;
            txt.Maximum = 9999;
            txt.Increment = decimalPlaces > 0 ? 0.1M : 1;
            txt.DecimalPlaces = decimalPlaces;
            txt.Name = "propertiesPanelNumericUpDown" + name;
            txt.Value = value;
            txt.Left = posX;
            txt.Top = posY;
            txt.Width = width;
            //txt.GotFocus += txt_GotFocus;
            return txt;
        }

        /*
        void txt_GotFocus(object sender, EventArgs e)
        {
            //((NumericUpDown)sender).SelectAll();
        }*/

        protected decimal[] ValuesFromString(string input)
        {
            string[] values = input.Split(',');
            decimal[] numbers = new decimal[values.Length];

            for(int i = 0; i < values.Length; i++)
            {
                numbers[i] = decimal.Parse(values[i]);
            }
            return numbers;
        }

        public abstract int CreateControls(Control panel, int posY);
    }
}
