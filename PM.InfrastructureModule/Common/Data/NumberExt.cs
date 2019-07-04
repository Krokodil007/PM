using System.Globalization;

namespace PM.InfrastructureModule.Common.Data
{
    public static class NumberExt
    {
        /// <summary>
        /// Конвертация числа из string в double с учётом разных десятичных разделителей в разных культурах
        /// </summary>
        public static double ToDouble(this string val)
        {
            var delimiter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            return double.Parse(val.Replace(",", delimiter).Replace(".", delimiter));
        }

        public static decimal ToDecimal(this string val)
        {
            if (string.IsNullOrEmpty(val))
                val = "0";
            var delimiter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            return decimal.Parse(val.Replace(",", delimiter).Replace(".", delimiter));
        }

        /// <summary>
        /// Дробная часть числа
        /// </summary>
        public static double Fraction(this decimal val)
        {
            double fract = (double) (val % 1);
           // int.TryParse(fract.ToString("0.####"), out var result);

            return fract;
        }
    }
}