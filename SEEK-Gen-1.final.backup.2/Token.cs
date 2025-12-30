using System;

namespace LoopLanguage
{
	#region Token Type Enumeration

	/// <summary>
	/// Defines all token types recognized by the LOOP Language lexer.
	/// Organized by category: structural, literals, keywords, operators, delimiters.
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

		// Control flow keywords
		IF,
		ELIF,
		ELSE,
		WHILE,
		FOR,
		BREAK,
		CONTINUE,
		PASS,

		// Function/class keywords
		DEF,
		RETURN,
		CLASS,
		LAMBDA,
		IMPORT,

		// Scope keywords
		GLOBAL,

		// Logical operators
		AND,
		OR,
		NOT,
		IN,
		IS,

		// Literal values
		TRUE,
		FALSE,
		NONE,

		// Arithmetic operators
		PLUS,           // +
		MINUS,          // -
		STAR,           // *
		SLASH,          // /
		PERCENT,        // %
		DOUBLE_STAR,    // **
		DOUBLE_SLASH,   // //

		// Comparison operators
		EQUAL_EQUAL,    // ==
		BANG_EQUAL,     // !=
		LESS,           // <
		GREATER,        // >
		LESS_EQUAL,     // <=
		GREATER_EQUAL,  // >=

		// Assignment operators
		EQUAL,          // =
		PLUS_EQUAL,     // +=
		MINUS_EQUAL,    // -=
		STAR_EQUAL,     // *=
		SLASH_EQUAL,    // /=

		// Bitwise operators
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
	/// Represents a single token produced by the lexer.
	/// Contains type information, lexeme (original text), optional literal value, and line number for error reporting.
	/// </summary>
	public class Token
	{
		/// <summary>
		/// The type of this token (keyword, operator, literal, etc.)
		/// </summary>
		public TokenType Type { get; private set; }

		/// <summary>
		/// The original text from the source code that produced this token
		/// </summary>
		public string Lexeme { get; private set; }

		/// <summary>
		/// For NUMBER and STRING tokens, contains the parsed value (double or string)
		/// </summary>
		public object Literal { get; private set; }

		/// <summary>
		/// Line number in source code where this token was found (1-indexed)
		/// </summary>
		public int LineNumber { get; private set; }

		/// <summary>
		/// Creates a new token with all required information
		/// </summary>
		/// <param name="type">Token type</param>
		/// <param name="lexeme">Original text</param>
		/// <param name="literal">Parsed value (for literals)</param>
		/// <param name="line">Line number (1-indexed)</param>
		public Token(TokenType type, string lexeme, object literal, int line)
		{
			Type = type;
			Lexeme = lexeme;
			Literal = literal;
			LineNumber = line;
		}

		/// <summary>
		/// Returns a string representation for debugging
		/// </summary>
		public override string ToString()
		{
			if (Literal != null)
			{
				return string.Format("Token({0}, '{1}', {2}, Line {3})",
					Type, Lexeme, Literal, LineNumber);
			}
			return string.Format("Token({0}, '{1}', Line {2})",
				Type, Lexeme, LineNumber);
		}
	}

	#endregion
}