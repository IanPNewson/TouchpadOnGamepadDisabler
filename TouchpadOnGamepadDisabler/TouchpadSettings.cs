using Microsoft.Win32;
using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;

namespace TouchpadOnGamepadDisabler
{
    public class TouchpadSettings
    {

        private static string KeyPath
        {
            get
            {
                const string userRoot = "HKEY_CURRENT_USER";
                const string subkey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PrecisionTouchPad\\Status";
                return userRoot + "\\" + subkey;
            }
        }

        private static string KeyName { get => "Enabled"; }

        private static int? KeyValue
        {
            get
            {
                return (int?)Registry.GetValue(KeyPath, KeyName, null);
            }
            set
            {
                Registry.SetValue(KeyPath, KeyName, value);
            }
        }

        public static bool Enabled
        {
            get => KeyValue == 1;
            private set
            {
                KeyValue = value ? 1 : 0;
            }
        }

        public static void Enable()
        {
            if (Enabled) return;
            Toggle();
        }

        public static void Disable()
        {
            if (!Enabled) return;
            Toggle();
        }

        public static void Toggle()
        {
            var keystrokeSimulator = new InputSimulator();
            keystrokeSimulator.Keyboard.ModifiedKeyStroke(new List<VirtualKeyCode>() { VirtualKeyCode.LWIN, VirtualKeyCode.CONTROL }, VirtualKeyCode.F24);

        }

    }

}