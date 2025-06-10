using System;

namespace CloudGaminWheelController.HelperMethods;

public class Helper
{
    public static short NormalizeAxisToShort(int value, bool invert = false, double deadzone = 0.05)
    {
        double normalized = invert
            ? (1.0 - (value / 65535.0))
            : (value / 65535.0);

        // Apply deadzone
        if (normalized < deadzone)
            normalized = 0;

        return (short)(normalized * short.MaxValue * 2 - short.MaxValue);
    }
}
