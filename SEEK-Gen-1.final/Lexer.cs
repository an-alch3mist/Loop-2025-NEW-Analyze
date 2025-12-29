using System;
using System.Collections.Generic;
using System.Text;

namespace LoopLanguage
{
    /// <summary>
    /// Lexical analyzer that converts source code into tokens.
    /// Handles Python-style indentation, comments, and all operators/keywords.
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
        
        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
        {
            { "if", TokenType.IF },
            { "elif", TokenType.ELIF },
            { "else", TokenType.ELSE },
            { "while", TokenType.WHILE },
            { "for", TokenType.FOR },
            { "def", TokenType.DEF },
            { "return", TokenType.RETURN },
            { "class", TokenType.CLASS },
            { "break", TokenType.BREAK },
            { "continue", TokenType.CONTINUE },
            { "pass", TokenType.PASS },
            { "global", TokenType.GLOBAL },
            { "lambda", TokenType.LAMBDA },
            { "import", TokenType.IMPORT },
            { "and", TokenType.AND },
            { "or", TokenType.OR },
            { "not", TokenType.NOT },
            { "in", TokenType.IN },
            { "is", TokenType.IS },
            { "True", TokenType.TRUE },
            { "False", TokenType.FALSE },
            { "None", TokenType.NONE }
        };
        
        #endregion
        
        #region Constructor
        
        public Lexer()
        {
            tokens = new List<Token>();
            indentStack = new Stack<int>();
            indentStack.Push(0);
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Tokenizes the input source code and returns list of tokens
        /// </summary>
        public List<Token> Tokenize(string input)
        {
            source = ValidateAndClean(input);
            tokens = new List<Token>();
            start = 0;
            current = 0;
            line = 1;
            indentStack = new Stack<int>();
            indentStack.Push(0);
            
            ProcessIndentation();
            
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }
            
            // Emit remaining DEDENTs
            while (indentStack.Count > 1)
            {
                indentStack.Pop();
                AddToken(TokenType.DEDENT);
            }
            
            AddToken(TokenType.NEWLINE);
            AddToken(TokenType.EOF);
            
            return tokens;
        }
        
        #endregion
        
        #region Input Validation
        
        /// <summary>
        /// Normalizes line endings, converts tabs to spaces, removes invalid characters
        /// </summary>
        private string ValidateAndClean(string input)
        {
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
        
        #region Indentation Processing
        
        private void ProcessIndentation()
        {
            if (IsAtEnd()) return;
            
            // Count leading spaces
            int spaces = 0;
            while (!IsAtEnd() && Peek() == ' ')
            {
                spaces++;
                Advance();
            }
            
            // Skip empty lines and comments
            if (IsAtEnd() || Peek() == '\n' || Peek() == '#' || (Peek() == '/' && PeekNext() == '/'))
            {
                return;
            }
            
            int currentIndent = indentStack.Peek();
            
            if (spaces > currentIndent)
            {
                indentStack.Push(spaces);
                AddToken(TokenType.INDENT);
            }
            else if (spaces < currentIndent)
            {
                while (indentStack.Count > 0 && indentStack.Peek() > spaces)
                {
                    indentStack.Pop();
                    AddToken(TokenType.DEDENT);
                }
                
                if (indentStack.Peek() != spaces)
                {
                    throw new LexerError($"Line {line}: Indentation mismatch");
                }
            }
        }
        
        #endregion
        
        #region Token Scanning
        
        private void ScanToken()
        {
            char c = Advance();
            
            switch (c)
            {
                case '\n':
                    AddToken(TokenType.NEWLINE);
                    line++;
                    ProcessIndentation();
                    break;
                case ' ':
                case '\t':
                case '\r':
                    // Whitespace (already handled)
                    break;
                case '#':
                    // Python-style comment
                    while (!IsAtEnd() && Peek() != '\n') Advance();
                    break;
                case '/':
                    if (Match('/'))
                    {
                        // C-style comment
                        while (!IsAtEnd() && Peek() != '\n') Advance();
                    }
                    else if (Match('='))
                    {
                        AddToken(TokenType.SLASH_EQUAL);
                    }
                    else if (Match('/'))
                    {
                        AddToken(TokenType.DOUBLE_SLASH);
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
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case ':': AddToken(TokenType.COLON); break;
                case '~': AddToken(TokenType.TILDE); break;
                case '^': AddToken(TokenType.CARET); break;
                
                // Operators with possible compound forms
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
                case '%':
                    AddToken(TokenType.PERCENT);
                    break;
                case '!':
                    if (Match('='))
                    {
                        AddToken(TokenType.BANG_EQUAL);
                    }
                    else
                    {
                        throw new LexerError($"Line {line}: Unexpected character '!'");
                    }
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
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
                        throw new LexerError($"Line {line}: Unexpected character '{c}'");
                    }
                    break;
            }
        }
        
        #endregion
        
        #region Literal Scanning
        
        private void ScanString(char quote)
        {
            StringBuilder sb = new StringBuilder();
            
            while (!IsAtEnd() && Peek() != quote)
            {
                if (Peek() == '\n')
                {
                    line++;
                }
                else if (Peek() == '\\')
                {
                    Advance();
                    if (!IsAtEnd())
                    {
                        char escaped = Advance();
                        switch (escaped)
                        {
                            case 'n': sb.Append('\n'); break;
                            case 't': sb.Append('\t'); break;
                            case 'r': sb.Append('\r'); break;
                            case '\\': sb.Append('\\'); break;
                            case '\'': sb.Append('\''); break;
                            case '"': sb.Append('"'); break;
                            default: sb.Append(escaped); break;
                        }
                    }
                    continue;
                }
                sb.Append(Advance());
            }
            
            if (IsAtEnd())
            {
                throw new LexerError($"Line {line}: Unterminated string");
            }
            
            Advance(); // Closing quote
            AddToken(TokenType.STRING, sb.ToString());
        }
        
        private void ScanNumber()
        {
            while (IsDigit(Peek())) Advance();
            
            // Handle decimal point
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance(); // Consume '.'
                while (IsDigit(Peek())) Advance();
            }
            
            string numStr = source.Substring(start, current - start);
            double value = double.Parse(numStr);
            AddToken(TokenType.NUMBER, value);
        }
        
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
