using System.Timers;
using SharpDX.XInput;

namespace TouchpadOnGamepadDisabler.Gamepad
{

    public class GamepadController
    {
        private Controller controller;
        private System.Timers.Timer pollTimer;

        // Define events for button presses
        public event EventHandler<GamepadEventArgs>? ButtonsPressed;

        // New event for axis changes
        public event EventHandler<GamepadAxisEventArgs> AxisChanged;

        // Event for controller disconnect
        public event EventHandler? ControllerDisconnected;

        public GamepadController()
        {
            controller = new Controller(UserIndex.One);
            pollTimer = new System.Timers.Timer(100); // Poll every 100 ms
            pollTimer.Elapsed += PollTimer_Elapsed;
        }

        // Start polling the gamepad
        public void Start()
        {
            pollTimer.Start();
        }

        // Stop polling the gamepad
        public void Stop()
        {
            pollTimer.Stop();
        }

        // Poll the controller at regular intervals
        private void PollTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (controller.IsConnected)
            {
                // Get current state of the controller
                var state = controller.GetState();
                HandleButtonPresses(state.Gamepad);
                HandleAxisChanges(state.Gamepad);
            }
            else
            {
                // Raise the disconnect event if the controller is disconnected
                ControllerDisconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        private void HandleButtonPresses(SharpDX.XInput.Gamepad gamepad)
        {
            GamepadButtons pressedButtons = GamepadButtons.None;

            // Check each button and update the pressedButtons variable using bitwise OR
            if ((gamepad.Buttons & GamepadButtonFlags.A) != 0)
                pressedButtons |= GamepadButtons.A;

            if ((gamepad.Buttons & GamepadButtonFlags.B) != 0)
                pressedButtons |= GamepadButtons.B;

            if ((gamepad.Buttons & GamepadButtonFlags.X) != 0)
                pressedButtons |= GamepadButtons.X;

            if ((gamepad.Buttons & GamepadButtonFlags.Y) != 0)
                pressedButtons |= GamepadButtons.Y;

            // D-Pad directions
            if ((gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0)
                pressedButtons |= GamepadButtons.DPadUp;

            if ((gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0)
                pressedButtons |= GamepadButtons.DPadDown;

            if ((gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0)
                pressedButtons |= GamepadButtons.DPadLeft;

            if ((gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0)
                pressedButtons |= GamepadButtons.DPadRight;

            // Shoulder buttons
            if ((gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0)
                pressedButtons |= GamepadButtons.L1;

            if ((gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0)
                pressedButtons |= GamepadButtons.R1;

            // Stick press buttons
            if ((gamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0)
                pressedButtons |= GamepadButtons.L3;

            if ((gamepad.Buttons & GamepadButtonFlags.RightThumb) != 0)
                pressedButtons |= GamepadButtons.R3;

            // Analog triggers (L2, R2)
            if (gamepad.LeftTrigger > 50) // Trigger value range is 0-255, 50 is about 20% pressed
                pressedButtons |= GamepadButtons.L2;

            if (gamepad.RightTrigger > 50)
                pressedButtons |= GamepadButtons.R2;

            // If any buttons are pressed, raise the event
            if (pressedButtons != GamepadButtons.None)
            {
                ButtonsPressed?.Invoke(this, new GamepadEventArgs(pressedButtons));
            }
        }

        // Axis values to track changes
        private short prevLeftThumbX;
        private short prevLeftThumbY;
        private short prevRightThumbX;
        private short prevRightThumbY;
        private byte prevLeftTrigger;
        private byte prevRightTrigger;

        // Detect and raise event when an axis changes (left/right sticks or triggers)
        private void HandleAxisChanges(SharpDX.XInput.Gamepad gamepad)
        {
            float NormalizeAxis(short value)
            {
                return Math.Max(-1, Math.Min(1, value / 32767f));
            }

            // Normalize trigger values from [0, 255] to [0, 1]
            float NormalizeTrigger(byte value)
            {
                return value / 255f;
            }

            bool axisChanged = false;

            // Compare current axis values to previous ones
            if (gamepad.LeftThumbX != prevLeftThumbX || gamepad.LeftThumbY != prevLeftThumbY ||
                gamepad.RightThumbX != prevRightThumbX || gamepad.RightThumbY != prevRightThumbY ||
                gamepad.LeftTrigger != prevLeftTrigger || gamepad.RightTrigger != prevRightTrigger)
            {
                axisChanged = true;
            }

            // If any axis has changed, raise the AxisChanged event
            if (axisChanged)
            {
                AxisChanged?.Invoke(this, new GamepadAxisEventArgs(
                    NormalizeAxis(gamepad.LeftThumbX), NormalizeAxis(gamepad.LeftThumbY),
                    NormalizeAxis(gamepad.RightThumbX), NormalizeAxis(gamepad.RightThumbY),
                    NormalizeTrigger(gamepad.LeftTrigger), NormalizeTrigger(gamepad.RightTrigger)));

                // Update the previous axis values to the current ones
                prevLeftThumbX = gamepad.LeftThumbX;
                prevLeftThumbY = gamepad.LeftThumbY;
                prevRightThumbX = gamepad.RightThumbX;
                prevRightThumbY = gamepad.RightThumbY;
                prevLeftTrigger = gamepad.LeftTrigger;
                prevRightTrigger = gamepad.RightTrigger;
            }
        }

    }
}
