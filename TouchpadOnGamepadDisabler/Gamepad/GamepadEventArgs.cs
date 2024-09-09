namespace TouchpadOnGamepadDisabler.Gamepad
{
    public class GamepadEventArgs : EventArgs
    {
        public GamepadButtons ButtonsPressed { get; }

        public GamepadEventArgs(GamepadButtons buttonsPressed)
        {
            ButtonsPressed = buttonsPressed;
        }
    }
}
