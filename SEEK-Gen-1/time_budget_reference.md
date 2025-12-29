# ⏱️ TIME BUDGET REFERENCE - Complete Function Catalog

## 📋 Overview

LOOP Language has **two independent budget systems**:

1. **Instruction Budget** - Computational time-slicing (100 ops/frame)
2. **Time Budget** - Game animation delays (explicit yields)

---

## 🎯 INSTANT Functions (No Yield - Return Immediately)

These functions execute in **<1ms** and **never yield**. They can be called thousands of times per frame without performance impact.

### **Standard Built-ins** (Instant)

| Function | Return Type | Time | Notes |
|----------|-------------|------|-------|
| `print(*args)` | None | Instant | ❌ No yield - just Debug.Log |
| `len(obj)` | int | Instant | ❌ No yield - just .Count/.Length |
| `str(obj)` | string | Instant | ❌ No yield - conversion only |
| `int(value)` | int | Instant | ❌ No yield - **Validates integer!** |
| `float(value)` | float | Instant | ❌ No yield - conversion only |
| `abs(x)` | number | Instant | ❌ No yield - Math.Abs |
| `min(*args)` | number | Instant | ❌ No yield - LINQ Min |
| `max(*args)` | number | Instant | ❌ No yield - LINQ Max |
| `sum(list)` | number | Instant | ❌ No yield - loop sum |
| `range(start, stop, step)` | list | Instant | ❌ No yield - **Requires integers!** |
| `sorted(list, key, reverse)` | list | Instant | ❌ No yield - Sort operation |

**Critical Details:**
- ✅ `print()` - Instant, just outputs to console
- ✅ `range()` - **Validates integer arguments, errors on float**
- ✅ `int()` - **Validates integer value, errors on 1.5**
- ✅ `sorted()` - Instant even with lambda key function

---

### **Game Query Functions** (Instant)

| Function | Return Type | Time | Description |
|----------|-------------|------|-------------|
| `can_harvest()` | bool | Instant | ❌ No yield - query only |
| `get_ground_type()` | enum string | Instant | ❌ No yield - property access |
| `get_entity_type()` | enum string or None | Instant | ❌ No yield - property access |
| `get_pos_x()` | number | Instant | ❌ No yield - position.x |
| `get_pos_y()` | number | Instant | ❌ No yield - position.y |
| `get_world_size()` | number | Instant | ❌ No yield - constant |
| `get_water()` | number (0.0-1.0) | Instant | ❌ No yield - water level |
| `num_items(item)` | number | Instant | ❌ No yield - inventory[item] |
| `is_even(x, y)` | bool | Instant | ❌ No yield - (x+y)%2==0 |
| `is_odd(x, y)` | bool | Instant | ❌ No yield - (x+y)%2==1 |

**Example - Instant Queries in Tight Loop:**
```python
# This completes instantly (no yields)
for i in range(10000):
    x = get_pos_x()  # Instant
    y = get_pos_y()  # Instant
    ground = get_ground_type()  # Instant
    # Only instruction budget applies (yields every 100 ops)
```

---

## ⏳ TIME-SLICED Functions (Yield via Instruction Budget)

These functions don't explicitly yield, but **instruction budget applies**.

### **Loop Operations**

Loops yield automatically after 100 operations:

```python
# Small loop - completes in 1 frame (instant)
for i in range(50):
    sum += i

# Large loop - distributed across frames
for i in range(1000):  # Yields after every 100 iterations
    sum += i

# Nested loops - counts total operations
for i in range(100):
    for j in range(100):  # Total 10,000 ops → many yields
        sum += i + j
```

**Instruction Budget Rule:**
- Counter increments on: statement execution, expression evaluation, loop iteration
- Yields when counter >= 100 (configurable via `INSTRUCTIONS_PER_FRAME`)
- Resets counter to 0 after yield

---

## 🎬 YIELDING Functions (Explicit Time Budget)

These functions **ALWAYS yield** regardless of instruction budget. They pause Python execution until animation completes.

### **Sleep Function** (ALWAYS Yields)

| Function | Duration | Implementation | Notes |
|----------|----------|----------------|-------|
| `sleep(seconds)` | Exact duration | `yield return WaitForSeconds(seconds)` | ✅ Handles int AND float |

**Examples:**
```python
sleep(2)      # Yields exactly 2.0 seconds (int → float)
sleep(2.0)    # Yields exactly 2.0 seconds
sleep(0.5)    # Yields exactly 0.5 seconds
sleep(0)      # Yields once (minimum one frame)
sleep(-1)     # ERROR: duration cannot be negative
```

**Critical:** `sleep()` is **independent of instruction budget**. It always yields the specified duration.

---

### **Game Command Functions** (Animate - ALWAYS Yield)

These functions perform game actions with animations:

| Function | Duration | Implementation | Description |
|----------|----------|----------------|-------------|
| `move(direction)` | ~0.3s | `yield return WaitForSeconds(0.3f)` | Move animation |
| `harvest()` | ~0.2s | `yield return WaitForSeconds(0.2f)` | Harvest animation |
| `plant(entity)` | ~0.3s | `yield return WaitForSeconds(0.3f)` | Plant animation |
| `till()` | ~0.1s | `yield return WaitForSeconds(0.1f)` | Till animation |
| `use_item(item)` | ~0.1s | `yield return WaitForSeconds(0.1f)` | Use item animation |
| `do_a_flip()` | ~1.0s | `yield return WaitForSeconds(1.0f)` | Flip animation (easter egg) |

**Example - Animation Sequence:**
```python
# Each command yields independently
move(North)   # Yields ~0.3s
harvest()     # Yields ~0.2s
plant(Entities.Carrot)  # Yields ~0.3s
# Total time: ~0.8 seconds
```

---

## 📊 Combined Budget Example

```python
# Example showing both budgets
def complex_farming():
    # Instant queries (no yield)
    size = get_world_size()  # Instant
    
    # Large loop (instruction budget yields)
    for i in range(500):  # Yields 5 times (every 100 ops)
        x = get_pos_x()  # Instant
        
        # Explicit yield (time budget)
        if i % 100 == 0:
            move(East)  # Yields ~0.3s each time
    
    # Sleep (always yields)
    sleep(2)  # Yields exactly 2s
```

**Total Time:**
- Instant operations: ~0ms
- Instruction budget yields: 5 frames (5 × 16ms = ~80ms)
- Move commands: 5 × 0.3s = 1.5s
- Sleep: 2s
- **Total: ~3.58 seconds**

---

## 🔍 Function Type Quick Reference

### ❌ **Never Yield** (Instant)
```
print, len, str, int, float, abs, min, max, sum, range, sorted,
can_harvest, get_ground_type, get_entity_type,
get_pos_x, get_pos_y, get_world_size, get_water,
num_items, is_even, is_odd
```

### ⏱️ **Yield via Instruction Budget** (Time-Sliced)
```
Loops (for, while) when operation count > 100
Recursion when depth > 100
Large computations
```

### ✅ **Always Yield** (Animate)
```
sleep, move, harvest, plant, till, use_item, do_a_flip
```

---

## 🎯 Performance Guidelines

### **For Fast Queries:**
```python
# This is FAST (all instant)
for i in range(10000):
    x = get_pos_x()
    y = get_pos_y()
    ground = get_ground_type()
    # Only instruction budget applies
```

### **For Animations:**
```python
# This is SLOW (each move yields ~0.3s)
for i in range(10):
    move(North)  # Total time: 10 × 0.3s = 3 seconds
```

### **Best Practice:**
```python
# Optimize by batching instant queries
positions = []
for i in range(1000):
    x = get_pos_x()  # Instant
    y = get_pos_y()  # Instant
    positions.append((x, y))

# Then perform yielding operations
for pos in positions:
    move(North)  # Yields
```

---

## 🚨 Common Misconceptions

### ❌ **WRONG:** "print() yields"
**✅ CORRECT:** `print()` is instant, just writes to console

### ❌ **WRONG:** "range() yields for large ranges"
**✅ CORRECT:** `range(10000)` is instant, creates list immediately. Only the **loop using it** yields.

### ❌ **WRONG:** "sorted() yields for large lists"
**✅ CORRECT:** `sorted()` is instant, even for 10,000 items. It's a single operation.

### ❌ **WRONG:** "sleep(0) is instant"
**✅ CORRECT:** `sleep(0)` **still yields once** (one frame minimum)

---

## 📐 Implementation Details

### **Instant Function Pattern:**
```csharp
private object MyInstantFunction(List<object> args)
{
    // Validate arguments
    // Perform computation
    // Return result
    // ❌ NO yield return
}
```

### **Yielding Function Pattern:**
```csharp
private IEnumerator MyYieldingFunction(List<object> args)
{
    // Validate arguments
    // Perform action
    yield return new WaitForSeconds(duration);
    // ✅ ALWAYS yield return
}
```

### **Registration:**
```csharp
// Instant
globalScope.Define("print", new BuiltinFunction("print", Print));

// Yielding
globalScope.Define("sleep", new BuiltinFunction("sleep", Sleep));
globalScope.Define("move", new BuiltinFunction("move", gameBuiltins.Move));
```

---

## ✅ Validation Rules (NEW)

### **Integer-Required Contexts:**

1. **List Indexing:** `list[index]`
   - ✅ `list[0]` - OK
   - ✅ `list[1.0]` - OK (integer value)
   - ❌ `list[1.5]` - ERROR: "List index requires integer, got 1.5"

2. **Range Function:** `range(start, stop, step)`
   - ✅ `range(10)` - OK
   - ✅ `range(10.0)` - OK (integer value)
   - ❌ `range(10.5)` - ERROR: "range() requires integer, got 10.5"

3. **Slice Indices:** `list[start:stop:step]`
   - ✅ `list[1:5]` - OK
   - ✅ `list[1.0:5.0]` - OK (integer values)
   - ❌ `list[1.5:5]` - ERROR: "Slice start requires integer, got 1.5"

### **Number Equality (Python Behavior):**
```python
1 == 1.0     # True (unlike C#/Java)
1.0 == 1     # True
5 / 1 == 5   # True (5.0 == 5)
```

---

## 📊 Time Budget Summary

| Category | Count | Instant? | Yields? | Notes |
|----------|-------|----------|---------|-------|
| Standard Built-ins | 11 | ✅ Yes | ❌ No | print, range, len, sorted, etc. |
| Game Queries | 10 | ✅ Yes | ❌ No | get_pos_x, can_harvest, etc. |
| Sleep | 1 | ❌ No | ✅ Always | Exact duration |
| Game Commands | 6 | ❌ No | ✅ Always | Animations |
| Instruction Budget | N/A | ❌ No | ⏱️ After 100 ops | Loops, recursion |

**Total Functions:** 28 (21 instant + 1 sleep + 6 game commands)

---

## 🎓 Key Takeaways

1. ✅ **21 functions are instant** (no yield at all)
2. ✅ **1 function always yields** (sleep - exact duration)
3. ✅ **6 functions always yield** (game commands - animations)
4. ✅ **Instruction budget applies to loops/recursion** (100 ops)
5. ✅ **Integer validation in indexing/range** (Python behavior)
6. ✅ **Number equality works across types** (1 == 1.0 is True)

---

*Updated: December 2025 - Version 2.1 Extended*
