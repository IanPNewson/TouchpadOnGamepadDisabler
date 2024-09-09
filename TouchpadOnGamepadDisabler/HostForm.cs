using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchpadOnGamepadDisabler
{
    public class HostForm : Form
    {
        public HostForm()
        {
            this.Opacity = 0;
            this.Visible = false;

            new DesktopNotifications.Windows.WindowsNotificationManager()
    .ShowNotification(new DesktopNotifications.Notification()
    {
        Title = "Sample"
    }, null);


        }

    }
}
