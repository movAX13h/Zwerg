using System;
using System.Collections.Generic;
using Editor;
using System.Windows.Forms;

namespace Editor.Inputs
{
    public class InputVec2 : InputProperty
    {
        private TextBox[] inputs;

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
            string[] values = ValuesFromString(Value);

            inputs = new TextBox[2];
            inputs[0] = CreateNumericTextBox("x" + Name, values[0], posX, posY, 80);
            inputs[1] = CreateNumericTextBox("y" + Name, values[1], posX + 90, posY, 80);

            inputs[0].TextChanged += textChanged;
            inputs[1].TextChanged += textChanged;

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
            Value = string.Format("{0},{1}", inputs[0].Text, inputs[1].Text);
        }

    }
}
