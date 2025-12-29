using System;

namespace LoopLanguage
{
    #region Base Exception
    
    /// <summary>
    /// Base exception class for all LOOP Language errors
    /// </summary>
    public class LoopException : Exception
    {
        public LoopException(string message) : base(message) { }
        public LoopException(string message, Exception inner) : base(message, inner) { }
    }
    
    #endregion
    
    #region Lexer Exceptions
    
    /// <summary>
    /// Thrown when lexer encounters invalid syntax during tokenization
    /// </summary>
    public class LexerError : LoopException
    {
        public LexerError(string message) : base(message) { }
    }
    
    #endregion
    
    #region Parser Exceptions
    
    /// <summary>
    /// Thrown when parser encounters invalid syntax during AST construction
    /// </summary>
    public class ParserError : LoopException
    {
        public ParserError(string message) : base(message) { }
    }
    
    #endregion
    
    #region Runtime Exceptions
    
    /// <summary>
    /// Thrown during script execution for runtime errors
    /// Always includes line number for debugging
    /// </summary>
    public class RuntimeError : LoopException
    {
        public int LineNumber { get; private set; }
        
        public RuntimeError(string message) : base(message) 
        {
            LineNumber = -1;
        }
        
        public RuntimeError(int line, string message) : base($"Line {line}: {message}")
        {
            LineNumber = line;
        }
        
        public RuntimeError(string message, Exception inner) : base(message, inner) { }
    }
    
    /// <summary>
    /// Thrown when break statement is used outside a loop
    /// </summary>
    public class BreakException : LoopException
    {
        public BreakException() : base("break statement used outside loop") { }
    }
    
    /// <summary>
    /// Thrown when continue statement is used outside a loop
    /// </summary>
    public class ContinueException : LoopException
    {
        public ContinueException() : base("continue statement used outside loop") { }
    }
    
    /// <summary>
    /// Thrown to propagate return value from function
    /// </summary>
    public class ReturnException : LoopException
    {
        public object Value { get; private set; }
        
        public ReturnException(object value) : base("Return")
        {
            Value = value;
        }
    }
    
    #endregion
}
