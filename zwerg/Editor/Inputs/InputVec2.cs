using System;
using System.Collections.Generic;
using Editor;
using System.Windows.Forms;

namespace Editor.Inputs
{
    public class InputVec2 : InputProperty
    {
        private NumericUpDown[] inputs;

        public InputVec2(string name, string value):base("vec2", name, value)
        {

        }

        public override InputProperty Clone()
        {
            return new InputVec2(Name, Value);
        }

        public override int CreateControls(Control panel, int posY)
        {
            panel.Controls.Add(CreateLabel(Name, Name, posY));
            posY += 20;            

            int posX = 10;
            decimal[] values = ValuesFromString(Value);

            inputs = new NumericUpDown[2];
            inputs[0] = CreateNumericTextBox("x" + Name, values[0], posX, posY, 90);
            inputs[1] = CreateNumericTextBox("y" + Name, values[1], posX + 98, posY, 90);

            inputs[0].ValueChanged += textChanged;
            inputs[1].ValueChanged += textChanged;

            panel.Controls.Add(inputs[0]);
            panel.Controls.Add(inputs[1]);

            return 50;
        }

        private void textChanged(Object sender, EventArgs e)
        {
            updateValueFromInputs();
        }

        private void updateValueFromInputs()
        {
            Value = string.Format("{0},{1}", inputs[0].Text.TrimEnd('0'), inputs[1].Text.TrimEnd('0'));
        }

    }
}
