using System.Collections.Generic;

namespace LOOPLanguage
{
    /// <summary>
    /// Runtime representation of a class instance.
    /// Stores instance variables and provides access to methods.
    /// </summary>
    public class ClassInstance
    {
        #region Fields
        
        public string ClassName { get; private set; }
        public Dictionary<string, object> Fields { get; private set; }
        public Dictionary<string, UserFunction> Methods { get; private set; }
        
        #endregion
        
        #region Initialization
        
        public ClassInstance(string className)
        {
            ClassName = className;
            Fields = new Dictionary<string, object>();
            Methods = new Dictionary<string, UserFunction>();
        }
        
        #endregion
        
        #region Field Access
        
        public object GetField(string name)
        {
            if (Fields.ContainsKey(name))
            {
                return Fields[name];
            }
            
            throw new AttributeError(
                string.Format("'{0}' object has no attribute '{1}'", ClassName, name),
                -1
            );
        }
        
        public void SetField(string name, object value)
        {
            Fields[name] = value;
        }
        
        public bool HasField(string name)
        {
            return Fields.ContainsKey(name);
        }
        
        #endregion
        
        #region Method Access
        
        public void AddMethod(string name, UserFunction method)
        {
            Methods[name] = method;
        }
        
        public UserFunction GetMethod(string name)
        {
            if (Methods.ContainsKey(name))
            {
                return Methods[name];
            }
            
            throw new AttributeError(
                string.Format("'{0}' object has no method '{1}'", ClassName, name),
                -1
            );
        }
        
        public bool HasMethod(string name)
        {
            return Methods.ContainsKey(name);
        }
        
        #endregion
        
        #region String Representation
        
        public override string ToString()
        {
            return string.Format("<{0} instance>", ClassName);
        }
        
        #endregion
    }
    
    /// <summary>
    /// Runtime representation of a user-defined function.
    /// </summary>
    public class UserFunction
    {
        public string Name { get; set; }
        public List<string> Parameters { get; set; }
        public List<Stmt> Body { get; set; }
        public Scope ClosureScope { get; set; }
        
        public UserFunction(string name, List<string> parameters, List<Stmt> body, Scope closureScope)
        {
            Name = name;
            Parameters = parameters;
            Body = body;
            ClosureScope = closureScope;
        }
        
        public override string ToString()
        {
            return string.Format("<function {0}>", Name);
        }
    }
}