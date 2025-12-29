using System;
using System.Collections.Generic;
using System.Text;

namespace LOOPLanguage
{
    /// <summary>
    /// Tokenizes LOOP language source code into a stream of tokens.
    /// Handles Python-style indentation and all operators.
    /// </summary>
    public class Lexer
    {
        #region Fields
        
        private string source;
        private List<Token> tokens;
        private int start;
        private int current;
        private int line;
        private Stack<int> indentStack;
        
        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
        {
            {"if", TokenType.IF},
            {"elif", TokenType.ELIF},
            {"else", TokenType.ELSE},
            {"while", TokenType.WHILE},
            {"for", TokenType.FOR},
            {"def", TokenType.DEF},
            {"return", TokenType.RETURN},
            {"class", TokenType.CLASS},
            {"break", TokenType.BREAK},
            {"continue", TokenType.CONTINUE},
            {"pass", TokenType.PASS},
            {"global", TokenType.GLOBAL},
            {"lambda", TokenType.LAMBDA},
            {"import", TokenType.IMPORT},
            {"and", TokenType.AND},
            {"or", TokenType.OR},
            {"not", TokenType.NOT},
            {"in", TokenType.IN},
            {"is", TokenType.IS},
            {"True", TokenType.TRUE},
            {"False", TokenType.FALSE},
            {"None", TokenType.NONE}
        };
        
        #endregion
        
        #region Initialization
        
        /// <summary>
        /// Creates a new lexer for the given source code.
        /// </summary>
        public Lexer(string source)
        {
            this.source = ValidateAndClean(source);
            this.tokens = new List<Token>();
            this.start = 0;
            this.current = 0;
            this.line = 1;
            this.indentStack = new Stack<int>();
            this.indentStack.Push(0); // Initialize with base indentation level
        }
        
        /// <summary>
        /// Validates and cleans input to prevent parsing issues.
        /// </summary>
        private string ValidateAndClean(string input)
        {
            if (input == null) return "\n";
            
            // Normalize line endings
            input = input.Replace("\r\n", "\n");
            input = input.Replace("\r", "\n");
            
            // Convert tabs to 4 spaces
            input = input.Replace("\t", "    ");
            
            // Remove invisible characters
            input = input.Replace("\v", "");  // Vertical tab
            input = input.Replace("\f", "");  // Form feed
            input = input.Replace("\uFEFF", "");  // BOM
            
            // Ensure ends with newline
            if (!input.EndsWith("\n"))
            {
                input += "\n";
            }
            
            return input;
        }
        
        #endregion
        
        #region Main Tokenization
        
        /// <summary>
        /// Tokenizes the entire source code and returns the token list.
        /// </summary>
        public List<Token> Tokenize()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }
            
            // Emit remaining dedents at end of file
            while (indentStack.Count > 1)
            {
                indentStack.Pop();
                AddToken(TokenType.DEDENT);
            }
            
            AddToken(TokenType.EOF);
            return tokens;
        }
        
        /// <summary>
        /// Scans a single token from the current position.
        /// </summary>
        private void ScanToken()
        {
            char c = Advance();
            
            switch (c)
            {
                // Whitespace and newlines
                case '\n':
                    AddToken(TokenType.NEWLINE);
                    line++;
                    ProcessIndentation();
                    break;
                    
                case ' ':
                    // Whitespace is handled by ProcessIndentation
                    break;
                    
                // Comments
                case '#':
                    // Skip until end of line
                    while (Peek() != '\n' && !IsAtEnd()) Advance();
                    break;
                    
                case '/':
                    if (Match('/'))
                    {
                        // C-style comment
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else if (Match('='))
                    {
                        AddToken(TokenType.SLASH_EQUAL);
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                    
                // Single-character tokens
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '[': AddToken(TokenType.LEFT_BRACKET); break;
                case ']': AddToken(TokenType.RIGHT_BRACKET); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case ':': AddToken(TokenType.COLON); break;
                case '~': AddToken(TokenType.TILDE); break;
                case '%': AddToken(TokenType.PERCENT); break;
                case '^': AddToken(TokenType.CARET); break;
                
                // Operators that can be compound
                case '+':
                    AddToken(Match('=') ? TokenType.PLUS_EQUAL : TokenType.PLUS);
                    break;
                    
                case '-':
                    AddToken(Match('=') ? TokenType.MINUS_EQUAL : TokenType.MINUS);
                    break;
                    
                case '*':
                    if (Match('*'))
                    {
                        AddToken(TokenType.DOUBLE_STAR);
                    }
                    else if (Match('='))
                    {
                        AddToken(TokenType.STAR_EQUAL);
                    }
                    else
                    {
                        AddToken(TokenType.STAR);
                    }
                    break;
                    
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                    
                case '!':
                    if (Match('='))
                    {
                        AddToken(TokenType.BANG_EQUAL);
                    }
                    else
                    {
                        throw new LexerError("Unexpected character: '!'", line);
                    }
                    break;
                    
                case '<':
                    if (Match('<'))
                    {
                        AddToken(TokenType.LEFT_SHIFT);
                    }
                    else if (Match('='))
                    {
                        AddToken(TokenType.LESS_EQUAL);
                    }
                    else
                    {
                        AddToken(TokenType.LESS);
                    }
                    break;
                    
                case '>':
                    if (Match('>'))
                    {
                        AddToken(TokenType.RIGHT_SHIFT);
                    }
                    else if (Match('='))
                    {
                        AddToken(TokenType.GREATER_EQUAL);
                    }
                    else
                    {
                        AddToken(TokenType.GREATER);
                    }
                    break;
                    
                case '&':
                    AddToken(TokenType.AMPERSAND);
                    break;
                    
                case '|':
                    AddToken(TokenType.PIPE);
                    break;
                    
                // String literals
                case '"':
                case '\'':
                    ScanString(c);
                    break;
                    
                default:
                    if (IsDigit(c))
                    {
                        ScanNumber();
                    }
                    else if (IsAlpha(c))
                    {
                        ScanIdentifier();
                    }
                    else
                    {
                        throw new LexerError(
                            string.Format("Unexpected character: '{0}'", c), 
                            line
                        );
                    }
                    break;
            }
        }
        
        #endregion
        
        #region Indentation Processing
        
        /// <summary>
        /// Processes indentation at the start of a new line.
        /// Emits INDENT or DEDENT tokens as needed.
        /// </summary>
        private void ProcessIndentation()
        {
            // Skip if at end or next line is empty/comment
            if (IsAtEnd()) return;
            
            // Count leading spaces
            int spaces = 0;
            while (Peek() == ' ')
            {
                spaces++;
                Advance();
            }
            
            // Skip blank lines and comment-only lines
            if (Peek() == '\n' || Peek() == '#')
            {
                return;
            }
            
            // Validate indentation is multiple of 4
            if (spaces % 4 != 0)
            {
                throw new LexerError(
                    string.Format(
                        "Indentation error: expected multiple of 4 spaces, got {0}", 
                        spaces
                    ), 
                    line
                );
            }
            
            int currentIndent = indentStack.Peek();
            
            if (spaces > currentIndent)
            {
                // INDENT
                indentStack.Push(spaces);
                AddToken(TokenType.INDENT);
            }
            else if (spaces < currentIndent)
            {
                // DEDENT (possibly multiple)
                while (indentStack.Count > 0 && indentStack.Peek() > spaces)
                {
                    indentStack.Pop();
                    AddToken(TokenType.DEDENT);
                }
                
                // Validate we landed on a valid indentation level
                if (indentStack.Peek() != spaces)
                {
                    throw new LexerError(
                        string.Format(
                            "Indentation mismatch: dedented to {0} spaces, expected {1}", 
                            spaces, 
                            indentStack.Peek()
                        ), 
                        line
                    );
                }
            }
        }
        
        #endregion
        
        #region Literal Scanning
        
        /// <summary>
        /// Scans a string literal (supports both single and double quotes).
        /// </summary>
        private void ScanString(char quote)
        {
            StringBuilder sb = new StringBuilder();
            
            while (Peek() != quote && !IsAtEnd())
            {
                if (Peek() == '\n') line++;
                
                if (Peek() == '\\')
                {
                    Advance(); // Skip backslash
                    if (IsAtEnd()) break;
                    
                    char escaped = Advance();
                    switch (escaped)
                    {
                        case 'n': sb.Append('\n'); break;
                        case 't': sb.Append('\t'); break;
                        case 'r': sb.Append('\r'); break;
                        case '\\': sb.Append('\\'); break;
                        case '"': sb.Append('"'); break;
                        case '\'': sb.Append('\''); break;
                        default:
                            sb.Append('\\');
                            sb.Append(escaped);
                            break;
                    }
                }
                else
                {
                    sb.Append(Advance());
                }
            }
            
            if (IsAtEnd())
            {
                throw new LexerError("Unterminated string", line);
            }
            
            // Consume closing quote
            Advance();
            
            AddToken(TokenType.STRING, sb.ToString());
        }
        
        /// <summary>
        /// Scans a numeric literal (supports integers and floats).
        /// </summary>
        private void ScanNumber()
        {
            while (IsDigit(Peek())) Advance();
            
            // Look for decimal point
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance(); // Consume '.'
                while (IsDigit(Peek())) Advance();
            }
            
            string text = source.Substring(start, current - start);
            double value = double.Parse(text);
            AddToken(TokenType.NUMBER, value);
        }
        
        /// <summary>
        /// Scans an identifier or keyword.
        /// </summary>
        private void ScanIdentifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();
            
            string text = source.Substring(start, current - start);
            TokenType type;
            
            if (keywords.TryGetValue(text, out type))
            {
                AddToken(type);
            }
            else
            {
                AddToken(TokenType.IDENTIFIER);
            }
        }
        
        #endregion
        
        #region Helper Methods
        
        private char Advance()
        {
            return source[current++];
        }
        
        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (source[current] != expected) return false;
            
            current++;
            return true;
        }
        
        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return source[current];
        }
        
        private char PeekNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }
        
        private bool IsAtEnd()
        {
            return current >= source.Length;
        }
        
        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }
        
        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   c == '_';
        }
        
        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }
        
        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }
        
        private void AddToken(TokenType type, object literal)
        {
            string text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }
        
        #endregion
    }
}