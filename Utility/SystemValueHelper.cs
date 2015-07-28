using System;

namespace CodeCharm.Utility
{
    public static class SystemValueHelper
    {
        /// <summary>
        /// Converts a <see cref="System.Nullable{Boolean}" /> to a string or a null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>
        /// The difference of this function from <see cref="System.Nullable{Boolean}.ToString" /> is that 
        /// this function will return a null if value.HasValue is False,
        /// rather than an empty string ("").
        /// </remarks>
        public static string ToString(bool? value)
        {
            return value.HasValue ? value.Value.ToString() : null;
        }

        public static string ToString(DateTime? value)
        {
            return value.HasValue ? value.Value.ToString() : null;
        }

        public static string ToString(DateTime? value, IFormatProvider formatProvider)
        {
            return value.HasValue ? value.Value.ToString(formatProvider) : null;
        }

        public static string ToString(DateTime? value, string format)
        {
            return value.HasValue ? value.Value.ToString(format) : null;            
        }

        public static string ToString(DateTime? value, string format, IFormatProvider formatProvider)
        {
            return value.HasValue ? value.Value.ToString(format, formatProvider) : null;
        }

        public static string ToString(int? value)
        {
            return value.HasValue ? value.Value.ToString() : null;
        }

        public static T Multiply<T>(T factor1, T factor2) 
            where T : struct, IConvertible
        {
            TypeCode typeCode = factor1.GetTypeCode();
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    break;

                case TypeCode.Byte:
                    return (T)Convert.ChangeType(Convert.ToByte(factor1) * Convert.ToByte(factor2), typeof(T));

                case TypeCode.Char:
                    break;

                case TypeCode.DBNull:
                    break;

                case TypeCode.DateTime:
                    break;

                case TypeCode.Decimal:
                    return (T)Convert.ChangeType(Convert.ToDecimal(factor1) * Convert.ToDecimal(factor2), typeof(T));

                case TypeCode.Double:
                    return (T)Convert.ChangeType(Convert.ToDouble(factor1) * Convert.ToDouble(factor2), typeof(T));

                case TypeCode.Empty:
                    break;

                case TypeCode.Int16:
                    return (T)Convert.ChangeType(Convert.ToInt16(factor1) * Convert.ToInt16(factor2), typeof(T));

                case TypeCode.Int32:
                    return (T)Convert.ChangeType(Convert.ToInt32(factor1) * Convert.ToInt32(factor2), typeof(T));

                case TypeCode.Int64:
                    return (T)Convert.ChangeType(Convert.ToInt64(factor1) * Convert.ToInt64(factor2), typeof(T));

                case TypeCode.Object:
                    break;

                case TypeCode.SByte:
                    return (T)Convert.ChangeType(Convert.ToSByte(factor1) * Convert.ToSByte(factor2), typeof(T));

                case TypeCode.Single:
                    return (T)Convert.ChangeType(Convert.ToSingle(factor1) * Convert.ToSingle(factor2), typeof(T));

                case TypeCode.String:
                    break;

                case TypeCode.UInt16:
                    return (T)Convert.ChangeType(Convert.ToUInt16(factor1) * Convert.ToUInt16(factor2), typeof(T));

                case TypeCode.UInt32:
                    return (T)Convert.ChangeType(Convert.ToUInt32(factor1) * Convert.ToUInt32(factor2), typeof(T));

                case TypeCode.UInt64:
                    return (T)Convert.ChangeType(Convert.ToUInt64(factor1) * Convert.ToUInt64(factor2), typeof(T));

            }
            throw new NotSupportedException(string.Format("Type {0} does not support multiplication operations", typeof(T).FullName));
        }

        public static T Divide<T>(T dividend, T divisor)
            where T : struct, IConvertible
        {
            TypeCode typeCode = dividend.GetTypeCode();
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    break;

                case TypeCode.Byte:
                    return (T)Convert.ChangeType(Convert.ToByte(dividend) / Convert.ToByte(divisor), typeof(T));

                case TypeCode.Char:
                    break;

                case TypeCode.DBNull:
                    break;

                case TypeCode.DateTime:
                    break;

                case TypeCode.Decimal:
                    return (T)Convert.ChangeType(Convert.ToDecimal(dividend) / Convert.ToDecimal(divisor), typeof(T));

                case TypeCode.Double:
                    return (T)Convert.ChangeType(Convert.ToDouble(dividend) / Convert.ToDouble(divisor), typeof(T));

                case TypeCode.Empty:
                    break;

                case TypeCode.Int16:
                    return (T)Convert.ChangeType(Convert.ToInt16(dividend) / Convert.ToInt16(divisor), typeof(T));

                case TypeCode.Int32:
                    return (T)Convert.ChangeType(Convert.ToInt32(dividend) / Convert.ToInt32(divisor), typeof(T));

                case TypeCode.Int64:
                    return (T)Convert.ChangeType(Convert.ToInt64(dividend) / Convert.ToInt64(divisor), typeof(T));

                case TypeCode.Object:
                    break;

                case TypeCode.SByte:
                    return (T)Convert.ChangeType(Convert.ToSByte(dividend) / Convert.ToSByte(divisor), typeof(T));

                case TypeCode.Single:
                    return (T)Convert.ChangeType(Convert.ToSingle(dividend) / Convert.ToSingle(divisor), typeof(T));

                case TypeCode.String:
                    break;

                case TypeCode.UInt16:
                    return (T)Convert.ChangeType(Convert.ToUInt16(dividend) / Convert.ToUInt16(divisor), typeof(T));

                case TypeCode.UInt32:
                    return (T)Convert.ChangeType(Convert.ToUInt32(dividend) / Convert.ToUInt32(divisor), typeof(T));

                case TypeCode.UInt64:
                    return (T)Convert.ChangeType(Convert.ToUInt64(dividend) / Convert.ToUInt64(divisor), typeof(T));

            }
            throw new NotSupportedException(string.Format("Type {0} does not support division operations", typeof(T).FullName));
        }

        public static bool SequenceEquals(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
