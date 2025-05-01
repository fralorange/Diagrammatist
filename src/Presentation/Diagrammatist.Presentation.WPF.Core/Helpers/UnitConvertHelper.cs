using Diagrammatist.Presentation.WPF.Core.Shared.Enums;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    public static class UnitConvertHelper
    {
        /// <summary>
        /// Default DPI (dots per inch) value.
        /// </summary>
        public const double DefaultDpi = 96.0;
        private const double CmPerInch = 2.54;

        /// <summary>
        /// Converts a value from one measurement unit to another.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double ToPixels(double value, MeasurementUnit unit, double dpi = DefaultDpi)
        {
            return unit switch
            {
                MeasurementUnit.Pixels => value,
                MeasurementUnit.Inches => value * dpi,
                MeasurementUnit.Centimeters => (value / CmPerInch) * dpi,
                _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null),
            };
        }

        /// <summary>
        /// Converts a value from pixels to the specified measurement unit.
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="unit"></param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double FromPixels(double pixels, MeasurementUnit unit, double dpi = DefaultDpi)
        {
            return unit switch
            {
                MeasurementUnit.Pixels => pixels,
                MeasurementUnit.Inches => pixels / dpi,
                MeasurementUnit.Centimeters => (pixels / dpi) * CmPerInch,
                _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null),
            };
        }
    }
}
