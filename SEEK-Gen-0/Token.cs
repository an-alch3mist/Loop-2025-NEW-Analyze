using System;

namespace LOOPLanguage
{
    #region Token Type Enumeration
    
    /// <summary>
    /// Defines all token types recognized by the LOOP language lexer.
    /// </summary>
    public enum TokenType
    {
        // Structural tokens
        INDENT,
        DEDENT,
        NEWLINE,
        EOF,
        
        // Literals
        IDENTIFIER,
        STRING,
        NUMBER,
        
        // Keywords - Control Flow
        IF,
        ELIF,
        ELSE,
        WHILE,
        FOR,
        BREAK,
        CONTINUE,
        PASS,
        
        // Keywords - Functions and Classes
        DEF,
        RETURN,
        LAMBDA,
        CLASS,
        
        // Keywords - Scope
        GLOBAL,
        
        // Keywords - Import
        IMPORT,
        
        // Keywords - Logical
        AND,
        OR,
        NOT,
        IN,
        IS,
        
        // Keywords - Literals
        TRUE,
        FALSE,
        NONE,
        
        // Arithmetic Operators
        PLUS,           // +
        MINUS,          // -
        STAR,           // *
        SLASH,          // /
        DOUBLE_SLASH,   // //
        PERCENT,        // %
        DOUBLE_STAR,    // **
        
        // Comparison Operators
        EQUAL_EQUAL,    // ==
        BANG_EQUAL,     // !=
        LESS,           // <
        GREATER,        // >
        LESS_EQUAL,     // <=
        GREATER_EQUAL,  // >=
        
        // Assignment Operators
        EQUAL,          // =
        PLUS_EQUAL,     // +=
        MINUS_EQUAL,    // -=
        STAR_EQUAL,     // *=
        SLASH_EQUAL,    // /=
        
        // Bitwise Operators
        AMPERSAND,      // &
        PIPE,           // |
        CARET,          // ^
        TILDE,          // ~
        LEFT_SHIFT,     // <<
        RIGHT_SHIFT,    // >>
        
        // Delimiters
        LEFT_PAREN,     // (
        RIGHT_PAREN,    // )
        LEFT_BRACKET,   // [
        RIGHT_BRACKET,  // ]
        LEFT_BRACE,     // {
        RIGHT_BRACE,    // }
        DOT,            // .
        COMMA,          // ,
        COLON           // :
    }
    
    #endregion
    
    #region Token Class
    
    /// <summary>
    /// Represents a single token in the LOOP language.
    /// </summary>
    public class Token
    {
        public TokenType Type { get; private set; }
        public string Lexeme { get; private set; }
        public object Literal { get; private set; }
        public int LineNumber { get; private set; }
        
        /// <summary>
        /// Creates a new token.
        /// </summary>
        /// <param name="type">The type of token</param>
        /// <param name="lexeme">The raw text of the token</param>
        /// <param name="literal">The interpreted value (for numbers, strings, etc.)</param>
        /// <param name="lineNumber">The line number where this token appears</param>
        public Token(TokenType type, string lexeme, object literal, int lineNumber)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            LineNumber = lineNumber;
        }
        
        /// <summary>
        /// Returns a string representation of this token for debugging.
        /// </summary>
        public override string ToString()
        {
            return string.Format(
                "Token({0}, '{1}', {2}, Line {3})",
                Type,
                Lexeme,
                Literal ?? "null",
                LineNumber
            );
        }
    }
    
    #endregion
}