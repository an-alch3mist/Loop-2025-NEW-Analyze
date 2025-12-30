using System;
using System.Collections;
using System.Collections.Generic;

namespace LoopLanguage
{
    /// <summary>
    /// Represents a built-in function that can be called from user scripts.
    /// Wraps C# functions and handles both synchronous and coroutine-based functions.
    /// </summary>
    public class BuiltinFunction
    {
        #region Fields
        
        private string name;
        private Func<List<object>, object> syncImpl;
        private Func<List<object>, IEnumerator> asyncImpl;
        private bool isAsync;
        
        #endregion
        
        #region Constructor
        
        /// <summary>
        /// Creates a synchronous built-in function
        /// </summary>
        public BuiltinFunction(string functionName, Func<List<object>, object> implementation)
        {
            name = functionName;
            syncImpl = implementation;
            asyncImpl = null;
            isAsync = false;
        }
        
        /// <summary>
        /// Creates an asynchronous (coroutine-based) built-in function
        /// </summary>
        public BuiltinFunction(string functionName, Func<List<object>, IEnumerator> implementation)
        {
            name = functionName;
            syncImpl = null;
            asyncImpl = implementation;
            isAsync = true;
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Returns true if this function yields (returns IEnumerator)
        /// </summary>
        public bool IsAsync()
        {
            return isAsync;
        }
        
        /// <summary>
        /// Calls the synchronous implementation
        /// </summary>
        public object Call(List<object> arguments)
        {
            if (isAsync)
            {
                throw new RuntimeError($"Function '{name}' is async and must be called with CallAsync");
            }
            
            return syncImpl(arguments);
        }
        
        /// <summary>
        /// Calls the asynchronous implementation (returns IEnumerator)
        /// </summary>
        public IEnumerator CallAsync(List<object> arguments)
        {
            if (!isAsync)
            {
                throw new RuntimeError($"Function '{name}' is not async");
            }
            
            return asyncImpl(arguments);
        }
        
        /// <summary>
        /// Returns the function name
        /// </summary>
        public string GetName()
        {
            return name;
        }
        
        #endregion
    }
}
