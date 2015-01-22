using System;
using System.Globalization;
using System.Windows.Data;

namespace NewEdenMonitor
{
    public class NumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return ((int) value).ToString("#,##0");
            }

            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;

            if (str != null)
            {
                int integer;
                if (int.TryParse(str, NumberStyles.AllowThousands, culture, out integer))
                {
                    return integer;
                }
            }

            return 0;
        }
    }

    public class OnlineOfflineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "UNKNOWN";

            switch (value.ToString().ToLower())
            {
                case "true":
                    return "ONLINE";
                case "false":
                    return "OFFLINE";
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                if ((string)value == "ONLINE")
                    return "true";
                
                return "false";
            }

            return "false";
        }
    }
}
