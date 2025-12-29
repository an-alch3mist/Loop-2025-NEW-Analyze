namespace LOOPLanguage
{
    /// <summary>
    /// Collection of test scripts for the LOOP language interpreter.
    /// Tests all features including edge cases.
    /// </summary>
    public static class DemoScripts
    {
        #region Advanced Lambda Tests
        
        public static readonly string LAMBDA_WITH_LIST_COMP = @"
# Test: Lambda with list comprehension inside
nums = [1, 2, 3, 4, 5, 6, 7, 8]
result = (lambda x: [i*i for i in x if i % 2 == 0 and i > 3])(nums)
print(result)  # Expected: [16, 36, 64]
";
        
        public static readonly string LAMBDA_SORTED_TUPLES = @"
# Test: Sorted with lambda accessing tuple elements
data = [(1, 'b'), (3, 'a'), (2, 'c')]
sorted_data = sorted(data, key=lambda x: x[1])
print(sorted_data)  # Expected: [(3, 'a'), (1, 'b'), (2, 'c')]
";
        
        public static readonly string LAMBDA_MULTIPLE_CONDITIONS = @"
# Test: Lambda with multiple conditions
nums = [-5, 2, 15, 50, 101, 88]
filter_func = lambda x: x > 0 and x % 2 == 0 and x < 100
result = [x for x in nums if filter_func(x)]
print(result)  # Expected: [2, 50, 88]
";
        
        public static readonly string LAMBDA_IIFE_COMPLEX = @"
# Test: Immediately invoked lambda with complex logic
result = (lambda nums: [x * 2 for x in nums if x % 2 == 1 and x > 2])([1, 2, 3, 4, 5, 6, 7, 8, 9])
print(result)  # Expected: [6, 10, 14, 18]
";
        
        public static readonly string LAMBDA_DICT_SORTING = @"
# Test: Lambda sorting dict list by computed value
items = [
    {'name': 'apple', 'price': 3, 'qty': 10},
    {'name': 'banana', 'price': 1, 'qty': 20},
    {'name': 'cherry', 'price': 2, 'qty': 5}
]

sorted_items = sorted(items, key=lambda x: x['price'] * x['qty'])

for item in sorted_items:
    print(item['name'])
# Expected: cherry, banana, apple
";
        
        public static readonly string LAMBDA_NESTED_INDEXING = @"
# Test: Lambda with nested indexing
matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]
sorted_matrix = sorted(matrix, key=lambda row: row[1])

for row in sorted_matrix:
    print(row)
# Expected: [1, 2, 3], [4, 5, 6], [7, 8, 9]
";
        
        public static readonly string LAMBDA_MULTI_PARAM = @"
# Test: Multiple lambda parameters
combine = lambda a, b, c: a + b * c
result = combine(10, 5, 2)
print(result)  # Expected: 20

make_pair = lambda x, y: (x, y)
pair = make_pair(5, 10)
print(pair)  # Expected: (5, 10)
";
        
        #endregion
        
        #region Nested Function Call Tests
        
        public static readonly string NESTED_FUNCTION_CALLS = @"
# Test: Nested function calls as arguments
def _A(x):
    return x + 1

result = _A(_A(1) + _A(_A(2) + _A(4)))
print(result)  # Expected: 11
# Breakdown: _A(4)=5, 2+5=7, _A(7)=8, 1+8=9, _A(9)=10, _A(10)=11
";
        
        public static readonly string DEEPLY_NESTED_CALLS = @"
# Test: Deeply nested function calls
def triple(x):
    return x * 3

result = triple(triple(triple(2)))
print(result)  # Expected: 54 (2*3=6, 6*3=18, 18*3=54)
";
        
        public static readonly string NESTED_WITH_OPERATORS = @"
# Test: Nested calls with complex operators
def square(x):
    return x ** 2

result = square(square(2) + square(3))
print(result)  # Expected: 169 (4+9=13, 13**2=169)
";
        
        #endregion
        
        #region Nested Loop Tests
        
        public static readonly string NESTED_LOOPS_SIMPLE = @"
# Test: Simple nested loops
count = 0
for i in range(3):
    for j in range(4):
        count += 1

print(count)  # Expected: 12
";
        
        public static readonly string NESTED_LOOPS_WITH_BREAK = @"
# Test: Nested loops with break
result = []
for i in range(5):
    for j in range(5):
        if i == 2 and j == 2:
            break
        result.append((i, j))

print(len(result))  # Expected: 12 (not 25)
";
        
        public static readonly string NESTED_LOOPS_WITH_CONTINUE = @"
# Test: Nested loops with continue
sum = 0
for i in range(5):
    for j in range(5):
        if j % 2 == 0:
            continue
        sum += i + j

print(sum)  # Expected: 100
";
        
        public static readonly string TRIPLE_NESTED_LOOPS = @"
# Test: Triple nested loops
count = 0
for i in range(2):
    for j in range(3):
        for k in range(4):
            count += 1

print(count)  # Expected: 24 (2*3*4)
";
        
        #endregion
        
        #region Recursion Tests
        
        public static readonly string RECURSION_FACTORIAL = @"
# Test: Factorial recursion
def factorial(n):
    if n <= 1:
        return 1
    return n * factorial(n - 1)

print(factorial(5))  # Expected: 120
";
        
        public static readonly string RECURSION_FIBONACCI = @"
# Test: Fibonacci recursion
def fib(n):
    if n <= 1:
        return n
    return fib(n - 1) + fib(n - 2)

print(fib(10))  # Expected: 55
";
        
        public static readonly string RECURSION_DEPTH_LIMIT = @"
# Test: Recursion depth limit (should error)
def infinite_recursion(n):
    return infinite_recursion(n + 1)

# This should hit max recursion depth (100)
try:
    infinite_recursion(0)
except Exception as e:
    print('Recursion limit caught')
";
        
        public static readonly string MUTUAL_RECURSION = @"
# Test: Mutual recursion
def is_even(n):
    if n == 0:
        return True
    return is_odd(n - 1)

def is_odd(n):
    if n == 0:
        return False
    return is_even(n - 1)

print(is_even(10))  # Expected: True
print(is_odd(7))    # Expected: True
";
        
        #endregion
        
        #region Operator Precedence Tests
        
        public static readonly string EXPONENTIATION_PRECEDENCE = @"
# Test: Exponentiation operator precedence
result1 = 2 ** 3 ** 2  # Right-associative: 2 ** (3 ** 2) = 2 ** 9 = 512
print(result1)  # Expected: 512

result2 = 2 * 3 ** 2  # ** has higher precedence: 2 * (3 ** 2) = 2 * 9 = 18
print(result2)  # Expected: 18

result3 = (2 ** 3) ** 2  # Explicit grouping: 8 ** 2 = 64
print(result3)  # Expected: 64
";
        
        public static readonly string COMPLEX_PRECEDENCE = @"
# Test: Complex operator precedence
result = 10 + 5 * 2 ** 3 - 8 / 2
print(result)  # Expected: 46.0
# Breakdown: 2**3=8, 5*8=40, 8/2=4, 10+40-4=46
";
        
        public static readonly string BITWISE_PRECEDENCE = @"
# Test: Bitwise operator precedence
result1 = 5 | 3 & 1  # & has higher precedence: 5 | (3 & 1) = 5 | 1 = 5
print(result1)  # Expected: 5

result2 = 8 >> 1 + 1  # + has higher precedence: 8 >> (1 + 1) = 8 >> 2 = 2
print(result2)  # Expected: 2
";
        
        #endregion
        
        #region String Operation Tests
        
        public static readonly string STRING_CONCATENATION = @"
# Test: String concatenation
s1 = 'Hello'
s2 = ' '
s3 = 'World'
result = s1 + s2 + s3
print(result)  # Expected: Hello World

# Mixed types
result2 = 'Count: ' + str(42)
print(result2)  # Expected: Count: 42
";
        
        public static readonly string STRING_METHODS = @"
# Test: String methods
s = '  Hello World  '
print(len(s))  # Expected: 15

# Indexing
print(s[2])  # Expected: ' '
print(s[-1])  # Expected: ' '

# Slicing
print(s[2:7])  # Expected: 'Hello'
";
        
        public static readonly string STRING_IN_LISTS = @"
# Test: Strings in lists
words = ['apple', 'banana', 'cherry']
print(len(words))  # Expected: 3

# Sort strings
sorted_words = sorted(words)
print(sorted_words)  # Expected: ['apple', 'banana', 'cherry']

# Lambda with strings
longest = sorted(words, key=lambda x: len(x))
print(longest[-1])  # Expected: 'banana' or 'cherry' (both length 6)
";
        
        #endregion
        
        #region Loop Conditions Tests
        
        public static readonly string WHILE_COMPLEX_CONDITION = @"
# Test: While loop with complex condition
i = 0
sum = 0
while i < 10 and sum < 20:
    sum += i
    i += 1

print(i)    # Expected: 7
print(sum)  # Expected: 21
";
        
        public static readonly string FOR_WITH_CONDITION = @"
# Test: For loop with conditions inside
result = []
for i in range(10):
    if i % 2 == 0 and i > 2:
        result.append(i)

print(result)  # Expected: [4, 6, 8]
";
        
        public static readonly string NESTED_CONDITIONS_IN_LOOPS = @"
# Test: Nested conditions in loops
result = []
for i in range(5):
    if i % 2 == 0:
        for j in range(3):
            if j % 2 == 1:
                result.append(i * 10 + j)

print(result)  # Expected: [1, 21, 41]
";
        
        #endregion
        
        #region Tuple Tests
        
        public static readonly string TUPLE_BASICS = @"
# Test: Tuple creation and indexing
t = (1, 'a', 3.14)
print(t[0])   # Expected: 1
print(t[1])   # Expected: 'a'
print(t[2])   # Expected: 3.14
print(t[-1])  # Expected: 3.14
";
        
        public static readonly string TUPLE_IN_LIST = @"
# Test: Tuples in lists, iteration
data = [(1, 2), (3, 4), (5, 6)]
for pair in data:
    print(pair[0] + pair[1])
# Expected: 3, 7, 11
";
        
        public static readonly string SINGLE_ELEMENT_TUPLE = @"
# Test: Single element tuple
t = (42,)
print(len(t))  # Expected: 1
print(t[0])    # Expected: 42
";
        
        #endregion
        
        #region Enum Tests
        
        public static readonly string ENUM_USAGE = @"
# Test: Enum member access and comparison
ground = get_ground_type()
if ground == Grounds.Soil:
    print('Standing on soil')

# Multiple comparisons
if ground == Grounds.Grassland:
    print('Grassland detected')
elif ground == Grounds.Soil:
    print('Soil confirmed')
";
        
        public static readonly string ENUM_IN_FUNCTIONS = @"
# Test: Using enums in function calls
plant(Entities.Carrot)

if num_items(Items.Hay) > 50:
    print('Have enough hay')

use_item(Items.Water)
";
        
        #endregion
        
        #region Edge Case Tests
        
        public static readonly string EMPTY_COLLECTIONS = @"
# Test: Empty collections
empty_list = []
empty_tuple = ()
empty_dict = {}

print(len(empty_list))   # Expected: 0
print(len(empty_tuple))  # Expected: 0
print(len(empty_dict))   # Expected: 0
";
        
        public static readonly string NEGATIVE_INDEXING = @"
# Test: Negative indexing edge cases
items = [10, 20, 30, 40, 50]
print(items[-1])   # Expected: 50
print(items[-2])   # Expected: 40
print(items[-5])   # Expected: 10

s = 'hello'
print(s[-1])       # Expected: 'o'
print(s[-5])       # Expected: 'h'
";
        
        public static readonly string DIVISION_BY_ZERO = @"
# Test: Division by zero handling (should error)
try:
    result = 10 / 0
    print(result)
except Exception as e:
    print('Division by zero caught')
";
        
        public static readonly string LIST_MUTATION = @"
# Test: List mutation
items = [1, 2, 3]
items[0] = 100
items[-1] = 300

print(items)  # Expected: [100, 2, 300]
";
        
        #endregion
        
        #region Instruction Budget Test
        
        public static readonly string INSTRUCTION_BUDGET_TEST = @"
# Test: Instruction budget (100 iterations should be instant)
sum = 0
for i in range(100):
    sum += 1

print(sum)  # Expected: 100

# This should take multiple frames (>100 iterations)
sum2 = 0
for i in range(500):
    sum2 += 1

print(sum2)  # Expected: 500
";
        
        #endregion
        
        #region Complete Integration Test
        
        public static readonly string INTEGRATION_TEST = @"
# Complete integration test combining all features
def process_data(data):
    # Nested function
    def transform(x):
        return x ** 2
    
    # List comprehension with condition
    filtered = [x for x in data if x > 0]
    
    # Lambda with sorting
    sorted_data = sorted(filtered, key=lambda x: transform(x))
    
    return sorted_data

# Test with various data types
numbers = [-5, 3, -2, 7, 1, 9, -1]
result = process_data(numbers)
print(result)  # Expected: [1, 3, 7, 9]

# Tuple destructuring in loop
pairs = [(1, 2), (3, 4), (5, 6)]
sum = 0
for pair in pairs:
    sum += pair[0] + pair[1]
print(sum)  # Expected: 21

# Recursion with lambda
def apply_n_times(f, n, x):
    if n == 0:
        return x
    return f(apply_n_times(f, n - 1, x))

double = lambda x: x * 2
result = apply_n_times(double, 3, 5)
print(result)  # Expected: 40 (5*2*2*2)

print('Integration test complete!')
";
        
        #endregion
        
        #region Test Runner
        
        public static readonly string[] ALL_TESTS = new string[]
        {
            LAMBDA_WITH_LIST_COMP,
            LAMBDA_SORTED_TUPLES,
            LAMBDA_MULTIPLE_CONDITIONS,
            LAMBDA_IIFE_COMPLEX,
            LAMBDA_DICT_SORTING,
            LAMBDA_NESTED_INDEXING,
            LAMBDA_MULTI_PARAM,
            NESTED_FUNCTION_CALLS,
            DEEPLY_NESTED_CALLS,
            NESTED_WITH_OPERATORS,
            NESTED_LOOPS_SIMPLE,
            NESTED_LOOPS_WITH_BREAK,
            NESTED_LOOPS_WITH_CONTINUE,
            TRIPLE_NESTED_LOOPS,
            RECURSION_FACTORIAL,
            RECURSION_FIBONACCI,
            MUTUAL_RECURSION,
            EXPONENTIATION_PRECEDENCE,
            COMPLEX_PRECEDENCE,
            BITWISE_PRECEDENCE,
            STRING_CONCATENATION,
            STRING_METHODS,
            STRING_IN_LISTS,
            WHILE_COMPLEX_CONDITION,
            FOR_WITH_CONDITION,
            NESTED_CONDITIONS_IN_LOOPS,
            TUPLE_BASICS,
            TUPLE_IN_LIST,
            SINGLE_ELEMENT_TUPLE,
            EMPTY_COLLECTIONS,
            NEGATIVE_INDEXING,
            LIST_MUTATION,
            INSTRUCTION_BUDGET_TEST,
            INTEGRATION_TEST
        };
        
        #endregion
    }
}