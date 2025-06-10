using System;
using System.Threading;
using SharpDX.DirectInput;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using CloudGaminWheelController.WheelControllers;
using CloudGaminWheelController.HelperMethods;

class Program
{
    static void Main(string[] args)
    {
        var client = new ViGEmClient();
        var controller = client.CreateXbox360Controller();
        controller.Connect();

        var dinput = new DirectInput();
        var allDevices = new List<DeviceInstance>();

        allDevices.AddRange(dinput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly));
        allDevices.AddRange(dinput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AttachedOnly));
        allDevices.AddRange(dinput.GetDevices(DeviceType.Driving, DeviceEnumerationFlags.AttachedOnly));

        if (allDevices.Count == 0)
        {
            Console.WriteLine("No DirectInput devices found.");
            return;
        }

        // Print info for each device found
        Console.WriteLine("DirectInput devices found:");

        for (int i = 0; i < allDevices.Count; i++)
        {
            var device = allDevices[i];
            Console.WriteLine($"{i}: {device.InstanceName} - {device.ProductName} - GUID: {device.InstanceGuid}");
        }

        var joystick = new Joystick(dinput, allDevices[1].InstanceGuid);

        joystick.Properties.BufferSize = 128;
        joystick.Acquire();

        if (allDevices[1].ProductName.Contains("G923"))
        {
            LogitechG923.StartEmulation(joystick, controller);
        }
    }
}