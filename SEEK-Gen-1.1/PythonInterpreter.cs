using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LoopLanguage
{
    /// <summary>
    /// Main interpreter for LOOP Language (Python-like).
    /// Executes AST using coroutine-based execution with instruction budget.
    /// </summary>
    public class PythonInterpreter
    {
        #region Fields
        
        public Scope currentScope;
        private Scope globalScope;
        private int instructionCount;
        private const int INSTRUCTIONS_PER_FRAME = 100;
        private int currentLineNumber;
        private GameBuiltinMethods gameBuiltins;
        private HashSet<string> globalVariables;
        private int recursionDepth;
        private const int MAX_RECURSION_DEPTH = 100;
        
        #endregion
        
        #region Constructor
        
        public PythonInterpreter(GameBuiltinMethods gameCommands)
        {
            gameBuiltins = gameCommands;
            Reset();
        }
        
        #endregion
        
        #region Public Methods
        
        public void Reset()
        {
            globalScope = new Scope();
            currentScope = globalScope;
            instructionCount = 0;
            currentLineNumber = 1;
            globalVariables = new HashSet<string>();
            recursionDepth = 0;
            
            RegisterBuiltins();
            RegisterEnums();
            RegisterConstants();
        }
        
        public IEnumerator Execute(List<Stmt> statements)
        {
            foreach (Stmt stmt in statements)
            {
                IEnumerator routine = ExecuteStatement(stmt);
                if (routine != null)
                {
                    while (routine.MoveNext())
                    {
                        yield return routine.Current;
                    }
                }
            }
        }
        
        #endregion
        
        #region Builtin Registration
        
        private void RegisterBuiltins()
        {
            globalScope.Define("print", new BuiltinFunction("print", Print));
            globalScope.Define("sleep", new BuiltinFunction("sleep", Sleep));
            globalScope.Define("range", new BuiltinFunction("range", Range));
            globalScope.Define("len", new BuiltinFunction("len", Len));
            globalScope.Define("str", new BuiltinFunction("str", Str));
            globalScope.Define("int", new BuiltinFunction("int", Int));
            globalScope.Define("float", new BuiltinFunction("float", Float));
            globalScope.Define("abs", new BuiltinFunction("abs", Abs));
            globalScope.Define("min", new BuiltinFunction("min", Min));
            globalScope.Define("max", new BuiltinFunction("max", Max));
            globalScope.Define("sum", new BuiltinFunction("sum", Sum));
            globalScope.Define("sorted", new BuiltinFunction("sorted", Sorted));
            
            globalScope.Define("move", new BuiltinFunction("move", gameBuiltins.Move));
            globalScope.Define("harvest", new BuiltinFunction("harvest", gameBuiltins.Harvest));
            globalScope.Define("plant", new BuiltinFunction("plant", gameBuiltins.Plant));
            globalScope.Define("till", new BuiltinFunction("till", gameBuiltins.Till));
            globalScope.Define("use_item", new BuiltinFunction("use_item", gameBuiltins.UseItem));
            globalScope.Define("do_a_flip", new BuiltinFunction("do_a_flip", gameBuiltins.DoAFlip));
            
            globalScope.Define("can_harvest", new BuiltinFunction("can_harvest", gameBuiltins.CanHarvest));
            globalScope.Define("get_ground_type", new BuiltinFunction("get_ground_type", gameBuiltins.GetGroundType));
            globalScope.Define("get_entity_type", new BuiltinFunction("get_entity_type", gameBuiltins.GetEntityType));
            globalScope.Define("get_pos_x", new BuiltinFunction("get_pos_x", gameBuiltins.GetPosX));
            globalScope.Define("get_pos_y", new BuiltinFunction("get_pos_y", gameBuiltins.GetPosY));
            globalScope.Define("get_world_size", new BuiltinFunction("get_world_size", gameBuiltins.GetWorldSize));
            globalScope.Define("get_water", new BuiltinFunction("get_water", gameBuiltins.GetWater));
            globalScope.Define("num_items", new BuiltinFunction("num_items", gameBuiltins.NumItems));
            globalScope.Define("is_even", new BuiltinFunction("is_even", gameBuiltins.IsEven));
            globalScope.Define("is_odd", new BuiltinFunction("is_odd", gameBuiltins.IsOdd));
        }
        
        private void RegisterEnums()
        {
            globalScope.Define("Grounds", typeof(Grounds));
            globalScope.Define("Items", typeof(Items));
            globalScope.Define("Entities", typeof(Entities));
        }
        
        private void RegisterConstants()
        {
            globalScope.Define("North", "up");
            globalScope.Define("South", "down");
            globalScope.Define("East", "right");
            globalScope.Define("West", "left");
        }
        
        #endregion
        
        #region Statement Execution
        
        private IEnumerator ExecuteStatement(Stmt stmt)
        {
            IncrementInstructionCount();
            
            // .NET 2.0 Compliance: Cannot yield inside try-catch
            // Solution: Get routine first (may throw), then yield outside try-catch
            IEnumerator routine = null;
            
            // Special exceptions that should propagate
            if (stmt is BreakStmt)
            {
                throw new BreakException();
            }
            if (stmt is ContinueStmt)
            {
                throw new ContinueException();
            }
            if (stmt is ReturnStmt)
            {
                ExecuteReturn((ReturnStmt)stmt);
                yield break;
            }
            
            // Try to get the routine (errors handled here)
            try
            {
                if (stmt is ExpressionStmt)
                {
                    ExpressionStmt exprStmt = (ExpressionStmt)stmt;
                    object result = Evaluate(exprStmt.Expression);
                    
                    if (result is IEnumerator)
                    {
                        routine = (IEnumerator)result;
                    }
                }
                else if (stmt is AssignmentStmt)
                {
                    routine = ExecuteAssignment((AssignmentStmt)stmt);
                }
                else if (stmt is SubscriptAssignmentStmt)
                {
                    routine = ExecuteSubscriptAssignment((SubscriptAssignmentStmt)stmt);
                }
                else if (stmt is MemberAssignmentStmt)
                {
                    routine = ExecuteMemberAssignment((MemberAssignmentStmt)stmt);
                }
                else if (stmt is IfStmt)
                {
                    routine = ExecuteIf((IfStmt)stmt);
                }
                else if (stmt is WhileStmt)
                {
                    routine = ExecuteWhile((WhileStmt)stmt);
                }
                else if (stmt is ForStmt)
                {
                    routine = ExecuteFor((ForStmt)stmt);
                }
                else if (stmt is FunctionDefStmt)
                {
                    ExecuteFunctionDef((FunctionDefStmt)stmt);
                }
                else if (stmt is ClassDefStmt)
                {
                    ExecuteClassDef((ClassDefStmt)stmt);
                }
                else if (stmt is PassStmt)
                {
                    // Do nothing
                }
                else if (stmt is GlobalStmt)
                {
                    ExecuteGlobal((GlobalStmt)stmt);
                }
                else if (stmt is ImportStmt)
                {
                    ExecuteImport((ImportStmt)stmt);
                }
            }
            catch (RuntimeError)
            {
                throw; // Re-throw runtime errors as-is
            }
            catch (Exception e)
            {
                throw new RuntimeError(currentLineNumber, e.Message);
            }
            
            // Yield outside try-catch (safe for .NET 2.0)
            if (routine != null)
            {
                while (routine.MoveNext())
                {
                    yield return routine.Current;
                }
            }
        }
        
        private IEnumerator ExecuteAssignment(AssignmentStmt stmt)
        {
            object value = Evaluate(stmt.Value);
            
            if (value is IEnumerator)
            {
                IEnumerator routine = (IEnumerator)value;
                while (routine.MoveNext())
                {
                    yield return routine.Current;
                }
                value = null;
            }
            
            if (stmt.Operator != "=")
            {
                object currentValue = currentScope.Get(stmt.Target);
                value = ApplyCompoundOperator(currentValue, value, stmt.Operator);
            }
            
            if (globalVariables.Contains(stmt.Target))
            {
                globalScope.Define(stmt.Target, value);
            }
            else
            {
                currentScope.Set(stmt.Target, value);
            }
        }
        
        private IEnumerator ExecuteSubscriptAssignment(SubscriptAssignmentStmt stmt)
        {
            object obj = Evaluate(stmt.Object);
            object index = Evaluate(stmt.Index);
            object value = Evaluate(stmt.Value);
            
            if (value is IEnumerator)
            {
                IEnumerator routine = (IEnumerator)value;
                while (routine.MoveNext())
                {
                    yield return routine.Current;
                }
                value = null;
            }
            
            if (obj is List<object>)
            {
                List<object> list = (List<object>)obj;
                
                // Use strict integer validation
                int idx = NumberHandling.ToListIndex(index, list.Count);
                
                if (stmt.Operator != "=")
                {
                    value = ApplyCompoundOperator(list[idx], value, stmt.Operator);
                }
                
                list[idx] = value;
            }
            else if (obj is Dictionary<object, object>)
            {
                Dictionary<object, object> dict = (Dictionary<object, object>)obj;
                
                if (stmt.Operator != "=")
                {
                    if (dict.ContainsKey(index))
                    {
                        value = ApplyCompoundOperator(dict[index], value, stmt.Operator);
                    }
                }
                
                dict[index] = value;
            }
        }
        
        private IEnumerator ExecuteMemberAssignment(MemberAssignmentStmt stmt)
        {
            object obj = Evaluate(stmt.Object);
            object value = Evaluate(stmt.Value);
            
            if (value is IEnumerator)
            {
                IEnumerator routine = (IEnumerator)value;
                while (routine.MoveNext())
                {
                    yield return routine.Current;
                }
                value = null;
            }
            
            if (obj is ClassInstance)
            {
                ClassInstance instance = (ClassInstance)obj;
                
                if (stmt.Operator != "=")
                {
                    object currentValue = instance.GetField(stmt.Member);
                    value = ApplyCompoundOperator(currentValue, value, stmt.Operator);
                }
                
                instance.SetField(stmt.Member, value);
            }
        }
        
        private IEnumerator ExecuteIf(IfStmt stmt)
        {
            object conditionResult = Evaluate(stmt.Condition);
            
            if (IsTruthy(conditionResult))
            {
                foreach (Stmt thenStmt in stmt.ThenBranch)
                {
                    // .NET 2.0 Compliance: Get routine outside try-catch
                    IEnumerator routine = ExecuteStatement(thenStmt);
                    
                    if (routine != null)
                    {
                        while (routine.MoveNext())
                        {
                            yield return routine.Current;
                        }
                    }
                }
            }
            else if (stmt.ElseBranch != null)
            {
                foreach (Stmt elseStmt in stmt.ElseBranch)
                {
                    // .NET 2.0 Compliance: Get routine outside try-catch
                    IEnumerator routine = ExecuteStatement(elseStmt);
                    
                    if (routine != null)
                    {
                        while (routine.MoveNext())
                        {
                            yield return routine.Current;
                        }
                    }
                }
            }
        }
        
        private IEnumerator ExecuteWhile(WhileStmt stmt)
        {
            while (true)
            {
                IncrementInstructionCount();
                
                object conditionResult = Evaluate(stmt.Condition);
                if (!IsTruthy(conditionResult)) break;
                
                bool shouldBreak = false;
                
                foreach (Stmt bodyStmt in stmt.Body)
                {
                    // .NET 2.0 Compliance: Store routine outside try-catch
                    IEnumerator routine = null;
                    bool gotBreak = false;
                    bool gotContinue = false;
                    
                    try
                    {
                        routine = ExecuteStatement(bodyStmt);
                    }
                    catch (BreakException)
                    {
                        gotBreak = true;
                    }
                    catch (ContinueException)
                    {
                        gotContinue = true;
                    }
                    
                    if (gotBreak)
                    {
                        shouldBreak = true;
                        break;
                    }
                    
                    if (gotContinue)
                    {
                        break; // Skip rest of body
                    }
                    
                    // Yield outside try-catch
                    if (routine != null)
                    {
                        while (routine.MoveNext())
                        {
                            yield return routine.Current;
                        }
                    }
                }
                
                if (shouldBreak) break;
            }
        }
        
        private IEnumerator ExecuteFor(ForStmt stmt)
        {
            object iterableResult = Evaluate(stmt.Iterable);
            List<object> items = ToList(iterableResult);
            
            foreach (object item in items)
            {
                IncrementInstructionCount();
                
                currentScope.Set(stmt.Variable, item);
                
                bool shouldBreak = false;
                
                foreach (Stmt bodyStmt in stmt.Body)
                {
                    // .NET 2.0 Compliance: Store routine outside try-catch
                    IEnumerator routine = null;
                    bool gotBreak = false;
                    bool gotContinue = false;
                    
                    try
                    {
                        routine = ExecuteStatement(bodyStmt);
                    }
                    catch (BreakException)
                    {
                        gotBreak = true;
                    }
                    catch (ContinueException)
                    {
                        gotContinue = true;
                    }
                    
                    if (gotBreak)
                    {
                        shouldBreak = true;
                        break;
                    }
                    
                    if (gotContinue)
                    {
                        break;
                    }
                    
                    // Yield outside try-catch
                    if (routine != null)
                    {
                        while (routine.MoveNext())
                        {
                            yield return routine.Current;
                        }
                    }
                }
                
                if (shouldBreak) break;
            }
        }
        
        private void ExecuteFunctionDef(FunctionDefStmt stmt)
        {
            currentScope.Set(stmt.Name, stmt);
        }
        
        private void ExecuteClassDef(ClassDefStmt stmt)
        {
            Dictionary<string, FunctionDefStmt> methods = new Dictionary<string, FunctionDefStmt>();
            
            foreach (FunctionDefStmt method in stmt.Methods)
            {
                methods[method.Name] = method;
            }
            
            currentScope.Set(stmt.Name, methods);
        }
        
        private void ExecuteReturn(ReturnStmt stmt)
        {
            object value = null;
            
            if (stmt.Value != null)
            {
                value = Evaluate(stmt.Value);
            }
            
            throw new ReturnException(value);
        }
        
        private void ExecuteGlobal(GlobalStmt stmt)
        {
            foreach (string varName in stmt.Variables)
            {
                globalVariables.Add(varName);
            }
        }
        
        private void ExecuteImport(ImportStmt stmt)
        {
            // Import syntax for compatibility only
        }
        
        #endregion
        
        #region Expression Evaluation
        
        public object Evaluate(Expr expr)
        {
            IncrementInstructionCount();
            
            if (expr is LiteralExpr)
            {
                return ((LiteralExpr)expr).Value;
            }
            
            if (expr is VariableExpr)
            {
                return currentScope.Get(((VariableExpr)expr).Name);
            }
            
            if (expr is BinaryExpr)
            {
                return EvaluateBinary((BinaryExpr)expr);
            }
            
            if (expr is UnaryExpr)
            {
                return EvaluateUnary((UnaryExpr)expr);
            }
            
            if (expr is CallExpr)
            {
                return EvaluateCall((CallExpr)expr);
            }
            
            if (expr is IndexExpr)
            {
                return EvaluateIndex((IndexExpr)expr);
            }
            
            if (expr is SliceExpr)
            {
                return EvaluateSlice((SliceExpr)expr);
            }
            
            if (expr is ListExpr)
            {
                return EvaluateList((ListExpr)expr);
            }
            
            if (expr is TupleExpr)
            {
                return EvaluateTuple((TupleExpr)expr);
            }
            
            if (expr is DictExpr)
            {
                return EvaluateDict((DictExpr)expr);
            }
            
            if (expr is LambdaExpr)
            {
                return EvaluateLambda((LambdaExpr)expr);
            }
            
            if (expr is ListCompExpr)
            {
                return EvaluateListComp((ListCompExpr)expr);
            }
            
            if (expr is MemberAccessExpr)
            {
                return EvaluateMemberAccess((MemberAccessExpr)expr);
            }
            
            if (expr is ConditionalExpr)
            {
                return EvaluateConditional((ConditionalExpr)expr);
            }
            
            throw new RuntimeError($"Unknown expression type: {expr.GetType().Name}");
        }
        
        private object EvaluateBinary(BinaryExpr expr)
        {
            // Handle short-circuit operators
            if (expr.Operator == TokenType.AND)
            {
                object left = Evaluate(expr.Left);
                if (!IsTruthy(left)) return left;
                return Evaluate(expr.Right);
            }
            
            if (expr.Operator == TokenType.OR)
            {
                object left = Evaluate(expr.Left);
                if (IsTruthy(left)) return left;
                return Evaluate(expr.Right);
            }
            
            object leftVal = Evaluate(expr.Left);
            object rightVal = Evaluate(expr.Right);
            
            switch (expr.Operator)
            {
                case TokenType.PLUS:
                    if (leftVal is string || rightVal is string)
                    {
                        return ToString(leftVal) + ToString(rightVal);
                    }
                    return ToNumber(leftVal) + ToNumber(rightVal);
                    
                case TokenType.MINUS:
                    return ToNumber(leftVal) - ToNumber(rightVal);
                    
                case TokenType.STAR:
                    return ToNumber(leftVal) * ToNumber(rightVal);
                    
                case TokenType.SLASH:
                    double divisor = ToNumber(rightVal);
                    if (divisor == 0) throw new RuntimeError("Division by zero");
                    return ToNumber(leftVal) / divisor;
                    
                case TokenType.DOUBLE_SLASH:
                    double div = ToNumber(rightVal);
                    if (div == 0) throw new RuntimeError("Division by zero");
                    return Math.Floor(ToNumber(leftVal) / div);
                    
                case TokenType.PERCENT:
                    return ToNumber(leftVal) % ToNumber(rightVal);
                    
                case TokenType.DOUBLE_STAR:
                    return Math.Pow(ToNumber(leftVal), ToNumber(rightVal));
                
                case TokenType.EQUAL_EQUAL:
                    return IsEqual(leftVal, rightVal);
                case TokenType.BANG_EQUAL:
                    return !IsEqual(leftVal, rightVal);
                case TokenType.LESS:
                    return ToNumber(leftVal) < ToNumber(rightVal);
                case TokenType.GREATER:
                    return ToNumber(leftVal) > ToNumber(rightVal);
                case TokenType.LESS_EQUAL:
                    return ToNumber(leftVal) <= ToNumber(rightVal);
                case TokenType.GREATER_EQUAL:
                    return ToNumber(leftVal) >= ToNumber(rightVal);
                
                case TokenType.IN:
                    return CheckContains(rightVal, leftVal);
                    
                case TokenType.AMPERSAND:
                    return (int)ToNumber(leftVal) & (int)ToNumber(rightVal);
                case TokenType.PIPE:
                    return (int)ToNumber(leftVal) | (int)ToNumber(rightVal);
                case TokenType.CARET:
                    return (int)ToNumber(leftVal) ^ (int)ToNumber(rightVal);
                case TokenType.LEFT_SHIFT:
                    return (int)ToNumber(leftVal) << (int)ToNumber(rightVal);
                case TokenType.RIGHT_SHIFT:
                    return (int)ToNumber(leftVal) >> (int)ToNumber(rightVal);
            }
            
            throw new RuntimeError($"Unknown operator: {expr.Operator}");
        }
        
        private object EvaluateUnary(UnaryExpr expr)
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
                    return ~(int)ToNumber(operand);
            }
            
            throw new RuntimeError($"Unknown unary operator: {expr.Operator}");
        }
        
        private object EvaluateCall(CallExpr expr)
        {
            object callee = Evaluate(expr.Callee);
            List<object> arguments = new List<object>();
            
            foreach (Expr arg in expr.Arguments)
            {
                arguments.Add(Evaluate(arg));
            }
            
            if (callee is BuiltinFunction)
            {
                BuiltinFunction func = (BuiltinFunction)callee;
                
                if (func.IsAsync())
                {
                    return func.CallAsync(arguments);
                }
                
                return func.Call(arguments);
            }
            
            if (callee is LambdaFunction)
            {
                LambdaFunction lambda = (LambdaFunction)callee;
                return lambda.Call(this, arguments);
            }
            
            if (callee is FunctionDefStmt)
            {
                return CallUserFunction((FunctionDefStmt)callee, arguments);
            }
            
            if (callee is Dictionary<string, FunctionDefStmt>)
            {
                // Class constructor
                Dictionary<string, FunctionDefStmt> methods = (Dictionary<string, FunctionDefStmt>)callee;
                ClassInstance instance = new ClassInstance("Class", methods);
                
                if (methods.ContainsKey("__init__"))
                {
                    List<object> initArgs = new List<object> { instance };
                    initArgs.AddRange(arguments);
                    CallUserFunction(methods["__init__"], initArgs);
                }
                
                return instance;
            }
            
            throw new RuntimeError("Object is not callable");
        }
        
        private object CallUserFunction(FunctionDefStmt func, List<object> arguments)
        {
            recursionDepth++;
            if (recursionDepth > MAX_RECURSION_DEPTH)
            {
                recursionDepth--;
                throw new RuntimeError("Maximum recursion depth exceeded");
            }
            
            if (arguments.Count != func.Parameters.Count)
            {
                recursionDepth--;
                throw new RuntimeError($"Function expects {func.Parameters.Count} arguments, got {arguments.Count}");
            }
            
            Scope functionScope = new Scope(currentScope);
            
            for (int i = 0; i < func.Parameters.Count; i++)
            {
                functionScope.Define(func.Parameters[i], arguments[i]);
            }
            
            Scope previousScope = currentScope;
            currentScope = functionScope;
            
            object returnValue = null;
            
            try
            {
                foreach (Stmt stmt in func.Body)
                {
                    IEnumerator routine = ExecuteStatement(stmt);
                    if (routine != null)
                    {
                        while (routine.MoveNext())
                        {
                            // Consume coroutine (can't yield from synchronous context)
                        }
                    }
                }
            }
            catch (ReturnException e)
            {
                returnValue = e.Value;
            }
            finally
            {
                currentScope = previousScope;
                recursionDepth--;
            }
            
            return returnValue;
        }
        
        private object EvaluateIndex(IndexExpr expr)
        {
            object obj = Evaluate(expr.Object);
            object index = Evaluate(expr.Index);
            
            if (obj is List<object>)
            {
                List<object> list = (List<object>)obj;
                
                // Use strict integer validation for indexing
                int idx = NumberHandling.ToListIndex(index, list.Count);
                
                return list[idx];
            }
            
            if (obj is Dictionary<object, object>)
            {
                Dictionary<object, object> dict = (Dictionary<object, object>)obj;
                
                if (!dict.ContainsKey(index))
                {
                    throw new RuntimeError($"Key not found: {index}");
                }
                
                return dict[index];
            }
            
            if (obj is string)
            {
                string str = (string)obj;
                
                // Use strict integer validation for string indexing
                int idx = NumberHandling.ToListIndex(index, str.Length);
                
                return str[idx].ToString();
            }
            
            throw new RuntimeError("Object is not indexable");
        }
        
        private object EvaluateSlice(SliceExpr expr)
        {
            object obj = Evaluate(expr.Object);
            
            int start = 0;
            int stop = 0;
            int step = 1;
            
            if (obj is List<object>)
            {
                List<object> list = (List<object>)obj;
                stop = list.Count;
                
                if (expr.Start != null)
                {
                    start = NumberHandling.ToInteger(Evaluate(expr.Start), "Slice start");
                    if (start < 0) start = list.Count + start;
                }
                
                if (expr.Stop != null)
                {
                    stop = NumberHandling.ToInteger(Evaluate(expr.Stop), "Slice stop");
                    if (stop < 0) stop = list.Count + stop;
                }
                
                if (expr.Step != null)
                {
                    step = NumberHandling.ToInteger(Evaluate(expr.Step), "Slice step");
                    if (step == 0)
                    {
                        throw new RuntimeError("Slice step cannot be zero");
                    }
                }
                
                List<object> result = new List<object>();
                
                if (step > 0)
                {
                    for (int i = start; i < stop && i < list.Count; i += step)
                    {
                        if (i >= 0) result.Add(list[i]);
                    }
                }
                else if (step < 0)
                {
                    for (int i = start; i > stop && i >= 0; i += step)
                    {
                        if (i < list.Count) result.Add(list[i]);
                    }
                }
                
                return result;
            }
            
            if (obj is string)
            {
                string str = (string)obj;
                stop = str.Length;
                
                if (expr.Start != null)
                {
                    start = NumberHandling.ToInteger(Evaluate(expr.Start), "Slice start");
                    if (start < 0) start = str.Length + start;
                }
                
                if (expr.Stop != null)
                {
                    stop = NumberHandling.ToInteger(Evaluate(expr.Stop), "Slice stop");
                    if (stop < 0) stop = str.Length + stop;
                }
                
                if (expr.Step != null)
                {
                    step = NumberHandling.ToInteger(Evaluate(expr.Step), "Slice step");
                    if (step == 0)
                    {
                        throw new RuntimeError("Slice step cannot be zero");
                    }
                }
                
                string result = "";
                
                if (step > 0)
                {
                    for (int i = start; i < stop && i < str.Length; i += step)
                    {
                        if (i >= 0) result += str[i];
                    }
                }
                
                return result;
            }
            
            throw new RuntimeError("Object does not support slicing");
        }
        
        private object EvaluateList(ListExpr expr)
        {
            List<object> result = new List<object>();
            
            foreach (Expr element in expr.Elements)
            {
                result.Add(Evaluate(element));
            }
            
            return result;
        }
        
        private object EvaluateTuple(TupleExpr expr)
        {
            List<object> result = new List<object>();
            
            foreach (Expr element in expr.Elements)
            {
                result.Add(Evaluate(element));
            }
            
            return result; // Tuples are immutable lists internally
        }
        
        private object EvaluateDict(DictExpr expr)
        {
            Dictionary<object, object> result = new Dictionary<object, object>();
            
            for (int i = 0; i < expr.Keys.Count; i++)
            {
                object key = Evaluate(expr.Keys[i]);
                object value = Evaluate(expr.Values[i]);
                result[key] = value;
            }
            
            return result;
        }
        
        private object EvaluateLambda(LambdaExpr expr)
        {
            return new LambdaFunction(expr.Parameters, expr.Body, currentScope);
        }
        
        private object EvaluateListComp(ListCompExpr expr)
        {
            List<object> result = new List<object>();
            
            object iterableResult = Evaluate(expr.Iterable);
            List<object> items = ToList(iterableResult);
            
            foreach (object item in items)
            {
                IncrementInstructionCount();
                
                Scope compScope = new Scope(currentScope);
                compScope.Define(expr.Variable, item);
                
                Scope previousScope = currentScope;
                currentScope = compScope;
                
                bool includeItem = true;
                
                if (expr.Condition != null)
                {
                    object condResult = Evaluate(expr.Condition);
                    includeItem = IsTruthy(condResult);
                }
                
                if (includeItem)
                {
                    object elementValue = Evaluate(expr.Element);
                    result.Add(elementValue);
                }
                
                currentScope = previousScope;
            }
            
            return result;
        }
        
        private object EvaluateMemberAccess(MemberAccessExpr expr)
        {
            object obj = Evaluate(expr.Object);
            
            if (obj is Type)
            {
                Type type = (Type)obj;
                
                if (type == typeof(Grounds))
                {
                    if (expr.Member == "Soil") return Grounds.Soil;
                    if (expr.Member == "Turf") return Grounds.Turf;
                    if (expr.Member == "Grassland") return Grounds.Grassland;
                }
                else if (type == typeof(Items))
                {
                    if (expr.Member == "Hay") return Items.Hay;
                    if (expr.Member == "Wood") return Items.Wood;
                    if (expr.Member == "Carrot") return Items.Carrot;
                    if (expr.Member == "Pumpkin") return Items.Pumpkin;
                    if (expr.Member == "Power") return Items.Power;
                    if (expr.Member == "Sunflower") return Items.Sunflower;
                    if (expr.Member == "Water") return Items.Water;
                }
                else if (type == typeof(Entities))
                {
                    if (expr.Member == "Grass") return Entities.Grass;
                    if (expr.Member == "Bush") return Entities.Bush;
                    if (expr.Member == "Tree") return Entities.Tree;
                    if (expr.Member == "Carrot") return Entities.Carrot;
                    if (expr.Member == "Pumpkin") return Entities.Pumpkin;
                    if (expr.Member == "Sunflower") return Entities.Sunflower;
                }
                
                throw new RuntimeError($"Unknown enum member: {expr.Member}");
            }
            
            if (obj is ClassInstance)
            {
                ClassInstance instance = (ClassInstance)obj;
                
                if (instance.HasField(expr.Member))
                {
                    return instance.GetField(expr.Member);
                }
                
                if (instance.HasMethod(expr.Member))
                {
                    FunctionDefStmt method = instance.GetMethod(expr.Member);
                    // Return bound method (TODO: implement proper binding)
                    return method;
                }
                
                throw new RuntimeError($"Undefined attribute: {expr.Member}");
            }
            
            throw new RuntimeError("Object does not support member access");
        }
        
        private object EvaluateConditional(ConditionalExpr expr)
        {
            object condition = Evaluate(expr.Condition);
            
            if (IsTruthy(condition))
            {
                return Evaluate(expr.ThenExpr);
            }
            else
            {
                return Evaluate(expr.ElseExpr);
            }
        }
        
        #endregion
        
        #region Helper Methods
        
        private void IncrementInstructionCount()
        {
            instructionCount++;
        }
        
        public bool ShouldYield()
        {
            if (instructionCount >= INSTRUCTIONS_PER_FRAME)
            {
                instructionCount = 0;
                return true;
            }
            return false;
        }
        
        // Use NumberHandling for all conversions
        private double ToNumber(object value)
        {
            return NumberHandling.ToNumber(value);
        }
        
        private int ToInt(object value)
        {
            // Generic integer conversion (strict)
            return NumberHandling.ToInteger(value, "Conversion");
        }
        
        private string ToString(object value)
        {
            if (value == null) return "None";
            if (value is bool) return ((bool)value) ? "True" : "False";
            if (value is double)
            {
                return NumberHandling.NumberToString((double)value);
            }
            if (value is List<object>)
            {
                List<object> list = (List<object>)value;
                return "[" + string.Join(", ", list.Select(x => ToString(x))) + "]";
            }
            return value.ToString();
        }
        
        private List<object> ToList(object value)
        {
            if (value is List<object>)
            {
                return (List<object>)value;
            }
            
            if (value is string)
            {
                string str = (string)value;
                List<object> result = new List<object>();
                foreach (char c in str)
                {
                    result.Add(c.ToString());
                }
                return result;
            }
            
            throw new RuntimeError("Object is not iterable");
        }
        
        private bool IsTruthy(object value)
        {
            if (value == null) return false;
            if (value is bool) return (bool)value;
            if (value is double) return (double)value != 0.0;
            if (value is int) return (int)value != 0;
            if (value is string) return ((string)value).Length > 0;
            if (value is List<object>) return ((List<object>)value).Count > 0;
            return true;
        }
        
        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            
            // Use NumberHandling for numeric comparisons
            // This makes 1 == 1.0 return True (Python behavior)
            if ((a is double || a is int || a is float) && 
                (b is double || b is int || b is float))
            {
                return NumberHandling.NumbersEqual(a, b);
            }
            
            return a.Equals(b);
        }
        
        private bool CheckContains(object container, object item)
        {
            if (container is List<object>)
            {
                List<object> list = (List<object>)container;
                foreach (object element in list)
                {
                    if (IsEqual(element, item))
                    {
                        return true;
                    }
                }
                return false;
            }
            
            if (container is string)
            {
                string str = (string)container;
                string searchStr = ToString(item);
                return str.Contains(searchStr);
            }
            
            if (container is Dictionary<object, object>)
            {
                Dictionary<object, object> dict = (Dictionary<object, object>)container;
                return dict.ContainsKey(item);
            }
            
            throw new RuntimeError("Object does not support 'in' operator");
        }
        
        private object ApplyCompoundOperator(object current, object value, string op)
        {
            switch (op)
            {
                case "+=":
                    if (current is string || value is string)
                    {
                        return ToString(current) + ToString(value);
                    }
                    return ToNumber(current) + ToNumber(value);
                case "-=":
                    return ToNumber(current) - ToNumber(value);
                case "*=":
                    return ToNumber(current) * ToNumber(value);
                case "/=":
                    double divisor = ToNumber(value);
                    if (divisor == 0) throw new RuntimeError("Division by zero");
                    return ToNumber(current) / divisor;
                default:
                    throw new RuntimeError($"Unknown compound operator: {op}");
            }
        }
        
        #endregion
        
        #region Built-in Function Implementations
        
        private object Print(List<object> args)
        {
            string output = string.Join(" ", args.Select(x => ToString(x)));
            Debug.Log(output);
            return null;
        }
        
        private IEnumerator Sleep(List<object> args)
        {
            if (args.Count != 1)
            {
                throw new RuntimeError("sleep() expects 1 argument");
            }
            
            // Handle both integer and float arguments
            // Python-style: sleep(2) and sleep(2.0) both work
            double seconds = ToNumber(args[0]);
            
            // Ensure non-negative
            if (seconds < 0)
            {
                throw new RuntimeError("sleep() duration cannot be negative");
            }
            
            yield return new WaitForSeconds((float)seconds);
        }
        
        private object Range(List<object> args)
        {
            int start = 0;
            int stop = 0;
            int step = 1;
            
            if (args.Count == 1)
            {
                stop = NumberHandling.ToRangeValue(args[0]);
            }
            else if (args.Count == 2)
            {
                start = NumberHandling.ToRangeValue(args[0]);
                stop = NumberHandling.ToRangeValue(args[1]);
            }
            else if (args.Count == 3)
            {
                start = NumberHandling.ToRangeValue(args[0]);
                stop = NumberHandling.ToRangeValue(args[1]);
                step = NumberHandling.ToRangeValue(args[2]);
                
                if (step == 0)
                {
                    throw new RuntimeError("range() step cannot be zero");
                }
            }
            else
            {
                throw new RuntimeError("range() expects 1-3 arguments");
            }
            
            List<object> result = new List<object>();
            
            if (step > 0)
            {
                for (int i = start; i < stop; i += step)
                {
                    result.Add((double)i);
                }
            }
            else if (step < 0)
            {
                for (int i = start; i > stop; i += step)
                {
                    result.Add((double)i);
                }
            }
            
            return result;
        }
        
        private object Len(List<object> args)
        {
            if (args.Count != 1)
            {
                throw new RuntimeError("len() expects 1 argument");
            }
            
            object obj = args[0];
            
            if (obj is List<object>)
            {
                return (double)((List<object>)obj).Count;
            }
            
            if (obj is string)
            {
                return (double)((string)obj).Length;
            }
            
            if (obj is Dictionary<object, object>)
            {
                return (double)((Dictionary<object, object>)obj).Count;
            }
            
            throw new RuntimeError("Object has no len()");
        }
        
        private object Str(List<object> args)
        {
            if (args.Count != 1)
            {
                throw new RuntimeError("str() expects 1 argument");
            }
            
            return ToString(args[0]);
        }
        
        private object Int(List<object> args)
        {
            if (args.Count != 1)
            {
                throw new RuntimeError("int() expects 1 argument");
            }
            
            return (double)ToInt(args[0]);
        }
        
        private object Float(List<object> args)
        {
            if (args.Count != 1)
            {
                throw new RuntimeError("float() expects 1 argument");
            }
            
            return ToNumber(args[0]);
        }
        
        private object Abs(List<object> args)
        {
            if (args.Count != 1)
            {
                throw new RuntimeError("abs() expects 1 argument");
            }
            
            return Math.Abs(ToNumber(args[0]));
        }
        
        private object Min(List<object> args)
        {
            if (args.Count == 0)
            {
                throw new RuntimeError("min() expects at least 1 argument");
            }
            
            if (args.Count == 1 && args[0] is List<object>)
            {
                List<object> list = (List<object>)args[0];
                if (list.Count == 0)
                {
                    throw new RuntimeError("min() arg is an empty sequence");
                }
                return list.Min(x => ToNumber(x));
            }
            
            return args.Min(x => ToNumber(x));
        }
        
        private object Max(List<object> args)
        {
            if (args.Count == 0)
            {
                throw new RuntimeError("max() expects at least 1 argument");
            }
            
            if (args.Count == 1 && args[0] is List<object>)
            {
                List<object> list = (List<object>)args[0];
                if (list.Count == 0)
                {
                    throw new RuntimeError("max() arg is an empty sequence");
                }
                return list.Max(x => ToNumber(x));
            }
            
            return args.Max(x => ToNumber(x));
        }
        
        private object Sum(List<object> args)
        {
            if (args.Count != 1)
            {
                throw new RuntimeError("sum() expects 1 argument");
            }
            
            if (!(args[0] is List<object>))
            {
                throw new RuntimeError("sum() expects a list");
            }
            
            List<object> list = (List<object>)args[0];
            double result = 0;
            
            foreach (object item in list)
            {
                result += ToNumber(item);
            }
            
            return result;
        }
        
        private object Sorted(List<object> args)
        {
            if (args.Count < 1 || args.Count > 3)
            {
                throw new RuntimeError("sorted() expects 1-3 arguments");
            }
            
            object iterable = args[0];
            LambdaFunction keyFunc = null;
            bool reverse = false;
            
            // Parse optional arguments
            // In real implementation, would need proper keyword argument parsing
            if (args.Count >= 2 && args[1] is LambdaFunction)
            {
                keyFunc = (LambdaFunction)args[1];
            }
            
            if (args.Count == 3 && args[2] is bool)
            {
                reverse = (bool)args[2];
            }
            
            List<object> items = ToList(iterable);
            List<object> result = new List<object>(items);
            
            if (keyFunc != null)
            {
                // Sort with key function
                result.Sort((a, b) => {
                    object keyA = keyFunc.Call(this, new List<object> { a });
                    object keyB = keyFunc.Call(this, new List<object> { b });
                    
                    double numA = ToNumber(keyA);
                    double numB = ToNumber(keyB);
                    
                    if (reverse)
                    {
                        return numB.CompareTo(numA);
                    }
                    else
                    {
                        return numA.CompareTo(numB);
                    }
                });
            }
            else
            {
                // Default sort
                result.Sort((a, b) => {
                    double numA = ToNumber(a);
                    double numB = ToNumber(b);
                    
                    if (reverse)
                    {
                        return numB.CompareTo(numA);
                    }
                    else
                    {
                        return numA.CompareTo(numB);
                    }
                });
            }
            
            return result;
        }
        
        #endregion
    }
}
