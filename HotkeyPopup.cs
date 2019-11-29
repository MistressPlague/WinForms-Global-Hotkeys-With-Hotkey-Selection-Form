using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GlobalHotkeys
{
    public partial class HotkeyPopup : Form
    {
        KeyboardHook hook = new KeyboardHook();

        public HotkeyPopup()
        {
            InitializeComponent();

            // unregister all the registered hot keys.
            hook.UnregisterAllHotkeys();

            // register the event that is fired after the key press.
            hook.KeyPressed -= new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Length > 0 && comboBox2.Text.Length > 0)
            {
                ModifierKeys modifier = (ModifierKeys)Enum.Parse(typeof(ModifierKeys), comboBox1.Text, true);
                Keys key = (Keys)Enum.Parse(typeof(Keys), comboBox2.Text, true);

                // register the combination as hot key.
                hook.RegisterHotKey(modifier, key);

                Close();
            }
            else
            {
                Close();
            }
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            // send the hotkey to a method in your main form.
            var principalForm = Application.OpenForms.OfType<LaunchpadManager>().Single();
            principalForm.ReceivedHotkeys(e.Modifier, e.Key);
        }

        // add this to your main form, handle it as you wish.
        //public void ReceivedHotkeys(ModifierKeys modifier, Keys key)
        //{
        //    MessageBox.Show(modifier.ToString() + " + " + key.ToString());
        //}

        public void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = new Point(e.X, e.Y);
        }

        public void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (downPoint == Point.Empty)
            {
                return;
            }
            Point location = new Point(Left + e.X - downPoint.X, Top + e.Y - downPoint.Y);
            Location = location;
        }

        public void Form_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = Point.Empty;
        }

        public void Form_MouseUp(object sender, EventArgs e)
        {
            downPoint = Point.Empty;
        }

        public Point downPoint = Point.Empty;
    }
}
