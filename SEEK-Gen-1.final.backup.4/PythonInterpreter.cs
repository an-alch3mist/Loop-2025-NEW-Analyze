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
	/// 
	/// (.NET 2.0 limitation: cannot use yield return inside try-catch)
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
		private const int MAX_RECURSION_DEPTH = 30;
		private ConsoleManager console;  // ← ADD THIS

		// Control flow flags for .NET 2.0 compatibility
		private bool breakFlag = false;
		private bool continueFlag = false;

		#endregion

		#region Constructor
		public PythonInterpreter(GameBuiltinMethods gameCommands, ConsoleManager consoleManager = null)
		{
			gameBuiltins = gameCommands;
			console = consoleManager;
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
			breakFlag = false;
			continueFlag = false;

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

		public bool ShouldYield()
		{
			return instructionCount >= INSTRUCTIONS_PER_FRAME;
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
			globalScope.Define("walk", new BuiltinFunction("walk", gameBuiltins.Walk));
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

			if (stmt is ExpressionStmt)
			{
				ExpressionStmt exprStmt = (ExpressionStmt)stmt;
				object result = Evaluate(exprStmt.Expression);

				if (result is IEnumerator)
				{
					IEnumerator routine = (IEnumerator)result;
					// ★ FIXED: Execute and discard FunctionReturn wrapper
					while (routine.MoveNext())
					{
						object current = routine.Current;
						if (current is FunctionReturn)
						{
							// Function returned, but we're in expression statement so discard value
							break;
						}
						yield return current;
					}
				}
			}
			else if (stmt is AssignmentStmt)
			{
				IEnumerator routine = ExecuteAssignment((AssignmentStmt)stmt);
				if (routine != null)
				{
					while (routine.MoveNext())
					{
						yield return routine.Current;
					}
				}
			}
			else if (stmt is SubscriptAssignmentStmt)
			{
				IEnumerator routine = ExecuteSubscriptAssignment((SubscriptAssignmentStmt)stmt);
				if (routine != null)
				{
					while (routine.MoveNext())
					{
						yield return routine.Current;
					}
				}
			}
			else if (stmt is MemberAssignmentStmt)
			{
				IEnumerator routine = ExecuteMemberAssignment((MemberAssignmentStmt)stmt);
				if (routine != null)
				{
					while (routine.MoveNext())
					{
						yield return routine.Current;
					}
				}
			}
			else if (stmt is IfStmt)
			{
				IEnumerator routine = ExecuteIf((IfStmt)stmt);
				while (routine.MoveNext())
				{
					yield return routine.Current;
				}
			}
			else if (stmt is WhileStmt)
			{
				IEnumerator routine = ExecuteWhile((WhileStmt)stmt);
				while (routine.MoveNext())
				{
					yield return routine.Current;
				}
			}
			else if (stmt is ForStmt)
			{
				IEnumerator routine = ExecuteFor((ForStmt)stmt);
				while (routine.MoveNext())
				{
					yield return routine.Current;
				}
			}
			else if (stmt is FunctionDefStmt)
			{
				ExecuteFunctionDef((FunctionDefStmt)stmt);
			}
			else if (stmt is ClassDefStmt)
			{
				ExecuteClassDef((ClassDefStmt)stmt);
			}
			else if (stmt is ReturnStmt)
			{
				ReturnStmt returnStmt = (ReturnStmt)stmt;

				if (returnStmt.Value != null)
				{
					object returnValue = Evaluate(returnStmt.Value);

					// ★ CRITICAL: If return expression contains async function calls,
					// execute them first and extract the actual return value
					if (returnValue is IEnumerator)
					{
						IEnumerator routine = (IEnumerator)returnValue;
						object extractedValue = null;

						while (routine.MoveNext())
						{
							object current = routine.Current;

							if (current is FunctionReturn)
							{
								extractedValue = ((FunctionReturn)current).Value;
								break;
							}

							extractedValue = current;
							yield return current;
						}

						// Now throw the exception with the extracted value
						throw new ReturnException(extractedValue);
					}
					else
					{
						// Synchronous return value
						throw new ReturnException(returnValue);
					}
				}
				else
				{
					// Return without value (returns None)
					throw new ReturnException(null);
				}
			}
			else if (stmt is BreakStmt)
			{
				throw new BreakException();
			}
			else if (stmt is ContinueStmt)
			{
				throw new ContinueException();
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

		#endregion

		#region Assignment Execution


		private IEnumerator ExecuteAssignment(AssignmentStmt stmt)
		{
			object value = Evaluate(stmt.Value);

			if (value is IEnumerator)
			{
				IEnumerator routine = (IEnumerator)value;
				object lastValue = null;

				while (routine.MoveNext())
				{
					object current = routine.Current;

					// ★ FIXED: Check for FunctionReturn wrapper
					if (current is FunctionReturn)
					{
						lastValue = ((FunctionReturn)current).Value;
						break; // Don't yield FunctionReturn to Unity
					}

					lastValue = current;
					yield return current;
				}

				value = lastValue;
			}

			// ... rest of assignment logic unchanged ...
			if (stmt.Operator == "=")
			{
				if (globalVariables.Contains(stmt.Target))
				{
					globalScope.Set(stmt.Target, value);
				}
				else
				{
					currentScope.Set(stmt.Target, value);
				}
			}
			else
			{
				object currentValue = currentScope.Get(stmt.Target);
				object newValue = ApplyCompoundOperator(currentValue, value, stmt.Operator);

				if (globalVariables.Contains(stmt.Target))
				{
					globalScope.Set(stmt.Target, newValue);
				}
				else
				{
					currentScope.Set(stmt.Target, newValue);
				}
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
				object lastValue = null;

				while (routine.MoveNext())
				{
					lastValue = routine.Current;
					yield return routine.Current;
				}

				value = lastValue;
			}

			if (obj is List<object>)
			{
				List<object> list = (List<object>)obj;
				int idx = NumberHandling.ToListIndex(index, list.Count);

				if (stmt.Operator == "=")
				{
					list[idx] = value;
				}
				else
				{
					list[idx] = ApplyCompoundOperator(list[idx], value, stmt.Operator);
				}
			}
			else if (obj is Dictionary<object, object>)
			{
				Dictionary<object, object> dict = (Dictionary<object, object>)obj;

				if (stmt.Operator == "=")
				{
					dict[index] = value;
				}
				else
				{
					if (!dict.ContainsKey(index))
					{
						throw new RuntimeError($"Key not found: {index}");
					}
					dict[index] = ApplyCompoundOperator(dict[index], value, stmt.Operator);
				}
			}
			else
			{
				throw new RuntimeError("Can only assign to list or dict subscript");
			}
		}

		private IEnumerator ExecuteMemberAssignment(MemberAssignmentStmt stmt)
		{
			object obj = Evaluate(stmt.Object);
			object value = Evaluate(stmt.Value);

			if (value is IEnumerator)
			{
				IEnumerator routine = (IEnumerator)value;
				object lastValue = null;

				while (routine.MoveNext())
				{
					lastValue = routine.Current;
					yield return routine.Current;
				}

				value = lastValue;
			}

			if (obj is ClassInstance)
			{
				ClassInstance instance = (ClassInstance)obj;

				if (stmt.Operator == "=")
				{
					instance.SetField(stmt.Member, value);
				}
				else
				{
					object currentValue = instance.GetField(stmt.Member);
					object newValue = ApplyCompoundOperator(currentValue, value, stmt.Operator);
					instance.SetField(stmt.Member, newValue);
				}
			}
			else
			{
				throw new RuntimeError("Can only assign to class instance member");
			}
		}

		private object ApplyCompoundOperator(object current, object value, string op)
		{
			double a = ToNumber(current);
			double b = ToNumber(value);

			switch (op)
			{
				case "+=": return a + b;
				case "-=": return a - b;
				case "*=": return a * b;
				case "/=": return a / b;
				default:
					throw new RuntimeError($"Unknown compound operator: {op}");
			}
		}

		#endregion

		#region Control Flow

		private IEnumerator ExecuteIf(IfStmt stmt)
		{
			object condition = Evaluate(stmt.Condition);

			if (IsTruthy(condition))
			{
				foreach (Stmt thenStmt in stmt.ThenBranch)
				{
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

		// FIXED: Proper break/continue handling for .NET 2.0
		private IEnumerator ExecuteWhile(WhileStmt stmt)
		{
			while (true)
			{
				IncrementInstructionCount();

				object conditionResult = Evaluate(stmt.Condition);
				if (!IsTruthy(conditionResult)) break;

				// Execute body statements with break/continue support
				bool shouldBreak = false;
				bool shouldContinue = false;

				foreach (Stmt bodyStmt in stmt.Body)
				{
					// Execute statement and catch control flow exceptions
					IEnumerator routine = ExecuteStatementWithControlFlow(bodyStmt, out shouldBreak, out shouldContinue);

					// If break or continue was triggered, stop executing body
					if (shouldBreak || shouldContinue)
					{
						break;
					}

					// Execute the routine if it exists
					if (routine != null)
					{
						bool exceptionDuringExecution = false;

						// Manually iterate to catch exceptions without yield in try-catch
						while (true)
						{
							bool hasMore = false;

							try
							{
								hasMore = routine.MoveNext();
							}
							catch (BreakException)
							{
								shouldBreak = true;
								exceptionDuringExecution = true;
								break;
							}
							catch (ContinueException)
							{
								shouldContinue = true;
								exceptionDuringExecution = true;
								break;
							}

							if (!hasMore) break;

							yield return routine.Current;
						}

						if (exceptionDuringExecution)
						{
							break;
						}
					}
				}

				if (shouldBreak) break;
				if (shouldContinue) continue;
			}
		}

		// FIXED: Proper break/continue handling for .NET 2.0
		private IEnumerator ExecuteFor(ForStmt stmt)
		{
			object iterableResult = Evaluate(stmt.Iterable);
			List<object> items = ToList(iterableResult);

			foreach (object item in items)
			{
				IncrementInstructionCount();

				currentScope.Set(stmt.Variable, item);

				// Execute body statements with break/continue support
				bool shouldBreak = false;
				bool shouldContinue = false;

				foreach (Stmt bodyStmt in stmt.Body)
				{
					// Execute statement and catch control flow exceptions
					IEnumerator routine = ExecuteStatementWithControlFlow(bodyStmt, out shouldBreak, out shouldContinue);

					// If break or continue was triggered, stop executing body
					if (shouldBreak || shouldContinue)
					{
						break;
					}

					// Execute the routine if it exists
					if (routine != null)
					{
						bool exceptionDuringExecution = false;

						// Manually iterate to catch exceptions without yield in try-catch
						while (true)
						{
							bool hasMore = false;

							try
							{
								hasMore = routine.MoveNext();
							}
							catch (BreakException)
							{
								shouldBreak = true;
								exceptionDuringExecution = true;
								break;
							}
							catch (ContinueException)
							{
								shouldContinue = true;
								exceptionDuringExecution = true;
								break;
							}

							if (!hasMore) break;

							yield return routine.Current;
						}

						if (exceptionDuringExecution)
						{
							break;
						}
					}
				}

				if (shouldBreak) break;
				if (shouldContinue) continue;
			}
		}

		// Helper method to execute statement and catch immediate control flow exceptions
		private IEnumerator ExecuteStatementWithControlFlow(Stmt stmt, out bool shouldBreak, out bool shouldContinue)
		{
			shouldBreak = false;
			shouldContinue = false;

			try
			{
				return ExecuteStatement(stmt);
			}
			catch (BreakException)
			{
				shouldBreak = true;
				return null;
			}
			catch (ContinueException)
			{
				shouldContinue = true;
				return null;
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

			throw new RuntimeError("Unknown expression type");
		}

		private object EvaluateBinary(BinaryExpr expr)
		{
			// Short-circuit evaluation for logical operators
			if (expr.Operator == TokenType.AND)
			{
				object leftVal = Evaluate(expr.Left);

				// ★ FIX: If left operand is async, handle it
				if (leftVal is IEnumerator)
				{
					return EvaluateBinaryAsync(expr, leftVal, null);
				}

				if (!IsTruthy(leftVal)) return false;

				object rightVal = Evaluate(expr.Right);

				// ★ FIX: If right operand is async, handle it
				if (rightVal is IEnumerator)
				{
					return EvaluateBinaryAsync(expr, leftVal, rightVal);
				}

				return IsTruthy(rightVal);
			}

			if (expr.Operator == TokenType.OR)
			{
				object leftVal = Evaluate(expr.Left);

				// ★ FIX: If left operand is async, handle it
				if (leftVal is IEnumerator)
				{
					return EvaluateBinaryAsync(expr, leftVal, null);
				}

				if (IsTruthy(leftVal)) return true;

				object rightVal = Evaluate(expr.Right);

				// ★ FIX: If right operand is async, handle it
				if (rightVal is IEnumerator)
				{
					return EvaluateBinaryAsync(expr, leftVal, rightVal);
				}

				return IsTruthy(rightVal);
			}

			object leftValue = Evaluate(expr.Left);
			object rightValue = Evaluate(expr.Right);

			// ★ FIX: If either operand is an IEnumerator (async function result), 
			// return an IEnumerator that executes them first
			if (leftValue is IEnumerator || rightValue is IEnumerator)
			{
				return EvaluateBinaryAsync(expr, leftValue, rightValue);
			}

			// ★ All operands resolved, perform operation synchronously
			return EvaluateBinarySync(leftValue, rightValue, expr.Operator);
		}

		/// <summary>
		/// ★ NEW METHOD: Handles binary operations with IEnumerator operands
		/// Executes async operands first, then performs the operation
		/// </summary>
		private IEnumerator EvaluateBinaryAsync(BinaryExpr expr, object leftValue, object rightValue)
		{
			// Resolve left operand if it's an IEnumerator
			if (leftValue is IEnumerator)
			{
				IEnumerator leftRoutine = (IEnumerator)leftValue;
				object resolvedLeft = null;

				while (leftRoutine.MoveNext())
				{
					object current = leftRoutine.Current;

					if (current is FunctionReturn)
					{
						resolvedLeft = ((FunctionReturn)current).Value;
						break;
					}

					resolvedLeft = current;
					yield return current;
				}

				leftValue = resolvedLeft;
			}

			// For short-circuit operators, check condition after resolving left
			if (expr.Operator == TokenType.AND)
			{
				if (!IsTruthy(leftValue))
				{
					yield return new FunctionReturn(false);
					yield break;
				}
			}
			else if (expr.Operator == TokenType.OR)
			{
				if (IsTruthy(leftValue))
				{
					yield return new FunctionReturn(true);
					yield break;
				}
			}

			// Need to evaluate right operand for short-circuit operators
			if (rightValue == null)
			{
				rightValue = Evaluate(expr.Right);
			}

			// Resolve right operand if it's an IEnumerator
			if (rightValue is IEnumerator)
			{
				IEnumerator rightRoutine = (IEnumerator)rightValue;
				object resolvedRight = null;

				while (rightRoutine.MoveNext())
				{
					object current = rightRoutine.Current;

					if (current is FunctionReturn)
					{
						resolvedRight = ((FunctionReturn)current).Value;
						break;
					}

					resolvedRight = current;
					yield return current;
				}

				rightValue = resolvedRight;
			}

			// Both operands resolved, perform the operation
			object result = EvaluateBinarySync(leftValue, rightValue, expr.Operator);

			// Wrap result in FunctionReturn so it can be extracted by caller
			yield return new FunctionReturn(result);
		}

		/// <summary>
		/// ★ NEW METHOD: Performs binary operation on resolved (non-IEnumerator) values
		/// Extracted from original EvaluateBinary for reuse
		/// </summary>
		private object EvaluateBinarySync(object leftValue, object rightValue, TokenType op)
		{
			// Handle string concatenation
			if (op == TokenType.PLUS && (leftValue is string || rightValue is string))
			{
				return ToString(leftValue) + ToString(rightValue);
			}

			// Handle 'in' operator
			if (op == TokenType.IN)
			{
				if (rightValue is List<object>)
				{
					List<object> list = (List<object>)rightValue;
					foreach (object item in list)
					{
						if (AreEqual(leftValue, item))
						{
							return true;
						}
					}
					return false;
				}

				if (rightValue is string)
				{
					string str = (string)rightValue;
					string search = ToString(leftValue);
					return str.Contains(search);
				}

				if (rightValue is Dictionary<object, object>)
				{
					Dictionary<object, object> dict = (Dictionary<object, object>)rightValue;
					return dict.ContainsKey(leftValue);
				}

				throw new RuntimeError("'in' requires list, string, or dict");
			}

			// Handle 'is' operator
			if (op == TokenType.IS)
			{
				return ReferenceEquals(leftValue, rightValue);
			}

			// Handle equality/inequality (works with any type, no number conversion needed)
			if (op == TokenType.EQUAL_EQUAL)
			{
				return AreEqual(leftValue, rightValue);
			}

			if (op == TokenType.BANG_EQUAL)
			{
				return !AreEqual(leftValue, rightValue);
			}

			// Handle AND/OR (should only reach here for non-short-circuit evaluation)
			if (op == TokenType.AND)
			{
				return IsTruthy(leftValue) && IsTruthy(rightValue);
			}

			if (op == TokenType.OR)
			{
				return IsTruthy(leftValue) || IsTruthy(rightValue);
			}

			// From here on, we need numeric values
			// Convert to numbers for arithmetic and numeric comparisons
			double leftNum = ToNumber(leftValue);
			double rightNum = ToNumber(rightValue);

			switch (op)
			{
				// Arithmetic operators
				case TokenType.PLUS: return leftNum + rightNum;
				case TokenType.MINUS: return leftNum - rightNum;
				case TokenType.STAR: return leftNum * rightNum;
				case TokenType.SLASH: return leftNum / rightNum;
				case TokenType.PERCENT: return leftNum % rightNum;
				case TokenType.DOUBLE_STAR: return Math.Pow(leftNum, rightNum);
				case TokenType.DOUBLE_SLASH: return Math.Floor(leftNum / rightNum);

				// Numeric comparison operators
				case TokenType.LESS: return leftNum < rightNum;
				case TokenType.GREATER: return leftNum > rightNum;
				case TokenType.LESS_EQUAL: return leftNum <= rightNum;
				case TokenType.GREATER_EQUAL: return leftNum >= rightNum;

				// Bitwise operators
				case TokenType.AMPERSAND: return (double)((int)leftNum & (int)rightNum);
				case TokenType.PIPE: return (double)((int)leftNum | (int)rightNum);
				case TokenType.CARET: return (double)((int)leftNum ^ (int)rightNum);
				case TokenType.LEFT_SHIFT: return (double)((int)leftNum << (int)rightNum);
				case TokenType.RIGHT_SHIFT: return (double)((int)leftNum >> (int)rightNum);

				default:
					throw new RuntimeError($"Unknown binary operator: {op}");
			}
		}

		private object EvaluateUnary(UnaryExpr expr)
		{
			object operand = Evaluate(expr.Operand);

			// ★ FIX: If operand is an IEnumerator (async function result),
			// return an IEnumerator that executes it first
			if (operand is IEnumerator)
			{
				return EvaluateUnaryAsync(expr, operand);
			}

			return EvaluateUnarySync(operand, expr.Operator);
		}

		/// <summary>
		/// ★ NEW METHOD: Handles unary operations with IEnumerator operands
		/// </summary>
		private IEnumerator EvaluateUnaryAsync(UnaryExpr expr, object operandValue)
		{
			// Resolve operand if it's an IEnumerator
			if (operandValue is IEnumerator)
			{
				IEnumerator routine = (IEnumerator)operandValue;
				object resolvedValue = null;

				while (routine.MoveNext())
				{
					object current = routine.Current;

					if (current is FunctionReturn)
					{
						resolvedValue = ((FunctionReturn)current).Value;
						break;
					}

					resolvedValue = current;
					yield return current;
				}

				operandValue = resolvedValue;
			}

			// Perform the operation
			object result = EvaluateUnarySync(operandValue, expr.Operator);

			// Wrap result in FunctionReturn
			yield return new FunctionReturn(result);
		}

		/// <summary>
		/// ★ NEW METHOD: Performs unary operation on resolved value
		/// </summary>
		private object EvaluateUnarySync(object operand, TokenType op)
		{
			switch (op)
			{
				case TokenType.MINUS:
					return -ToNumber(operand);

				case TokenType.NOT:
					return !IsTruthy(operand);

				case TokenType.TILDE:
					return (double)(~(int)ToNumber(operand));

				default:
					throw new RuntimeError($"Unknown unary operator: {op}");
			}
		}


		private object EvaluateCall(CallExpr expr)
		{
			object callee = Evaluate(expr.Callee);

			// Evaluate positional arguments
			List<object> arguments = new List<object>();
			bool hasAsyncArgs = false;

			foreach (Expr arg in expr.Arguments)
			{
				object argValue = Evaluate(arg);
				arguments.Add(argValue);

				// Check if any argument is an IEnumerator (async function result)
				if (argValue is IEnumerator)
				{
					hasAsyncArgs = true;
				}
			}

			// Evaluate keyword arguments
			Dictionary<string, object> kwargs = new Dictionary<string, object>();
			foreach (var kvp in expr.KeywordArguments)
			{
				object kwValue = Evaluate(kvp.Value);
				kwargs[kvp.Key] = kwValue;

				if (kwValue is IEnumerator)
				{
					hasAsyncArgs = true;
				}
			}
			if (hasAsyncArgs)
			{
				return ExecuteCallWithAsyncArgs(callee, arguments, kwargs);
			}

			return ExecuteCallSync(callee, arguments, kwargs);
		}


		/// <summary>
		/// Executes a call where one or more arguments are IEnumerators (async function results)
		/// Returns an IEnumerator that first executes all async arguments, then calls the function
		/// </summary>
		private IEnumerator ExecuteCallWithAsyncArgs(object callee, List<object> arguments, Dictionary<string, object> kwargs)
		{
			List<object> resolvedArgs = new List<object>();

			foreach (object arg in arguments)
			{
				if (arg is IEnumerator)
				{
					IEnumerator routine = (IEnumerator)arg;
					object resolvedValue = null;

					while (routine.MoveNext())
					{
						object current = routine.Current;

						// ★ Extract FunctionReturn wrapper
						if (current is FunctionReturn)
						{
							resolvedValue = ((FunctionReturn)current).Value;
							break;
						}

						resolvedValue = current;
						yield return current; // Yield animations/waits to Unity
					}

					resolvedArgs.Add(resolvedValue);
				}
				else
				{
					resolvedArgs.Add(arg);
				}
			}

			// Resolve keyword arguments
			Dictionary<string, object> resolvedKwargs = new Dictionary<string, object>();
			foreach (var kvp in kwargs)
			{
				if (kvp.Value is IEnumerator)
				{
					IEnumerator routine = (IEnumerator)kvp.Value;
					object resolvedValue = null;

					while (routine.MoveNext())
					{
						object current = routine.Current;

						if (current is FunctionReturn)
						{
							resolvedValue = ((FunctionReturn)current).Value;
							break;
						}

						resolvedValue = current;
						yield return current;
					}

					resolvedKwargs[kvp.Key] = resolvedValue;
				}
				else
				{
					resolvedKwargs[kvp.Key] = kvp.Value;
				}
			}

			object result = ExecuteCallSync(callee, resolvedArgs, resolvedKwargs);

			if (result is IEnumerator)
			{
				IEnumerator resultRoutine = (IEnumerator)result;
				object finalResult = null;

				while (resultRoutine.MoveNext())
				{
					object current = resultRoutine.Current;

					if (current is FunctionReturn)
					{
						finalResult = ((FunctionReturn)current).Value;
						// ★ Yield the FunctionReturn so calling code can extract it
						yield return current;
						yield break;
					}

					finalResult = current;
					yield return current;
				}

				// If no FunctionReturn was yielded, yield one now
				yield return new FunctionReturn(finalResult);
			}
			else
			{
				// Sync result - wrap in FunctionReturn
				yield return new FunctionReturn(result);
			}
		}

		// ============================================
		// NEW METHOD: Execute synchronous call
		// Add this method right after ExecuteCallWithAsyncArgs
		// ============================================

		/// <summary>
		/// Executes a function call with resolved (non-IEnumerator) arguments
		/// This is the core call logic extracted from the original EvaluateCall
		/// </summary>
		private object ExecuteCallSync(object callee, List<object> arguments, Dictionary<string, object> kwargs)
		{
			// Built-in function call
			if (callee is BuiltinFunction)
			{
				BuiltinFunction func = (BuiltinFunction)callee;

				if (func.IsAsync())
				{
					return func.CallAsync(arguments);
				}
				else
				{
					// Special handling for sorted() with kwargs
					if (func.GetName() == "sorted" && kwargs.Count > 0)
					{
						return SortedWithKwargs(arguments, kwargs);
					}

					return func.Call(arguments);
				}
			}

			// BoundListMethod call (for .sort() with keyword arguments)
			if (callee is BoundListMethod)
			{
				BoundListMethod boundMethod = (BoundListMethod)callee;

				if (boundMethod.MethodName == "sort")
				{
					return ExecuteListSort(boundMethod.List, arguments, kwargs);
				}

				throw new RuntimeError($"Unknown bound list method: {boundMethod.MethodName}");
			}

			// User-defined function call
			if (callee is FunctionDefStmt)
			{
				return CallUserFunction((FunctionDefStmt)callee, arguments, kwargs, null);
			}

			// Lambda function call
			if (callee is LambdaFunction)
			{
				LambdaFunction lambda = (LambdaFunction)callee;
				return lambda.Call(this, arguments);
			}

			// Constructor call
			if (callee is Dictionary<string, FunctionDefStmt>)
			{
				Dictionary<string, FunctionDefStmt> classMethods = (Dictionary<string, FunctionDefStmt>)callee;
				return CreateClassInstance(classMethods, arguments);
			}

			throw new RuntimeError("Can only call functions and classes");
		}

		// ============================================
		// Add this NEW method in PythonInterpreter.cs
		// Place it after EvaluateCall() method
		// ============================================
		/// <summary>
		/// Handles sorted() with keyword arguments (key=, reverse=)
		/// Example: sorted(items, key=lambda x: x[0], reverse=True)
		/// </summary>
		private object SortedWithKwargs(List<object> positionalArgs, Dictionary<string, object> kwargs)
		{
			if (positionalArgs.Count < 1)
			{
				throw new RuntimeError("sorted() expects at least 1 argument");
			}

			object iterable = positionalArgs[0];
			LambdaFunction keyFunc = null;
			bool reverse = false;

			// Check for 'key' keyword argument
			if (kwargs.ContainsKey("key"))
			{
				object keyValue = kwargs["key"];
				if (keyValue is LambdaFunction)
				{
					keyFunc = (LambdaFunction)keyValue;
				}
				else
				{
					throw new RuntimeError("sorted() key must be a lambda function");
				}
			}

			// Check for 'reverse' keyword argument
			if (kwargs.ContainsKey("reverse"))
			{
				reverse = IsTruthy(kwargs["reverse"]);
			}

			List<object> items = ToList(iterable);
			List<object> result = new List<object>(items);

			if (keyFunc != null)
			{
				result.Sort((a, b) =>
				{
					object keyA = keyFunc.Call(this, new List<object> { a });
					object keyB = keyFunc.Call(this, new List<object> { b });
					int comparison = CompareObjects(keyA, keyB);
					return reverse ? -comparison : comparison;
				});
			}
			else
			{
				result.Sort((a, b) =>
				{
					int comparison = CompareObjects(a, b);
					return reverse ? -comparison : comparison;
				});
			}

			return result;
		}

		// ============================================
		// FILE: PythonInterpreter.cs
		// Add this NEW method after SortedWithKwargs() (around line 648)
		// Implements Python-style .sort() with key= and reverse= support
		// ============================================

		/// <summary>
		/// Executes list.sort() with optional key function and reverse flag
		/// Supports: list.sort(), list.sort(key=func), list.sort(key=func, reverse=True)
		/// Python style: sorts in place, returns None
		/// </summary>
		private object ExecuteListSort(List<object> list, List<object> positionalArgs, Dictionary<string, object> kwargs)
		{
			LambdaFunction keyFunc = null;
			FunctionDefStmt keyFuncDef = null;
			bool reverse = false;

			// Handle positional arguments (for backward compatibility)
			// list.sort(lambda x: x) or list.sort(myFunc)
			if (positionalArgs.Count >= 1)
			{
				if (positionalArgs[0] is LambdaFunction)
				{
					keyFunc = (LambdaFunction)positionalArgs[0];
				}
				else if (positionalArgs[0] is FunctionDefStmt)
				{
					keyFuncDef = (FunctionDefStmt)positionalArgs[0];
				}
			}

			if (positionalArgs.Count >= 2)
			{
				reverse = IsTruthy(positionalArgs[1]);
			}

			// Handle keyword arguments (Python style)
			// list.sort(key=lambda x: x, reverse=True)
			if (kwargs.ContainsKey("key"))
			{
				object keyValue = kwargs["key"];
				if (keyValue is LambdaFunction)
				{
					keyFunc = (LambdaFunction)keyValue;
				}
				else if (keyValue is FunctionDefStmt)
				{
					keyFuncDef = (FunctionDefStmt)keyValue;
				}
				else
				{
					throw new RuntimeError("sort() key must be a function or lambda");
				}
			}

			if (kwargs.ContainsKey("reverse"))
			{
				reverse = IsTruthy(kwargs["reverse"]);
			}

			// Validate arguments
			if (positionalArgs.Count > 2)
			{
				throw new RuntimeError("sort() takes at most 2 positional arguments");
			}

			// Validate that we only have known keyword arguments
			foreach (string key in kwargs.Keys)
			{
				if (key != "key" && key != "reverse")
				{
					throw new RuntimeError($"sort() got unexpected keyword argument '{key}'");
				}
			}

			// Perform the sort
			if (keyFunc != null)
			{
				// Sort with lambda key function
				list.Sort((a, b) =>
				{
					object keyA = keyFunc.Call(this, new List<object> { a });
					object keyB = keyFunc.Call(this, new List<object> { b });
					int comparison = CompareObjects(keyA, keyB);
					return reverse ? -comparison : comparison;
				});
			}
			else if (keyFuncDef != null)
			{
				// Sort with user-defined key function
				list.Sort((a, b) =>
				{
					object keyA = CallUserFunction(keyFuncDef, new List<object> { a }, new Dictionary<string, object>(), null);
					object keyB = CallUserFunction(keyFuncDef, new List<object> { b }, new Dictionary<string, object>(), null);
					int comparison = CompareObjects(keyA, keyB);
					return reverse ? -comparison : comparison;
				});
			}
			else
			{
				// Default sort (no key function)
				// Only works for simple comparable types
				try
				{
					list.Sort((a, b) =>
					{
						int comparison = CompareObjects(a, b);
						return reverse ? -comparison : comparison;
					});
				}
				catch (Exception e)
				{
					throw new RuntimeError($"sort() cannot compare items: {e.Message}");
				}
			}

			// Python's .sort() returns None (sorts in place)
			return null;
		}


		private object CallUserFunction(FunctionDefStmt func, List<object> arguments, Dictionary<string, object> kwargs, ClassInstance instance)
		{
			// Handle null kwargs (backward compatibility)
			if (kwargs == null)
			{
				kwargs = new Dictionary<string, object>();
			}

			// Combine positional and keyword arguments
			List<object> finalArgs = new List<object>(arguments);

			// Fill in keyword arguments by parameter name
			for (int i = finalArgs.Count; i < func.Parameters.Count; i++)
			{
				string paramName = func.Parameters[i];
				if (kwargs.ContainsKey(paramName))
				{
					finalArgs.Add(kwargs[paramName]);
				}
				else
				{
					throw new RuntimeError($"Missing required argument: '{paramName}'");
				}
			}

			// Check for extra keyword arguments
			foreach (string kwargName in kwargs.Keys)
			{
				if (!func.Parameters.Contains(kwargName))
				{
					throw new RuntimeError($"Function got unexpected keyword argument: '{kwargName}'");
				}
			}

			if (finalArgs.Count != func.Parameters.Count)
			{
				throw new RuntimeError($"Function expects {func.Parameters.Count} arguments, got {finalArgs.Count}");
			}

			recursionDepth++;
			if (recursionDepth > MAX_RECURSION_DEPTH)
			{
				throw new RuntimeError("Maximum recursion depth exceeded");
			}

			Scope functionScope = new Scope(currentScope);

			if (instance != null)
			{
				functionScope.Define("self", instance);
			}

			for (int i = 0; i < func.Parameters.Count; i++)
			{
				functionScope.Define(func.Parameters[i], finalArgs[i]);
			}

			Scope previousScope = currentScope;
			currentScope = functionScope;

			try
			{
				// ★ NEW: Check if any statement in the function is async
				bool hasAsyncStatement = false;

				for (int stmtIndex = 0; stmtIndex < func.Body.Count; stmtIndex++)
				{
					Stmt stmt = func.Body[stmtIndex];

					// Try to execute synchronously first
					object result = ExecuteStatementSync(stmt);

					if (result is IEnumerator)
					{
						// Found async statement - switch to async execution
						// ★ CRITICAL FIX: Pass the existing routine AND start from NEXT statement
						// to avoid re-executing the statement that triggered async mode
						hasAsyncStatement = true;
						recursionDepth--;
						currentScope = previousScope;
						return ExecuteFunctionBodyAsync(func, functionScope, previousScope, finalArgs, instance, stmtIndex + 1, (IEnumerator)result);
					}
				}

				// ★ All statements executed synchronously, function returns None
				recursionDepth--;
				return null;
			}
			catch (ReturnException e)
			{
				recursionDepth--;
				currentScope = previousScope;
				return e.Value;  // ★ Return the actual value, not wrapped
			}
			finally
			{
				currentScope = previousScope;
			}
		}
		private object ExecuteStatementSync(Stmt stmt)
		{
			// ★ FIX: Don't special-case return statements
			// Let them go through normal IEnumerator path so async mode handles them properly
			IEnumerator routine = ExecuteStatement(stmt);

			// ★ FIX: Don't execute routine here - just return it if it exists
			// The calling code will execute it properly without duplication
			if (routine != null)
			{
				return routine;
			}

			return null;
		}

		private IEnumerator ExecuteFunctionBodyAsync(FunctionDefStmt func, Scope functionScope, Scope previousScope, List<object> arguments, ClassInstance instance, int startIndex = 0, IEnumerator existingRoutine = null)
		{
			currentScope = functionScope;
			recursionDepth++;

			object returnValue = null; // ★ Track return value
			bool hasReturned = false;

			// ★ CRITICAL FIX: Execute the existing routine from sync mode first
			// This ensures the statement that triggered async mode executes exactly once
			if (existingRoutine != null)
			{
				// ★ FIX: Save currentScope before executing existingRoutine
				// The recursive call will modify currentScope, so we need to restore it
				Scope scopeBeforeExistingRoutine = currentScope;

				while (true)
				{
					bool hasMore = false;
					bool returnCaught = false;

					try
					{
						hasMore = existingRoutine.MoveNext();
					}
					catch (ReturnException e)
					{
						returnValue = e.Value;
						hasReturned = true;
						returnCaught = true;
					}

					if (returnCaught)
					{
						// ★ CRITICAL FIX: Decrement BEFORE yielding FunctionReturn
						// Parent stops calling MoveNext after getting FunctionReturn,
						// so code after yield never executes!
						recursionDepth--;
						currentScope = previousScope;
						yield return new FunctionReturn(returnValue);
						yield break;
					}

					if (!hasMore) break;

					yield return existingRoutine.Current;
				}

				// ★ FIX: Restore currentScope after executing existingRoutine
				// This ensures remaining statements execute with correct variable bindings
				currentScope = scopeBeforeExistingRoutine;
			}

			for (int stmtIndex = startIndex; stmtIndex < func.Body.Count; stmtIndex++)
			{
				Stmt stmt = func.Body[stmtIndex];
				IEnumerator routine = null;
				bool shouldReturn = false;

				try
				{
					routine = ExecuteStatement(stmt);
				}
				catch (ReturnException e)
				{
					returnValue = e.Value;
					hasReturned = true;
					shouldReturn = true;
				}

				if (shouldReturn)
				{
					break;
				}

				if (routine != null)
				{
					// ★ FIX: Save currentScope before executing statement routine
					// Recursive calls will modify currentScope, so we need to restore it
					Scope scopeBeforeStatement = currentScope;

					while (true)
					{
						bool hasMore = false;
						bool returnCaught = false;

						try
						{
							hasMore = routine.MoveNext();
						}
						catch (ReturnException e)
						{
							returnValue = e.Value;
							hasReturned = true;
							returnCaught = true;
						}

						if (returnCaught)
						{
							break;
						}

						if (!hasMore) break;

						yield return routine.Current;
					}

					// ★ FIX: Restore currentScope after executing statement routine
					currentScope = scopeBeforeStatement;

					if (hasReturned)
					{
						break;
					}
				}
			}

			// ★ CRITICAL FIX: Decrement BEFORE yielding FunctionReturn
			// Parent stops calling MoveNext after getting FunctionReturn,
			// so code after yield never executes!
			recursionDepth--;
			currentScope = previousScope;

			// Yield the return value wrapped in FunctionReturn
			// This allows calling code to extract it
			yield return new FunctionReturn(returnValue);
		}
		private object CreateClassInstance(Dictionary<string, FunctionDefStmt> classMethods, List<object> arguments)
		{
			ClassInstance instance = new ClassInstance("Class", classMethods);

			if (classMethods.ContainsKey("__init__"))
			{
				CallUserFunction(classMethods["__init__"], arguments, new Dictionary<string, object>(), instance);
			}

			return instance;
		}

		private object EvaluateIndex(IndexExpr expr)
		{
			object obj = Evaluate(expr.Object);
			object index = Evaluate(expr.Index);

			if (obj is List<object>)
			{
				List<object> list = (List<object>)obj;
				int idx = NumberHandling.ToListIndex(index, list.Count);
				return list[idx];
			}

			if (obj is string)
			{
				string str = (string)obj;
				int idx = NumberHandling.ToListIndex(index, str.Length);
				return str[idx].ToString();
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

			throw new RuntimeError("Can only index lists, strings, and dicts");
		}

		private object EvaluateSlice(SliceExpr expr)
		{
			object obj = Evaluate(expr.Object);

			// Handle string slicing
			if (obj is string)
			{
				string str = (string)obj;
				int strLength = str.Length;

				int strStart = 0;
				int strStop = strLength;
				int strStep = 1;

				if (expr.Start != null)
				{
					strStart = NumberHandling.ToInteger(Evaluate(expr.Start), "Slice start");
					if (strStart < 0) strStart = strLength + strStart;
					strStart = Math.Max(0, Math.Min(strStart, strLength));
				}

				if (expr.Stop != null)
				{
					strStop = NumberHandling.ToInteger(Evaluate(expr.Stop), "Slice stop");
					if (strStop < 0) strStop = strLength + strStop;
					strStop = Math.Max(0, Math.Min(strStop, strLength));
				}

				if (expr.Step != null)
				{
					strStep = NumberHandling.ToInteger(Evaluate(expr.Step), "Slice step");
					if (strStep == 0)
					{
						throw new RuntimeError("Slice step cannot be zero");
					}
				}

				string strResult = "";

				if (strStep > 0)
				{
					for (int i = strStart; i < strStop; i += strStep)
					{
						strResult += str[i];
					}
				}
				else
				{
					for (int i = strStart; i > strStop; i += strStep)
					{
						strResult += str[i];
					}
				}

				return strResult;
			}

			// Handle list slicing
			if (!(obj is List<object>))
			{
				throw new RuntimeError("Can only slice lists and strings");
			}

			List<object> list = (List<object>)obj;
			int length = list.Count;

			int start = 0;
			int stop = length;
			int step = 1;

			if (expr.Start != null)
			{
				start = NumberHandling.ToInteger(Evaluate(expr.Start), "Slice start");
				if (start < 0) start = length + start;
				start = Math.Max(0, Math.Min(start, length));
			}

			if (expr.Stop != null)
			{
				stop = NumberHandling.ToInteger(Evaluate(expr.Stop), "Slice stop");
				if (stop < 0) stop = length + stop;
				stop = Math.Max(0, Math.Min(stop, length));
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
				for (int i = start; i < stop; i += step)
				{
					result.Add(list[i]);
				}
			}
			else
			{
				for (int i = start; i > stop; i += step)
				{
					result.Add(list[i]);
				}
			}

			return result;
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

			return result;
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
				currentScope.Set(expr.Variable, item);

				bool include = true;
				if (expr.Condition != null)
				{
					include = IsTruthy(Evaluate(expr.Condition));
				}

				if (include)
				{
					result.Add(Evaluate(expr.Element));
				}
			}

			return result;
		}

		// ============================================
		// FILE: PythonInterpreter.cs
		// Replace EvaluateMemberAccess() method (around line 818)
		// This adds .sort() support with keyword arguments
		// ============================================
		private object EvaluateMemberAccess(MemberAccessExpr expr)
		{
			object obj = Evaluate(expr.Object);

			// Enum member access
			if (obj is Type)
			{
				Type type = (Type)obj;
				var field = type.GetField(expr.Member);

				if (field != null && field.IsStatic)
				{
					return field.GetValue(null);
				}

				throw new RuntimeError($"Enum '{type.Name}' has no member '{expr.Member}'");
			}

			// List method access
			if (obj is List<object>)
			{
				List<object> list = (List<object>)obj;

				switch (expr.Member)
				{
					case "append":
						return new BuiltinFunction("append", args =>
						{
							if (args.Count != 1)
								throw new RuntimeError("append() takes exactly 1 argument");
							list.Add(args[0]);
							return null;
						});

					case "pop":
						return new BuiltinFunction("pop", args =>
						{
							if (list.Count == 0)
								throw new RuntimeError("pop from empty list");
							int index = list.Count - 1;
							if (args.Count == 1)
								index = NumberHandling.ToListIndex(args[0], list.Count);
							object value = list[index];
							list.RemoveAt(index);
							return value;
						});

					case "clear":
						return new BuiltinFunction("clear", args =>
						{
							if (args.Count != 0)
								throw new RuntimeError("clear() takes no arguments");
							list.Clear();
							return null;
						});

					case "remove":
						return new BuiltinFunction("remove", args =>
						{
							if (args.Count != 1)
								throw new RuntimeError("remove() takes exactly 1 argument");
							list.Remove(args[0]);
							return null;
						});

					// NEW: .sort() method - returns BoundListMethod for keyword argument support
					case "sort":
						return new BoundListMethod(list, "sort", this);

					default:
						throw new RuntimeError($"List has no method '{expr.Member}'");
				}
			}

			// Dictionary method access
			if (obj is Dictionary<object, object>)
			{
				Dictionary<object, object> dict = (Dictionary<object, object>)obj;

				switch (expr.Member)
				{
					case "keys":
						return new BuiltinFunction("keys", args =>
						{
							if (args.Count != 0)
								throw new RuntimeError("keys() takes no arguments");
							return new List<object>(dict.Keys);
						});

					case "values":
						return new BuiltinFunction("values", args =>
						{
							if (args.Count != 0)
								throw new RuntimeError("values() takes no arguments");
							return new List<object>(dict.Values);
						});

					case "get":
						return new BuiltinFunction("get", args =>
						{
							if (args.Count < 1 || args.Count > 2)
								throw new RuntimeError("get() takes 1 or 2 arguments");
							object key = args[0];
							if (dict.ContainsKey(key))
								return dict[key];
							return args.Count == 2 ? args[1] : null;
						});

					case "clear":
						return new BuiltinFunction("clear", args =>
						{
							if (args.Count != 0)
								throw new RuntimeError("clear() takes no arguments");
							dict.Clear();
							return null;
						});

					default:
						throw new RuntimeError($"Dict has no method '{expr.Member}'");
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
					FunctionDefStmt method = instance.GetMethod(expr.Member);
					return new BoundMethod(instance, method);
				}

				throw new RuntimeError($"Undefined member: {expr.Member}");
			}

			throw new RuntimeError("Can only access members of enums, class instances, lists, and dicts");
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

		#region Helper Classes
		private class BoundMethod
		{
			public ClassInstance Instance;
			public FunctionDefStmt Method;

			public BoundMethod(ClassInstance instance, FunctionDefStmt method)
			{
				Instance = instance;
				Method = method;
			}
		}

		// ============================================
		// FILE: PythonInterpreter.cs
		// Add this NEW class after the existing BoundMethod class (around line 1029)
		// This enables keyword arguments for list methods like .sort()
		// ============================================

		/// <summary>
		/// Bound list method that supports keyword arguments
		/// Wraps a list and method name for deferred execution with kwargs
		/// </summary>
		private class BoundListMethod
		{
			public List<object> List;
			public string MethodName;
			public PythonInterpreter Interpreter;

			public BoundListMethod(List<object> list, string methodName, PythonInterpreter interpreter)
			{
				List = list;
				MethodName = methodName;
				Interpreter = interpreter;
			}
		}


		/// <summary>
		/// Wrapper to communicate function return values through coroutines
		/// When a function completes, it yields this wrapper containing the return value
		/// Calling code can check for this and extract the value
		/// </summary>
		private class FunctionReturn
		{
			public object Value;
			public FunctionReturn(object value) { Value = value; }
		}
		#endregion

		#region Helper Methods

		private void IncrementInstructionCount()
		{
			instructionCount++;
			if (instructionCount >= INSTRUCTIONS_PER_FRAME)
			{
				instructionCount = 0;
			}
		}

		private bool IsTruthy(object value)
		{
			if (value == null) return false;
			if (value is bool) return (bool)value;
			if (value is double) return (double)value != 0.0;
			if (value is string) return ((string)value).Length > 0;
			if (value is List<object>) return ((List<object>)value).Count > 0;
			return true;
		}

		private bool AreEqual(object a, object b)
		{
			if (a == null && b == null) return true;
			if (a == null || b == null) return false;

			if (a is double || b is double)
			{
				return NumberHandling.NumbersEqual(a, b);
			}

			return a.Equals(b);
		}

		private double ToNumber(object value)
		{
			return NumberHandling.ToNumber(value);
		}

		private int ToInt(object value)
		{
			return NumberHandling.ToInteger(value, "Conversion");
		}

		private string ToString(object value)
		{
			if (value == null) return "None";
			if (value is bool) return ((bool)value) ? "True" : "False";
			if (value is double) return NumberHandling.NumberToString((double)value);
			if (value is string) return (string)value;

			if (value is List<object>)
			{
				List<object> list = (List<object>)value;
				List<string> elements = new List<string>();

				foreach (object item in list)
				{
					elements.Add(ToStringRepr(item));
				}

				return "[" + string.Join(", ", elements.ToArray()) + "]";
			}

			if (value is Dictionary<object, object>)
			{
				Dictionary<object, object> dict = (Dictionary<object, object>)value;
				List<string> pairs = new List<string>();

				foreach (var kvp in dict)
				{
					pairs.Add(ToStringRepr(kvp.Key) + ": " + ToStringRepr(kvp.Value));
				}

				return "{" + string.Join(", ", pairs.ToArray()) + "}";
			}

			if (value is LambdaFunction)
			{
				return ((LambdaFunction)value).ToString();
			}

			if (value is ClassInstance)
			{
				return ((ClassInstance)value).ToString();
			}

			return value.ToString();
		}

		private string ToStringRepr(object value)
		{
			if (value is string)
			{
				return "'" + ((string)value) + "'";
			}
			return ToString(value);
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

		/// <summary>
		/// Synchronous version for use in callbacks (like sort key functions)
		/// Throws error if async operations detected
		/// </summary>
		private object CallUserFunctionSync(FunctionDefStmt func, List<object> arguments, Dictionary<string, object> kwargs, ClassInstance instance)
		{
			// Handle null kwargs (backward compatibility)
			if (kwargs == null)
			{
				kwargs = new Dictionary<string, object>();
			}

			// Combine positional and keyword arguments
			List<object> finalArgs = new List<object>(arguments);

			// Fill in keyword arguments by parameter name
			for (int i = finalArgs.Count; i < func.Parameters.Count; i++)
			{
				string paramName = func.Parameters[i];
				if (kwargs.ContainsKey(paramName))
				{
					finalArgs.Add(kwargs[paramName]);
				}
				else
				{
					throw new RuntimeError($"Missing required argument: '{paramName}'");
				}
			}

			// Check for extra keyword arguments
			foreach (string kwargName in kwargs.Keys)
			{
				if (!func.Parameters.Contains(kwargName))
				{
					throw new RuntimeError($"Function got unexpected keyword argument: '{kwargName}'");
				}
			}

			if (finalArgs.Count != func.Parameters.Count)
			{
				throw new RuntimeError($"Function expects {func.Parameters.Count} arguments, got {finalArgs.Count}");
			}

			recursionDepth++;
			if (recursionDepth > MAX_RECURSION_DEPTH)
			{
				throw new RuntimeError("Maximum recursion depth exceeded");
			}

			Scope functionScope = new Scope(currentScope);

			if (instance != null)
			{
				functionScope.Define("self", instance);
			}

			for (int i = 0; i < func.Parameters.Count; i++)
			{
				functionScope.Define(func.Parameters[i], finalArgs[i]);
			}

			Scope previousScope = currentScope;
			currentScope = functionScope;

			try
			{
				foreach (Stmt stmt in func.Body)
				{
					object result = ExecuteStatementSync(stmt);

					if (result is IEnumerator)
					{
						throw new RuntimeError("Cannot use sleep() or animation functions inside comparison/key functions");
					}
				}

				recursionDepth--;
				return null;
			}
			catch (ReturnException e)
			{
				recursionDepth--;
				currentScope = previousScope;
				return e.Value;
			}
			finally
			{
				currentScope = previousScope;
			}
		}
		#endregion

		#region Built-in Functions

		private object Print(List<object> args)
		{
			List<string> parts = new List<string>();

			foreach (object arg in args)
				parts.Add(ToString(arg));

			string output = string.Join(" ", parts.ToArray());
			// Write to both Unity console AND ConsoleManager
			Debug.Log(output);

			if (this.console != null)
				this.console.WriteLine(output);
			return null;
		}

		private IEnumerator Sleep(List<object> args)
		{
			if (args.Count != 1)
			{
				throw new RuntimeError("sleep() expects 1 argument");
			}

			float seconds = (float)ToNumber(args[0]);
			yield return new WaitForSeconds(seconds);
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

			// Python's int() truncates floats: int(10.5) -> 10
			double num = ToNumber(args[0]);
			return (double)(int)num;
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
				result.Sort((a, b) =>
				{
					object keyA = keyFunc.Call(this, new List<object> { a });
					object keyB = keyFunc.Call(this, new List<object> { b });

					int comparison = CompareObjects(keyA, keyB);

					return reverse ? -comparison : comparison;
				});
			}
			else
			{
				// Default sort
				result.Sort((a, b) =>
				{
					int comparison = CompareObjects(a, b);

					return reverse ? -comparison : comparison;
				});
			}

			return result;
		}

		/// <summary>
		/// Compare two objects for sorting (handles numbers, strings, lists, etc.)
		/// </summary>
		private int CompareObjects(object a, object b)
		{
			// Handle null
			if (a == null && b == null) return 0;
			if (a == null) return -1;
			if (b == null) return 1;

			// Both are numbers
			if ((a is double || a is int || a is float) && (b is double || b is int || b is float))
			{
				double numA = ToNumber(a);
				double numB = ToNumber(b);
				return numA.CompareTo(numB);
			}

			// Both are strings
			if (a is string && b is string)
			{
				return string.Compare((string)a, (string)b, StringComparison.Ordinal);
			}

			// Both are booleans
			if (a is bool && b is bool)
			{
				bool boolA = (bool)a;
				bool boolB = (bool)b;
				return boolA.CompareTo(boolB);
			}

			// Both are lists (compare element by element)
			if (a is List<object> && b is List<object>)
			{
				List<object> listA = (List<object>)a;
				List<object> listB = (List<object>)b;

				int minLen = Math.Min(listA.Count, listB.Count);
				for (int i = 0; i < minLen; i++)
				{
					int cmp = CompareObjects(listA[i], listB[i]);
					if (cmp != 0) return cmp;
				}

				// If all elements equal, shorter list comes first
				return listA.Count.CompareTo(listB.Count);
			}

			// Try to convert both to numbers as fallback
			try
			{
				double numA = ToNumber(a);
				double numB = ToNumber(b);
				return numA.CompareTo(numB);
			}
			catch
			{
				// If conversion fails, compare by type name then string representation
				string typeA = a.GetType().Name;
				string typeB = b.GetType().Name;

				int typeComparison = string.Compare(typeA, typeB, StringComparison.Ordinal);
				if (typeComparison != 0) return typeComparison;

				return string.Compare(ToString(a), ToString(b), StringComparison.Ordinal);
			}
		}

		#endregion
	}
}