using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchpadOnGamepadDisabler.Gamepad;

namespace TouchpadOnGamepadDisabler
{
    public class HostForm : Form
    {

        private GamepadController gamepad;

        public HostForm()
        {
            this.Opacity = 0;
            this.Visible = false;
            this.ShowInTaskbar = false;

            DateTime? lastUsed = null;

            gamepad = new GamepadController();
            gamepad.Start();
            gamepad.ButtonsPressed += (_, __) =>
            {
                lastUsed = DateTime.Now;
            };
            gamepad.AxisChanged += (_, __) =>
            {
                lastUsed = DateTime.Now;
            };

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);

                    if (lastUsed == null) continue;
                    if (TouchpadSettings.Enabled)
                    {
                        if (lastUsed.Value > DateTime.Now.AddSeconds(-2))
                        {
                            TouchpadSettings.Disable();
                            Notify("Gamepad in use", "Touchpad has been disabled");
                        }
                    }
                    else
                    {
                        if (lastUsed.Value < DateTime.Now.AddSeconds(-10))
                        {
                            TouchpadSettings.Enable();
                            Notify("Gamepad not used", "Touchpad has been reenabled");

                        }
                    }
                }
            })
                .Start();

        }

        private static void Notify(string title, string body)
        {
            new DesktopNotifications.Windows.WindowsNotificationManager()
                .ShowNotification(new DesktopNotifications.Notification()
                {
                    Title = title,
                    Body = body
                }, null);
        }

        private static readonly int WS_EX_TOOLWINDOW = 0x00000080;

        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= WS_EX_TOOLWINDOW;
                return Params;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
