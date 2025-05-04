using Diagrammatist.Presentation.WPF.Core.Shared.Enums;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// A class that represents a helper for converting units.
    /// </summary>
    public static class ChangeConvertUnit
    {
        /// <summary>
        /// Converts a value from one measurement unit to another.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <param name="initialPx"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double ToPixels(double value, ChangeUnit unit, double initialPx)
        {
            if (unit == ChangeUnit.Percent)
                return initialPx * (value / 100.0);

            var mu = unit switch
            {
                ChangeUnit.Pixels => MeasurementUnit.Pixels,
                ChangeUnit.Inches => MeasurementUnit.Inches,
                ChangeUnit.Centimeters => MeasurementUnit.Centimeters,
                _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null),
            };

            return UnitConvertHelper.ToPixels(value, mu);
        }

        /// <summary>
        /// Converts a value from pixels to the specified measurement unit.
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="unit"></param>
        /// <param name="initialPx"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double FromPixels(double pixels, ChangeUnit unit, double initialPx)
        {
            if (unit == ChangeUnit.Percent)
                return (pixels / initialPx) * 100.0;

            var mu = unit switch
            {
                ChangeUnit.Pixels => MeasurementUnit.Pixels,
                ChangeUnit.Inches => MeasurementUnit.Inches,
                ChangeUnit.Centimeters => MeasurementUnit.Centimeters,
                _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null),
            };

            return UnitConvertHelper.FromPixels(pixels, mu);
        }
    }
}
