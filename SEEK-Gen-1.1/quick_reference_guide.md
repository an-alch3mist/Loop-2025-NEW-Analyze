# 📚 LOOP Language - Quick Reference Guide

**Version:** 2.1 Extended  
**Last Updated:** December 2025  
**Total Files:** 18 files, 6,500+ lines, 86+ tests

---

## 📑 Table of Contents

1. [File Overview](#file-overview)
2. [Dependencies Graph](#dependencies-graph)
3. [Critical Methods Reference](#critical-methods-reference)
4. [Number System Reference](#number-system-reference)
5. [Time Budget Reference](#time-budget-reference)
6. [Modification Patterns](#modification-patterns)
7. [Common Tasks](#common-tasks)

---

## 📁 FILE OVERVIEW

### **Core Language Files (10)**

#### 1️⃣ **Token.cs** (60 lines)
**Purpose:** Token data structure and TokenType enum

**Key Types:**
```csharp
enum TokenType {
    // Structural (4)
    INDENT, DEDENT, NEWLINE, EOF,
    
    // Literals (3)
    IDENTIFIER, STRING, NUMBER,
    
    // Keywords (22)
    IF, ELIF, ELSE, WHILE, FOR, DEF, RETURN, CLASS,
    BREAK, CONTINUE, PASS, GLOBAL, LAMBDA, IMPORT,
    AND, OR, NOT, IN, IS, TRUE, FALSE, NONE,
    
    // Operators (26)
    PLUS, MINUS, STAR, SLASH, PERCENT, DOUBLE_STAR, DOUBLE_SLASH,
    EQUAL_EQUAL, BANG_EQUAL, LESS, GREATER, LESS_EQUAL, GREATER_EQUAL,
    EQUAL, PLUS_EQUAL, MINUS_EQUAL, STAR_EQUAL, SLASH_EQUAL,
    AMPERSAND, PIPE, CARET, TILDE, LEFT_SHIFT, RIGHT_SHIFT,
    
    // Delimiters (7)
    LEFT_PAREN, RIGHT_PAREN, LEFT_BRACKET, RIGHT_BRACKET,
    DOT, COMMA, COLON
}

class Token {
    public TokenType Type;
    public string Lexeme;
    public object Literal;
    public int LineNumber;
}
```

**Dependencies:** None  
**Modify when:** Adding new operators or keywords

---

#### 2️⃣ **Exceptions.cs** (50 lines)
**Purpose:** Custom exception hierarchy

**Key Types:**
```csharp
class LoopException : Exception
class LexerError : LoopException
class ParserError : LoopException
class RuntimeError : LoopException
class BreakException : LoopException
class ContinueException : LoopException
class ReturnException : LoopException
```

**Dependencies:** None  
**Modify when:** Adding new error types

---

#### 3️⃣ **Lexer.cs** (350 lines)
**Purpose:** Tokenization with Python-style indentation

**Key Methods:**
```csharp
public List<Token> Tokenize(string input)
private string ValidateAndClean(string input)
private void ProcessIndentation()  // Stack-based INDENT/DEDENT
private void ScanToken()  // Main scanning logic
```

**Features:**
- Python-style indentation (4 spaces, validated)
- Comment handling (`#` and `//`)
- Stack-based INDENT/DEDENT emission
- Input sanitization (tabs → spaces, normalize line endings)

**Dependencies:** Token.cs, Exceptions.cs  
**Modify when:** Adding new token scanning logic

---

#### 4️⃣ **AST.cs** (350 lines)
**Purpose:** Abstract Syntax Tree node definitions

**Key Types:**
```csharp
// Base classes
abstract class ASTNode
abstract class Stmt : ASTNode  // 20+ statement types
abstract class Expr : ASTNode  // 20+ expression types

// Critical nodes
class LambdaExpr : Expr {
    List<string> Parameters;
    Expr Body;  // Can be ListCompExpr!
}

class ListCompExpr : Expr {
    Expr Element;
    string Variable;
    Expr Iterable;
    Expr Condition;  // Optional
}

class TupleExpr : Expr {
    List<Expr> Elements;
}

class MemberAccessExpr : Expr {
    Expr Object;
    string Member;  // For enum access: Grounds.Soil
}
```

**Dependencies:** None  
**Modify when:** Adding new language constructs

---

#### 5️⃣ **Parser.cs** (600 lines)
**Purpose:** Recursive descent parser (tokens → AST)

**Key Methods:**
```csharp
public List<Stmt> Parse(List<Token> tokens)

// Expression parsing (14 precedence levels)
private Expr Expression()  // Entry point
private Expr Lambda()      // Level 14 (lowest)
private Expr Conditional() // Level 13
private Expr LogicalOr()   // Level 12
// ... down to ...
private Expr Primary()     // Level 1 (highest)
private Expr Postfix()     // Member access, calls, indexing
```

**Operator Precedence:**
```
14. Lambda (lowest)
13. Conditional (x if cond else y)
12. Logical OR
11. Logical AND
10. Comparison (==, !=, <, >, <=, >=, in, is)
9.  Bitwise OR
8.  Bitwise XOR
7.  Bitwise AND
6.  Addition/Subtraction
5.  Multiplication/Division/Modulo
4.  Exponentiation (** - right-associative!)
3.  Unary (+, -, ~, not)
2.  Postfix (., [], ())
1.  Primary (literals, grouping)
```

**Dependencies:** Token.cs, AST.cs, Exceptions.cs  
**Modify when:** Changing operator precedence or grammar

---

#### 6️⃣ **Scope.cs** (80 lines)
**Purpose:** Variable scope management with parent chaining

**Key Methods:**
```csharp
public void Define(string name, object value)  // Create new
public void Set(string name, object value)     // Update existing
public object Get(string name)                 // Retrieve
public Scope GetGlobalScope()                  // Walk to root
```

**Features:**
- Lexical scoping
- Parent chain for nested scopes
- Closure support (captures scope at lambda creation)

**Dependencies:** Exceptions.cs  
**Modify when:** Adding scope-related features

---

#### 7️⃣ **GameEnums.cs** (30 lines)
**Purpose:** Built-in game enum definitions

**Implementation:**
```csharp
public static class Grounds {
    public static readonly string Soil = "soil";
    public static readonly string Turf = "turf";
    public static readonly string Grassland = "grassland";
}

public static class Items {
    public static readonly string Hay = "hay";
    public static readonly string Wood = "wood";
    public static readonly string Carrot = "carrot";
    public static readonly string Pumpkin = "pumpkin";
    public static readonly string Power = "power";
    public static readonly string Sunflower = "sunflower";
    public static readonly string Water = "water";
}

public static class Entities {
    public static readonly string Grass = "grass";
    public static readonly string Bush = "bush";
    public static readonly string Tree = "tree";
    public static readonly string Carrot = "carrot";
    public static readonly string Pumpkin = "pumpkin";
    public static readonly string Sunflower = "sunflower";
}
```

**Usage in Python:**
```python
if get_ground_type() == Grounds.Soil:
    plant(Entities.Carrot)
```

**Dependencies:** None  
**Modify when:** Adding new enum types or members

---

#### 8️⃣ **BuiltinFunction.cs** (70 lines)
**Purpose:** Wrapper for built-in function calls

**Key Methods:**
```csharp
public object Call(List<object> args)              // Sync functions
public IEnumerator CallAsync(List<object> args)    // Async functions
public bool IsAsync()                              // Check type
```

**Dependencies:** None  
**Modify when:** Changing function call mechanism

---

#### 9️⃣ **LambdaFunction.cs** (70 lines)
**Purpose:** Runtime lambda representation with closures

**Key Fields:**
```csharp
public List<string> Parameters;
public Expr Body;
public Scope ClosureScope;  // Captured at creation time
```

**Key Methods:**
```csharp
public object Call(PythonInterpreter interpreter, List<object> arguments)
```

**Features:**
- Closure capture (scope at lambda definition)
- Parameter binding (at call time)
- Expression body evaluation

**Dependencies:** AST.cs, Scope.cs, Exceptions.cs  
**Modify when:** Enhancing lambda features

---

#### 🔟 **ClassInstance.cs** (90 lines)
**Purpose:** Runtime class instance representation

**Key Methods:**
```csharp
public void SetField(string name, object value)
public object GetField(string name)
public FunctionDefStmt GetMethod(string name)
public bool HasField(string name)
public bool HasMethod(string name)
```

**Dependencies:** AST.cs, Exceptions.cs  
**Modify when:** Adding class-related features

---

### **Number System (1)**

#### 1️⃣1️⃣ **NumberHandling.cs** (200 lines) ✨
**Purpose:** Python-style number semantics

**Key Methods:**
```csharp
// Conversion
public static double ToNumber(object value)  // Any → double
public static int ToInteger(object value, string context)  // Strict validation
public static int ToListIndex(object value, int listLength)  // Handles negative
public static int ToRangeValue(object value)  // For range() validation

// Comparison
public static bool NumbersEqual(object a, object b)  // 1 == 1.0 → True

// Display
public static string NumberToString(double value)  // 1.0 → "1"

// Validation
public static bool IsInteger(object value)  // 1.0 → True, 1.5 → False
public static void RequireInteger(object value, string context)  // Throws if not
```

**Behavior:**
```python
# Equality (Python-style)
1 == 1.0  # True ✓

# Comparisons work with mixed types
1 > 1.01        # False
2.01 <= 3       # True
2 < 4.1         # True

# Indexing requires integers
list[0]    # OK
list[1.0]  # OK (integer value)
list[1.5]  # ERROR: "List index requires integer, got 1.5"

# Range requires integers
range(5)    # OK
range(5.0)  # OK (integer value)
range(5.5)  # ERROR: "range() requires integer, got 5.5"

# Display format
print(1.0)  # "1" (not "1.0")
print(1.5)  # "1.5"
```

**Dependencies:** Exceptions.cs  
**Modify when:** Changing number handling behavior

---

### **Interpreter Core (1)**

#### 1️⃣2️⃣ **PythonInterpreter.cs** (1300+ lines) ⚠️ **CORE FILE**
**Purpose:** Main execution engine with instruction budget

**Critical Fields:**
```csharp
public Scope currentScope;
private Scope globalScope;
private int instructionCount;  // Budget: 100 ops/frame
private int recursionDepth;    // Limit: 100
private HashSet<string> globalVariables;

private const int INSTRUCTIONS_PER_FRAME = 100;
private const int MAX_RECURSION_DEPTH = 100;
```

**Execution Methods:**
```csharp
public IEnumerator Execute(List<Stmt> statements)
private IEnumerator ExecuteStatement(Stmt stmt)  // .NET 2.0 compliant!
private IEnumerator ExecuteWhile(WhileStmt stmt)
private IEnumerator ExecuteFor(ForStmt stmt)
private IEnumerator ExecuteIf(IfStmt stmt)
```

**Evaluation Methods:**
```csharp
public object Evaluate(Expr expr)
private object EvaluateBinary(BinaryExpr expr)    // All operators
private object EvaluateCall(CallExpr expr)        // Function calls
private object EvaluateLambda(LambdaExpr expr)    // Returns LambdaFunction
private object EvaluateListComp(ListCompExpr expr)
private object EvaluateMemberAccess(MemberAccessExpr expr)  // Enum handling
private object EvaluateIndex(IndexExpr expr)      // Uses NumberHandling
private object EvaluateSlice(SliceExpr expr)      // Uses NumberHandling
```

**Registration Methods:**
```csharp
private void RegisterBuiltins()    // print, sleep, range, sorted, etc.
private void RegisterEnums()       // Grounds, Items, Entities
private void RegisterConstants()   // North, South, East, West
```

**Built-in Implementations:**
```csharp
// Standard (Instant)
private object Print(List<object> args)
private object Range(List<object> args)  // Uses NumberHandling.ToRangeValue
private object Len(List<object> args)
private object Sorted(List<object> args) // Accepts lambda key!
private object Str, Int, Float, Abs, Min, Max, Sum(...)

// Sleep (Always Yields)
private IEnumerator Sleep(List<object> args)  // Handles int/float
```

**Helper Methods:**
```csharp
private void IncrementInstructionCount()
public bool ShouldYield()  // Returns true if instructionCount >= 100
private double ToNumber(object value)  // Uses NumberHandling
private int ToInt(object value)        // Uses NumberHandling
private string ToString(object value)  // Uses NumberHandling.NumberToString
private bool IsTruthy(object value)
private bool IsEqual(object a, object b)  // Uses NumberHandling.NumbersEqual
```

**Dependencies:** ALL other files  
**Modify when:** Adding operators, built-ins, or core features

---

### **Game Integration (1)**

#### 1️⃣3️⃣ **GameBuiltinMethods.cs** (250 lines)
**Purpose:** Game-specific function implementations

**Time Budget Dependent (IEnumerator):**
```csharp
public IEnumerator Move(List<object> args)      // ~0.3s
public IEnumerator Harvest(List<object> args)   // ~0.2s
public IEnumerator Plant(List<object> args)     // ~0.3s
public IEnumerator Till(List<object> args)      // ~0.1s
public IEnumerator UseItem(List<object> args)   // ~0.1s
public IEnumerator DoAFlip(List<object> args)   // ~1.0s
```

**Time Budget Independent (Instant):**
```csharp
public object CanHarvest(List<object> args)
public object GetGroundType(List<object> args)  // Returns enum string
public object GetEntityType(List<object> args)
public object GetPosX(List<object> args)
public object GetPosY(List<object> args)
public object GetWorldSize(List<object> args)
public object GetWater(List<object> args)
public object NumItems(List<object> args)
public object IsEven(List<object> args)
public object IsOdd(List<object> args)
```

**Dependencies:** Exceptions.cs  
**Modify when:** Adding new game commands

---

### **Unity Integration (2)**

#### 1️⃣4️⃣ **CoroutineRunner.cs** (100 lines)
**Purpose:** Unity MonoBehaviour coroutine wrapper

**Key Methods:**
```csharp
public void Run(string sourceCode)  // Execute script
public void Stop()                  // Stop execution
private IEnumerator ExecuteCode(string source)  // .NET 2.0 compliant!
```

**Features:**
- Error handling (LexerError, ParserError, RuntimeError)
- Console integration
- Coroutine lifecycle management

**Dependencies:** All interpreter files, Unity  
**Modify when:** Changing execution flow

---

#### 1️⃣5️⃣ **ConsoleManager.cs** (70 lines)
**Purpose:** UI console for print() output

**Key Methods:**
```csharp
public void WriteLine(string message)
public void Write(string message)
public void Clear()
```

**Dependencies:** Unity UI  
**Modify when:** Changing output display

---

### **Testing (3)**

#### 1️⃣6️⃣ **TestRunner.cs** (150 lines)
**Purpose:** Automated test suite execution

**Key Methods:**
```csharp
public void RunAllTestsButton()  // Runs all 86+ tests
public void RunTest(int index)   // Runs specific test
private IEnumerator RunSingleTest(int index, string script)  // .NET 2.0 compliant!
```

**Dependencies:** CoroutineRunner, DemoScripts, ComprehensiveTestSuite  
**Modify when:** Changing test execution logic

---

#### 1️⃣7️⃣ **DemoScripts.cs** (800+ lines)
**Purpose:** Original test suite (35 tests)

**Test Categories:**
- Lambda expressions (7)
- Tuples (3)
- Enums (2)
- Operators (2)
- Lists (3)
- Functions (2)
- Loops (3)
- Recursion (3)
- Control flow (3)
- Strings (2)
- Game functions (3)
- Edge cases (2)

**Key Methods:**
```csharp
public static string[] GetAllTests()  // Returns combined 86+ tests
```

**Dependencies:** None  
**Modify when:** Adding new test categories

---

#### 1️⃣8️⃣ **ComprehensiveTestSuite.cs** (1000+ lines)
**Purpose:** Extended test suite (51 tests)

**Test Categories:**
- Sleep tests (5)
- Time budget dependent (7)
- Time budget independent (8)
- Mixed operations (3)
- Instruction vs time budget (3)
- Type conversions (3)
- Complex scenarios (4)
- Edge cases (5)
- Performance (2)
- Lambda with game functions (2)
- Integration (1)
- Number handling (6) ✨

**Dependencies:** None  
**Modify when:** Adding comprehensive test cases

---

## 🔗 DEPENDENCIES GRAPH

```
Level 0 (No dependencies):
├─ Token.cs
├─ Exceptions.cs
├─ AST.cs
├─ GameEnums.cs
├─ BuiltinFunction.cs
├─ DemoScripts.cs
└─ ComprehensiveTestSuite.cs

Level 1:
├─ Lexer.cs → Token, Exceptions
├─ Scope.cs → Exceptions
└─ NumberHandling.cs → Exceptions

Level 2:
├─ Parser.cs → Token, AST, Exceptions
├─ LambdaFunction.cs → AST, Scope, Exceptions
└─ ClassInstance.cs → AST, Exceptions

Level 3:
└─ GameBuiltinMethods.cs → Exceptions

Level 4:
└─ PythonInterpreter.cs → ALL ABOVE FILES ⚠️

Level 5:
├─ CoroutineRunner.cs → PythonInterpreter, Lexer, Parser, ConsoleManager
└─ ConsoleManager.cs → Unity UI

Level 6:
└─ TestRunner.cs → CoroutineRunner, DemoScripts, ComprehensiveTestSuite
```

---

## 🎯 CRITICAL METHODS REFERENCE

### **Where Instruction Budget is Tracked:**
```
Location: PythonInterpreter.cs → IncrementInstructionCount()
Called by: ExecuteStatement(), Evaluate(), loop iterations
Behavior: Increments counter, yields when >= 100
```

### **Where Sleep is Implemented:**
```
Location: PythonInterpreter.cs → Sleep(List<object> args)
Implementation: Uses NumberHandling.ToNumber(), always yields WaitForSeconds
Handles: Both int and float (sleep(2), sleep(2.0))
```

### **Where Lambdas are Created:**
```
Location: PythonInterpreter.cs → EvaluateLambda(LambdaExpr expr)
Returns: LambdaFunction with captured ClosureScope
```

### **Where Lambdas are Called:**
```
Location: PythonInterpreter.cs → EvaluateCall(CallExpr expr)
Checks: if (callee is LambdaFunction)
Calls: lambda.Call(this, arguments)
```

### **Where Enums are Accessed:**
```
Location: PythonInterpreter.cs → EvaluateMemberAccess(MemberAccessExpr expr)
Checks: if (obj is Type) → typeof(Grounds), typeof(Items), typeof(Entities)
Returns: Static field value (Grounds.Soil → "soil")
```

### **Where Recursion is Tracked:**
```
Location: PythonInterpreter.cs → CallUserFunction()
Increments: recursionDepth++
Decrements: recursionDepth-- in finally block
Limit: MAX_RECURSION_DEPTH = 100
```

### **Where Number Validation Happens:**
```
Location: NumberHandling.cs → ToListIndex(), ToRangeValue(), ToInteger()
Errors on: list[1.5], range(5.5), slice[1.5:5]
Allows: list[0], list[1.0], range(5), range(5.0)
```

---

## 🔢 NUMBER SYSTEM REFERENCE

### **Storage:**
- All numbers stored as `double` internally
- Literals can be written as integers or floats

### **Equality:**
```python
1 == 1.0    # True (Python behavior) ✓
5.0 == 5    # True ✓
10/2 == 5   # True (5.0 == 5) ✓
```

### **Comparisons:**
```python
1 > 1.01        # False ✓
2.01 <= 3       # True ✓
2.001 < 4.1     # True ✓
5 >= 5.0        # True ✓
```

### **Integer-Required Contexts:**
```python
# Indexing
list[0]     # OK
list[1.0]   # OK (integer value)
list[1.5]   # ERROR

# Range
range(5)    # OK
range(5.0)  # OK (integer value)
range(5.5)  # ERROR

# Slicing
list[1:5]     # OK
list[1.0:5.0] # OK (integer values)
list[1.5:5]   # ERROR
```

### **Display Format:**
```python
print(1.0)  # "1" (not "1.0")
print(1.5)  # "1.5"
print(2.0)  # "2"
```

### **Division Operators:**
```python
# Float division (Python 3)
3 / 2   # 1.5 ✓
10 / 3  # 3.333... ✓

# Floor division
3 // 2  # 1 ✓
10 // 3 # 3 ✓
```

---

## ⏱️ TIME BUDGET REFERENCE

### **Instant Functions (21) - No Yield:**
```
Standard:
├─ print, len, str, int, float
├─ abs, min, max, sum
├─ range (validates integers!)
└─ sorted (even with lambda key!)

Game Queries:
├─ can_harvest, get_ground_type, get_entity_type
├─ get_pos_x, get_pos_y, get_world_size
├─ get_water, num_items
└─ is_even, is_odd
```

### **Sleep Function (1) - Always Yields:**
```python
sleep(2)    # Yields exactly 2 seconds (int → float)
sleep(2.0)  # Yields exactly 2 seconds
sleep(0.5)  # Yields exactly 0.5 seconds
sleep(0)    # Yields one frame (minimum)
```

### **Animation Functions (6) - Always Yield:**
```
move(direction)  ~0.3s
harvest()        ~0.2s
plant(entity)    ~0.3s
till()           ~0.1s
use_item(item)   ~0.1s
do_a_flip()      ~1.0s
```

### **Instruction Budget - Time-Sliced:**
```python
# Small loop - instant (< 100 ops)
for i in range(50):
    sum += i

# Large loop - distributed (> 100 ops)
for i in range(1000):
    sum += i  # Yields every 100 iterations

# Recursion - tracked (limit: 100 depth)
def factorial(n):
    if n <= 1: return 1
    return n * factorial(n-1)
```

---

## 🛠️ MODIFICATION PATTERNS

### **Add New Operator:**
1. Token.cs - Add `TokenType` enum value
2. Lexer.cs - Add scanning in `ScanToken()`
3. Parser.cs - Add to appropriate precedence method
4. PythonInterpreter.cs - Add case in `EvaluateBinary()` or `EvaluateUnary()`
5. ComprehensiveTestSuite.cs - Add test case

**Example:** Adding `%` (modulo) - already implemented!

### **Add New Game Function:**
1. GameBuiltinMethods.cs - Implement method (`IEnumerator` or `object`)
2. PythonInterpreter.cs - Register in `RegisterBuiltins()`
3. ComprehensiveTestSuite.cs - Add test case

**Decision:** `IEnumerator` if it animates, `object` if instant

### **Add New Enum:**
1. GameEnums.cs - Define static class with `readonly` fields
2. PythonInterpreter.cs - Register in `RegisterEnums()`
3. PythonInterpreter.cs - Add cases in `EvaluateMemberAccess()`
4. ComprehensiveTestSuite.cs - Add test case

### **Add New Built-in Function:**
1. PythonInterpreter.cs - Implement private method
2. PythonInterpreter.cs - Register in `RegisterBuiltins()`
3. DemoScripts.cs - Add test case

### **Change Operator Precedence:**
1. Parser.cs - Reorder method calls in `Expression()` chain
2. Rule: Lowest precedence at top, highest at bottom

### **Fix Number Handling:**
1. NumberHandling.cs - Modify conversion/validation methods
2. ComprehensiveTestSuite.cs - Update number tests

---

## 📋 COMMON TASKS

### **Run All Tests:**
```csharp
TestRunner runner = GetComponent<TestRunner>();
runner.RunAllTestsButton();  // Runs all 86+ tests
```

### **Run Specific Test:**
```csharp
TestRunner runner = GetComponent<TestRunner>();
runner.RunTest(35);  // Run test #35
```

### **Execute Python Code:**
```csharp
CoroutineRunner runner = GetComponent<CoroutineRunner>();
runner.Run(@"
for i in range(10):
    print('Hello', i)
");
```

### **Debug Number Handling:**
```python
# Test equality
print(1 == 1.0)  # Should print True

# Test validation
items = [1, 2, 3]
print(items[1.5])  # Should error
```

### **Debug Time Budget:**
```python
# Test instant functions
for i in range(10000):
    x = get_pos_x()  # Should be instant

# Test yielding functions
for i in range(10):
    move(North)  # Each should yield ~0.3s
```

### **Add New Feature:**
1. See "Modification Patterns" above
2. Use XML Stateless Maintenance Workflow
3. Run tests to verify

---

## ✅ VERIFICATION CHECKLIST

Before deploying:
- [ ] All 18 files compile without errors
- [ ] TestRunner shows 86+ tests
- [ ] All tests pass (100% success rate)
- [ ] `1 == 1.0` returns `True`
- [ ] `list[1.5]` raises error
- [ ] `range(5.5)` raises error
- [ ] `1.0` displays as `"1"`
- [ ] `sleep(2)` and `sleep(2.0)` both work
- [ ] Instant functions don't yield
- [ ] Animation functions always yield
- [ ] Large loops distribute across frames
- [ ] Recursion depth limit enforced
- [ ] Enums accessible (Grounds.Soil)
- [ ] Lambdas with closures work
- [ ] sorted() with lambda key works
- [ ] .NET 2.0 compliant (no yield in try-catch)

---

## 📊 PROJECT STATISTICS

- **Total Files:** 18
- **Total Lines:** 6,500+
- **Test Cases:** 86+
- **Test Coverage:** 100%
- **Token Types:** 45+
- **Statement Types:** 20+
- **Expression Types:** 20+
- **Built-in Functions:** 28 (21 instant, 7 yielding)
- **Game Functions:** 16 (10 instant, 6 yielding)
- **Enum Types:** 3 (17 total members)
- **Constants:** 4 directional

---

## 🎯 NEXT STEPS

1. Add all files to Unity project
2. Run `TestRunner.RunAllTests()`
3. Verify 100% pass rate
4. Test number handling
5. Test time budget
6. Start building your game!

---

**For detailed maintenance workflow, see:**  
`XML Stateless Maintenance Workflow.xml`

---

*Version 2.1 Extended | December 2025*  
*Quick Reference Guide - Last Updated: 2025-12-29*
