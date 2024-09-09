namespace TouchpadOnGamepadDisabler.Gamepad
{
    [Flags]
    public enum GamepadButtons
    {
        None = 0,
        A = 1,
        B = 2,
        X = 4,
        Y = 8,
        DPadUp = 16,
        DPadDown = 32,
        DPadLeft = 64,
        DPadRight = 128,
        L1 = 256,  // Left Shoulder
        R1 = 512,  // Right Shoulder
        L2 = 1024, // Left Trigger
        R2 = 2048, // Right Trigger
        L3 = 4096, // Left Stick Press
        R3 = 8192  // Right Stick Press
    }
}
