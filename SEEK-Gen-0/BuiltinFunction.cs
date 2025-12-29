using System;
using System.Collections.Generic;

namespace LOOPLanguage
{
    /// <summary>
    /// Represents a built-in function (like print, len, range, etc.).
    /// </summary>
    public class BuiltinFunction
    {
        #region Fields
        
        public string Name { get; private set; }
        public Func<List<object>, object> Implementation { get; private set; }
        public int MinArgs { get; private set; }
        public int MaxArgs { get; private set; }
        
        #endregion
        
        #region Initialization
        
        /// <summary>
        /// Creates a built-in function.
        /// </summary>
        /// <param name="name">Function name</param>
        /// <param name="implementation">C# function that implements the logic</param>
        /// <param name="minArgs">Minimum number of arguments (-1 for variadic)</param>
        /// <param name="maxArgs">Maximum number of arguments (-1 for unlimited)</param>
        public BuiltinFunction(string name, Func<List<object>, object> implementation, int minArgs = 0, int maxArgs = -1)
        {
            Name = name;
            Implementation = implementation;
            MinArgs = minArgs;
            MaxArgs = maxArgs;
        }
        
        #endregion
        
        #region Execution
        
        /// <summary>
        /// Calls the built-in function with the given arguments.
        /// </summary>
        public object Call(List<object> arguments, int lineNumber)
        {
            // Validate argument count
            if (MinArgs >= 0 && arguments.Count < MinArgs)
            {
                throw new ArgumentError(
                    string.Format("{0}() takes at least {1} argument(s), got {2}", Name, MinArgs, arguments.Count),
                    lineNumber
                );
            }
            
            if (MaxArgs >= 0 && arguments.Count > MaxArgs)
            {
                throw new ArgumentError(
                    string.Format("{0}() takes at most {1} argument(s), got {2}", Name, MaxArgs, arguments.Count),
                    lineNumber
                );
            }
            
            try
            {
                return Implementation(arguments);
            }
            catch (Exception e)
            {
                if (e is LOOPException)
                {
                    throw;
                }
                
                throw new RuntimeError(
                    string.Format("{0}(): {1}", Name, e.Message),
                    lineNumber
                );
            }
        }
        
        #endregion
        
        #region String Representation
        
        public override string ToString()
        {
            return string.Format("<built-in function {0}>", Name);
        }
        
        #endregion
    }
}