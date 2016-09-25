using System.Windows.Forms;

namespace Athame.UI
{
    internal class SplitStringParser
    {
        private const char SplitChar = '|';
        private readonly string[] strComponents;
        private readonly Control control;

        public SplitStringParser(string str)
        {
            strComponents = str.Split(SplitChar);
        }

        public string Get(bool state)
        {
            return state ? strComponents[0] : strComponents[1];
        }

        public string Get(int index)
        {
            return strComponents[index];
        }

        public SplitStringParser(Control control)
        {
            this.control = control;
            strComponents = control.Text.Split(SplitChar);
        }

        public void Update(bool state)
        {
            control.Text = Get(state);
        }

        public void Update(int index)
        {
            control.Text = Get(index);
        }
    }
}
