using System;
using SharpDX.DirectInput;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using CloudGaminWheelController.HelperMethods;

namespace CloudGaminWheelController.WheelControllers;

public class LogitechG923
{
    public static void StartEmulation(Joystick joystick, IXbox360Controller controller)
    {
        Console.WriteLine("Reading input... Press Ctrl+C to exit.");

        while (true)
        {
            joystick.Poll();
            var state = joystick.GetCurrentState();

            // Map steering (X axis)
            short xValue = (short)(state.X - 32768); // center around 0
            controller.SetAxisValue(Xbox360Axis.LeftThumbX, xValue);

            // Throttle
            short rt = Helper.NormalizeAxisToShort(state.Y, invert: true);
            //controller.SetSliderValue(1, rt);
            controller.SetAxisValue(Xbox360Axis.RightThumbY, rt);

            // Brake
            byte lt = Helper.NormalizeAxisToByte(state.RotationZ, invert: true);
            //controller.SetSliderValue(0, lt);
            controller.SetSliderValue(Xbox360Slider.LeftTrigger, lt);

            //Console.WriteLine($"X: {state.X}, Y: {state.Y}, Z: {state.Z}, Rx: {state.RotationX}, Ry: {state.RotationY}, Rz: {state.RotationZ}");

            var buttons = state.Buttons;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i])
                {
                    Console.WriteLine($"Button {i} is pressed");
                }
            }

            // Assuming you've polled the device and have the state
            int pov = state.PointOfViewControllers[0];  // 0–35900 degrees, or -1 for neutral

            if (pov == -1)
            {
                Console.WriteLine("D-Pad not pressed");
            }
            else
            {
                if (pov >= 0 && pov < 4500)
                    Console.WriteLine("D-Pad Up");
                else if (pov >= 4500 && pov < 13500)
                    Console.WriteLine("D-Pad Right");
                else if (pov >= 13500 && pov < 22500)
                    Console.WriteLine("D-Pad Down");
                else if (pov >= 22500 && pov < 31500)
                    Console.WriteLine("D-Pad Left");
                else
                    Console.WriteLine("D-Pad Up (diagonal or wrap-around)");
            }

            controller.SetButtonState(Xbox360Button.B, buttons[2]); // B button
            controller.SetButtonState(Xbox360Button.A, buttons[0]); // A button
            controller.SetButtonState(Xbox360Button.X, buttons[1]); // X button
            controller.SetButtonState(Xbox360Button.Y, buttons[3]); // Y button
            controller.SetButtonState(Xbox360Button.RightShoulder, buttons[4]); // Shift Gear Up
            controller.SetButtonState(Xbox360Button.LeftShoulder, buttons[5]); // Shift Gear Down

            controller.SubmitReport();

            Thread.Sleep(16); // ~60 FPS
        }

    }
}
