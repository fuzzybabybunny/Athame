using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Athame.UI
{
    public class RadioButtonEnumMapper
    {
        private readonly Dictionary<int, RadioButton> mappings = new Dictionary<int, RadioButton>();

        public int Value { get; private set; }
        public event EventHandler ValueChanged;

        protected void OnValueChanged(object sender, EventArgs args)
        {
            if (ValueChanged != null)
                ValueChanged(sender, args);
        }

        public void Assign(RadioButton radioButton, int value)
        {
            mappings[value] = radioButton;
            Value = value;
            radioButton.CheckedChanged += (sender, args) =>
            {
                Value = value;
                OnValueChanged(sender, args);
            };
        }

        public void Select(int value)
        {
            var rb = mappings[value];
            rb.Checked = true;
        }


    }
}
