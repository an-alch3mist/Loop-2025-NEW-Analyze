# Test Cases for 'not in' Operator Fix

## Issue
The parser was throwing an error when using `not in` operator:
```
[PARSER ERROR] Parse error at line 3: Expected ':' after if condition at line 3, got 'not'
```

## Root Cause
The parser treated `not` and `in` as separate operators instead of recognizing `not in` as a combined membership test operator.

## Solution
Modified the `Comparison()` method in Parser.cs to check for `not in` pattern:
- After parsing the left expression, check if next token is NOT followed by IN
- If so, consume both tokens and create a `UnaryExpr(NOT, BinaryExpr(left, IN, right))`
- This creates the AST: `not (expr in container)`

## Test Cases

### Test 1: Basic 'not in' with dictionary
```python
d = {"a": 1, "b": 2}
keyToLook = "a"

if keyToLook not in d:
    print("a is not in dictionary")
else:
    print("a IS in dictionary")  # Expected to print

# Expected output: "a IS in dictionary"
```

### Test 2: 'not in' with list
```python
numbers = [1, 2, 3, 4, 5]

if 10 not in numbers:
    print("10 is not in list")  # Expected to print

if 3 not in numbers:
    print("3 is not in list")
else:
    print("3 IS in list")  # Expected to print

# Expected output:
# 10 is not in list
# 3 IS in list
```

### Test 3: 'not in' with string
```python
text = "hello world"

if "x" not in text:
    print("x not found")  # Expected to print

if "hello" not in text:
    print("hello not found")
else:
    print("hello found")  # Expected to print

# Expected output:
# x not found
# hello found
```

### Test 4: Combined with 'and'
```python
config = {"debug": True, "port": 8080}

if "ssl" not in config and config["debug"]:
    print("SSL missing and debug enabled")  # Expected to print

# Expected output: "SSL missing and debug enabled"
```

### Test 5: Combined with 'or'
```python
data = {"name": "Alice", "age": 30}

if "email" not in data or "phone" not in data:
    print("Contact info incomplete")  # Expected to print

# Expected output: "Contact info incomplete"
```

### Test 6: Using 'in' and 'not in' together
```python
allowed = ["admin", "user", "guest"]
banned = ["spammer", "bot"]

username = "user"

if username in allowed and username not in banned:
    print("Access granted")  # Expected to print

# Expected output: "Access granted"
```

### Test 7: 'not in' in while loop
```python
items = [1, 2, 3]
counter = 10

while counter not in items:
    print(f"Counter: {counter}")
    counter = counter - 1
    if counter < 1:
        break

# Expected output:
# Counter: 10
# Counter: 9
# Counter: 8
# ... (until counter reaches 3)
```

### Test 8: 'not in' with expressions
```python
d = {1: "one", 2: "two", 3: "three"}

x = 5
y = 2

if x - 1 not in d:
    print("4 is not a key")  # Expected to print

if y + 1 not in d:
    print("3 is not a key")
else:
    print("3 IS a key")  # Expected to print

# Expected output:
# 4 is not a key
# 3 IS a key
```

### Test 9: Regular 'not' operator still works
```python
x = True
y = False

if not x:
    print("x is False")
else:
    print("x is True")  # Expected to print

if not y:
    print("y is False")  # Expected to print

# Expected output:
# x is True
# y is False
```

### Test 10: Edge case - 'not' followed by non-IN token
```python
# This should still work (regular NOT operator)
x = 5

if not (x > 10):
    print("x is not greater than 10")  # Expected to print

# Expected output: "x is not greater than 10"
```

## Summary

### What Works Now:
✅ `if item not in list:`
✅ `if key not in dict:`
✅ `if char not in string:`
✅ `while x not in collection:`
✅ Combined with `and`, `or`
✅ Regular `not` operator still works
✅ `not` with parentheses: `not (x > 5)`

### Implementation Details:
**File:** Parser.cs
**Method:** Comparison()
**Lines Added:** ~20 lines

**How it works:**
1. Parse left expression: `keyToLook`
2. Check if next token is NOT
3. If yes, peek ahead to see if token after is IN
4. If yes, consume both and parse right expression: `d`
5. Create AST: `UnaryExpr(NOT, BinaryExpr(keyToLook, IN, d))`
6. Interpreter evaluates this as: `not (keyToLook in d)`

### Backward Compatibility:
✅ All existing code continues to work
✅ No breaking changes
✅ `in` operator unchanged
✅ `not` operator unchanged
✅ Only adds support for `not in` combination
