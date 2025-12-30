using System;
using System.Collections.Generic;

namespace LoopLanguage
{
	/// <summary>
	/// Recursive descent parser that builds an AST from tokens.
	/// Implements Python-style grammar with proper precedence and indentation.
	/// </summary>
	public class Parser
	{
		#region Fields

		private List<Token> tokens;
		private int current;

		#endregion

		#region Constructor

		public Parser()
		{
			tokens = new List<Token>();
			current = 0;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Parses list of tokens into an AST
		/// </summary>
		public List<Stmt> Parse(List<Token> tokenList)
		{
			tokens = tokenList;
			current = 0;

			List<Stmt> statements = new List<Stmt>();

			while (!IsAtEnd())
			{
				if (Match(TokenType.NEWLINE))
				{
					continue; // Skip blank lines
				}
				statements.Add(Statement());
			}

			return statements;
		}

		#endregion

		#region Statement Parsing

		private Stmt Statement()
		{
			try
			{
				if (Match(TokenType.IF)) return IfStatement();
				if (Match(TokenType.WHILE)) return WhileStatement();
				if (Match(TokenType.FOR)) return ForStatement();
				if (Match(TokenType.DEF)) return FunctionDef();
				if (Match(TokenType.CLASS)) return ClassDef();
				if (Match(TokenType.RETURN)) return ReturnStatement();
				if (Match(TokenType.BREAK)) return BreakStatement();
				if (Match(TokenType.CONTINUE)) return ContinueStatement();
				if (Match(TokenType.PASS)) return PassStatement();
				if (Match(TokenType.GLOBAL)) return GlobalStatement();
				if (Match(TokenType.IMPORT)) return ImportStatement();

				return SimpleStatement();
			}
			catch (Exception e)
			{
				throw new ParserError($"Parse error at line {Previous().LineNumber}: {e.Message}");
			}
		}

		private Stmt SimpleStatement()
		{
			// Try assignment first
			if (Check(TokenType.IDENTIFIER))
			{
				int savedPos = current;
				Token id = Advance();

				// Check for assignment operators
				if (Match(TokenType.EQUAL, TokenType.PLUS_EQUAL, TokenType.MINUS_EQUAL,
						  TokenType.STAR_EQUAL, TokenType.SLASH_EQUAL))
				{
					string op = Previous().Lexeme;
					Expr value = Expression();
					ConsumeNewline();
					return new AssignmentStmt(id.Lexeme, value, op);
				}

				// Check for subscript/member assignment
				if (Match(TokenType.LEFT_BRACKET, TokenType.DOT))
				{
					current = savedPos;
					Expr target = Expression();

					if (target is IndexExpr)
					{
						IndexExpr indexExpr = (IndexExpr)target;
						if (Match(TokenType.EQUAL, TokenType.PLUS_EQUAL, TokenType.MINUS_EQUAL,
								  TokenType.STAR_EQUAL, TokenType.SLASH_EQUAL))
						{
							string op = Previous().Lexeme;
							Expr value = Expression();
							ConsumeNewline();
							return new SubscriptAssignmentStmt(indexExpr.Object, indexExpr.Index, value, op);
						}
					}
					else if (target is MemberAccessExpr)
					{
						MemberAccessExpr memberExpr = (MemberAccessExpr)target;
						if (Match(TokenType.EQUAL, TokenType.PLUS_EQUAL, TokenType.MINUS_EQUAL,
								  TokenType.STAR_EQUAL, TokenType.SLASH_EQUAL))
						{
							string op = Previous().Lexeme;
							Expr value = Expression();
							ConsumeNewline();
							return new MemberAssignmentStmt(memberExpr.Object, memberExpr.Member, value, op);
						}
					}
				}

				// Restore position and parse as expression
				current = savedPos;
			}

			// Expression statement
			Expr expr = Expression();
			ConsumeNewline();
			return new ExpressionStmt(expr);
		}

		private Stmt IfStatement()
		{
			Expr condition = Expression();
			Consume(TokenType.COLON, "Expected ':' after if condition");
			ConsumeNewline();

			Consume(TokenType.INDENT, "Expected indented block after if");
			List<Stmt> thenBranch = Block();

			List<Stmt> elseBranch = null;

			while (Match(TokenType.ELIF))
			{
				Expr elifCondition = Expression();
				Consume(TokenType.COLON, "Expected ':' after elif condition");
				ConsumeNewline();

				Consume(TokenType.INDENT, "Expected indented block after elif");
				List<Stmt> elifBranch = Block();

				// Chain elif as nested if-else
				if (elseBranch == null)
				{
					elseBranch = new List<Stmt>();
				}
				elseBranch.Add(new IfStmt(elifCondition, elifBranch, null));
			}

			if (Match(TokenType.ELSE))
			{
				Consume(TokenType.COLON, "Expected ':' after else");
				ConsumeNewline();

				Consume(TokenType.INDENT, "Expected indented block after else");

				if (elseBranch == null)
				{
					elseBranch = Block();
				}
				else
				{
					// Append to existing elif chain
					List<Stmt> finalElse = Block();
					IfStmt lastElif = (IfStmt)elseBranch[elseBranch.Count - 1];
					lastElif.ElseBranch = finalElse;
				}
			}

			return new IfStmt(condition, thenBranch, elseBranch);
		}

		private Stmt WhileStatement()
		{
			Expr condition = Expression();
			Consume(TokenType.COLON, "Expected ':' after while condition");
			ConsumeNewline();

			Consume(TokenType.INDENT, "Expected indented block after while");
			List<Stmt> body = Block();

			return new WhileStmt(condition, body);
		}

		private Stmt ForStatement()
		{
			Token variable = Consume(TokenType.IDENTIFIER, "Expected variable name after 'for'");
			Consume(TokenType.IN, "Expected 'in' in for loop");
			Expr iterable = Expression();
			Consume(TokenType.COLON, "Expected ':' after for clause");
			ConsumeNewline();

			Consume(TokenType.INDENT, "Expected indented block after for");
			List<Stmt> body = Block();

			return new ForStmt(variable.Lexeme, iterable, body);
		}

		private Stmt FunctionDef()
		{
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
			ConsumeNewline();

			Consume(TokenType.INDENT, "Expected indented block after function definition");
			List<Stmt> body = Block();

			return new FunctionDefStmt(name.Lexeme, parameters, body);
		}

		private Stmt ClassDef()
		{
			Token name = Consume(TokenType.IDENTIFIER, "Expected class name");
			Consume(TokenType.COLON, "Expected ':' after class name");
			ConsumeNewline();

			Consume(TokenType.INDENT, "Expected indented block after class definition");

			List<FunctionDefStmt> methods = new List<FunctionDefStmt>();

			while (!Check(TokenType.DEDENT) && !IsAtEnd())
			{
				if (Match(TokenType.NEWLINE)) continue;

				if (Match(TokenType.DEF))
				{
					methods.Add((FunctionDefStmt)FunctionDef());
				}
				else
				{
					throw new ParserError("Only method definitions allowed in class body");
				}
			}

			Consume(TokenType.DEDENT, "Expected dedent after class body");

			return new ClassDefStmt(name.Lexeme, methods);
		}

		private Stmt ReturnStatement()
		{
			if (Match(TokenType.NEWLINE) || IsAtEnd())
			{
				return new ReturnStmt(null);
			}

			Expr value = Expression();
			ConsumeNewline();
			return new ReturnStmt(value);
		}

		private Stmt BreakStatement()
		{
			ConsumeNewline();
			return new BreakStmt();
		}

		private Stmt ContinueStatement()
		{
			ConsumeNewline();
			return new ContinueStmt();
		}

		private Stmt PassStatement()
		{
			ConsumeNewline();
			return new PassStmt();
		}

		private Stmt GlobalStatement()
		{
			List<string> variables = new List<string>();

			do
			{
				Token variable = Consume(TokenType.IDENTIFIER, "Expected variable name");
				variables.Add(variable.Lexeme);
			} while (Match(TokenType.COMMA));

			ConsumeNewline();
			return new GlobalStmt(variables);
		}

		private Stmt ImportStatement()
		{
			Token enumName = Consume(TokenType.IDENTIFIER, "Expected enum name after 'import'");

			string memberName = null;
			if (Match(TokenType.DOT))
			{
				Token member = Consume(TokenType.IDENTIFIER, "Expected member name after '.'");
				memberName = member.Lexeme;
			}

			ConsumeNewline();
			return new ImportStmt(enumName.Lexeme, memberName);
		}

		private List<Stmt> Block()
		{
			List<Stmt> statements = new List<Stmt>();

			while (!Check(TokenType.DEDENT) && !IsAtEnd())
			{
				if (Match(TokenType.NEWLINE)) continue;
				statements.Add(Statement());
			}

			Consume(TokenType.DEDENT, "Expected dedent");
			return statements;
		}

		#endregion

		#region Expression Parsing

		private Expr Expression()
		{
			return Lambda();
		}

		private Expr Lambda()
		{
			if (Match(TokenType.LAMBDA))
			{
				List<string> parameters = new List<string>();

				// Parameters (optional)
				if (!Check(TokenType.COLON))
				{
					do
					{
						Token param = Consume(TokenType.IDENTIFIER, "Expected parameter name");
						parameters.Add(param.Lexeme);
					} while (Match(TokenType.COMMA));
				}

				Consume(TokenType.COLON, "Expected ':' after lambda parameters");

				// Lambda body can be any expression (including list comprehension)
				Expr body = Conditional();

				return new LambdaExpr(parameters, body);
			}

			return Conditional();
		}

		private Expr Conditional()
		{
			Expr expr = LogicalOr();

			if (Match(TokenType.IF))
			{
				Expr condition = LogicalOr();
				Consume(TokenType.ELSE, "Expected 'else' in conditional expression");
				Expr elseExpr = Conditional();

				return new ConditionalExpr(condition, expr, elseExpr);
			}

			return expr;
		}

		private Expr LogicalOr()
		{
			Expr expr = LogicalAnd();

			while (Match(TokenType.OR))
			{
				TokenType op = Previous().Type;
				Expr right = LogicalAnd();
				expr = new BinaryExpr(expr, op, right);
			}

			return expr;
		}

		private Expr LogicalAnd()
		{
			Expr expr = LogicalNot();

			while (Match(TokenType.AND))
			{
				TokenType op = Previous().Type;
				Expr right = LogicalNot();
				expr = new BinaryExpr(expr, op, right);
			}

			return expr;
		}

		private Expr LogicalNot()
		{
			if (Match(TokenType.NOT))
			{
				TokenType op = Previous().Type;
				Expr operand = LogicalNot();
				return new UnaryExpr(op, operand);
			}

			return Comparison();
		}

		private Expr Comparison()
		{
			Expr expr = BitwiseOr();

			while (Match(TokenType.EQUAL_EQUAL, TokenType.BANG_EQUAL,
						 TokenType.LESS, TokenType.GREATER,
						 TokenType.LESS_EQUAL, TokenType.GREATER_EQUAL,
						 TokenType.IN, TokenType.IS))
			{
				TokenType op = Previous().Type;
				Expr right = BitwiseOr();
				expr = new BinaryExpr(expr, op, right);
			}

			return expr;
		}

		private Expr BitwiseOr()
		{
			Expr expr = BitwiseXor();

			while (Match(TokenType.PIPE))
			{
				TokenType op = Previous().Type;
				Expr right = BitwiseXor();
				expr = new BinaryExpr(expr, op, right);
			}

			return expr;
		}

		private Expr BitwiseXor()
		{
			Expr expr = BitwiseAnd();

			while (Match(TokenType.CARET))
			{
				TokenType op = Previous().Type;
				Expr right = BitwiseAnd();
				expr = new BinaryExpr(expr, op, right);
			}

			return expr;
		}

		private Expr BitwiseAnd()
		{
			Expr expr = Shift();

			while (Match(TokenType.AMPERSAND))
			{
				TokenType op = Previous().Type;
				Expr right = Shift();
				expr = new BinaryExpr(expr, op, right);
			}

			return expr;
		}

		private Expr Shift()
		{
			Expr expr = Addition();

			while (Match(TokenType.LEFT_SHIFT, TokenType.RIGHT_SHIFT))
			{
				TokenType op = Previous().Type;
				Expr right = Addition();
				expr = new BinaryExpr(expr, op, right);
			}

			return expr;
		}

		private Expr Addition()
		{
			Expr expr = Multiplication();

			while (Match(TokenType.PLUS, TokenType.MINUS))
			{
				TokenType op = Previous().Type;
				Expr right = Multiplication();
				expr = new BinaryExpr(expr, op, right);
			}

			return expr;
		}

		private Expr Multiplication()
		{
			Expr expr = Exponentiation();

			while (Match(TokenType.STAR, TokenType.SLASH, TokenType.DOUBLE_SLASH, TokenType.PERCENT))
			{
				TokenType op = Previous().Type;
				Expr right = Exponentiation();
				expr = new BinaryExpr(expr, op, right);
			}

			return expr;
		}

		private Expr Exponentiation()
		{
			Expr expr = Unary();

			// Right-associative: 2**3**4 = 2**(3**4)
			if (Match(TokenType.DOUBLE_STAR))
			{
				TokenType op = Previous().Type;
				Expr right = Exponentiation(); // Recursive for right-associativity
				expr = new BinaryExpr(expr, op, right);
			}

			return expr;
		}

		private Expr Unary()
		{
			if (Match(TokenType.MINUS, TokenType.PLUS, TokenType.TILDE, TokenType.NOT))
			{
				TokenType op = Previous().Type;
				Expr operand = Unary();
				return new UnaryExpr(op, operand);
			}

			return Primary();
		}

		// ============================================
		// FILE: Parser.cs
		// Replace the Primary() method (around line 477)
		// This fixes multi-line list parsing with comments
		// ============================================
		private Expr Primary()
		{
			if (Match(TokenType.TRUE)) return new LiteralExpr(true);
			if (Match(TokenType.FALSE)) return new LiteralExpr(false);
			if (Match(TokenType.NONE)) return new LiteralExpr(null);

			if (Match(TokenType.NUMBER))
			{
				return new LiteralExpr(Previous().Literal);
			}

			if (Match(TokenType.STRING))
			{
				return new LiteralExpr(Previous().Literal);
			}

			if (Match(TokenType.IDENTIFIER))
			{
				string name = Previous().Lexeme;
				return Postfix(new VariableExpr(name));
			}

			if (Match(TokenType.LEFT_PAREN))
			{
				// Could be grouped expression or tuple
				// Skip indentation tokens (Python ignores indentation inside parentheses)
				while (Match(TokenType.NEWLINE, TokenType.INDENT, TokenType.DEDENT)) { }

				if (Match(TokenType.RIGHT_PAREN))
				{
					return new TupleExpr(new List<Expr>());
				}

				Expr first = Expression();

				if (Match(TokenType.COMMA))
				{
					// Tuple
					List<Expr> elements = new List<Expr> { first };

					if (!Check(TokenType.RIGHT_PAREN))
					{
						do
						{
							// Skip newlines and indentation inside tuples
							// Python ignores indentation inside parentheses
							while (Match(TokenType.NEWLINE, TokenType.INDENT, TokenType.DEDENT)) { }

							if (Check(TokenType.RIGHT_PAREN)) break;
							elements.Add(Expression());
						} while (Match(TokenType.COMMA));
					}

					// Skip indentation before closing paren
					while (Match(TokenType.NEWLINE, TokenType.INDENT, TokenType.DEDENT)) { }

					Consume(TokenType.RIGHT_PAREN, "Expected ')' after tuple");
					return new TupleExpr(elements);
				}

				// Skip indentation before closing paren
				while (Match(TokenType.NEWLINE, TokenType.INDENT, TokenType.DEDENT)) { }

				Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression");
				return Postfix(first);
			}

			if (Match(TokenType.LEFT_BRACKET))
			{
				// List or list comprehension

				// Skip newlines and indentation tokens after opening bracket
				// (Python ignores indentation inside brackets)
				while (Match(TokenType.NEWLINE, TokenType.INDENT, TokenType.DEDENT)) { }

				if (Match(TokenType.RIGHT_BRACKET))
				{
					return new ListExpr(new List<Expr>());
				}

				Expr first = Expression();

				// Check for list comprehension
				if (Match(TokenType.FOR))
				{
					Token variable = Consume(TokenType.IDENTIFIER, "Expected variable in list comprehension");
					Consume(TokenType.IN, "Expected 'in' in list comprehension");

					Expr iterable = LogicalOr();

					Expr condition = null;
					if (Match(TokenType.IF))
					{
						condition = LogicalOr();
					}

					// Skip indentation before closing bracket
					while (Match(TokenType.NEWLINE, TokenType.INDENT, TokenType.DEDENT)) { }

					Consume(TokenType.RIGHT_BRACKET, "Expected ']' after list comprehension");
					return new ListCompExpr(first, variable.Lexeme, iterable, condition);
				}

				// Regular list
				List<Expr> elements = new List<Expr> { first };

				while (Match(TokenType.COMMA))
				{
					// Skip newlines and indentation after comma (for multi-line lists)
					// Python ignores indentation inside brackets
					while (Match(TokenType.NEWLINE, TokenType.INDENT, TokenType.DEDENT)) { }

					if (Check(TokenType.RIGHT_BRACKET)) break;
					elements.Add(Expression());
				}

				// Skip newlines and indentation before closing bracket
				while (Match(TokenType.NEWLINE, TokenType.INDENT, TokenType.DEDENT)) { }

				Consume(TokenType.RIGHT_BRACKET, "Expected ']' after list");
				return new ListExpr(elements);
			}

			throw new ParserError($"Unexpected token: {Peek().Lexeme}");
		}

		private Expr Postfix(Expr expr)
		{
			while (true)
			{
				if (Match(TokenType.LEFT_PAREN))
				{
					// Function call
					List<Expr> arguments = new List<Expr>();
					Dictionary<string, Expr> kwargs = new Dictionary<string, Expr>();

					if (!Check(TokenType.RIGHT_PAREN))
					{
						do
						{
							// Check for keyword argument: IDENTIFIER = Expression
							if (Check(TokenType.IDENTIFIER))
							{
								int savedPos = current;
								Token id = Advance();
								if (Match(TokenType.EQUAL))
								{
									// It's a keyword argument
									kwargs[id.Lexeme] = Expression();
									continue;
								}
								// Not a kwarg, restore and parse as positional
								current = savedPos;
							}
							arguments.Add(Expression());
						} while (Match(TokenType.COMMA));
					}

					// ★ FIX: Consume the closing parenthesis BEFORE creating CallExpr
					Consume(TokenType.RIGHT_PAREN, "Expected ')' after arguments");
					expr = new CallExpr(expr, arguments, kwargs);
				}
				else if (Match(TokenType.LEFT_BRACKET))
				{
					// Indexing or slicing
					Expr start = null;
					Expr stop = null;
					Expr step = null;

					if (!Check(TokenType.COLON))
					{
						start = Expression();
					}

					if (Match(TokenType.COLON))
					{
						// Slicing
						if (!Check(TokenType.COLON) && !Check(TokenType.RIGHT_BRACKET))
						{
							stop = Expression();
						}

						if (Match(TokenType.COLON))
						{
							if (!Check(TokenType.RIGHT_BRACKET))
							{
								step = Expression();
							}
						}

						Consume(TokenType.RIGHT_BRACKET, "Expected ']' after slice");
						expr = new SliceExpr(expr, start, stop, step);
					}
					else
					{
						// Indexing
						Consume(TokenType.RIGHT_BRACKET, "Expected ']' after index");
						expr = new IndexExpr(expr, start);
					}
				}
				else if (Match(TokenType.DOT))
				{
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
			return Peek().Type == type;
		}

		private Token Advance()
		{
			if (!IsAtEnd()) current++;
			return Previous();
		}

		private bool IsAtEnd()
		{
			return Peek().Type == TokenType.EOF;
		}

		private Token Peek()
		{
			return tokens[current];
		}

		private Token Previous()
		{
			return tokens[current - 1];
		}

		private Token Consume(TokenType type, string message)
		{
			if (Check(type)) return Advance();

			throw new ParserError($"{message} at line {Peek().LineNumber}, got '{Peek().Lexeme}'");
		}

		private void ConsumeNewline()
		{
			if (!Match(TokenType.NEWLINE) && !IsAtEnd())
			{
				throw new ParserError($"Expected newline at line {Peek().LineNumber}");
			}
		}

		#endregion
	}
}