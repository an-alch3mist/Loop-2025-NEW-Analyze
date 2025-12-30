using System.Collections.Generic;

namespace LoopLanguage
{
    /// <summary>
    /// Runtime representation of a lambda expression.
    /// Captures closure scope and supports immediate invocation (IIFE pattern).
    /// </summary>
    public class LambdaFunction
    {
        #region Fields
        
        public List<string> Parameters { get; private set; }
        public Expr Body { get; private set; }
        public Scope ClosureScope { get; private set; }
        
        #endregion
        
        #region Constructor
        
        /// <summary>
        /// Creates a lambda function with parameters, body, and captured scope
        /// </summary>
        public LambdaFunction(List<string> parameters, Expr body, Scope closureScope)
        {
            Parameters = parameters;
            Body = body;
            ClosureScope = closureScope;
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Calls the lambda with provided arguments.
        /// Creates new local scope, binds parameters, evaluates body expression.
        /// </summary>
        public object Call(PythonInterpreter interpreter, List<object> arguments)
        {
            // Validate argument count
            if (arguments.Count != Parameters.Count)
            {
                throw new RuntimeError(
                    $"Lambda expects {Parameters.Count} arguments, got {arguments.Count}"
                );
            }
            
            // Create new local scope with closure as parent
            Scope lambdaScope = new Scope(ClosureScope);
            
            // Bind parameters to arguments
            for (int i = 0; i < Parameters.Count; i++)
            {
                lambdaScope.Define(Parameters[i], arguments[i]);
            }
            
            // Evaluate body expression in lambda scope
            Scope previousScope = interpreter.currentScope;
            interpreter.currentScope = lambdaScope;
            
            try
            {
                object result = interpreter.Evaluate(Body);
                return result;
            }
            finally
            {
                interpreter.currentScope = previousScope;
            }
        }
        
        /// <summary>
        /// Returns string representation for debugging
        /// </summary>
        public override string ToString()
        {
            return $"<lambda with {Parameters.Count} parameters>";
        }
        
        #endregion
    }
}
