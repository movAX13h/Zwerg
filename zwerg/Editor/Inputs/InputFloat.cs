using System;
using System.Collections.Generic;
using Editor;
using System.Windows.Forms;

namespace Editor.Inputs
{
    public class InputFloat : InputProperty
    {
        private TextBox input;

        public InputFloat(string name, string value) : base("float", name, value)
        {

        }

        public override InputProperty Clone()
        {
            return new InputFloat(Name, Value);
        }

        public override int CreateControls(Control panel, int posY)
        {
            panel.Controls.Add(CreateLabel(Name, Name, posY));
            posY += 20;            

            input = CreateNumericTextBox(Name, Value, 10, posY, 170);
            input.TextChanged += textChanged;
            panel.Controls.Add(input);
            return 50;
        }

        private void textChanged(Object sender, EventArgs e)
        {
            updateValueFromInputs();
        }

        private void updateValueFromInputs()
        {
            Value = input.Text;
        }
    }
}
