using System;
using System.Windows.Forms;

namespace Editor.Inputs
{
    public class InputVec3 : InputProperty
    {
        private NumericUpDown[] inputs;

        public InputVec3(string name, string value):base("vec3", name, value)
        {

        }

        public override InputProperty Clone()
        {
            return new InputVec3(Name, Value);
        }

        public override int CreateControls(Control panel, int posY)
        {
            panel.Controls.Add(CreateLabel(Name, Name, posY));
            posY += 20;            

            int posX = 10;
            decimal[] values = ValuesFromString(Value);

            inputs = new NumericUpDown[3];
            inputs[0] = CreateNumericTextBox("x" + Name, values[0], posX, posY, 60);
            inputs[1] = CreateNumericTextBox("y" + Name, values[1], posX + 64, posY, 60);
            inputs[2] = CreateNumericTextBox("z" + Name, values[2], posX + 128, posY, 60);

            inputs[0].ValueChanged += textChanged;
            inputs[1].ValueChanged += textChanged;
            inputs[2].ValueChanged += textChanged;

            panel.Controls.Add(inputs[0]);
            panel.Controls.Add(inputs[1]);
            panel.Controls.Add(inputs[2]);
            return 50;
        }

        private void textChanged(Object sender, EventArgs e)
        {
            updateValueFromInputs();
        }

        private void updateValueFromInputs()
        {
            Value = string.Format("{0},{1},{2}",
                inputs[0].Text.TrimEnd('0'),
                inputs[1].Text.TrimEnd('0'),
                inputs[2].Text.TrimEnd('0'));
        }
    }
}
