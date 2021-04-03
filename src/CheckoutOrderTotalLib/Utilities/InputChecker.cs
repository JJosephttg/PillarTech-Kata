using System;

namespace CheckoutOrderTotalLib {
    internal static class InputChecker {
        public static void CheckBadInput(double num, string paramName) {
            if (num <= 0 || !double.IsFinite(num)) ThrowOutOfRange(paramName, $"Parameter '{paramName}' must be finite and be greater than 0");
        }

        public static void ThrowOutOfRange(string paramName, string message) => throw new ArgumentOutOfRangeException(paramName, message);
    }
}
