using System.Collections.Generic;

namespace LoopLanguage
{
    /// <summary>
    /// Represents a variable scope with parent chain for nested scopes.
    /// Supports local/global variables, closures, and scope chaining.
    /// </summary>
    public class Scope
    {
        #region Fields
        
        private Dictionary<string, object> variables;
        private Scope parent;
        
        #endregion
        
        #region Constructor
        
        /// <summary>
        /// Creates a new scope with optional parent
        /// </summary>
        /// <param name="parentScope">Parent scope for nested scopes (null for global)</param>
        public Scope(Scope parentScope = null)
        {
            variables = new Dictionary<string, object>();
            parent = parentScope;
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Defines a new variable in this scope (local)
        /// </summary>
        public void Define(string name, object value)
        {
            variables[name] = value;
        }
        
        /// <summary>
        /// Sets value of existing variable (searches parent chain)
        /// </summary>
        public void Set(string name, object value)
        {
            if (variables.ContainsKey(name))
            {
                variables[name] = value;
            }
            else if (parent != null)
            {
                parent.Set(name, value);
            }
            else
            {
                // Variable doesn't exist, create it in current scope
                variables[name] = value;
            }
        }
        
        /// <summary>
        /// Gets value of variable (searches parent chain)
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
            
            throw new RuntimeError($"Undefined variable: {name}");
        }
        
        /// <summary>
        /// Checks if variable exists in this scope or parent chain
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
        /// Returns the parent scope (null if global)
        /// </summary>
        public Scope GetParent()
        {
            return parent;
        }
        
        /// <summary>
        /// Walks up the scope chain to find the global scope
        /// </summary>
        public Scope GetGlobalScope()
        {
            Scope current = this;
            while (current.parent != null)
            {
                current = current.parent;
            }
            return current;
        }
        
        /// <summary>
        /// Returns all variable names in this scope (not including parents)
        /// </summary>
        public List<string> GetLocalVariableNames()
        {
            return new List<string>(variables.Keys);
        }
        
        #endregion
    }
}
