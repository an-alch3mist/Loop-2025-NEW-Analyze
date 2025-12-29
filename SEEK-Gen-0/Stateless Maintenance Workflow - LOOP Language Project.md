# LOOP Language Project - Stateless Maintenance Workflow

## 📋 PART 1: PROJECT MAP TEMPLATE

Copy this entire section at the start of every new chat session:

```xml
<project_map>
  <project_name>LOOP Language - Unity C# Python Interpreter</project_name>
  <unity_version>2020.3+</unity_version>
  <dotnet_version>2.0 Standard</dotnet_version>
  <constraint>No `yield return` inside try-catch in IEnumerators</constraint>
  
  <architecture>
    <layer name="Lexer/Tokenization">
      <file>Token.cs</file>
      <responsibility>Defines TokenType enum and Token class</responsibility>
      <key_signatures>
        - enum TokenType { IDENTIFIER, NUMBER, STRING, IF, WHILE, FOR, ... }
        - class Token(TokenType, string lexeme, object literal, int line)
      </key_signatures>
    </layer>
    
    <layer name="Lexer/Tokenization">
      <file>Lexer.cs</file>
      <responsibility>Tokenizes source code, handles Python-style indentation</responsibility>
      <key_signatures>
        - List&lt;Token&gt; Tokenize()
        - ProcessIndentation() - emits INDENT/DEDENT tokens
        - Stack&lt;int&gt; indentStack
      </key_signatures>
    </layer>
    
    <layer name="Exceptions">
      <file>Exceptions.cs</file>
      <responsibility>All custom exception types</responsibility>
      <key_signatures>
        - LOOPException(base)
        - LexerError, ParseError, RuntimeError
        - NameError, TypeError, IndexError, KeyError
        - BreakException, ContinueException, ReturnException (control flow)
      </key_signatures>
    </layer>
    
    <layer name="AST">
      <file>AST.cs</file>
      <responsibility>All AST node definitions</responsibility>
      <key_signatures>
        - abstract class ASTNode { int LineNumber }
        - abstract class Stmt : ASTNode
        - abstract class Expr : ASTNode
        - Statement types: AssignmentStmt, IfStmt, WhileStmt, ForStmt, FunctionDefStmt, etc.
        - Expression types: BinaryExpr, UnaryExpr, LiteralExpr, CallExpr, IndexExpr, LambdaExpr, ListCompExpr, etc.
      </key_signatures>
    </layer>
    
    <layer name="Parser">
      <file>Parser.cs</file>
      <responsibility>Recursive descent parser, builds AST from tokens</responsibility>
      <key_signatures>
        - List&lt;Stmt&gt; Parse()
        - ParseStatement(), ParseExpression()
        - Operator precedence chain: ParseLambda() → ParseLogicalOr() → ParseLogicalAnd() → ParseComparison() → ... → ParseExponentiation() → ParseUnary() → ParsePrimary()
        - ParseSuite() - handles indented blocks
      </key_signatures>
    </layer>
    
    <layer name="Scope">
      <file>Scope.cs</file>
      <responsibility>Variable scope management with parent chain</responsibility>
      <key_signatures>
        - object Get(string name)
        - void Set(string name, object value)
        - void Update(string name, object value)
        - bool Contains(string name)
        - Scope parent
      </key_signatures>
    </layer>
    
    <layer name="Runtime">
      <file>BuiltinFunction.cs</file>
      <responsibility>Wrapper for built-in functions (print, len, range, etc.)</responsibility>
      <key_signatures>
        - Func&lt;List&lt;object&gt;, object&gt; Implementation
        - object Call(List&lt;object&gt; arguments, int lineNumber)
        - int MinArgs, int MaxArgs
      </key_signatures>
    </layer>
    
    <layer name="Runtime">
      <file>LambdaFunction.cs</file>
      <responsibility>Runtime representation of lambda expressions with closure support</responsibility>
      <key_signatures>
        - List&lt;string&gt; Parameters
        - Expr Body
        - Scope ClosureScope
      </key_signatures>
    </layer>
    
    <layer name="Runtime">
      <file>ClassInstance.cs</file>
      <responsibility>Runtime class instances and user-defined functions</responsibility>
      <key_signatures>
        - class ClassInstance { Dictionary&lt;string, object&gt; Fields; Dictionary&lt;string, UserFunction&gt; Methods }
        - class UserFunction { List&lt;string&gt; Parameters; List&lt;Stmt&gt; Body; Scope ClosureScope }
      </key_signatures>
    </layer>
    
    <layer name="Game">
      <file>GameEnums.cs</file>
      <responsibility>Game enum definitions (Grounds, Items, Entities)</responsibility>
      <key_signatures>
        - static class Grounds { Soil, Turf, Grassland }
        - static class Items { Hay, Wood, Carrot, Pumpkin, Power, Sunflower, Water }
        - static class Entities { Grass, Bush, Tree, Carrot, Pumpkin, Sunflower }
      </key_signatures>
    </layer>
    
    <layer name="Interpreter">
      <file>PythonInterpreter.cs</file>
      <responsibility>Main execution engine with instruction budget system</responsibility>
      <key_signatures>
        - IEnumerator Run(string source)
        - IEnumerator ExecuteStatements(List&lt;Stmt&gt; statements)
        - IEnumerator ExecuteStatement(Stmt stmt)
        - object Evaluate(Expr expr)
        - object EvaluateBinaryExpr(BinaryExpr expr)
        - object CallFunction(object callee, List&lt;object&gt; arguments, int lineNumber)
        - object CallLambda(LambdaFunction lambda, List&lt;object&gt; arguments, int lineNumber)
        - INSTRUCTIONS_PER_FRAME = 100
        - int instructionCount, int recursionDepth
        - Scope globalScope, Scope currentScope
        - RegisterBuiltins(), RegisterEnums(), RegisterConstants()
      </key_signatures>
    </layer>
    
    <layer name="Game">
      <file>GameBuiltinMethods.cs</file>
      <responsibility>Unity-specific game commands (move, harvest, plant, etc.)</responsibility>
      <key_signatures>
        - IEnumerator CallMethod(string name, List&lt;object&gt; arguments)
        - IEnumerator Move(object direction) - yields
        - IEnumerator Harvest() - yields
        - IEnumerator Plant(object entity) - yields
        - bool CanHarvest() - instant
        - string GetGroundType() - instant
        - int NumItems(object item) - instant
        - void RegisterInScope(Scope scope)
      </key_signatures>
    </layer>
    
    <layer name="Unity/UI">
      <file>ConsoleManager.cs</file>
      <responsibility>In-game console for print() output</responsibility>
      <key_signatures>
        - void WriteLine(string text)
        - void Clear()
        - Text consoleText, ScrollRect scrollRect
        - static ConsoleManager Instance
      </key_signatures>
    </layer>
    
    <layer name="Unity/Runner">
      <file>CoroutineRunner.cs</file>
      <responsibility>Safe coroutine execution wrapper</responsibility>
      <key_signatures>
        - void RunScript(IEnumerator routine)
        - void StopCurrentScript()
        - bool IsRunning()
        - Coroutine currentCoroutine
      </key_signatures>
    </layer>
    
    <layer name="Testing">
      <file>DemoScripts.cs</file>
      <responsibility>Comprehensive test suite for all features</responsibility>
      <key_signatures>
        - static readonly string[] ALL_TESTS
        - Test categories: Lambda, Nested Functions, Nested Loops, Recursion, Operators, Strings, Tuples, Enums, Edge Cases
      </key_signatures>
    </layer>
  </architecture>
  
  <features>
    <supported>
      - Lambda expressions with closures
      - List comprehensions
      - Tuples
      - Dictionaries
      - Classes with __init__
      - Recursion (max depth: 100)
      - All arithmetic operators including ** (exponentiation)
      - Bitwise operators
      - Negative indexing
      - List slicing
      - Instruction budget (100 ops/frame)
      - Enum types (Grounds, Items, Entities)
      - Built-in constants (North, South, East, West)
      - Game commands (yielding and instant)
    </supported>
    
    <critical_rules>
      - .NET 2.0: No `yield return` inside try-catch in IEnumerators
      - Exponentiation (**) is right-associative
      - sleep() and game commands ALWAYS yield regardless of instruction budget
      - All exceptions must include line numbers
      - Lambda body is Expr, not Stmt
    </critical_rules>
  </features>
</project_map>
```

---

## 🔍 PART 2: SCOUT PROMPT TEMPLATE

Use this prompt when requesting a new feature:

```xml
<feature_request>
  <context>
    I am working on the LOOP Language project (Unity C# Python Interpreter).
    Below is the complete project map showing all files and their responsibilities.
  </context>
  
  <!-- PASTE THE PROJECT MAP FROM PART 1 HERE -->
  
  <new_feature>
    <goal>[DESCRIBE YOUR FEATURE HERE]</goal>
    <example_usage>
      [PROVIDE PYTHON CODE EXAMPLE IF APPLICABLE]
    </example_usage>
  </new_feature>
  
  <scout_instructions>
    **DO NOT GENERATE CODE YET.**
    
    Instead, please analyze the project map and tell me:
    
    1. **Which 2-4 specific files** need to be modified/created for this feature?
    2. **What are the key changes** required in each file? (brief summary)
    3. **Are there any risks or edge cases** I should know about?
    4. **Do I need to provide you with the full content** of any files, or can you work from the map alone?
    
    Once I confirm, I will provide the necessary file contents and you can generate the solution.
  </scout_instructions>
</feature_request>
```

---

## 💡 PART 3: WORKFLOW EXAMPLE

**Scenario:** "I want to add a modulo (%) operator to the language."

### **Step 1: User sends Scout Prompt**

```
<feature_request>
  <context>
    I am working on the LOOP Language project (Unity C# Python Interpreter).
    Below is the complete project map showing all files and their responsibilities.
  </context>
  
  [PASTE FULL PROJECT MAP HERE]
  
  <new_feature>
    <goal>Add support for the modulo (%) operator</goal>
    <example_usage>
result = 10 % 3  # Should return 1
print(17 % 5)     # Should return 2
    </example_usage>
  </new_feature>
  
  <scout_instructions>
    DO NOT GENERATE CODE YET.
    
    Instead, please analyze the project map and tell me:
    1. Which 2-4 specific files need to be modified/created for this feature?
    2. What are the key changes required in each file?
    3. Are there any risks or edge cases I should know about?
    4. Do I need to provide you with the full content of any files?
  </scout_instructions>
</feature_request>
```

---

### **Step 2: Assistant (Claude) responds with analysis**

```
Based on the project map, here's what needs to change:

**Files to Modify:**
1. **Token.cs** - Add `PERCENT` to the TokenType enum
2. **Lexer.cs** - Add tokenization for '%' character
3. **Parser.cs** - Add `PERCENT` to the multiplication precedence level
4. **PythonInterpreter.cs** - Add evaluation case for modulo in EvaluateBinaryExpr()

**Key Changes:**
1. Token.cs: `enum TokenType { ..., PERCENT, ... }`
2. Lexer.cs: `case '%': AddToken(TokenType.PERCENT); break;`
3. Parser.cs: In ParseMultiplication(), add PERCENT to Match() alongside STAR and SLASH
4. PythonInterpreter.cs: In EvaluateBinaryExpr(), add:
   ```csharp
   case TokenType.PERCENT:
       return ToNumber(left) % ToNumber(right);
   ```

**Edge Cases:**
- Division by zero: `10 % 0` should throw DivisionByZeroError
- Negative modulo: `-10 % 3` should work correctly (C# handles this)
- Float modulo: `10.5 % 3.2` should work (C# supports this)

**Files Needed:**
I can work from the map alone - no need to provide file contents since the changes are straightforward additions to existing patterns.

Should I proceed with generating the code changes?
```

---

### **Step 3: User confirms**

```
Yes, please proceed! Generate the changes for all 4 files.
```

---

### **Step 4: Assistant generates precise diffs/code**

```
Here are the changes for each file:

**1. Token.cs** - Add to TokenType enum (line ~45):
```csharp
// Arithmetic Operators
PLUS,
MINUS,
STAR,
SLASH,
PERCENT,        // ADD THIS LINE
DOUBLE_STAR,
```

**2. Lexer.cs** - Add to ScanToken() switch (line ~120):
```csharp
case '%':
    AddToken(TokenType.PERCENT);
    break;
```

**3. Parser.cs** - Modify ParseMultiplication() (line ~380):
```csharp
private Expr ParseMultiplication()
{
    Expr expr = ParseExponentiation();
    
    while (Match(TokenType.STAR, TokenType.SLASH, TokenType.PERCENT))  // ADD PERCENT HERE
    {
        TokenType op = Previous().Type;
        Expr right = ParseExponentiation();
        expr = new BinaryExpr(expr, op, right);
    }
    
    return expr;
}
```

**4. PythonInterpreter.cs** - Add to EvaluateBinaryExpr() (line ~520):
```csharp
case TokenType.SLASH:
    double divisor = ToNumber(right);
    if (divisor == 0) throw new DivisionByZeroError(currentLineNumber);
    return (double)((int)(ToNumber(left) / divisor));
    
case TokenType.PERCENT:                    // ADD THIS CASE
    double mod = ToNumber(right);
    if (mod == 0) throw new DivisionByZeroError(currentLineNumber);
    return ToNumber(left) % mod;
```

**Test Script:**
```python
print(10 % 3)      # Expected: 1
print(17 % 5)      # Expected: 2
print(-10 % 3)     # Expected: 2 (C# behavior)
print(10.5 % 3.2)  # Expected: 1.0 (approx)

# Should error:
# print(10 % 0)    # DivisionByZeroError
```

Done! The modulo operator is now fully integrated.
```

---

## 🎯 KEY PRINCIPLES OF THIS WORKFLOW

1. **The Map is Your Anchor** - Paste it every time, it replaces needing actual code files
2. **Scout First, Code Later** - Always use the scout prompt to identify minimal changes
3. **Precision Over Bulk** - Only request/modify the specific files needed
4. **Build on Patterns** - Most features follow existing patterns (operators, functions, etc.)
5. **Test Immediately** - Include test scripts with every feature request

---

## 📝 COMMON SCENARIOS

### Adding a New Game Function (e.g., `teleport()`)
**Files needed:** GameBuiltinMethods.cs, PythonInterpreter.cs (registration)

### Adding a New Operator (e.g., `//` integer division)
**Files needed:** Token.cs, Lexer.cs, Parser.cs, PythonInterpreter.cs

### Adding a New Statement Type (e.g., `match` statement)
**Files needed:** Token.cs, AST.cs, Parser.cs, PythonInterpreter.cs

### Adding a New Built-in Function (e.g., `zip()`)
**Files needed:** PythonInterpreter.cs (RegisterBuiltins section only)

### Fixing a Bug
**Files needed:** Usually 1-2 files - use scout prompt to identify which

---

## ✅ MAINTENANCE CHECKLIST

Every time you start a new chat:
- [ ] Paste the complete Project Map
- [ ] Use the Scout Prompt template
- [ ] Wait for file analysis before requesting code
- [ ] Test the changes with provided test scripts
- [ ] Update the Project Map if files/responsibilities change

---

## 🚀 BONUS: Batch Testing Without UI

Add this method to **PythonInterpreter.cs**:

```csharp
public IEnumerator RunAllTests()
{
    for (int i = 0; i < DemoScripts.ALL_TESTS.Length; i++)
    {
        Debug.Log($"Running test {i + 1}/{DemoScripts.ALL_TESTS.Length}");
        ConsoleManager.Instance?.WriteLine($"\n--- TEST {i + 1} ---");
        
        yield return Run(DemoScripts.ALL_TESTS[i]);
        
        yield return new WaitForSeconds(0.5f); // Pause between tests
    }
    
    Debug.Log("All tests complete!");
}
```

**To run all tests at once:**
```csharp
// In Unity editor or another script:
PythonInterpreter interpreter = FindObjectOfType<PythonInterpreter>();
StartCoroutine(interpreter.RunAllTests());
```

This will run every test script from DemoScripts.cs sequentially and output results to the console!

---

**END OF STATELESS MAINTENANCE WORKFLOW DOCUMENT**
