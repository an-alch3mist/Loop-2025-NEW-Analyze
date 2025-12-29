using System.Collections.Generic;

namespace LOOPLanguage
{
    /// <summary>
    /// Manages variable scopes (global, local, closure).
    /// Supports nested scopes with parent chain lookup.
    /// </summary>
    public class Scope
    {
        #region Fields
        
        private Dictionary<string, object> variables;
        private Scope parent;
        
        #endregion
        
        #region Initialization
        
        /// <summary>
        /// Creates a new scope with optional parent.
        /// </summary>
        public Scope(Scope parent = null)
        {
            this.variables = new Dictionary<string, object>();
            this.parent = parent;
        }
        
        #endregion
        
        #region Variable Management
        
        /// <summary>
        /// Gets a variable's value from this scope or parent scopes.
        /// </summary>
        public object Get(string name)
        {
            if (variables.ContainsKey(name))
            {
                return variables[name];
            }
            
            if (parent != null)
            {
                return parent.Get(name);
            }
            
            throw new NameError(name, -1);
        }
        
        /// <summary>
        /// Checks if a variable exists in this scope or parent scopes.
        /// </summary>
        public bool Contains(string name)
        {
            if (variables.ContainsKey(name))
            {
                return true;
            }
            
            if (parent != null)
            {
                return parent.Contains(name);
            }
            
            return false;
        }
        
        /// <summary>
        /// Sets a variable in this scope.
        /// Creates it if it doesn't exist.
        /// </summary>
        public void Set(string name, object value)
        {
            variables[name] = value;
        }
        
        /// <summary>
        /// Updates a variable if it exists in this scope or parent scopes.
        /// Throws error if variable doesn't exist anywhere.
        /// </summary>
        public void Update(string name, object value)
        {
            if (variables.ContainsKey(name))
            {
                variables[name] = value;
                return;
            }
            
            if (parent != null)
            {
                parent.Update(name, value);
                return;
            }
            
            throw new NameError(name, -1);
        }
        
        /// <summary>
        /// Defines a variable in the current scope only.
        /// </summary>
        public void Define(string name, object value)
        {
            variables[name] = value;
        }
        
        /// <summary>
        /// Gets the parent scope.
        /// </summary>
        public Scope GetParent()
        {
            return parent;
        }
        
        /// <summary>
        /// Clears all variables in this scope.
        /// </summary>
        public void Clear()
        {
            variables.Clear();
        }
        
        #endregion
    }
}