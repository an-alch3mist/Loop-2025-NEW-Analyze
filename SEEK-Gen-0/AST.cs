using System.Collections.Generic;

namespace LOOPLanguage
{
    #region Base Node Classes
    
    /// <summary>
    /// Base class for all AST nodes.
    /// </summary>
    public abstract class ASTNode
    {
        public int LineNumber { get; set; }
    }
    
    /// <summary>
    /// Base class for all statement nodes.
    /// </summary>
    public abstract class Stmt : ASTNode { }
    
    /// <summary>
    /// Base class for all expression nodes.
    /// </summary>
    public abstract class Expr : ASTNode { }
    
    #endregion
    
    #region Statement Types
    
    /// <summary>
    /// Expression statement (standalone expression).
    /// </summary>
    public class ExpressionStmt : Stmt
    {
        public Expr Expression { get; set; }
        
        public ExpressionStmt(Expr expression)
        {
            Expression = expression;
        }
    }
    
    /// <summary>
    /// Variable assignment statement.
    /// </summary>
    public class AssignmentStmt : Stmt
    {
        public string Target { get; set; }
        public Expr Value { get; set; }
        public string Operator { get; set; } // "=", "+=", "-=", etc.
        
        public AssignmentStmt(string target, Expr value, string op)
        {
            Target = target;
            Value = value;
            Operator = op;
        }
    }
    
    /// <summary>
    /// Index assignment statement (e.g., list[0] = value).
    /// </summary>
    public class IndexAssignmentStmt : Stmt
    {
        public Expr Object { get; set; }
        public Expr Index { get; set; }
        public Expr Value { get; set; }
        
        public IndexAssignmentStmt(Expr obj, Expr index, Expr value)
        {
            Object = obj;
            Index = index;
            Value = value;
        }
    }
    
    /// <summary>
    /// If-elif-else conditional statement.
    /// </summary>
    public class IfStmt : Stmt
    {
        public Expr Condition { get; set; }
        public List<Stmt> ThenBranch { get; set; }
        public List<Stmt> ElseBranch { get; set; }
        
        public IfStmt(Expr condition, List<Stmt> thenBranch, List<Stmt> elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }
    }
    
    /// <summary>
    /// While loop statement.
    /// </summary>
    public class WhileStmt : Stmt
    {
        public Expr Condition { get; set; }
        public List<Stmt> Body { get; set; }
        
        public WhileStmt(Expr condition, List<Stmt> body)
        {
            Condition = condition;
            Body = body;
        }
    }
    
    /// <summary>
    /// For loop statement.
    /// </summary>
    public class ForStmt : Stmt
    {
        public string Variable { get; set; }
        public Expr Iterable { get; set; }
        public List<Stmt> Body { get; set; }
        
        public ForStmt(string variable, Expr iterable, List<Stmt> body)
        {
            Variable = variable;
            Iterable = iterable;
            Body = body;
        }
    }
    
    /// <summary>
    /// Function definition statement.
    /// </summary>
    public class FunctionDefStmt : Stmt
    {
        public string Name { get; set; }
        public List<string> Parameters { get; set; }
        public List<Stmt> Body { get; set; }
        
        public FunctionDefStmt(string name, List<string> parameters, List<Stmt> body)
        {
            Name = name;
            Parameters = parameters;
            Body = body;
        }
    }
    
    /// <summary>
    /// Class definition statement.
    /// </summary>
    public class ClassDefStmt : Stmt
    {
        public string Name { get; set; }
        public List<FunctionDefStmt> Methods { get; set; }
        
        public ClassDefStmt(string name, List<FunctionDefStmt> methods)
        {
            Name = name;
            Methods = methods;
        }
    }
    
    /// <summary>
    /// Return statement.
    /// </summary>
    public class ReturnStmt : Stmt
    {
        public Expr Value { get; set; }
        
        public ReturnStmt(Expr value)
        {
            Value = value;
        }
    }
    
    /// <summary>
    /// Break statement.
    /// </summary>
    public class BreakStmt : Stmt { }
    
    /// <summary>
    /// Continue statement.
    /// </summary>
    public class ContinueStmt : Stmt { }
    
    /// <summary>
    /// Pass statement (no-op).
    /// </summary>
    public class PassStmt : Stmt { }
    
    /// <summary>
    /// Global variable declaration.
    /// </summary>
    public class GlobalStmt : Stmt
    {
        public List<string> Variables { get; set; }
        
        public GlobalStmt(List<string> variables)
        {
            Variables = variables;
        }
    }
    
    /// <summary>
    /// Import statement for enum members.
    /// </summary>
    public class ImportStmt : Stmt
    {
        public string EnumName { get; set; }
        public string MemberName { get; set; }
        
        public ImportStmt(string enumName, string memberName)
        {
            EnumName = enumName;
            MemberName = memberName;
        }
    }
    
    #endregion
    
    #region Expression Types
    
    /// <summary>
    /// Binary expression (e.g., a + b, x == y).
    /// </summary>
    public class BinaryExpr : Expr
    {
        public Expr Left { get; set; }
        public TokenType Operator { get; set; }
        public Expr Right { get; set; }
        
        public BinaryExpr(Expr left, TokenType op, Expr right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }
    
    /// <summary>
    /// Unary expression (e.g., -x, not y).
    /// </summary>
    public class UnaryExpr : Expr
    {
        public TokenType Operator { get; set; }
        public Expr Operand { get; set; }
        
        public UnaryExpr(TokenType op, Expr operand)
        {
            Operator = op;
            Operand = operand;
        }
    }
    
    /// <summary>
    /// Literal value expression (number, string, bool, None).
    /// </summary>
    public class LiteralExpr : Expr
    {
        public object Value { get; set; }
        
        public LiteralExpr(object value)
        {
            Value = value;
        }
    }
    
    /// <summary>
    /// Variable reference expression.
    /// </summary>
    public class VariableExpr : Expr
    {
        public string Name { get; set; }
        
        public VariableExpr(string name)
        {
            Name = name;
        }
    }
    
    /// <summary>
    /// Function call expression.
    /// </summary>
    public class CallExpr : Expr
    {
        public Expr Callee { get; set; }
        public List<Expr> Arguments { get; set; }
        
        public CallExpr(Expr callee, List<Expr> arguments)
        {
            Callee = callee;
            Arguments = arguments;
        }
    }
    
    /// <summary>
    /// Index access expression (e.g., list[0], dict["key"]).
    /// </summary>
    public class IndexExpr : Expr
    {
        public Expr Object { get; set; }
        public Expr Index { get; set; }
        
        public IndexExpr(Expr obj, Expr index)
        {
            Object = obj;
            Index = index;
        }
    }
    
    /// <summary>
    /// Slice expression (e.g., list[1:5], str[::2]).
    /// </summary>
    public class SliceExpr : Expr
    {
        public Expr Object { get; set; }
        public Expr Start { get; set; }
        public Expr Stop { get; set; }
        public Expr Step { get; set; }
        
        public SliceExpr(Expr obj, Expr start, Expr stop, Expr step)
        {
            Object = obj;
            Start = start;
            Stop = stop;
            Step = step;
        }
    }
    
    /// <summary>
    /// List literal expression (e.g., [1, 2, 3]).
    /// </summary>
    public class ListExpr : Expr
    {
        public List<Expr> Elements { get; set; }
        
        public ListExpr(List<Expr> elements)
        {
            Elements = elements;
        }
    }
    
    /// <summary>
    /// Tuple literal expression (e.g., (1, 2, 3)).
    /// </summary>
    public class TupleExpr : Expr
    {
        public List<Expr> Elements { get; set; }
        
        public TupleExpr(List<Expr> elements)
        {
            Elements = elements;
        }
    }
    
    /// <summary>
    /// Dictionary literal expression (e.g., {"a": 1, "b": 2}).
    /// </summary>
    public class DictExpr : Expr
    {
        public List<Expr> Keys { get; set; }
        public List<Expr> Values { get; set; }
        
        public DictExpr(List<Expr> keys, List<Expr> values)
        {
            Keys = keys;
            Values = values;
        }
    }
    
    /// <summary>
    /// Lambda expression (anonymous function).
    /// </summary>
    public class LambdaExpr : Expr
    {
        public List<string> Parameters { get; set; }
        public Expr Body { get; set; }
        
        public LambdaExpr(List<string> parameters, Expr body)
        {
            Parameters = parameters;
            Body = body;
        }
    }
    
    /// <summary>
    /// List comprehension expression (e.g., [x*2 for x in nums if x > 0]).
    /// </summary>
    public class ListCompExpr : Expr
    {
        public Expr Element { get; set; }
        public string Variable { get; set; }
        public Expr Iterable { get; set; }
        public Expr Condition { get; set; }
        
        public ListCompExpr(Expr element, string variable, Expr iterable, Expr condition)
        {
            Element = element;
            Variable = variable;
            Iterable = iterable;
            Condition = condition;
        }
    }
    
    /// <summary>
    /// Member access expression (e.g., obj.property, Grounds.Soil).
    /// </summary>
    public class MemberAccessExpr : Expr
    {
        public Expr Object { get; set; }
        public string Member { get; set; }
        
        public MemberAccessExpr(Expr obj, string member)
        {
            Object = obj;
            Member = member;
        }
    }
    
    /// <summary>
    /// Conditional expression (ternary operator: a if cond else b).
    /// </summary>
    public class ConditionalExpr : Expr
    {
        public Expr Condition { get; set; }
        public Expr ThenValue { get; set; }
        public Expr ElseValue { get; set; }
        
        public ConditionalExpr(Expr condition, Expr thenValue, Expr elseValue)
        {
            Condition = condition;
            ThenValue = thenValue;
            ElseValue = elseValue;
        }
    }
    
    #endregion
}