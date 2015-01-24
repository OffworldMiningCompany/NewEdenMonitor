using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEdenMonitor
{
    public static class Extensions
    {
        public static byte[] ToByteArray(this object value)
        {
            byte[] array = null;

            if (!(value is DBNull))
            {
                array = (byte[])value;
            }

            return array;
        }

        public static DateTime ToDateTime(this object value)
        {
            if (value is DateTime)
            {
                return (DateTime) value;
            }

            return default(DateTime);
        }

        public static Int32 ToInt32(this object value)
        {
            if (value is Int16 || value is Int32 || value is UInt16 || value is UInt32 || value is Int64 || value is UInt64)
            {
                return Convert.ToInt32(value);;
            }

            return default(int);
        }

        public static Int64 ToInt64(this object value)
        {
            if (value is Int16 || value is Int32 || value is Int64 || value is UInt16 || value is UInt32 || value is UInt64)
            {
                return Convert.ToInt64(value);
            }

            return default(long);
        }
    }
}
