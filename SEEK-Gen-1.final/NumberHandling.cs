using System;

namespace LoopLanguage
{
    /// <summary>
    /// Handles Python-style number semantics:
    /// - All numbers stored as double internally (simplifies implementation)
    /// - Comparisons work across int/float: 1 == 1.0 is True
    /// - Indexing requires integers: list[1.5] raises error
    /// - Range requires integers: range(1.5) raises error
    /// </summary>
    public static class NumberHandling
    {
        #region Conversion Methods
        
        /// <summary>
        /// Converts value to double (Python's numeric type)
        /// Handles: int, float, double, bool, string
        /// </summary>
        public static double ToNumber(object value)
        {
            if (value is double) return (double)value;
            if (value is int) return (int)value;
            if (value is float) return (float)value;
            if (value is long) return (long)value;
            if (value is bool) return ((bool)value) ? 1.0 : 0.0;
            
            if (value is string)
            {
                string str = (string)value;
                double result;
                if (double.TryParse(str, out result))
                {
                    return result;
                }
                throw new RuntimeError($"Cannot convert '{str}' to number");
            }
            
            throw new RuntimeError($"Cannot convert {value?.GetType().Name ?? "null"} to number");
        }
        
        /// <summary>
        /// Converts value to integer (for indexing, range, etc.)
        /// STRICT: Rejects non-integer values (Python behavior)
        /// </summary>
        public static int ToInteger(object value, string context)
        {
            double num = ToNumber(value);
            
            // Check if it's actually an integer value
            if (Math.Abs(num - Math.Floor(num)) > 0.0000001)
            {
                throw new RuntimeError(
                    $"{context} requires integer, got {num} (float/decimal)"
                );
            }
            
            // Check range (prevent overflow)
            if (num > int.MaxValue || num < int.MinValue)
            {
                throw new RuntimeError(
                    $"{context} value {num} out of integer range"
                );
            }
            
            return (int)num;
        }
        
        /// <summary>
        /// Converts value to integer, allowing negative indices
        /// Used for list indexing: list[-1] is valid
        /// </summary>
        public static int ToListIndex(object value, int listLength)
        {
            int index = ToInteger(value, "List index");
            
            // Handle negative indexing
            if (index < 0)
            {
                index = listLength + index;
            }
            
            // Validate range
            if (index < 0 || index >= listLength)
            {
                throw new RuntimeError($"List index {index} out of range (length: {listLength})");
            }
            
            return index;
        }
        
        /// <summary>
        /// Converts value to integer for range() function
        /// Must be integer, can be negative
        /// </summary>
        public static int ToRangeValue(object value)
        {
            return ToInteger(value, "range()");
        }
        
        /// <summary>
        /// Checks if two numeric values are equal (Python semantics)
        /// 1 == 1.0 returns True
        /// </summary>
        public static bool NumbersEqual(object a, object b)
        {
            double numA = ToNumber(a);
            double numB = ToNumber(b);
            
            return Math.Abs(numA - numB) < 0.0000001;
        }
        
        #endregion
        
        #region Validation Methods
        
        /// <summary>
        /// Checks if value is an integer (not necessarily int type)
        /// 1.0 returns True, 1.5 returns False
        /// </summary>
        public static bool IsInteger(object value)
        {
            double num = ToNumber(value);
            return Math.Abs(num - Math.Floor(num)) < 0.0000001;
        }
        
        /// <summary>
        /// Validates that value is an integer, throws if not
        /// Used in contexts that REQUIRE integers
        /// </summary>
        public static void RequireInteger(object value, string context)
        {
            if (!IsInteger(value))
            {
                throw new RuntimeError(
                    $"{context} requires integer, got {ToNumber(value)}"
                );
            }
        }
        
        #endregion
        
        #region Display Methods
        
        /// <summary>
        /// Converts number to string (Python display format)
        /// 1.0 displays as "1", 1.5 displays as "1.5"
        /// </summary>
        public static string NumberToString(double value)
        {
            // If it's an integer value, display without decimal
            if (Math.Abs(value - Math.Floor(value)) < 0.0000001)
            {
                return ((int)value).ToString();
            }
            
            // Otherwise display with decimals
            return value.ToString("G");
        }
        
        #endregion
        
        #region Comparison Methods
        
        /// <summary>
        /// Compares two numeric values (Python semantics)
        /// Returns: -1 if a < b, 0 if a == b, 1 if a > b
        /// </summary>
        public static int CompareNumbers(object a, object b)
        {
            double numA = ToNumber(a);
            double numB = ToNumber(b);
            
            if (Math.Abs(numA - numB) < 0.0000001)
            {
                return 0; // Equal
            }
            
            return numA < numB ? -1 : 1;
        }
        
        #endregion
    }
}
