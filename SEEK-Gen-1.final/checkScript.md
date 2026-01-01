## linear loop time budget dependednt along with return
```py
def _A():
	sum = 0
	for i in [0, 1]:
		walk(1, 0)
		print(i)
		sum += 1
	sleep(0.5)
	walk(-1, 0)
	sum += 10
	return sum
print(_A())


def _B(_base):
	sum = 0
	list = [1, 2,
	3, 4, "10", 0.01, -0.1, 1000, "hmm"]
	print("parsed list: ", list)
	sleep(0.1)
	list.sort(key = lambda val: len(str(val)))
	print(list)
	for y in range(10):
	
		walk(1, 0)
		print(y, "somthng")
		sleep(0.1)
	return _base**6
print(_B(10))

```

## recursive with anim(time budget dependent)
```py (it works now)

list = []
def recursive(depth):
    print(depth)
    walk(1, 0)
    list.append(depth)
    if(depth > 3):
        return
    recursive(depth + 1)
    recursive(depth + 1)

recursive(0)
print(list)

```

## named arguements
```py
def _A(val, mul):
	sum = 0
	prod = 1
	for i in range(1, val * mul):
		walk(1, 0)		
		print(i)
		sum += i
		prod *= i
	return {"sum": sum, "prod": prod}

result = _A(5, 2)
result = _A(5, mul = 2)
result = _A(val = 5, mul = 2)
print(result["sum"])
print(result["prod"])
```

## string operations
```py
# Minimal tests to find the exact issue
# Test 1: Simple string literal
print("Test 1: String literal")
s1 = "hello"
print(s1)

# Test 2: String method with no args
print("Test 2: String method no args")

s2 = "heLLo\nwell".upper().lower().split("\n")
print(s2)

# Test 3: String method with one arg
print("Test 3: String method one arg")
s3 = "a-b--c".split(2*"-")
print(s3)

# Test 4: String method with two args
print("Test 4: String method two args")
s4 = "hello".replace(2*"l", 2*"z")
print(s4)

# Test 5: join with list
print("Test 5: join")
s5 = (2*"hmm").join(["a", "b"])
print(s5)
```

## Simple Method Chaining Examples
```py
print("=" * 60)

print("\n=== Test 1: List Chaining ===")
nums = [3, 1, 4, 1, 5]
nums.sort()
print("Sorted:", nums)

# Test 2: Dict literal chaining (works now!)
print("\n=== Test 2: Dict Chaining ===")
my_dict = {"name": "Alice", "age": 30}
keys = my_dict.keys()
print("Keys:", keys)

# Test 3: Lambda IIFE (works now!)
print("\n=== Test 3: Lambda Call ===")
result = (lambda x: x * 2)(21)
print("Result:", result)

# Test 4: Your original intent (corrected for Python)
print("\n=== Test 4: String Operations ===")

# In Python, join() is a STRING method, not list method
# Correct: "-".join(list)
# Wrong:   list.join("-")  <-- This is JavaScript!

parts = ["1", "2", "well hmm"]
joined = "-".join(parts)
print("Joined:", joined)

trimmed = joined.strip()
print("Trimmed:", trimmed)

split_result = trimmed.split("ll")
print("Split:", split_result)

# One-liner version:
one_liner = "-".join(["1", "2", "well hmm"]).strip().split("ll")
print("One-liner:", one_liner)
```

## type()
```py
x = 42
y = "hello"
z = [1, 2, 3]

# ✅ All these work now:
if type(x) == "<class 'number'>":
    print("x is a number")

if type(y) == "<class 'str'>":
    print("y is a string")

if type(z) == "<class 'list'>":
    print("z is a list")

# ✅ Also works without the full format:
if type(x) == "number":  # TypeObject.Equals handles this
    print("x is still a number")

# ✅ Print the type directly:
print(type(42))      # <class 'number'>
print(type(10.5))    # <class 'number'>  (same as int!)
print(type("hi"))    # <class 'str'>
print(type([1,2]))   # <class 'list'>
dict = {"a": walk(1, 0)}
print(type(dict) == "<class 'dict'>")

# ✅ Type comparisons work:
if type(10) == type(20.5):
    print("Both are numbers!")  # This prints!
```