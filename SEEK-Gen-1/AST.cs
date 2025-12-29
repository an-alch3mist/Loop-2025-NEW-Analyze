using System.Collections.Generic;

namespace LoopLanguage
{
    #region Base Classes
    
    /// <summary>
    /// Base class for all AST nodes
    /// </summary>
    public abstract class ASTNode { }
    
    /// <summary>
    /// Base class for statement nodes (do not return values)
    /// </summary>
    public abstract class Stmt : ASTNode { }
    
    /// <summary>
    /// Base class for expression nodes (evaluate to values)
    /// </summary>
    public abstract class Expr : ASTNode { }
    
    #endregion
    
    #region Statement Classes
    
    /// <summary>
    /// Statement that wraps an expression (e.g., function call as statement)
    /// </summary>
    public class ExpressionStmt : Stmt
    {
        public Expr Expression;
        
        public ExpressionStmt(Expr expression)
        {
            Expression = expression;
        }
    }
    
    /// <summary>
    /// Variable assignment statement: x = 5, x += 2, etc.
    /// </summary>
    public class AssignmentStmt : Stmt
    {
        public string Target;
        public Expr Value;
        public string Operator; // "=", "+=", "-=", "*=", "/="
        
        public AssignmentStmt(string target, Expr value, string op)
        {
            Target = target;
            Value = value;
            Operator = op;
        }
    }
    
    /// <summary>
    /// Subscript assignment: list[0] = value, dict[key] = value
    /// </summary>
    public class SubscriptAssignmentStmt : Stmt
    {
        public Expr Object;
        public Expr Index;
        public Expr Value;
        public string Operator; // "=", "+=", etc.
        
        public SubscriptAssignmentStmt(Expr obj, Expr index, Expr value, string op)
        {
            Object = obj;
            Index = index;
            Value = value;
            Operator = op;
        }
    }
    
    /// <summary>
    /// Member assignment: obj.field = value
    /// </summary>
    public class MemberAssignmentStmt : Stmt
    {
        public Expr Object;
        public string Member;
        public Expr Value;
        public string Operator;
        
        public MemberAssignmentStmt(Expr obj, string member, Expr value, string op)
        {
            Object = obj;
            Member = member;
            Value = value;
            Operator = op;
        }
    }
    
    /// <summary>
    /// If-elif-else statement with condition branches
    /// </summary>
    public class IfStmt : Stmt
    {
        public Expr Condition;
        public List<Stmt> ThenBranch;
        public List<Stmt> ElseBranch; // Can be null
        
        public IfStmt(Expr condition, List<Stmt> thenBranch, List<Stmt> elseBranch = null)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }
    }
    
    /// <summary>
    /// While loop statement
    /// </summary>
    public class WhileStmt : Stmt
    {
        public Expr Condition;
        public List<Stmt> Body;
        
        public WhileStmt(Expr condition, List<Stmt> body)
        {
            Condition = condition;
            Body = body;
        }
    }
    
    /// <summary>
    /// For loop statement: for var in iterable
    /// </summary>
    public class ForStmt : Stmt
    {
        public string Variable;
        public Expr Iterable;
        public List<Stmt> Body;
        
        public ForStmt(string variable, Expr iterable, List<Stmt> body)
        {
            Variable = variable;
            Iterable = iterable;
            Body = body;
        }
    }
    
    /// <summary>
    /// Function definition statement
    /// </summary>
    public class FunctionDefStmt : Stmt
    {
        public string Name;
        public List<string> Parameters;
        public List<Stmt> Body;
        
        public FunctionDefStmt(string name, List<string> parameters, List<Stmt> body)
        {
            Name = name;
            Parameters = parameters;
            Body = body;
        }
    }
    
    /// <summary>
    /// Class definition statement
    /// </summary>
    public class ClassDefStmt : Stmt
    {
        public string Name;
        public List<FunctionDefStmt> Methods;
        
        public ClassDefStmt(string name, List<FunctionDefStmt> methods)
        {
            Name = name;
            Methods = methods;
        }
    }
    
    /// <summary>
    /// Return statement with optional value
    /// </summary>
    public class ReturnStmt : Stmt
    {
        public Expr Value; // Can be null for 'return' without value
        
        public ReturnStmt(Expr value = null)
        {
            Value = value;
        }
    }
    
    /// <summary>
    /// Break statement (exits loop)
    /// </summary>
    public class BreakStmt : Stmt { }
    
    /// <summary>
    /// Continue statement (skips to next iteration)
    /// </summary>
    public class ContinueStmt : Stmt { }
    
    /// <summary>
    /// Pass statement (no operation)
    /// </summary>
    public class PassStmt : Stmt { }
    
    /// <summary>
    /// Global variable declaration: global x, y
    /// </summary>
    public class GlobalStmt : Stmt
    {
        public List<string> Variables;
        
        public GlobalStmt(List<string> variables)
        {
            Variables = variables;
        }
    }
    
    /// <summary>
    /// Import statement: import Items.Grass
    /// </summary>
    public class ImportStmt : Stmt
    {
        public string EnumName;  // e.g., "Items"
        public string MemberName; // e.g., "Grass" (can be null)
        
        public ImportStmt(string enumName, string memberName = null)
        {
            EnumName = enumName;
            MemberName = memberName;
        }
    }
    
    #endregion
    
    #region Expression Classes
    
    /// <summary>
    /// Binary operation: left op right (e.g., a + b, x == y)
    /// </summary>
    public class BinaryExpr : Expr
    {
        public Expr Left;
        public TokenType Operator;
        public Expr Right;
        
        public BinaryExpr(Expr left, TokenType op, Expr right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }
    
    /// <summary>
    /// Unary operation: op operand (e.g., -x, not y, ~z)
    /// </summary>
    public class UnaryExpr : Expr
    {
        public TokenType Operator;
        public Expr Operand;
        
        public UnaryExpr(TokenType op, Expr operand)
        {
            Operator = op;
            Operand = operand;
        }
    }
    
    /// <summary>
    /// Literal value: 42, "hello", True, None
    /// </summary>
    public class LiteralExpr : Expr
    {
        public object Value;
        
        public LiteralExpr(object value)
        {
            Value = value;
        }
    }
    
    /// <summary>
    /// Variable reference: x, myVar
    /// </summary>
    public class VariableExpr : Expr
    {
        public string Name;
        
        public VariableExpr(string name)
        {
            Name = name;
        }
    }
    
    /// <summary>
    /// Function/method call: func(arg1, arg2)
    /// </summary>
    public class CallExpr : Expr
    {
        public Expr Callee;
        public List<Expr> Arguments;
        
        public CallExpr(Expr callee, List<Expr> arguments)
        {
            Callee = callee;
            Arguments = arguments;
        }
    }
    
    /// <summary>
    /// Array/dict indexing: obj[index]
    /// </summary>
    public class IndexExpr : Expr
    {
        public Expr Object;
        public Expr Index;
        
        public IndexExpr(Expr obj, Expr index)
        {
            Object = obj;
            Index = index;
        }
    }
    
    /// <summary>
    /// Array slicing: obj[start:stop:step]
    /// </summary>
    public class SliceExpr : Expr
    {
        public Expr Object;
        public Expr Start;  // Can be null
        public Expr Stop;   // Can be null
        public Expr Step;   // Can be null
        
        public SliceExpr(Expr obj, Expr start, Expr stop, Expr step)
        {
            Object = obj;
            Start = start;
            Stop = stop;
            Step = step;
        }
    }
    
    /// <summary>
    /// List literal: [1, 2, 3]
    /// </summary>
    public class ListExpr : Expr
    {
        public List<Expr> Elements;
        
        public ListExpr(List<Expr> elements)
        {
            Elements = elements;
        }
    }
    
    /// <summary>
    /// Tuple literal: (1, 2, 3)
    /// </summary>
    public class TupleExpr : Expr
    {
        public List<Expr> Elements;
        
        public TupleExpr(List<Expr> elements)
        {
            Elements = elements;
        }
    }
    
    /// <summary>
    /// Dictionary literal: {key1: val1, key2: val2}
    /// </summary>
    public class DictExpr : Expr
    {
        public List<Expr> Keys;
        public List<Expr> Values;
        
        public DictExpr(List<Expr> keys, List<Expr> values)
        {
            Keys = keys;
            Values = values;
        }
    }
    
    /// <summary>
    /// Lambda expression: lambda x, y: x + y
    /// </summary>
    public class LambdaExpr : Expr
    {
        public List<string> Parameters;
        public Expr Body;
        
        public LambdaExpr(List<string> parameters, Expr body)
        {
            Parameters = parameters;
            Body = body;
        }
    }
    
    /// <summary>
    /// List comprehension: [x*2 for x in nums if x > 0]
    /// </summary>
    public class ListCompExpr : Expr
    {
        public Expr Element;
        public string Variable;
        public Expr Iterable;
        public Expr Condition; // Can be null
        
        public ListCompExpr(Expr element, string variable, Expr iterable, Expr condition = null)
        {
            Element = element;
            Variable = variable;
            Iterable = iterable;
            Condition = condition;
        }
    }
    
    /// <summary>
    /// Member access: obj.member, Grounds.Soil
    /// </summary>
    public class MemberAccessExpr : Expr
    {
        public Expr Object;
        public string Member;
        
        public MemberAccessExpr(Expr obj, string member)
        {
            Object = obj;
            Member = member;
        }
    }
    
    /// <summary>
    /// Ternary conditional: x if condition else y
    /// </summary>
    public class ConditionalExpr : Expr
    {
        public Expr Condition;
        public Expr ThenExpr;
        public Expr ElseExpr;
        
        public ConditionalExpr(Expr condition, Expr thenExpr, Expr elseExpr)
        {
            Condition = condition;
            ThenExpr = thenExpr;
            ElseExpr = elseExpr;
        }
    }
    
    #endregion
}
