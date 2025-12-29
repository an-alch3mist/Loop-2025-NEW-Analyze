=== LOOP LANGUAGE FEATURE REQUEST ===

[FULL PROJECT MAP PASTED HERE - 500+ lines]

FEATURE REQUEST:
The Test Runner is failing as for the following when ran

REQUIREMENTS:
1. Should successfully execute the edge cases such as without error, here's the log report from unity after runAllTests is made:
	========================================
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunAllTests>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:97)
	UnityEngine.MonoBehaviour:StartCoroutine (System.Collections.IEnumerator)
	LoopLanguage.TestRunner:RunAllTestsButton () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:62)

	STARTING COMPREHENSIVE TEST SUITE
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunAllTests>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:98)
	UnityEngine.MonoBehaviour:StartCoroutine (System.Collections.IEnumerator)
	LoopLanguage.TestRunner:RunAllTestsButton () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:62)

	Total tests: 86
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunAllTests>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:99)
	UnityEngine.MonoBehaviour:StartCoroutine (System.Collections.IEnumerator)
	LoopLanguage.TestRunner:RunAllTestsButton () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:62)

	(35 original + 45 extended tests)
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunAllTests>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:100)
	UnityEngine.MonoBehaviour:StartCoroutine (System.Collections.IEnumerator)
	LoopLanguage.TestRunner:RunAllTestsButton () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:62)

	========================================
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunAllTests>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:101)
	UnityEngine.MonoBehaviour:StartCoroutine (System.Collections.IEnumerator)
	LoopLanguage.TestRunner:RunAllTestsButton () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:62)


	[TEST 1] Running: Lambda with list comprehension inside
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.MonoBehaviour:StartCoroutine (System.Collections.IEnumerator)
	LoopLanguage.TestRunner:RunAllTestsButton () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:62)

	[TEST 1] ✗ FAILED: Lambda with list comprehension inside
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:201)
	UnityEngine.MonoBehaviour:StartCoroutine (System.Collections.IEnumerator)
	LoopLanguage.TestRunner:RunAllTestsButton () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:62)

	Error: Parse error at line 4: Expected 'else' in conditional expression at line 4, got ']'
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:202)
	UnityEngine.MonoBehaviour:StartCoroutine (System.Collections.IEnumerator)
	LoopLanguage.TestRunner:RunAllTestsButton () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:62)


	[TEST 2] Running: Sorted with lambda accessing tuple elements
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 2] ✗ FAILED: Sorted with lambda accessing tuple elements
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:201)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Error: Parse error at line 4: Expected ')' after arguments at line 4, got '='
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:202)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 3] Running: Lambda with multiple conditions
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 3] ✗ FAILED: Lambda with multiple conditions
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:201)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Error: Parse error at line 5: Expected 'else' in conditional expression at line 5, got ']'
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:202)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 4] Running: Immediately invoked lambda expression (IIFE)
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 4] ✗ FAILED: Immediately invoked lambda expression (IIFE)
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:201)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Error: Parse error at line 7: Expected 'else' in conditional expression at line 7, got ']'
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:202)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 5] Running: Lambda with nested indexing
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 5] ✗ FAILED: Lambda with nested indexing
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:201)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Error: Parse error at line 4: Expected ')' after arguments at line 4, got '='
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:202)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 6] Running: Lambda with multiple parameters
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	20
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[5, 10]
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 6] ✓ PASSED: Lambda with multiple parameters
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 7] Running: Tuple creation and indexing
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	1
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	a
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	3.14
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	3.14
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 7] ✓ PASSED: Tuple creation and indexing
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 8] Running: Tuple in list, iteration
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	3
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	7
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	11
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 8] ✓ PASSED: Tuple in list, iteration
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 9] Running: Single element tuple
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	1
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	42
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 9] ✓ PASSED: Single element tuple
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 10] Running: Enum member access and comparison
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Standing on soil
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteIf>d__19:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:345)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 10] ✓ PASSED: Enum member access and comparison
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 11] Running: Using enum in function calls
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Planted carrot at (0, 0)
	UnityEngine.Debug:Log (object)
	LoopLanguage.GameBuiltinMethods/<Plant>d__7:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/GameBuiltinMethods.cs:114)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Harvested! Hay count: 1
	UnityEngine.Debug:Log (object)
	LoopLanguage.GameBuiltinMethods/<Harvest>d__6:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/GameBuiltinMethods.cs:98)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteIf>d__19:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:345)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 11] ✓ PASSED: Using enum in function calls
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 12] Running: Directional constants in movement
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Moved to (0, 0)
	UnityEngine.Debug:Log (object)
	LoopLanguage.GameBuiltinMethods/<Move>d__5:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/GameBuiltinMethods.cs:80)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Moved to (1, 0)
	UnityEngine.Debug:Log (object)
	LoopLanguage.GameBuiltinMethods/<Move>d__5:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/GameBuiltinMethods.cs:80)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Moved to (1, 1)
	UnityEngine.Debug:Log (object)
	LoopLanguage.GameBuiltinMethods/<Move>d__5:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/GameBuiltinMethods.cs:80)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Moved to (0, 1)
	UnityEngine.Debug:Log (object)
	LoopLanguage.GameBuiltinMethods/<Move>d__5:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/GameBuiltinMethods.cs:80)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Movement complete
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 12] ✓ PASSED: Directional constants in movement
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 13] Running: Exponentiation operator precedence
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	512
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	18
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	64
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 13] ✓ PASSED: Exponentiation operator precedence
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 14] Running: Comprehensive operator test
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 14] ✗ FAILED: Comprehensive operator test
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:201)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Error: Parse error at line 32: Expected ')' after arguments at line 32, got '<<'
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:202)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 15] Running: Negative indexing
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	50
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	40
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	10
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 15] ✓ PASSED: Negative indexing
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 16] Running: List slicing
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[2, 3, 4]
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[0, 1, 2]
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[7, 8, 9]
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[0, 2, 4, 6, 8]
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[1, 3, 5, 7]
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 16] ✓ PASSED: List slicing
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 17] Running: List comprehension
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 17] ✗ FAILED: List comprehension
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:201)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	Error: Parse error at line 7: Expected 'else' in conditional expression at line 7, got ']'
	UnityEngine.Debug:LogError (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:202)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 18] Running: Nested function calls as arguments
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	12
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 18] ✓ PASSED: Nested function calls as arguments
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 19] Running: Deeply nested function calls
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	26
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 19] ✓ PASSED: Deeply nested function calls
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 20] Running: Nested for loops
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	0
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	1
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	2
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	3
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	4
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	5
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	6
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	7
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	8
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 20] ✓ PASSED: Nested for loops
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 21] Running: Nested while loops
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	0
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	1
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	2
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	3
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	4
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	5
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	6
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	7
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	8
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<ExecuteWhile>d__20:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:415)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 21] ✓ PASSED: Nested while loops
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 22] Running: Triple nested loops
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	8
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 22] ✓ PASSED: Triple nested loops
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 23] Running: Recursion with factorial
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	120
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	720
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 23] ✓ PASSED: Recursion with factorial
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 24] Running: Recursion with Fibonacci
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	55
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	[TEST 24] ✓ PASSED: Recursion with Fibonacci
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:195)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)


	[TEST 25] Running: Break and continue in loops
	UnityEngine.Debug:Log (object)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:128)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	0
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	1
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	2
	UnityEngine.Debug:Log (object)
	LoopLanguage.PythonInterpreter:Print (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:1259)
	LoopLanguage.BuiltinFunction:Call (System.Collections.Generic.List`1<object>) (at Assets/Scripts/LOOP-2025 [NEW]/BuiltinFunction.cs:68)
	LoopLanguage.PythonInterpreter:EvaluateCall (LoopLanguage.CallExpr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:727)
	LoopLanguage.PythonInterpreter:Evaluate (LoopLanguage.Expr) (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:556)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:156)
	LoopLanguage.PythonInterpreter/<ExecuteFor>d__21:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter/<ExecuteStatement>d__15:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter/<Execute>d__11:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner/<RunSingleTest>d__12:MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine:InvokeMoveNext (System.Collections.IEnumerator,intptr)

	ContinueException: continue statement used outside loop
	LoopLanguage.PythonInterpreter+<ExecuteStatement>d__15.MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:225)
	LoopLanguage.PythonInterpreter+<ExecuteIf>d__19.MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:345)
	LoopLanguage.PythonInterpreter+<ExecuteStatement>d__15.MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter+<ExecuteFor>d__21.MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:473)
	LoopLanguage.PythonInterpreter+<ExecuteStatement>d__15.MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:220)
	LoopLanguage.PythonInterpreter+<Execute>d__11.MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/PythonInterpreter.cs:62)
	LoopLanguage.TestRunner+<RunSingleTest>d__12.MoveNext () (at Assets/Scripts/LOOP-2025 [NEW]/TestRunner.cs:165)
	UnityEngine.SetupCoroutine.InvokeMoveNext (System.Collections.IEnumerator enumerator, System.IntPtr returnValueAddress) (at <913bb12059fd4cef8da5cc94ad2f0933>:0)

	2. 
CONSTRAINTS:
- Must maintain .NET 2.0 compliance(do not yield return value/null/anything inside tr-catch clause which is inside IEnumerator)
- Must respect instruction budget system

TASK: SCOUT MODE (DO NOT GENERATE CODE YET)

Please analyze the Project Map above and tell me:
1. Which 2-4 specific files need to be modified?
2. What is the high-level implementation approach?
3. Are there any architectural concerns?
4. What test cases should be added?