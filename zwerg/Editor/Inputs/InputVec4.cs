using System;
using System.Collections.Generic;
using Editor;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Editor.Inputs
{
    public class InputVec4 : InputProperty
    {
        private NumericUpDown[] inputs;

        public InputVec4(string name, string value):base("vec4", name, value)
        {

        }

        public override InputProperty Clone()
        {
            return new InputVec4(Name, Value);
        }

        public override int CreateControls(Control panel, int posY)
        {
            panel.Controls.Add(CreateLabel(Name, Name, posY));
            posY += 20;            

            int posX = 10;
            decimal[] values = ValuesFromString(Value);

            inputs = new NumericUpDown[4];
            inputs[0] = CreateNumericTextBox("x" + Name, values[0], posX, posY, 35);
            inputs[1] = CreateNumericTextBox("y" + Name, values[1], posX + 45, posY, 35);
            inputs[2] = CreateNumericTextBox("z" + Name, values[2], posX + 90, posY, 35);
            inputs[3] = CreateNumericTextBox("w" + Name, values[3], posX + 135, posY, 35);

            inputs[0].ValueChanged += textChanged;
            inputs[1].ValueChanged += textChanged;
            inputs[2].ValueChanged += textChanged;
            inputs[3].ValueChanged += textChanged;

            panel.Controls.Add(inputs[0]);
            panel.Controls.Add(inputs[1]);
            panel.Controls.Add(inputs[2]);
            panel.Controls.Add(inputs[3]);

            return 50;
        }

        private void textChanged(Object sender, EventArgs e)
        {
            updateValueFromInputs();
        }

        private void updateValueFromInputs()
        {
            Value = string.Format("{0},{1},{2},{3}", 
                inputs[0].Text.TrimEnd('0'), 
                inputs[1].Text.TrimEnd('0'),
                inputs[2].Text.TrimEnd('0'),
                inputs[3].Text.TrimEnd('0'));
        }

    }
}
