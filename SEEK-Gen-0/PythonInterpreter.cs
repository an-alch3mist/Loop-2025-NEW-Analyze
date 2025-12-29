using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LOOPLanguage
{
    /// <summary>
    /// Main interpreter that executes LOOP language scripts.
    /// Implements instruction budget for frame-rate friendly execution.
    /// </summary>
    public class PythonInterpreter : MonoBehaviour
    {
        #region Configuration
        
        private const int INSTRUCTIONS_PER_FRAME = 100;
        private const int MAX_RECURSION_DEPTH = 100;
        
        #endregion
        
        #region Fields
        
        private Scope globalScope;
        private Scope currentScope;
        private int instructionCount;
        private int currentLineNumber;
        private int recursionDepth;
        private GameBuiltinMethods gameBuiltins;
        private bool isRunning;
        
        #endregion
        
        #region Initialization
        
        void Awake()
        {
            gameBuiltins = GetComponent<GameBuiltinMethods>();
            if (gameBuiltins == null)
            {
                gameBuiltins = gameObject.AddComponent<GameBuiltinMethods>();
            }
            
            Reset();
        }
        
        /// <summary>
        /// Resets the interpreter to initial state.
        /// </summary>
        public void Reset()
        {
            globalScope = new Scope();
            currentScope = globalScope;
            instructionCount = 0;
            currentLineNumber = 0;
            recursionDepth = 0;
            isRunning = false;
            
            RegisterBuiltins();
            RegisterEnums();
            RegisterConstants();
        }
        
        #endregion
        
        #region Built-in Registration
        
        private void RegisterBuiltins()
        {
            // Standard built-ins
            globalScope.Define("print", new BuiltinFunction("print", args => {
                string output = string.Join(" ", args.Select(a => ToString(a)));
                Debug.Log(output);
                ConsoleManager.Instance?.WriteLine(output);
                return null;
            }, 0, -1));
            
            globalScope.Define("len", new BuiltinFunction("len", args => {
                object obj = args[0];
                if (obj is string) return (double)((string)obj).Length;
                if (obj is IList) return (double)((IList)obj).Count;
                if (obj is IDictionary) return (double)((IDictionary)obj).Count;
                throw new TypeError("len() argument must be a sequence or collection", -1);
            }, 1, 1));
            
            globalScope.Define("range", new BuiltinFunction("range", args => {
                int start = 0, stop, step = 1;
                
                if (args.Count == 1)
                {
                    stop = (int)ToNumber(args[0]);
                }
                else if (args.Count == 2)
                {
                    start = (int)ToNumber(args[0]);
                    stop = (int)ToNumber(args[1]);
                }
                else
                {
                    start = (int)ToNumber(args[0]);
                    stop = (int)ToNumber(args[1]);
                    step = (int)ToNumber(args[2]);
                }
                
                List<object> result = new List<object>();
                if (step > 0)
                {
                    for (int i = start; i < stop; i += step)
                        result.Add((double)i);
                }
                else if (step < 0)
                {
                    for (int i = start; i > stop; i += step)
                        result.Add((double)i);
                }
                
                return result;
            }, 1, 3));
            
            globalScope.Define("str", new BuiltinFunction("str", args => {
                return ToString(args[0]);
            }, 1, 1));
            
            globalScope.Define("int", new BuiltinFunction("int", args => {
                return (double)(int)ToNumber(args[0]);
            }, 1, 1));
            
            globalScope.Define("float", new BuiltinFunction("float", args => {
                return ToNumber(args[0]);
            }, 1, 1));
            
            globalScope.Define("abs", new BuiltinFunction("abs", args => {
                return Math.Abs(ToNumber(args[0]));
            }, 1, 1));
            
            globalScope.Define("min", new BuiltinFunction("min", args => {
                if (args.Count == 1 && args[0] is IList)
                {
                    IList list = (IList)args[0];
                    if (list.Count == 0) throw new RuntimeError("min() arg is empty sequence", -1);
                    double minVal = ToNumber(list[0]);
                    for (int i = 1; i < list.Count; i++)
                        minVal = Math.Min(minVal, ToNumber(list[i]));
                    return minVal;
                }
                
                double min = ToNumber(args[0]);
                for (int i = 1; i < args.Count; i++)
                    min = Math.Min(min, ToNumber(args[i]));
                return min;
            }, 1, -1));
            
            globalScope.Define("max", new BuiltinFunction("max", args => {
                if (args.Count == 1 && args[0] is IList)
                {
                    IList list = (IList)args[0];
                    if (list.Count == 0) throw new RuntimeError("max() arg is empty sequence", -1);
                    double maxVal = ToNumber(list[0]);
                    for (int i = 1; i < list.Count; i++)
                        maxVal = Math.Max(maxVal, ToNumber(list[i]));
                    return maxVal;
                }
                
                double max = ToNumber(args[0]);
                for (int i = 1; i < args.Count; i++)
                    max = Math.Max(max, ToNumber(args[i]));
                return max;
            }, 1, -1));
            
            globalScope.Define("sum", new BuiltinFunction("sum", args => {
                IList list = (IList)args[0];
                double total = 0;
                foreach (object item in list)
                    total += ToNumber(item);
                return total;
            }, 1, 1));
            
            globalScope.Define("sorted", new BuiltinFunction("sorted", args => {
                IList source = (IList)args[0];
                List<object> result = new List<object>();
                foreach (object item in source)
                    result.Add(item);
                
                // Check for key function
                LambdaFunction keyFunc = null;
                bool reverse = false;
                
                if (args.Count > 1 && args[1] is LambdaFunction)
                {
                    keyFunc = (LambdaFunction)args[1];
                }
                
                if (args.Count > 2)
                {
                    reverse = IsTruthy(args[2]);
                }
                
                if (keyFunc != null)
                {
                    result.Sort((a, b) => {
                        object keyA = CallLambda(keyFunc, new List<object> { a }, -1);
                        object keyB = CallLambda(keyFunc, new List<object> { b }, -1);
                        int cmp = CompareValues(keyA, keyB);
                        return reverse ? -cmp : cmp;
                    });
                }
                else
                {
                    result.Sort((a, b) => {
                        int cmp = CompareValues(a, b);
                        return reverse ? -cmp : cmp;
                    });
                }
                
                return result;
            }, 1, 3));
        }
        
        private void RegisterEnums()
        {
            // Register enum classes
            globalScope.Define("Grounds", CreateEnumClass(typeof(Grounds)));
            globalScope.Define("Items", CreateEnumClass(typeof(Items)));
            globalScope.Define("Entities", CreateEnumClass(typeof(Entities)));
        }
        
        private Dictionary<string, object> CreateEnumClass(Type enumType)
        {
            Dictionary<string, object> enumClass = new Dictionary<string, object>();
            
            foreach (var field in enumType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.IsLiteral)
                {
                    enumClass[field.Name] = field.GetValue(null);
                }
            }
            
            return enumClass;
        }
        
        private void RegisterConstants()
        {
            // Directional constants
            globalScope.Define("North", "up");
            globalScope.Define("South", "down");
            globalScope.Define("East", "right");
            globalScope.Define("West", "left");
        }
        
        #endregion
        
        #region Main Execution
        
        /// <summary>
        /// Runs a LOOP script with instruction budget for smooth frame rate.
        /// </summary>
        public IEnumerator Run(string source)
        {
            if (isRunning)
            {
                Debug.LogWarning("Script already running!");
                yield break;
            }
            
            isRunning = true;
            instructionCount = 0;
            
            try
            {
                // Lex
                Lexer lexer = new Lexer(source);
                List<Token> tokens = lexer.Tokenize();
                
                // Parse
                Parser parser = new Parser(tokens);
                List<Stmt> statements = parser.Parse();
                
                // Execute
                IEnumerator executor = ExecuteStatements(statements);
                while (executor.MoveNext())
                {
                    yield return executor.Current;
                }
            }
            catch (LOOPException e)
            {
                Debug.LogError(e.ToString());
                ConsoleManager.Instance?.WriteLine("[ERROR] " + e.ToString());
                Reset();
            }
            catch (Exception e)
            {
                Debug.LogError("Internal error: " + e.Message + "\n" + e.StackTrace);
                ConsoleManager.Instance?.WriteLine("[INTERNAL ERROR] " + e.Message);
                Reset();
            }
            finally
            {
                isRunning = false;
            }
        }
        
        private IEnumerator ExecuteStatements(List<Stmt> statements)
        {
            foreach (Stmt stmt in statements)
            {
                IEnumerator executor = ExecuteStatement(stmt);
                while (executor.MoveNext())
                {
                    yield return executor.Current;
                }
            }
        }
        
        #endregion
        
        #region Statement Execution
        
        private IEnumerator ExecuteStatement(Stmt stmt)
        {
            currentLineNumber = stmt.LineNumber;
            
            // Increment instruction budget
            IncrementInstructions();
            
            // Check for budget exceeded (time-slicing)
            if (instructionCount >= INSTRUCTIONS_PER_FRAME)
            {
                instructionCount = 0;
                yield return null; // Pause until next frame
            }
            
            try
            {
                if (stmt is ExpressionStmt)
                {
                    IEnumerator executor = EvaluateExpressionCoroutine(((ExpressionStmt)stmt).Expression);
                    while (executor.MoveNext())
                    {
                        yield return executor.Current;
                    }
                }
                else if (stmt is AssignmentStmt)
                {
                    AssignmentStmt assignment = (AssignmentStmt)stmt;
                    object value = null;
                    
                    IEnumerator executor = EvaluateExpressionCoroutine(assignment.Value);
                    while (executor.MoveNext())
                    {
                        if (executor.Current is object)
                        {
                            value = executor.Current;
                        }
                        else
                        {
                            yield return executor.Current;
                        }
                    }
                    
                    if (assignment.Operator == "=")
                    {
                        currentScope.Set(assignment.Target, value);
                    }
                    else
                    {
                        object current = currentScope.Get(assignment.Target);
                        
                        switch (assignment.Operator)
                        {
                            case "+=":
                                currentScope.Set(assignment.Target, Add(current, value));
                                break;
                            case "-=":
                                currentScope.Set(assignment.Target, ToNumber(current) - ToNumber(value));
                                break;
                            case "*=":
                                currentScope.Set(assignment.Target, ToNumber(current) * ToNumber(value));
                                break;
                            case "/=":
                                double divisor = ToNumber(value);
                                if (divisor == 0) throw new DivisionByZeroError(currentLineNumber);
                                currentScope.Set(assignment.Target, (double)((int)(ToNumber(current) / divisor)));
                                break;
                        }
                    }
                }
                else if (stmt is IndexAssignmentStmt)
                {
                    IndexAssignmentStmt indexAssign = (IndexAssignmentStmt)stmt;
                    object obj = Evaluate(indexAssign.Object);
                    object index = Evaluate(indexAssign.Index);
                    object value = Evaluate(indexAssign.Value);
                    
                    if (obj is IList)
                    {
                        IList list = (IList)obj;
                        int idx = NormalizeIndex((int)ToNumber(index), list.Count);
                        list[idx] = value;
                    }
                    else if (obj is IDictionary)
                    {
                        ((IDictionary)obj)[index] = value;
                    }
                    else
                    {
                        throw new TypeError("Object does not support item assignment", currentLineNumber);
                    }
                }
                else if (stmt is IfStmt)
                {
                    IfStmt ifStmt = (IfStmt)stmt;
                    object condition = Evaluate(ifStmt.Condition);
                    
                    if (IsTruthy(condition))
                    {
                        IEnumerator executor = ExecuteStatements(ifStmt.ThenBranch);
                        while (executor.MoveNext())
                        {
                            yield return executor.Current;
                        }
                    }
                    else if (ifStmt.ElseBranch != null)
                    {
                        IEnumerator executor = ExecuteStatements(ifStmt.ElseBranch);
                        while (executor.MoveNext())
                        {
                            yield return executor.Current;
                        }
                    }
                }
                else if (stmt is WhileStmt)
                {
                    WhileStmt whileStmt = (WhileStmt)stmt;
                    
                    while (IsTruthy(Evaluate(whileStmt.Condition)))
                    {
                        try
                        {
                            IEnumerator executor = ExecuteStatements(whileStmt.Body);
                            while (executor.MoveNext())
                            {
                                yield return executor.Current;
                            }
                        }
                        catch (BreakException)
                        {
                            break;
                        }
                        catch (ContinueException)
                        {
                            continue;
                        }
                    }
                }
                else if (stmt is ForStmt)
                {
                    ForStmt forStmt = (ForStmt)stmt;
                    object iterable = Evaluate(forStmt.Iterable);
                    
                    if (!(iterable is IList))
                    {
                        throw new TypeError("for loop requires an iterable", currentLineNumber);
                    }
                    
                    IList list = (IList)iterable;
                    
                    foreach (object item in list)
                    {
                        currentScope.Set(forStmt.Variable, item);
                        
                        try
                        {
                            IEnumerator executor = ExecuteStatements(forStmt.Body);
                            while (executor.MoveNext())
                            {
                                yield return executor.Current;
                            }
                        }
                        catch (BreakException)
                        {
                            break;
                        }
                        catch (ContinueException)
                        {
                            continue;
                        }
                    }
                }
                else if (stmt is FunctionDefStmt)
                {
                    FunctionDefStmt funcDef = (FunctionDefStmt)stmt;
                    UserFunction func = new UserFunction(
                        funcDef.Name,
                        funcDef.Parameters,
                        funcDef.Body,
                        currentScope
                    );
                    currentScope.Set(funcDef.Name, func);
                }
                else if (stmt is ClassDefStmt)
                {
                    ClassDefStmt classDef = (ClassDefStmt)stmt;
                    
                    // Store class definition
                    Dictionary<string, UserFunction> methods = new Dictionary<string, UserFunction>();
                    foreach (FunctionDefStmt method in classDef.Methods)
                    {
                        UserFunction func = new UserFunction(
                            method.Name,
                            method.Parameters,
                            method.Body,
                            currentScope
                        );
                        methods[method.Name] = func;
                    }
                    
                    currentScope.Set(classDef.Name, methods);
                }
                else if (stmt is ReturnStmt)
                {
                    ReturnStmt returnStmt = (ReturnStmt)stmt;
                    object value = returnStmt.Value != null ? Evaluate(returnStmt.Value) : null;
                    throw new ReturnException(value);
                }
                else if (stmt is BreakStmt)
                {
                    throw new BreakException();
                }
                else if (stmt is ContinueStmt)
                {
                    throw new ContinueException();
                }
                else if (stmt is GlobalStmt)
                {
                    // Global declarations are handled during execution
                    // Nothing to do here
                }
                else if (stmt is ImportStmt)
                {
                    // Import statements are handled by registering enum members
                    // Nothing to do at runtime
                }
            }
            catch (LOOPException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new RuntimeError(e.Message, currentLineNumber);
            }
        }
        
        #endregion
        
        #region Expression Evaluation
        
        private IEnumerator EvaluateExpressionCoroutine(Expr expr)
        {
            object result = null;
            
            try
            {
                if (expr is CallExpr)
                {
                    CallExpr call = (CallExpr)expr;
                    object callee = Evaluate(call.Callee);
                    List<object> arguments = new List<object>();
                    
                    foreach (Expr arg in call.Arguments)
                    {
                        arguments.Add(Evaluate(arg));
                    }
                    
                    // Check if it's a game builtin that yields
                    if (callee is string)
                    {
                        string funcName = (string)callee;
                        if (gameBuiltins != null)
                        {
                            IEnumerator gameFunc = gameBuiltins.CallMethod(funcName, arguments);
                            if (gameFunc != null)
                            {
                                while (gameFunc.MoveNext())
                                {
                                    yield return gameFunc.Current;
                                }
                                result = null;
                            }
                        }
                    }
                    else
                    {
                        result = CallFunction(callee, arguments, currentLineNumber);
                    }
                }
                else
                {
                    result = Evaluate(expr);
                }
            }
            catch (LOOPException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new RuntimeError(e.Message, currentLineNumber);
            }
            
            yield return result;
        }
        
        private object Evaluate(Expr expr)
        {
            currentLineNumber = expr.LineNumber;
            IncrementInstructions();
            
            if (expr is LiteralExpr)
            {
                return ((LiteralExpr)expr).Value;
            }
            else if (expr is VariableExpr)
            {
                return currentScope.Get(((VariableExpr)expr).Name);
            }
            else if (expr is BinaryExpr)
            {
                return EvaluateBinaryExpr((BinaryExpr)expr);
            }
            else if (expr is UnaryExpr)
            {
                return EvaluateUnaryExpr((UnaryExpr)expr);
            }
            else if (expr is CallExpr)
            {
                return EvaluateCallExpr((CallExpr)expr);
            }
            else if (expr is IndexExpr)
            {
                return EvaluateIndexExpr((IndexExpr)expr);
            }
            else if (expr is SliceExpr)
            {
                return EvaluateSliceExpr((SliceExpr)expr);
            }
            else if (expr is ListExpr)
            {
                List<object> result = new List<object>();
                foreach (Expr elem in ((ListExpr)expr).Elements)
                {
                    result.Add(Evaluate(elem));
                }
                return result;
            }
            else if (expr is TupleExpr)
            {
                List<object> result = new List<object>();
                foreach (Expr elem in ((TupleExpr)expr).Elements)
                {
                    result.Add(Evaluate(elem));
                }
                return result; // Tuples are represented as lists internally
            }
            else if (expr is DictExpr)
            {
                DictExpr dictExpr = (DictExpr)expr;
                Dictionary<object, object> result = new Dictionary<object, object>();
                
                for (int i = 0; i < dictExpr.Keys.Count; i++)
                {
                    object key = Evaluate(dictExpr.Keys[i]);
                    object value = Evaluate(dictExpr.Values[i]);
                    result[key] = value;
                }
                
                return result;
            }
            else if (expr is LambdaExpr)
            {
                LambdaExpr lambdaExpr = (LambdaExpr)expr;
                return new LambdaFunction(lambdaExpr.Parameters, lambdaExpr.Body, currentScope);
            }
            else if (expr is ListCompExpr)
            {
                return EvaluateListComprehension((ListCompExpr)expr);
            }
            else if (expr is MemberAccessExpr)
            {
                return EvaluateMemberAccess((MemberAccessExpr)expr);
            }
            
            throw new RuntimeError("Unknown expression type", currentLineNumber);
        }
        
        private object EvaluateBinaryExpr(BinaryExpr expr)
        {
            object left = Evaluate(expr.Left);
            
            // Short-circuit evaluation for logical operators
            if (expr.Operator == TokenType.AND)
            {
                if (!IsTruthy(left)) return left;
                return Evaluate(expr.Right);
            }
            
            if (expr.Operator == TokenType.OR)
            {
                if (IsTruthy(left)) return left;
                return Evaluate(expr.Right);
            }
            
            object right = Evaluate(expr.Right);
            
            switch (expr.Operator)
            {
                case TokenType.PLUS:
                    return Add(left, right);
                    
                case TokenType.MINUS:
                    return ToNumber(left) - ToNumber(right);
                    
                case TokenType.STAR:
                    return ToNumber(left) * ToNumber(right);
                    
                case TokenType.SLASH:
                case TokenType.DOUBLE_SLASH:
                    double divisor = ToNumber(right);
                    if (divisor == 0) throw new DivisionByZeroError(currentLineNumber);
                    return (double)((int)(ToNumber(left) / divisor));
                    
                case TokenType.PERCENT:
                    return ToNumber(left) % ToNumber(right);
                    
                case TokenType.DOUBLE_STAR:
                    return Math.Pow(ToNumber(left), ToNumber(right));
                    
                case TokenType.EQUAL_EQUAL:
                    return IsEqual(left, right);
                    
                case TokenType.BANG_EQUAL:
                    return !IsEqual(left, right);
                    
                case TokenType.LESS:
                    return ToNumber(left) < ToNumber(right);
                    
                case TokenType.GREATER:
                    return ToNumber(left) > ToNumber(right);
                    
                case TokenType.LESS_EQUAL:
                    return ToNumber(left) <= ToNumber(right);
                    
                case TokenType.GREATER_EQUAL:
                    return ToNumber(left) >= ToNumber(right);
                    
                case TokenType.IN:
                    return Contains(right, left);
                    
                case TokenType.IS:
                    return object.ReferenceEquals(left, right);
                    
                case TokenType.AMPERSAND:
                    return (double)((int)ToNumber(left) & (int)ToNumber(right));
                    
                case TokenType.PIPE:
                    return (double)((int)ToNumber(left) | (int)ToNumber(right));
                    
                case TokenType.CARET:
                    return (double)((int)ToNumber(left) ^ (int)ToNumber(right));
                    
                case TokenType.LEFT_SHIFT:
                    return (double)((int)ToNumber(left) << (int)ToNumber(right));
                    
                case TokenType.RIGHT_SHIFT:
                    return (double)((int)ToNumber(left) >> (int)ToNumber(right));
            }
            
            throw new RuntimeError("Unknown binary operator", currentLineNumber);
        }
        
        private object EvaluateUnaryExpr(UnaryExpr expr)
        {
            object operand = Evaluate(expr.Operand);
            
            switch (expr.Operator)
            {
                case TokenType.MINUS:
                    return -ToNumber(operand);
                    
                case TokenType.PLUS:
                    return ToNumber(operand);
                    
                case TokenType.NOT:
                    return !IsTruthy(operand);
                    
                case TokenType.TILDE:
                    return (double)(~(int)ToNumber(operand));
            }
            
            throw new RuntimeError("Unknown unary operator", currentLineNumber);
        }
        
        private object EvaluateCallExpr(CallExpr expr)
        {
            object callee = Evaluate(expr.Callee);
            List<object> arguments = new List<object>();
            
            foreach (Expr arg in expr.Arguments)
            {
                arguments.Add(Evaluate(arg));
            }
            
            return CallFunction(callee, arguments, currentLineNumber);
        }
        
        private object EvaluateIndexExpr(IndexExpr expr)
        {
            object obj = Evaluate(expr.Object);
            object index = Evaluate(expr.Index);
            
            if (obj is IList)
            {
                IList list = (IList)obj;
                int idx = NormalizeIndex((int)ToNumber(index), list.Count);
                return list[idx];
            }
            else if (obj is IDictionary)
            {
                IDictionary dict = (IDictionary)obj;
                if (!dict.Contains(index))
                {
                    throw new KeyError(ToString(index), currentLineNumber);
                }
                return dict[index];
            }
            else if (obj is string)
            {
                string str = (string)obj;
                int idx = NormalizeIndex((int)ToNumber(index), str.Length);
                return str[idx].ToString();
            }
            
            throw new TypeError("Object does not support indexing", currentLineNumber);
        }
        
        private object EvaluateSliceExpr(SliceExpr expr)
        {
            object obj = Evaluate(expr.Object);
            
            if (!(obj is IList || obj is string))
            {
                throw new TypeError("Object does not support slicing", currentLineNumber);
            }
            
            int length = obj is IList ? ((IList)obj).Count : ((string)obj).Length;
            
            int start = expr.Start != null ? (int)ToNumber(Evaluate(expr.Start)) : 0;
            int stop = expr.Stop != null ? (int)ToNumber(Evaluate(expr.Stop)) : length;
            int step = expr.Step != null ? (int)ToNumber(Evaluate(expr.Step)) : 1;
            
            // Normalize negative indices
            if (start < 0) start = length + start;
            if (stop < 0) stop = length + stop;
            
            // Clamp to valid range
            start = Math.Max(0, Math.Min(start, length));
            stop = Math.Max(0, Math.Min(stop, length));
            
            if (obj is IList)
            {
                IList list = (IList)obj;
                List<object> result = new List<object>();
                
                if (step > 0)
                {
                    for (int i = start; i < stop; i += step)
                    {
                        result.Add(list[i]);
                    }
                }
                else if (step < 0)
                {
                    for (int i = start; i > stop; i += step)
                    {
                        result.Add(list[i]);
                    }
                }
                
                return result;
            }
            else
            {
                string str = (string)obj;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                
                if (step > 0)
                {
                    for (int i = start; i < stop; i += step)
                    {
                        sb.Append(str[i]);
                    }
                }
                else if (step < 0)
                {
                    for (int i = start; i > stop; i += step)
                    {
                        sb.Append(str[i]);
                    }
                }
                
                return sb.ToString();
            }
        }
        
        private object EvaluateListComprehension(ListCompExpr expr)
        {
            object iterable = Evaluate(expr.Iterable);
            
            if (!(iterable is IList))
            {
                throw new TypeError("List comprehension requires an iterable", currentLineNumber);
            }
            
            IList source = (IList)iterable;
            List<object> result = new List<object>();
            
            Scope previousScope = currentScope;
            currentScope = new Scope(previousScope);
            
            try
            {
                foreach (object item in source)
                {
                    currentScope.Set(expr.Variable, item);
                    
                    // Check condition
                    if (expr.Condition != null)
                    {
                        if (!IsTruthy(Evaluate(expr.Condition)))
                        {
                            continue;
                        }
                    }
                    
                    // Evaluate element expression
                    result.Add(Evaluate(expr.Element));
                }
            }
            finally
            {
                currentScope = previousScope;
            }
            
            return result;
        }
        
        private object EvaluateMemberAccess(MemberAccessExpr expr)
        {
            object obj = Evaluate(expr.Object);
            
            // Enum member access
            if (obj is Dictionary<string, object>)
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)obj;
                if (dict.ContainsKey(expr.Member))
                {
                    return dict[expr.Member];
                }
            }
            
            // Class instance member access
            if (obj is ClassInstance)
            {
                ClassInstance instance = (ClassInstance)obj;
                
                if (instance.HasField(expr.Member))
                {
                    return instance.GetField(expr.Member);
                }
                
                if (instance.HasMethod(expr.Member))
                {
                    return instance.GetMethod(expr.Member);
                }
            }
            
            throw new AttributeError(
                string.Format("Object has no attribute '{0}'", expr.Member),
                currentLineNumber
            );
        }
        
        #endregion
        
        #region Function Calling
        
        private object CallFunction(object callee, List<object> arguments, int lineNumber)
        {
            // Built-in function
            if (callee is BuiltinFunction)
            {
                return ((BuiltinFunction)callee).Call(arguments, lineNumber);
            }
            
            // Lambda function
            if (callee is LambdaFunction)
            {
                return CallLambda((LambdaFunction)callee, arguments, lineNumber);
            }
            
            // User-defined function
            if (callee is UserFunction)
            {
                return CallUserFunction((UserFunction)callee, arguments, lineNumber);
            }
            
            // Class constructor
            if (callee is Dictionary<string, UserFunction>)
            {
                return CallClassConstructor((Dictionary<string, UserFunction>)callee, arguments, lineNumber);
            }
            
            throw new TypeError("Object is not callable", lineNumber);
        }
        
        private object CallLambda(LambdaFunction lambda, List<object> arguments, int lineNumber)
        {
            // Validate argument count
            if (arguments.Count != lambda.Parameters.Count)
            {
                throw new ArgumentError(
                    string.Format("Lambda expects {0} arguments, got {1}", lambda.Parameters.Count, arguments.Count),
                    lineNumber
                );
            }
            
            // Create new scope with closure scope as parent
            Scope lambdaScope = new Scope(lambda.ClosureScope);
            
            // Bind parameters
            for (int i = 0; i < lambda.Parameters.Count; i++)
            {
                lambdaScope.Set(lambda.Parameters[i], arguments[i]);
            }
            
            // Execute body
            Scope previousScope = currentScope;
            currentScope = lambdaScope;
            
            try
            {
                return Evaluate(lambda.Body);
            }
            finally
            {
                currentScope = previousScope;
            }
        }
        
        private object CallUserFunction(UserFunction func, List<object> arguments, int lineNumber)
        {
            // Check recursion depth
            if (recursionDepth >= MAX_RECURSION_DEPTH)
            {
                throw new RecursionError(lineNumber);
            }
            
            // Validate argument count
            if (arguments.Count != func.Parameters.Count)
            {
                throw new ArgumentError(
                    string.Format("{0}() takes {1} arguments, got {2}", func.Name, func.Parameters.Count, arguments.Count),
                    lineNumber
                );
            }
            
            // Create new local scope
            Scope functionScope = new Scope(func.ClosureScope);
            
            // Bind parameters
            for (int i = 0; i < func.Parameters.Count; i++)
            {
                functionScope.Set(func.Parameters[i], arguments[i]);
            }
            
            // Execute function body
            Scope previousScope = currentScope;
            currentScope = functionScope;
            recursionDepth++;
            
            try
            {
                foreach (Stmt stmt in func.Body)
                {
                    IEnumerator executor = ExecuteStatement(stmt);
                    while (executor.MoveNext())
                    {
                        // Note: Functions cannot yield in synchronous context
                        // This is a limitation - game commands in functions won't work
                    }
                }
                
                return null;
            }
            catch (ReturnException ret)
            {
                return ret.Value;
            }
            finally
            {
                currentScope = previousScope;
                recursionDepth--;
            }
        }
        
        private object CallClassConstructor(Dictionary<string, UserFunction> methods, List<object> arguments, int lineNumber)
        {
            ClassInstance instance = new ClassInstance("CustomClass");
            
            // Add methods to instance
            foreach (var kvp in methods)
            {
                instance.AddMethod(kvp.Key, kvp.Value);
            }
            
            // Call __init__ if it exists
            if (methods.ContainsKey("__init__"))
            {
                List<object> initArgs = new List<object> { instance };
                initArgs.AddRange(arguments);
                CallUserFunction(methods["__init__"], initArgs, lineNumber);
            }
            
            return instance;
        }
        
        #endregion
        
        #region Helper Methods
        
        private void IncrementInstructions()
        {
            instructionCount++;
        }
        
        private int NormalizeIndex(int index, int length)
        {
            if (index < 0)
            {
                index = length + index;
            }
            
            if (index < 0 || index >= length)
            {
                throw new IndexError("Index out of range", currentLineNumber);
            }
            
            return index;
        }
        
        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            if (obj is double) return (double)obj != 0;
            if (obj is string) return ((string)obj).Length > 0;
            if (obj is IList) return ((IList)obj).Count > 0;
            return true;
        }
        
        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            
            // Numeric comparison
            if (a is double && b is double)
            {
                return Math.Abs((double)a - (double)b) < 0.0000001;
            }
            
            return a.Equals(b);
        }
        
        private bool Contains(object container, object item)
        {
            if (container is IList)
            {
                IList list = (IList)container;
                foreach (object elem in list)
                {
                    if (IsEqual(elem, item)) return true;
                }
                return false;
            }
            
            if (container is string)
            {
                return ((string)container).Contains(ToString(item));
            }
            
            if (container is IDictionary)
            {
                return ((IDictionary)container).Contains(item);
            }
            
            throw new TypeError("'in' requires a sequence or collection", currentLineNumber);
        }
        
        private double ToNumber(object obj)
        {
            if (obj is double) return (double)obj;
            if (obj is int) return (double)(int)obj;
            if (obj is string)
            {
                double result;
                if (double.TryParse((string)obj, out result))
                {
                    return result;
                }
            }
            
            throw new TypeError("Cannot convert to number", currentLineNumber);
        }
        
        private string ToString(object obj)
        {
            if (obj == null) return "None";
            if (obj is string) return (string)obj;
            if (obj is bool) return (bool)obj ? "True" : "False";
            if (obj is double) return ((double)obj).ToString();
            if (obj is IList)
            {
                IList list = (IList)obj;
                return "[" + string.Join(", ", list.Cast<object>().Select(o => ToString(o))) + "]";
            }
            if (obj is IDictionary)
            {
                IDictionary dict = (IDictionary)obj;
                List<string> pairs = new List<string>();
                foreach (object key in dict.Keys)
                {
                    pairs.Add(ToString(key) + ": " + ToString(dict[key]));
                }
                return "{" + string.Join(", ", pairs) + "}";
            }
            
            return obj.ToString();
        }
        
        private object Add(object a, object b)
        {
            // String concatenation
            if (a is string || b is string)
            {
                return ToString(a) + ToString(b);
            }
            
            // Numeric addition
            return ToNumber(a) + ToNumber(b);
        }
        
        private int CompareValues(object a, object b)
        {
            // Compare numbers
            if (a is double && b is double)
            {
                return ((double)a).CompareTo((double)b);
            }
            
            // Compare strings
            if (a is string && b is string)
            {
                return ((string)a).CompareTo((string)b);
            }
            
            throw new TypeError("Cannot compare incompatible types", -1);
        }
        
        #endregion
    }
}