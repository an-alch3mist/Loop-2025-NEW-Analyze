using System.Collections.Generic;

namespace LOOPLanguage
{
    /// <summary>
    /// Runtime representation of a lambda expression.
    /// Supports closures (capturing outer scope variables).
    /// </summary>
    public class LambdaFunction
    {
        #region Fields
        
        public List<string> Parameters { get; private set; }
        public Expr Body { get; private set; }
        public Scope ClosureScope { get; private set; }
        
        #endregion
        
        #region Initialization
        
        /// <summary>
        /// Creates a new lambda function.
        /// </summary>
        /// <param name="parameters">Parameter names</param>
        /// <param name="body">Expression to evaluate when called</param>
        /// <param name="closureScope">Captured scope for closure variables</param>
        public LambdaFunction(List<string> parameters, Expr body, Scope closureScope)
        {
            Parameters = parameters;
            Body = body;
            ClosureScope = closureScope;
        }
        
        #endregion
        
        #region String Representation
        
        public override string ToString()
        {
            return string.Format("<lambda ({0} parameters)>", Parameters.Count);
        }
        
        #endregion
    }
}