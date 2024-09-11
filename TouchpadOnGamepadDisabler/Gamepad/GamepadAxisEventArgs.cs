namespace TouchpadOnGamepadDisabler.Gamepad
{
    public class GamepadAxisEventArgs : EventArgs
    {
        public float LeftThumbX { get; }
        public float LeftThumbY { get; }
        public float RightThumbX { get; }
        public float RightThumbY { get; }
        public float LeftTrigger { get; }
        public float RightTrigger { get; }

        public GamepadAxisEventArgs(float leftThumbX, float leftThumbY, float rightThumbX, float rightThumbY, float leftTrigger, float rightTrigger)
        {
            LeftThumbX = leftThumbX;
            LeftThumbY = leftThumbY;
            RightThumbX = rightThumbX;
            RightThumbY = rightThumbY;
            LeftTrigger = leftTrigger;
            RightTrigger = rightTrigger;
        }
    }
}
