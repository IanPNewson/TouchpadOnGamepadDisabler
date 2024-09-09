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

            DateTime? lastUsed = null;

            gamepad = new GamepadController();
            gamepad.Start();
            gamepad.ButtonsPressed += (_, __) =>
            {
                lastUsed = DateTime.Now;
            };

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000 * 10/*s*/);

                    new DesktopNotifications.Windows.WindowsNotificationManager()
                        .ShowNotification(new DesktopNotifications.Notification()
                        {
                            Title = "Gamepad last used",
                            Body = lastUsed == null ? $"Never" : $"{lastUsed.Value:HHmmss}"
                        }, null);
                }
            })
                .Start();

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
