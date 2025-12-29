using System;
using System.Collections.Generic;

namespace LOOPLanguage
{
    /// <summary>
    /// Parses tokens into an Abstract Syntax Tree (AST).
    /// Implements recursive descent parsing with operator precedence.
    /// </summary>
    public class Parser
    {
        #region Fields
        
        private List<Token> tokens;
        private int current;
        
        #endregion
        
        #region Initialization
        
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            this.current = 0;
        }
        
        #endregion
        
        #region Main Parsing Entry
        
        /// <summary>
        /// Parses all tokens into a list of statements.
        /// </summary>
        public List<Stmt> Parse()
        {
            List<Stmt> statements = new List<Stmt>();
            
            while (!IsAtEnd())
            {
                // Skip newlines at top level
                if (Match(TokenType.NEWLINE))
                {
                    continue;
                }
                
                statements.Add(ParseStatement());
            }
            
            return statements;
        }
        
        #endregion
        
        #region Statement Parsing
        
        private Stmt ParseStatement()
        {
            try
            {
                // Compound statements
                if (Match(TokenType.IF)) return ParseIfStatement();
                if (Match(TokenType.WHILE)) return ParseWhileStatement();
                if (Match(TokenType.FOR)) return ParseForStatement();
                if (Match(TokenType.DEF)) return ParseFunctionDef();
                if (Match(TokenType.CLASS)) return ParseClassDef();
                
                // Simple statements
                if (Match(TokenType.RETURN)) return ParseReturnStatement();
                if (Match(TokenType.BREAK)) return new BreakStmt { LineNumber = Previous().LineNumber };
                if (Match(TokenType.CONTINUE)) return new ContinueStmt { LineNumber = Previous().LineNumber };
                if (Match(TokenType.PASS)) return new PassStmt { LineNumber = Previous().LineNumber };
                if (Match(TokenType.GLOBAL)) return ParseGlobalStatement();
                if (Match(TokenType.IMPORT)) return ParseImportStatement();
                
                // Check if it's an assignment or expression statement
                return ParseExpressionOrAssignment();
            }
            catch (Exception e)
            {
                throw new ParseError(e.Message, CurrentToken().LineNumber);
            }
        }
        
        private Stmt ParseIfStatement()
        {
            int line = Previous().LineNumber;
            Expr condition = ParseExpression();
            Consume(TokenType.COLON, "Expected ':' after if condition");
            List<Stmt> thenBranch = ParseSuite();
            
            List<Stmt> elseBranch = null;
            
            // Handle elif and else
            while (Match(TokenType.ELIF))
            {
                Expr elifCondition = ParseExpression();
                Consume(TokenType.COLON, "Expected ':' after elif condition");
                List<Stmt> elifBranch = ParseSuite();
                
                // Convert elif to nested if-else
                if (elseBranch == null)
                {
                    elseBranch = new List<Stmt>();
                }
                
                IfStmt nestedIf = new IfStmt(elifCondition, elifBranch, null);
                nestedIf.LineNumber = line;
                elseBranch.Add(nestedIf);
            }
            
            if (Match(TokenType.ELSE))
            {
                Consume(TokenType.COLON, "Expected ':' after else");
                
                if (elseBranch == null)
                {
                    elseBranch = ParseSuite();
                }
                else
                {
                    // Attach else branch to last elif
                    ((IfStmt)elseBranch[elseBranch.Count - 1]).ElseBranch = ParseSuite();
                }
            }
            
            IfStmt stmt = new IfStmt(condition, thenBranch, elseBranch);
            stmt.LineNumber = line;
            return stmt;
        }
        
        private Stmt ParseWhileStatement()
        {
            int line = Previous().LineNumber;
            Expr condition = ParseExpression();
            Consume(TokenType.COLON, "Expected ':' after while condition");
            List<Stmt> body = ParseSuite();
            
            WhileStmt stmt = new WhileStmt(condition, body);
            stmt.LineNumber = line;
            return stmt;
        }
        
        private Stmt ParseForStatement()
        {
            int line = Previous().LineNumber;
            Token varToken = Consume(TokenType.IDENTIFIER, "Expected variable name in for loop");
            Consume(TokenType.IN, "Expected 'in' in for loop");
            Expr iterable = ParseExpression();
            Consume(TokenType.COLON, "Expected ':' after for clause");
            List<Stmt> body = ParseSuite();
            
            ForStmt stmt = new ForStmt(varToken.Lexeme, iterable, body);
            stmt.LineNumber = line;
            return stmt;
        }
        
        private Stmt ParseFunctionDef()
        {
            int line = Previous().LineNumber;
            Token name = Consume(TokenType.IDENTIFIER, "Expected function name");
            Consume(TokenType.LEFT_PAREN, "Expected '(' after function name");
            
            List<string> parameters = new List<string>();
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    Token param = Consume(TokenType.IDENTIFIER, "Expected parameter name");
                    parameters.Add(param.Lexeme);
                } while (Match(TokenType.COMMA));
            }
            
            Consume(TokenType.RIGHT_PAREN, "Expected ')' after parameters");
            Consume(TokenType.COLON, "Expected ':' after function signature");
            
            List<Stmt> body = ParseSuite();
            
            FunctionDefStmt stmt = new FunctionDefStmt(name.Lexeme, parameters, body);
            stmt.LineNumber = line;
            return stmt;
        }
        
        private Stmt ParseClassDef()
        {
            int line = Previous().LineNumber;
            Token name = Consume(TokenType.IDENTIFIER, "Expected class name");
            Consume(TokenType.COLON, "Expected ':' after class name");
            Consume(TokenType.NEWLINE, "Expected newline after ':'");
            Consume(TokenType.INDENT, "Expected indent after class definition");
            
            List<FunctionDefStmt> methods = new List<FunctionDefStmt>();
            
            while (!Check(TokenType.DEDENT) && !IsAtEnd())
            {
                if (Match(TokenType.NEWLINE)) continue;
                
                if (Match(TokenType.DEF))
                {
                    methods.Add((FunctionDefStmt)ParseFunctionDef());
                }
                else
                {
                    throw new ParseError("Only method definitions allowed in class body", CurrentToken().LineNumber);
                }
            }
            
            Consume(TokenType.DEDENT, "Expected dedent after class body");
            
            ClassDefStmt stmt = new ClassDefStmt(name.Lexeme, methods);
            stmt.LineNumber = line;
            return stmt;
        }
        
        private Stmt ParseReturnStatement()
        {
            int line = Previous().LineNumber;
            Expr value = null;
            
            if (!Check(TokenType.NEWLINE))
            {
                value = ParseExpression();
            }
            
            ReturnStmt stmt = new ReturnStmt(value);
            stmt.LineNumber = line;
            return stmt;
        }
        
        private Stmt ParseGlobalStatement()
        {
            int line = Previous().LineNumber;
            List<string> variables = new List<string>();
            
            do
            {
                Token var = Consume(TokenType.IDENTIFIER, "Expected variable name");
                variables.Add(var.Lexeme);
            } while (Match(TokenType.COMMA));
            
            GlobalStmt stmt = new GlobalStmt(variables);
            stmt.LineNumber = line;
            return stmt;
        }
        
        private Stmt ParseImportStatement()
        {
            int line = Previous().LineNumber;
            Token enumName = Consume(TokenType.IDENTIFIER, "Expected enum name after import");
            
            string memberName = null;
            if (Match(TokenType.DOT))
            {
                Token member = Consume(TokenType.IDENTIFIER, "Expected member name after '.'");
                memberName = member.Lexeme;
            }
            
            ImportStmt stmt = new ImportStmt(enumName.Lexeme, memberName);
            stmt.LineNumber = line;
            return stmt;
        }
        
        private Stmt ParseExpressionOrAssignment()
        {
            int line = CurrentToken().LineNumber;
            
            // Try to parse as assignment first
            if (Check(TokenType.IDENTIFIER))
            {
                int checkpoint = current;
                Token target = Advance();
                
                // Simple assignment
                if (Match(TokenType.EQUAL, TokenType.PLUS_EQUAL, TokenType.MINUS_EQUAL,
                         TokenType.STAR_EQUAL, TokenType.SLASH_EQUAL))
                {
                    string op = Previous().Lexeme;
                    Expr value = ParseExpression();
                    
                    AssignmentStmt stmt = new AssignmentStmt(target.Lexeme, value, op);
                    stmt.LineNumber = line;
                    return stmt;
                }
                
                // Reset and check for index assignment
                current = checkpoint;
            }
            
            // Try index assignment (e.g., list[0] = value)
            Expr expr = ParseExpression();
            
            if (expr is IndexExpr && Match(TokenType.EQUAL))
            {
                IndexExpr indexExpr = (IndexExpr)expr;
                Expr value = ParseExpression();
                
                IndexAssignmentStmt stmt = new IndexAssignmentStmt(indexExpr.Object, indexExpr.Index, value);
                stmt.LineNumber = line;
                return stmt;
            }
            
            // Just an expression statement
            ExpressionStmt stmt2 = new ExpressionStmt(expr);
            stmt2.LineNumber = line;
            return stmt2;
        }
        
        /// <summary>
        /// Parses a suite (block of code after ':').
        /// Can be single-line or multi-line (indented).
        /// </summary>
        private List<Stmt> ParseSuite()
        {
            List<Stmt> statements = new List<Stmt>();
            
            if (Match(TokenType.NEWLINE))
            {
                // Multi-line suite
                Consume(TokenType.INDENT, "Expected indent after ':'");
                
                while (!Check(TokenType.DEDENT) && !IsAtEnd())
                {
                    if (Match(TokenType.NEWLINE)) continue;
                    statements.Add(ParseStatement());
                }
                
                Consume(TokenType.DEDENT, "Expected dedent after suite");
            }
            else
            {
                // Single-line suite
                statements.Add(ParseStatement());
            }
            
            return statements;
        }
        
        #endregion
        
        #region Expression Parsing (Operator Precedence)
        
        private Expr ParseExpression()
        {
            return ParseLambda();
        }
        
        private Expr ParseLambda()
        {
            if (Match(TokenType.LAMBDA))
            {
                int line = Previous().LineNumber;
                List<string> parameters = new List<string>();
                
                // Parse parameters (if any)
                if (!Check(TokenType.COLON))
                {
                    do
                    {
                        Token param = Consume(TokenType.IDENTIFIER, "Expected parameter name");
                        parameters.Add(param.Lexeme);
                    } while (Match(TokenType.COMMA));
                }
                
                Consume(TokenType.COLON, "Expected ':' after lambda parameters");
                
                // Parse body (any expression, including list comprehension)
                Expr body = ParseLogicalOr();
                
                LambdaExpr expr = new LambdaExpr(parameters, body);
                expr.LineNumber = line;
                return expr;
            }
            
            return ParseLogicalOr();
        }
        
        private Expr ParseLogicalOr()
        {
            Expr expr = ParseLogicalAnd();
            
            while (Match(TokenType.OR))
            {
                TokenType op = Previous().Type;
                Expr right = ParseLogicalAnd();
                expr = new BinaryExpr(expr, op, right);
            }
            
            return expr;
        }
        
        private Expr ParseLogicalAnd()
        {
            Expr expr = ParseLogicalNot();
            
            while (Match(TokenType.AND))
            {
                TokenType op = Previous().Type;
                Expr right = ParseLogicalNot();
                expr = new BinaryExpr(expr, op, right);
            }
            
            return expr;
        }
        
        private Expr ParseLogicalNot()
        {
            if (Match(TokenType.NOT))
            {
                TokenType op = Previous().Type;
                Expr operand = ParseLogicalNot();
                return new UnaryExpr(op, operand);
            }
            
            return ParseComparison();
        }
        
        private Expr ParseComparison()
        {
            Expr expr = ParseBitwiseOr();
            
            while (Match(TokenType.EQUAL_EQUAL, TokenType.BANG_EQUAL, TokenType.LESS,
                        TokenType.GREATER, TokenType.LESS_EQUAL, TokenType.GREATER_EQUAL,
                        TokenType.IN, TokenType.IS))
            {
                TokenType op = Previous().Type;
                Expr right = ParseBitwiseOr();
                expr = new BinaryExpr(expr, op, right);
            }
            
            return expr;
        }
        
        private Expr ParseBitwiseOr()
        {
            Expr expr = ParseBitwiseXor();
            
            while (Match(TokenType.PIPE))
            {
                TokenType op = Previous().Type;
                Expr right = ParseBitwiseXor();
                expr = new BinaryExpr(expr, op, right);
            }
            
            return expr;
        }
        
        private Expr ParseBitwiseXor()
        {
            Expr expr = ParseBitwiseAnd();
            
            while (Match(TokenType.CARET))
            {
                TokenType op = Previous().Type;
                Expr right = ParseBitwiseAnd();
                expr = new BinaryExpr(expr, op, right);
            }
            
            return expr;
        }
        
        private Expr ParseBitwiseAnd()
        {
            Expr expr = ParseShift();
            
            while (Match(TokenType.AMPERSAND))
            {
                TokenType op = Previous().Type;
                Expr right = ParseShift();
                expr = new BinaryExpr(expr, op, right);
            }
            
            return expr;
        }
        
        private Expr ParseShift()
        {
            Expr expr = ParseAddition();
            
            while (Match(TokenType.LEFT_SHIFT, TokenType.RIGHT_SHIFT))
            {
                TokenType op = Previous().Type;
                Expr right = ParseAddition();
                expr = new BinaryExpr(expr, op, right);
            }
            
            return expr;
        }
        
        private Expr ParseAddition()
        {
            Expr expr = ParseMultiplication();
            
            while (Match(TokenType.PLUS, TokenType.MINUS))
            {
                TokenType op = Previous().Type;
                Expr right = ParseMultiplication();
                expr = new BinaryExpr(expr, op, right);
            }
            
            return expr;
        }
        
        private Expr ParseMultiplication()
        {
            Expr expr = ParseExponentiation();
            
            while (Match(TokenType.STAR, TokenType.SLASH, TokenType.DOUBLE_SLASH, TokenType.PERCENT))
            {
                TokenType op = Previous().Type;
                Expr right = ParseExponentiation();
                expr = new BinaryExpr(expr, op, right);
            }
            
            return expr;
        }
        
        private Expr ParseExponentiation()
        {
            Expr expr = ParseUnary();
            
            // Right-associative: 2**3**4 = 2**(3**4)
            if (Match(TokenType.DOUBLE_STAR))
            {
                TokenType op = Previous().Type;
                Expr right = ParseExponentiation(); // Recursive for right-associativity
                expr = new BinaryExpr(expr, op, right);
            }
            
            return expr;
        }
        
        private Expr ParseUnary()
        {
            if (Match(TokenType.MINUS, TokenType.PLUS, TokenType.TILDE, TokenType.NOT))
            {
                TokenType op = Previous().Type;
                Expr operand = ParseUnary();
                return new UnaryExpr(op, operand);
            }
            
            return ParsePrimary();
        }
        
        private Expr ParsePrimary()
        {
            int line = CurrentToken().LineNumber;
            
            // Literals
            if (Match(TokenType.TRUE))
            {
                return new LiteralExpr(true) { LineNumber = line };
            }
            
            if (Match(TokenType.FALSE))
            {
                return new LiteralExpr(false) { LineNumber = line };
            }
            
            if (Match(TokenType.NONE))
            {
                return new LiteralExpr(null) { LineNumber = line };
            }
            
            if (Match(TokenType.NUMBER))
            {
                return new LiteralExpr(Previous().Literal) { LineNumber = line };
            }
            
            if (Match(TokenType.STRING))
            {
                return new LiteralExpr(Previous().Literal) { LineNumber = line };
            }
            
            // Identifier or variable
            if (Match(TokenType.IDENTIFIER))
            {
                string name = Previous().Lexeme;
                return ParsePostfix(new VariableExpr(name) { LineNumber = line });
            }
            
            // List literal or list comprehension
            if (Match(TokenType.LEFT_BRACKET))
            {
                return ParseListOrComprehension();
            }
            
            // Tuple or grouped expression
            if (Match(TokenType.LEFT_PAREN))
            {
                return ParseTupleOrGrouped();
            }
            
            // Dictionary
            if (Match(TokenType.LEFT_BRACE))
            {
                return ParseDictionary();
            }
            
            throw new ParseError("Unexpected token in expression", CurrentToken().LineNumber);
        }
        
        private Expr ParsePostfix(Expr expr)
        {
            while (true)
            {
                if (Match(TokenType.LEFT_PAREN))
                {
                    // Function call
                    expr = ParseCall(expr);
                }
                else if (Match(TokenType.LEFT_BRACKET))
                {
                    // Index or slice
                    expr = ParseIndexOrSlice(expr);
                }
                else if (Match(TokenType.DOT))
                {
                    // Member access
                    Token member = Consume(TokenType.IDENTIFIER, "Expected member name after '.'");
                    expr = new MemberAccessExpr(expr, member.Lexeme);
                }
                else
                {
                    break;
                }
            }
            
            return expr;
        }
        
        private Expr ParseCall(Expr callee)
        {
            List<Expr> arguments = new List<Expr>();
            
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    arguments.Add(ParseExpression());
                } while (Match(TokenType.COMMA));
            }
            
            Consume(TokenType.RIGHT_PAREN, "Expected ')' after arguments");
            
            return new CallExpr(callee, arguments);
        }
        
        private Expr ParseIndexOrSlice(Expr obj)
        {
            Expr start = null;
            Expr stop = null;
            Expr step = null;
            
            if (!Check(TokenType.COLON))
            {
                start = ParseExpression();
            }
            
            if (Match(TokenType.COLON))
            {
                // It's a slice
                if (!Check(TokenType.COLON) && !Check(TokenType.RIGHT_BRACKET))
                {
                    stop = ParseExpression();
                }
                
                if (Match(TokenType.COLON))
                {
                    if (!Check(TokenType.RIGHT_BRACKET))
                    {
                        step = ParseExpression();
                    }
                }
                
                Consume(TokenType.RIGHT_BRACKET, "Expected ']' after slice");
                return new SliceExpr(obj, start, stop, step);
            }
            
            // It's an index
            Consume(TokenType.RIGHT_BRACKET, "Expected ']' after index");
            return new IndexExpr(obj, start);
        }
        
        private Expr ParseListOrComprehension()
        {
            if (Check(TokenType.RIGHT_BRACKET))
            {
                Advance();
                return new ListExpr(new List<Expr>());
            }
            
            Expr first = ParseExpression();
            
            // Check for list comprehension
            if (Match(TokenType.FOR))
            {
                Token var = Consume(TokenType.IDENTIFIER, "Expected variable in list comprehension");
                Consume(TokenType.IN, "Expected 'in' in list comprehension");
                Expr iterable = ParseExpression();
                
                Expr condition = null;
                if (Match(TokenType.IF))
                {
                    condition = ParseExpression();
                }
                
                Consume(TokenType.RIGHT_BRACKET, "Expected ']' after list comprehension");
                
                return new ListCompExpr(first, var.Lexeme, iterable, condition);
            }
            
            // Regular list
            List<Expr> elements = new List<Expr> { first };
            
            while (Match(TokenType.COMMA))
            {
                if (Check(TokenType.RIGHT_BRACKET)) break;
                elements.Add(ParseExpression());
            }
            
            Consume(TokenType.RIGHT_BRACKET, "Expected ']' after list elements");
            
            return new ListExpr(elements);
        }
        
        private Expr ParseTupleOrGrouped()
        {
            if (Check(TokenType.RIGHT_PAREN))
            {
                Advance();
                return new TupleExpr(new List<Expr>()); // Empty tuple
            }
            
            Expr first = ParseExpression();
            
            if (Match(TokenType.COMMA))
            {
                // It's a tuple
                List<Expr> elements = new List<Expr> { first };
                
                if (!Check(TokenType.RIGHT_PAREN))
                {
                    do
                    {
                        if (Check(TokenType.RIGHT_PAREN)) break;
                        elements.Add(ParseExpression());
                    } while (Match(TokenType.COMMA));
                }
                
                Consume(TokenType.RIGHT_PAREN, "Expected ')' after tuple");
                return new TupleExpr(elements);
            }
            
            // It's a grouped expression
            Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression");
            return first;
        }
        
        private Expr ParseDictionary()
        {
            List<Expr> keys = new List<Expr>();
            List<Expr> values = new List<Expr>();
            
            if (!Check(TokenType.RIGHT_BRACE))
            {
                do
                {
                    Expr key = ParseExpression();
                    Consume(TokenType.COLON, "Expected ':' after dictionary key");
                    Expr value = ParseExpression();
                    
                    keys.Add(key);
                    values.Add(value);
                } while (Match(TokenType.COMMA));
            }
            
            Consume(TokenType.RIGHT_BRACE, "Expected '}' after dictionary");
            
            return new DictExpr(keys, values);
        }
        
        #endregion
        
        #region Helper Methods
        
        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }
        
        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return CurrentToken().Type == type;
        }
        
        private Token Advance()
        {
            if (!IsAtEnd()) current++;
            return Previous();
        }
        
        private Token CurrentToken()
        {
            return tokens[current];
        }
        
        private Token Previous()
        {
            return tokens[current - 1];
        }
        
        private bool IsAtEnd()
        {
            return CurrentToken().Type == TokenType.EOF;
        }
        
        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw new ParseError(message, CurrentToken().LineNumber);
        }
        
        #endregion
    }
}