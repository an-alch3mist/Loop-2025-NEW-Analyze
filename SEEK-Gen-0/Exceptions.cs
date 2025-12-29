using System;

namespace LOOPLanguage
{
    #region Base Exception
    
    /// <summary>
    /// Base exception class for all LOOP language errors.
    /// </summary>
    public class LOOPException : Exception
    {
        public int LineNumber { get; private set; }
        
        public LOOPException(string message) : base(message)
        {
            LineNumber = -1;
        }
        
        public LOOPException(string message, int lineNumber) : base(message)
        {
            LineNumber = lineNumber;
        }
        
        public override string ToString()
        {
            if (LineNumber >= 0)
            {
                return string.Format("Line {0}: {1}", LineNumber, Message);
            }
            return Message;
        }
    }
    
    #endregion
    
    #region Lexer Exceptions
    
    /// <summary>
    /// Thrown when the lexer encounters invalid syntax during tokenization.
    /// </summary>
    public class LexerError : LOOPException
    {
        public LexerError(string message) : base(message) { }
        public LexerError(string message, int lineNumber) : base(message, lineNumber) { }
    }
    
    #endregion
    
    #region Parser Exceptions
    
    /// <summary>
    /// Thrown when the parser encounters invalid syntax during AST construction.
    /// </summary>
    public class ParseError : LOOPException
    {
        public ParseError(string message) : base(message) { }
        public ParseError(string message, int lineNumber) : base(message, lineNumber) { }
    }
    
    #endregion
    
    #region Runtime Exceptions
    
    /// <summary>
    /// Thrown during script execution when a runtime error occurs.
    /// </summary>
    public class RuntimeError : LOOPException
    {
        public RuntimeError(string message) : base(message) { }
        public RuntimeError(string message, int lineNumber) : base(message, lineNumber) { }
    }
    
    /// <summary>
    /// Thrown when division by zero is attempted.
    /// </summary>
    public class DivisionByZeroError : RuntimeError
    {
        public DivisionByZeroError(int lineNumber) 
            : base("Division by zero", lineNumber) { }
    }
    
    /// <summary>
    /// Thrown when an index is out of range for a list/tuple/string.
    /// </summary>
    public class IndexError : RuntimeError
    {
        public IndexError(string message, int lineNumber) 
            : base(message, lineNumber) { }
    }
    
    /// <summary>
    /// Thrown when a key is not found in a dictionary.
    /// </summary>
    public class KeyError : RuntimeError
    {
        public KeyError(string key, int lineNumber) 
            : base(string.Format("Key not found: '{0}'", key), lineNumber) { }
    }
    
    /// <summary>
    /// Thrown when a variable is referenced before assignment.
    /// </summary>
    public class NameError : RuntimeError
    {
        public NameError(string variableName, int lineNumber) 
            : base(string.Format("Name '{0}' is not defined", variableName), lineNumber) { }
    }
    
    /// <summary>
    /// Thrown when an operation is performed on incompatible types.
    /// </summary>
    public class TypeError : RuntimeError
    {
        public TypeError(string message, int lineNumber) 
            : base(message, lineNumber) { }
    }
    
    /// <summary>
    /// Thrown when a function is called with the wrong number of arguments.
    /// </summary>
    public class ArgumentError : RuntimeError
    {
        public ArgumentError(string message, int lineNumber) 
            : base(message, lineNumber) { }
    }
    
    /// <summary>
    /// Thrown when an attribute or member is not found on an object.
    /// </summary>
    public class AttributeError : RuntimeError
    {
        public AttributeError(string message, int lineNumber) 
            : base(message, lineNumber) { }
    }
    
    /// <summary>
    /// Thrown when recursion depth is exceeded.
    /// </summary>
    public class RecursionError : RuntimeError
    {
        public RecursionError(int lineNumber) 
            : base("Maximum recursion depth exceeded", lineNumber) { }
    }
    
    #endregion
    
    #region Control Flow Exceptions
    
    /// <summary>
    /// Special exception used to implement 'break' statement.
    /// Not a real error - used for control flow.
    /// </summary>
    public class BreakException : Exception { }
    
    /// <summary>
    /// Special exception used to implement 'continue' statement.
    /// Not a real error - used for control flow.
    /// </summary>
    public class ContinueException : Exception { }
    
    /// <summary>
    /// Special exception used to implement 'return' statement.
    /// Not a real error - used for control flow.
    /// </summary>
    public class ReturnException : Exception
    {
        public object Value { get; private set; }
        
        public ReturnException(object value)
        {
            Value = value;
        }
    }
    
    #endregion
}