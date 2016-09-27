using System.Drawing;
using System.Windows.Forms;

namespace Athame.UI
{
    public partial class WaitForm : Form
    {
        
        public WaitForm()
        {
            InitializeComponent();
            Message = "Please wait...";
        }

        /*
         * Update: this did NOT work. Well, it kinda worked but it wasn't something
         * I ever want to write again. The point is, it worked in practice exactly the same as
         * Form.Show() and Form.Close(), but with a lot more icky thread-based crashing.
         * 
        /// <summary>
        /// Opens the form as a dialog on a separate thread. This should work in theory.
        /// </summary>
        /// <returns></returns>
        public static Thread Open(string message = "Please wait...")
        {
            var t = new Thread(o =>
            {
                using (var form = new WaitForm())
                {
                    try
                    {
                        form.ShowDialog();
                        form.Message = message;
                    }
                    catch (ThreadAbortException)
                    {
                        form.Close();
                    }
                }
            });
            t.Start();
            return t;
        }
         * */

        public WaitForm(string message)
        {
            InitializeComponent();
            Message = message;
        }

        public WaitForm(Form parentForm)
        {
            InitializeComponent();
            CenterInForm(parentForm);
        }

        public WaitForm(Form parentForm, string message)
        {
            InitializeComponent();
            CenterInForm(parentForm);
            Message = message;
        }

        private void CenterInForm(Form parentForm)
        {
            var tlCenterPoint = new Point
            {
                X = (parentForm.Left + (parentForm.Width / 2)) - (Width / 2),
                Y = (parentForm.Top + (parentForm.Height / 2)) - (Height / 2)
            };
            Location = tlCenterPoint;
        }

        public string Message { get { return messageLabel.Text; } set { messageLabel.Text = value; } }
    }
}
