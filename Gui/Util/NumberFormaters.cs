using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.Gui.Util
{
    public class NumberFormaters
    {
        public static double KILO = 1024.0;
        public static double MEGA = KILO * KILO;
        public static double GIGA = MEGA * KILO;
        public static double TERA = GIGA * KILO;

        public static string ConvertBytesToString(long bytes)
        {

            if (bytes < KILO)
                return string.Format("{0} B", FormatNumberWithVariableDecimalPlaces(bytes));
            if (bytes < MEGA)
                return string.Format("{0} KB", FormatNumberWithVariableDecimalPlaces(bytes/KILO));
            if (bytes < GIGA)
                return string.Format("{0} MB", FormatNumberWithVariableDecimalPlaces(bytes / MEGA));
            if (bytes < TERA)
                return string.Format("{0} GB", FormatNumberWithVariableDecimalPlaces(bytes / GIGA));
            return string.Format("{0} TB", FormatNumberWithVariableDecimalPlaces(bytes / TERA));
        }

        public static string FormatNumberWithVariableDecimalPlaces(double number)
        {
            if (number < 10)
                return string.Format("{0:#.##}", number);
            if(number < 100)
                return string.Format("{0:#.#}", number);
            if (number < 1000)
                return string.Format("{0:#}", number);
            return string.Format("{0:#,#}", number);
        }
    }
}
