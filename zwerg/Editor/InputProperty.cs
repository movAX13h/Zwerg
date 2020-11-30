using System.Windows.Forms;

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

        public abstract InputProperty Clone();

        protected Label CreateLabel(string name, string value, int pos)
        {
            Label control = new Label();
            control.Name = "propertiesPanelLabel" + name;
            control.Text = value;
            control.AutoSize = true;
            control.Top = pos;
            return control;
        }

        protected NumericUpDown CreateNumericTextBox(string name, decimal value, int posX, int posY, int width, int decimalPlaces = 4)
        {
            //TextBox txt = new TextBox();
            NumericUpDown control = new NumericUpDown();
            control.Minimum = -9999;
            control.Maximum = 9999;
            control.Increment = decimalPlaces > 0 ? 0.1M : 1;
            control.DecimalPlaces = decimalPlaces;
            control.Name = "propertiesPanelNumericUpDown" + name;
            control.Value = value;
            control.Left = posX;
            control.Top = posY;
            control.Width = width;
            return control;
        }

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
