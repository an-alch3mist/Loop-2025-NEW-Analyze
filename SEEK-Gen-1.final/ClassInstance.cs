using System.Collections.Generic;

namespace LoopLanguage
{
    /// <summary>
    /// Runtime representation of a class instance.
    /// Stores instance variables and provides method lookup.
    /// </summary>
    public class ClassInstance
    {
        #region Fields
        
        private string className;
        private Dictionary<string, object> fields;
        private Dictionary<string, FunctionDefStmt> methods;
        
        #endregion
        
        #region Constructor
        
        /// <summary>
        /// Creates a new class instance with class name and methods
        /// </summary>
        public ClassInstance(string name, Dictionary<string, FunctionDefStmt> classMethods)
        {
            className = name;
            fields = new Dictionary<string, object>();
            methods = new Dictionary<string, FunctionDefStmt>(classMethods);
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Sets an instance field value
        /// </summary>
        public void SetField(string name, object value)
        {
            fields[name] = value;
        }
        
        /// <summary>
        /// Gets an instance field value
        /// </summary>
        public object GetField(string name)
        {
            if (fields.ContainsKey(name))
            {
                return fields[name];
            }
            
            throw new RuntimeError($"Undefined field: {name}");
        }
        
        /// <summary>
        /// Checks if field exists
        /// </summary>
        public bool HasField(string name)
        {
            return fields.ContainsKey(name);
        }
        
        /// <summary>
        /// Gets a method definition
        /// </summary>
        public FunctionDefStmt GetMethod(string name)
        {
            if (methods.ContainsKey(name))
            {
                return methods[name];
            }
            
            throw new RuntimeError($"Undefined method: {name}");
        }
        
        /// <summary>
        /// Checks if method exists
        /// </summary>
        public bool HasMethod(string name)
        {
            return methods.ContainsKey(name);
        }
        
        /// <summary>
        /// Returns string representation
        /// </summary>
        public override string ToString()
        {
            return $"<{className} instance>";
        }
        
        #endregion
    }
}
