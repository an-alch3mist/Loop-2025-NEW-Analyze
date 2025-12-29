# HOW TO USE THE STATELESS WORKFLOW - Practical Examples

This document shows you exactly how to use the XML workflow system in real conversations with Claude.

---

## 🎯 Core Concept

**Problem:** Uploading all 18 files every time = token waste + slower responses  
**Solution:** Upload Project Map once → Claude tells you which 2-3 files needed → Upload only those  
**Result:** 80%+ reduction in upload size, faster & more focused responses

---

## 📋 Example 1: Simple Feature (Adding a Built-in Function)

### Your First Message:
```xml
<loop_language_context>
<!-- Paste the entire <project_map> section from the XML file -->
<!-- This is ~50-70 lines and gives Claude full architectural context -->

[Project Map XML here - see LOOP_LANGUAGE_STATELESS_WORKFLOW.xml]

</loop_language_context>

## Feature Request

I want to add a new built-in function `abs()` that returns the absolute value of a number.

**Example usage:**
```python
print(abs(-5))  # Should print: 5
print(abs(3.14)) # Should print: 3.14
print(abs(-0.5)) # Should print: 0.5
```

**Instructions:**
DO NOT generate code yet. Please tell me:
1. Which files need modification
2. What changes are needed
3. Complexity estimate
```

### Claude's Response:
```
Based on the Project Map, this is a SIMPLE change requiring only 1 file:

## File to Modify:
**PythonInterpreter.cs** - RegisterBuiltins() method

## Changes Needed:
Add abs() function to the built-in function registry:
- Validate 1 argument
- Convert to number using ToNumber()
- Return Math.Abs(value)

## Complexity: ⭐ SIMPLE (15 minutes)

## Next Step:
Please upload PythonInterpreter.cs and I'll provide the exact implementation.
```

### Your Second Message:
```
[Upload only PythonInterpreter.cs]

Here's the file. Please implement abs().
```

### Claude Provides Solution:
```csharp
// In PythonInterpreter.cs, inside RegisterBuiltins() method:

builtins["abs"] = new BuiltinFunction("abs", args => {
    if (args.Count != 1)
    {
        throw new RuntimeError("abs() expects 1 argument");
    }
    
    double value = ToNumber(args[0]);
    return Math.Abs(value);
});
```

**Done! Only 1 file uploaded instead of 18. 94% reduction in upload size.**

---

## 📋 Example 2: Moderate Feature (Adding an Operator)

### Your First Message:
```xml
<loop_language_context>
[Project Map XML]
</loop_language_context>

## Feature Request

Add the floor division operator `//` (already exists) - but let's pretend it doesn't for this example.

This should perform division and return the floor of the result:
```python
print(10 // 3)   # Should print: 3
print(17 // 5)   # Should print: 3
print(-10 // 3)  # Should print: -4 (Python semantics)
```

DO NOT generate code yet. Scout the files first.
```

### Claude's Response:
```
## Files to Modify (in order):

1. **Token.cs** (CRITICAL)
   - Check if DOUBLE_SLASH token exists, add if not
   
2. **Lexer.cs** (CRITICAL)
   - Add tokenization for '//' in ScanToken()
   - Must check for '//' before '/' to avoid conflicts
   
3. **PythonInterpreter.cs** (CRITICAL)
   - Add DOUBLE_SLASH case in EvaluateBinary()
   - Implement as Math.Floor(left / right)

## Complexity: ⭐⭐ MODERATE (45 minutes)

## Upload Order:
1. Token.cs (check token exists)
2. Lexer.cs (check tokenization)
3. PythonInterpreter.cs (implement operation)
```

### Your Second Message:
```
[Upload Token.cs, Lexer.cs, PythonInterpreter.cs]

Here are the 3 files. Please implement //.
```

### Claude Checks and Implements:
```
Checking Token.cs... ✅ DOUBLE_SLASH already exists!
Checking Lexer.cs... ✅ Already tokenized!
Checking PythonInterpreter.cs... ✅ Already implemented!

Good news - this operator already works! No changes needed.

Would you like me to verify the implementation is correct for Python semantics?
```

**Result: Files already had the feature. Claude saved you from duplicate work!**

---

## 📋 Example 3: Complex Feature (Exception Handling)

### Your First Message:
```xml
<loop_language_context>
[Project Map XML]
</loop_language_context>

## Feature Request

Implement try/except/finally exception handling:

```python
try:
    x = 10 / 0
except:
    print("Error caught!")
finally:
    print("Cleanup")
```

DO NOT generate code yet. Full analysis please.
```

### Claude's Response:
```
This is a COMPLEX feature requiring 5 files and significant work.

## Files to Modify (in order):

1. **Token.cs** (CRITICAL)
   - Add tokens: TRY, EXCEPT, FINALLY, RAISE, AS
   
2. **AST.cs** (CRITICAL)
   - Add TryStmt class
   - Add ExceptClause class
   - Add RaiseStmt class
   
3. **Parser.cs** (CRITICAL)
   - Add TryStatement() method
   - Handle try/except/finally parsing
   - Handle multiple except clauses
   - Handle 'as' variable binding
   
4. **Exceptions.cs** (MODERATE)
   - Add UserException class for raise statements
   - Add exception type registry
   
5. **PythonInterpreter.cs** (CRITICAL)
   - Add ExecuteTry() method
   - Exception catching logic (careful: .NET 2.0!)
   - Exception propagation
   - Finally block execution guarantee

## Complexity: ⭐⭐⭐⭐⭐ COMPLEX (8-12 hours)

## Estimated Effort:
- Token/AST changes: 1 hour
- Parser implementation: 3-4 hours
- Interpreter implementation: 4-6 hours
- Testing & debugging: 2-3 hours

## Challenges:
1. .NET 2.0 can't yield inside try-catch
2. Finally blocks must run even during returns
3. Multiple except clauses need proper matching
4. Exception type system

## Recommendation:
Break into phases:
1. Phase 1: Basic try/except (no finally)
2. Phase 2: Add finally blocks
3. Phase 3: Add except with type matching
4. Phase 4: Add raise statement

Would you like to proceed with Phase 1, or tackle the full implementation?
```

### Your Response:
```
Let's start with Phase 1 - basic try/except only.
Upload Token.cs, AST.cs, Parser.cs, PythonInterpreter.cs.
```

**Result: Claude breaks complex work into manageable phases with clear scope.**

---

## 📋 Example 4: Bug Fix

### Your First Message:
```xml
<loop_language_context>
[Project Map XML]
</loop_language_context>

## Bug Report

Test failing with error:
```
[TEST 29] Running: String indexing
P
n
RuntimeError: Can only slice lists
```

Test code:
```python
text = 'Python'
print(text[0])   # P (works)
print(text[-1])  # n (works)
print(text[1:4]) # yth (FAILS - "Can only slice lists")
```

DO NOT fix yet. Which files are involved?
```

### Claude's Response:
```
Based on the error "Can only slice lists", this is in:

## File to Check:
**PythonInterpreter.cs** - EvaluateSlice() method

## Analysis:
The error suggests EvaluateSlice() only handles List<object> but not strings.

## Expected Fix:
Add string handling in EvaluateSlice():
- Check if obj is string
- Perform string slicing (similar to list slicing)
- Return substring

## Complexity: ⭐ SIMPLE (30 minutes)

Upload PythonInterpreter.cs and I'll provide the fix.
```

### Your Second Message:
```
[Upload PythonInterpreter.cs]

Here's the file. Please fix string slicing.
```

### Claude Provides Fix:
```csharp
// In EvaluateSlice() method, add before the list check:

// Handle string slicing
if (obj is string)
{
    string str = (string)obj;
    int strLength = str.Length;
    
    int strStart = 0;
    int strStop = strLength;
    int strStep = 1;
    
    // [Calculate start, stop, step from expr.Start, expr.Stop, expr.Step]
    // [Build and return substring]
}

// Then existing list code...
```

**Result: Bug fixed with only 1 file upload!**

---

## 📋 Example 5: Adding a Game Command

### Your First Message:
```xml
<loop_language_context>
[Project Map XML]
</loop_language_context>

## Feature Request

Add a `teleport(x, y)` game command that instantly moves the player.

```python
teleport(5, 10)  # Move to position (5, 10)
print(get_pos_x())  # Should print: 5
print(get_pos_y())  # Should print: 10
```

Should yield (like move() does) to simulate animation time.

DO NOT code yet. Scout first.
```

### Claude's Response:
```
## Files to Modify:

1. **GameBuiltinMethods.cs** (PRIMARY)
   - Add Teleport(List<object> args) method
   - Should return IEnumerator (yields)
   - Validate 2 arguments (x, y)
   - Update playerPosition
   - Yield for animation time (~0.5s)
   
2. **PythonInterpreter.cs** (SECONDARY)
   - Add registration in RegisterGameBuiltins()
   - `builtins["teleport"] = new BuiltinFunction("teleport", args => gameBuiltins.Teleport(args));`

## Complexity: ⭐⭐ SIMPLE (30-45 minutes)

Upload GameBuiltinMethods.cs and PythonInterpreter.cs.
```

### Your Second Message:
```
[Upload 2 files]

Implement teleport().
```

**Result: Clean, focused implementation with only necessary files.**

---

## 🎯 Key Patterns

### Pattern 1: Always Start with Scout
```
1. Provide Project Map
2. Describe feature
3. Say "DO NOT code yet"
4. Get file list
5. Upload only those files
6. Get solution
```

### Pattern 2: Iterative Clarification
```
Claude: "I need files A, B, C"
You: "Here's A and B, but C is large. Can we work without it?"
Claude: "Yes, C only needed for edge case D. Skip for now."
```

### Pattern 3: Mid-Conversation Additions
```
You: [Uploads A, B]
Claude: "I notice you're using feature X. Can you also upload C?"
You: "Sure" [Uploads C]
Claude: "Thanks! Here's the complete solution..."
```

### Pattern 4: Phase Breaking
```
You: "This seems complex"
Claude: "Break into 3 phases..."
You: "Let's do Phase 1 only"
Claude: [Provides Phase 1 solution]
You: [Later] "Now Phase 2"
```

---

## 💡 Pro Tips

### Tip 1: Keep Project Map Updated
```
After major changes:
"Claude, based on these files, please update the <project_map> section of the XML"
[Claude generates updated XML]
[You save it for next session]
```

### Tip 2: Reference Known Issues
```
"I see in the Project Map that try/except isn't implemented.
Can we work around this for now?"
```

### Tip 3: Check First, Implement Second
```
"Before implementing, check if this already exists in the codebase"
[Claude checks]
"Already there! No work needed."
```

### Tip 4: Batch Related Changes
```
"I need to add 3 built-in functions: abs(), round(), ceil().
Scout once for all three, then implement together."
```

### Tip 5: Use Test-Driven Approach
```
"Here's the test that's failing [paste test].
Scout which files are involved, then fix."
```

---

## 📊 Comparison: With vs Without Workflow

### OLD WAY (Upload Everything):
```
Message 1: [Uploads all 18 files - 5000+ lines]
"Add abs() function"

Token usage: ~50,000 tokens
Claude: Overwhelmed, might miss details
Time: Slower responses
```

### NEW WAY (Stateless Workflow):
```
Message 1: [Project Map - 70 lines]
"Add abs() function - SCOUT FIRST"

Message 2: [Uploads 1 file - 1700 lines]
"Here's PythonInterpreter.cs"

Token usage: ~15,000 tokens (70% reduction!)
Claude: Focused, precise
Time: Faster responses
```

---

## 🔄 Workflow Cheat Sheet

```
┌─────────────────────────────────────────┐
│ 1. New Conversation                     │
│    → Paste Project Map XML              │
└─────────────────────────────────────────┘
              │
              ▼
┌─────────────────────────────────────────┐
│ 2. Request Feature                      │
│    → Say "DO NOT CODE YET"              │
│    → Describe what you want             │
└─────────────────────────────────────────┘
              │
              ▼
┌─────────────────────────────────────────┐
│ 3. Claude Scouts                        │
│    → Lists 2-5 files needed             │
│    → Explains changes                   │
│    → Estimates complexity               │
└─────────────────────────────────────────┘
              │
              ▼
┌─────────────────────────────────────────┐
│ 4. Upload Only Those Files              │
│    → 80%+ reduction in upload size      │
└─────────────────────────────────────────┘
              │
              ▼
┌─────────────────────────────────────────┐
│ 5. Get Targeted Solution                │
│    → Faster responses                   │
│    → More focused code                  │
│    → Better quality                     │
└─────────────────────────────────────────┘
```

---

## 🎓 You're Ready!

You now have:
- ✅ Project Map (architectural overview)
- ✅ Scout Prompt (feature request template)
- ✅ Workflow Examples (real conversations)
- ✅ Quick Reference (dependencies, modifications)
- ✅ This Usage Guide (practical tips)

### Next Steps:
1. Save `LOOP_LANGUAGE_STATELESS_WORKFLOW.xml` in your project
2. Save `QUICK_REFERENCE_GUIDE.md` for quick lookups
3. Start a new conversation with Project Map
4. Try adding a simple feature using the workflow
5. Enjoy working on unlimited features without token limits!

---

**Happy Coding! 🚀**
