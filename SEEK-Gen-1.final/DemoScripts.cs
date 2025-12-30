using System.Collections.Generic;

namespace LoopLanguage
{
    /// <summary>
    /// Comprehensive test suite for LOOP Language interpreter.
    /// Each script tests specific features and edge cases.
    /// All scripts should execute without errors.
    /// </summary>
    public static class DemoScripts
    {
        #region Lambda Expression Tests
        
        public static readonly string TEST_LAMBDA_WITH_LIST_COMP = @"
# Test: Lambda with list comprehension inside
nums = [1, 2, 3, 4, 5, 6, 7, 8]
result = (lambda x: [i*i for i in x if i % 2 == 0 and i > 3])(nums)
print(result)  # Expected: [16, 36, 64]
";
        
        public static readonly string TEST_LAMBDA_SORTED_TUPLES = @"
# Test: Sorted with lambda accessing tuple elements
data = [(1, 'b'), (3, 'a'), (2, 'c')]
sorted_data = sorted(data, key=lambda x: x[1])
print(sorted_data)  # Expected: [(3, 'a'), (1, 'b'), (2, 'c')]
";
        
        public static readonly string TEST_LAMBDA_MULTI_CONDITIONS = @"
# Test: Lambda with multiple conditions
nums = [-5, 2, 15, 50, 101, 88]
filter_func = lambda x: x > 0 and x % 2 == 0 and x < 100
result = [x for x in nums if filter_func(x)]
print(result)  # Expected: [2, 50, 88]
";
        
        public static readonly string TEST_LAMBDA_IIFE = @"
# Test: Immediately invoked lambda expression (IIFE)
result = (lambda x: x * 2)(5)
print(result)  # Expected: 10

# Complex IIFE with list comprehension
result2 = (lambda nums: [x * 2 for x in nums if x % 2 == 1 and x > 2])([1, 2, 3, 4, 5, 6, 7, 8, 9])
print(result2)  # Expected: [6, 10, 14, 18]
";
        
        public static readonly string TEST_LAMBDA_NESTED_INDEXING = @"
# Test: Lambda with nested indexing
matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]
sorted_matrix = sorted(matrix, key=lambda row: row[1])
for row in sorted_matrix:
    print(row)
# Expected: [1, 2, 3], [4, 5, 6], [7, 8, 9]
";
        
        public static readonly string TEST_LAMBDA_MULTI_PARAMS = @"
# Test: Lambda with multiple parameters
combine = lambda a, b, c: a + b * c
result = combine(10, 5, 2)
print(result)  # Expected: 20

make_pair = lambda x, y: (x, y)
pair = make_pair(5, 10)
print(pair)  # Expected: [5, 10]
";
        
        #endregion
        
        #region Tuple Tests
        
        public static readonly string TEST_TUPLE_BASIC = @"
# Test: Tuple creation and indexing
t = (1, 'a', 3.14)
print(t[0])   # Expected: 1
print(t[1])   # Expected: a
print(t[2])   # Expected: 3.14
print(t[-1])  # Expected: 3.14
";
        
        public static readonly string TEST_TUPLE_ITERATION = @"
# Test: Tuple in list, iteration
data = [(1, 2), (3, 4), (5, 6)]
for pair in data:
    print(pair[0] + pair[1])
# Expected: 3, 7, 11
";
        
        public static readonly string TEST_TUPLE_SINGLE = @"
# Test: Single element tuple
t = (42,)
print(len(t))  # Expected: 1
print(t[0])    # Expected: 42
";
        
        #endregion
        
        #region Enum Tests
        
        public static readonly string TEST_ENUM_COMPARISON = @"
# Test: Enum member access and comparison
ground = get_ground_type()
if ground == Grounds.Soil:
    print('Standing on soil')
elif ground == Grounds.Grassland:
    print('Standing on grassland')
elif ground == Grounds.Turf:
    print('Standing on turf')
";
        
        public static readonly string TEST_ENUM_IN_FUNCTIONS = @"
# Test: Using enum in function calls
plant(Entities.Carrot)
if can_harvest():
    harvest()

if num_items(Items.Carrot) > 0:
    print('Have carrots!')
";
        
        #endregion
        
        #region Built-in Constants Tests
        
        public static readonly string TEST_CONSTANTS_MOVEMENT = @"
# Test: Directional constants in movement
move(North)
move(East)
move(South)
move(West)
print('Movement complete')
";
        
        #endregion
        
        #region Operator Precedence Tests
        
        public static readonly string TEST_EXPONENTIATION = @"
# Test: Exponentiation operator precedence
result = 2 ** 3 ** 2  # Should be 2 ** (3 ** 2) = 2 ** 9 = 512
print(result)  # Expected: 512

result2 = 2 * 3 ** 2  # Should be 2 * (3 ** 2) = 2 * 9 = 18
print(result2)  # Expected: 18

result3 = (2 ** 3) ** 2  # Should be 8 ** 2 = 64
print(result3)  # Expected: 64
";
        
        public static readonly string TEST_ALL_OPERATORS = @"
# Test: Comprehensive operator test
x = 10
y = 3

# Arithmetic
print(x + y)   # 13
print(x - y)   # 7
print(x * y)   # 30
print(x / y)   # 3.333...
print(x % y)   # 1
print(x ** y)  # 1000

# Comparison
print(x == y)  # False
print(x != y)  # True
print(x < y)   # False
print(x > y)   # True
print(x <= y)  # False
print(x >= y)  # True

# Logical
print(True and False)  # False
print(True or False)   # True
print(not True)        # False

# Bitwise
print(5 & 3)   # 1
print(5 | 3)   # 7
print(5 ^ 3)   # 6
print(~5)      # -6
print(5 << 1)  # 10
print(5 >> 1)  # 2
";
        
        #endregion
        
        #region List Operations Tests
        
        public static readonly string TEST_NEGATIVE_INDEXING = @"
# Test: Negative indexing
items = [10, 20, 30, 40, 50]
print(items[-1])   # Expected: 50
print(items[-2])   # Expected: 40
print(items[-5])   # Expected: 10
";
        
        public static readonly string TEST_LIST_SLICING = @"
# Test: List slicing
nums = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
print(nums[2:5])    # Expected: [2, 3, 4]
print(nums[:3])     # Expected: [0, 1, 2]
print(nums[7:])     # Expected: [7, 8, 9]
print(nums[::2])    # Expected: [0, 2, 4, 6, 8]
print(nums[1:8:2])  # Expected: [1, 3, 5, 7]
";
        
        public static readonly string TEST_LIST_COMPREHENSION = @"
# Test: List comprehension
nums = [1, 2, 3, 4, 5]
doubled = [x * 2 for x in nums]
print(doubled)  # Expected: [2, 4, 6, 8, 10]

evens = [x for x in nums if x % 2 == 0]
print(evens)  # Expected: [2, 4]

squares_of_evens = [x*x for x in nums if x % 2 == 0]
print(squares_of_evens)  # Expected: [4, 16]
";
        
        #endregion
        
        #region Nested Function Calls Tests
        
        public static readonly string TEST_NESTED_FUNCTION_CALLS = @"
# Test: Nested function calls as arguments
def _A(x):
    return x + 1

result = _A(_A(1) + _A(_A(2) + _A(4)))
print(result)  # Expected: _A(_A(1) + _A(_A(2) + _A(4)))
               # = _A(2 + _A(3 + 5))
               # = _A(2 + _A(8))
               # = _A(2 + 9)
               # = _A(11)
               # = 12
";
        
        public static readonly string TEST_DEEPLY_NESTED_CALLS = @"
# Test: Deeply nested function calls
def double(x):
    return x * 2

def add(a, b):
    return a + b

result = add(double(add(2, 3)), double(double(4)))
print(result)  # Expected: add(double(5), double(8))
               # = add(10, 16)
               # = 26
";
        
        #endregion
        
        #region Nested Loops Tests
        
        public static readonly string TEST_NESTED_FOR_LOOPS = @"
# Test: Nested for loops
for i in range(3):
    for j in range(3):
        print(i * 3 + j)
# Expected: 0, 1, 2, 3, 4, 5, 6, 7, 8
";
        
        public static readonly string TEST_NESTED_WHILE_LOOPS = @"
# Test: Nested while loops
i = 0
while i < 3:
    j = 0
    while j < 3:
        print(i * 3 + j)
        j += 1
    i += 1
# Expected: 0, 1, 2, 3, 4, 5, 6, 7, 8
";
        
        public static readonly string TEST_TRIPLE_NESTED_LOOPS = @"
# Test: Triple nested loops
count = 0
for i in range(2):
    for j in range(2):
        for k in range(2):
            count += 1
print(count)  # Expected: 8
";
        
        #endregion
        
        #region Recursion Tests
        
        public static readonly string TEST_RECURSION_FACTORIAL = @"
# Test: Recursion with factorial
def factorial(n):
    if n <= 1:
        return 1
    return n * factorial(n - 1)

print(factorial(5))  # Expected: 120
print(factorial(6))  # Expected: 720
";
        
        public static readonly string TEST_RECURSION_FIBONACCI = @"
# Test: Recursion with Fibonacci
def fib(n):
    if n <= 1:
        return n
    return fib(n - 1) + fib(n - 2)

print(fib(10))  # Expected: 55
";
        
        public static readonly string TEST_RECURSION_DEPTH_LIMIT = @"
# Test: Recursion depth limit (should error at MAX_RECURSION_DEPTH)
def infinite_recursion(n):
    return infinite_recursion(n + 1)

# This should throw 'Maximum recursion depth exceeded' error
try:
    infinite_recursion(0)
except Exception as e:
    print('Recursion limit reached')
";
        
        #endregion
        
        #region Control Flow Tests
        
        public static readonly string TEST_BREAK_CONTINUE = @"
# Test: Break and continue in loops
for i in range(10):
    if i == 3:
        continue
    if i == 7:
        break
    print(i)
# Expected: 0, 1, 2, 4, 5, 6
";
        
        public static readonly string TEST_NESTED_BREAK = @"
# Test: Break in nested loops
for i in range(3):
    for j in range(3):
        if j == 2:
            break
        print(i * 3 + j)
# Expected: 0, 1, 3, 4, 6, 7
";
        
        public static readonly string TEST_WHILE_WITH_CONDITIONS = @"
# Test: While loop with complex conditions
i = 0
while i < 10 and i % 2 == 0 or i == 0:
    print(i)
    i += 2
    if i > 6:
        break
# Expected: 0, 2, 4, 6
";
        
        #endregion
        
        #region String Operations Tests
        
        public static readonly string TEST_STRING_CONCATENATION = @"
# Test: String concatenation
name = 'Alice'
greeting = 'Hello, ' + name + '!'
print(greeting)  # Expected: Hello, Alice!

# String with numbers
msg = 'Count: ' + str(42)
print(msg)  # Expected: Count: 42
";
        
        public static readonly string TEST_STRING_INDEXING = @"
# Test: String indexing
text = 'Python'
print(text[0])   # P
print(text[-1])  # n
print(text[1:4]) # yth
";
        
        #endregion
        
        #region Compound Assignment Tests
        
        public static readonly string TEST_COMPOUND_OPERATORS = @"
# Test: Compound assignment operators
x = 10
x += 5
print(x)  # 15

x -= 3
print(x)  # 12

x *= 2
print(x)  # 24

x /= 4
print(x)  # 6
";
        
        #endregion
        
        #region Instruction Budget Tests
        
        public static readonly string TEST_LARGE_LOOP = @"
# Test: Large loop (tests instruction budget / time slicing)
sum = 0
for i in range(1000):
    sum += 1
print(sum)  # Expected: 1000
";
        
        public static readonly string TEST_NESTED_LARGE_LOOPS = @"
# Test: Nested large loops (tests frame distribution)
count = 0
for i in range(50):
    for j in range(50):
        count += 1
print(count)  # Expected: 2500
";
        
        #endregion
        
        #region Game Function Tests
        
        public static readonly string TEST_GAME_MOVEMENT = @"
# Test: Game movement functions
x = get_pos_x()
y = get_pos_y()
print('Starting position:', x, y)

move(North)
move(East)
move(South)
move(West)

x = get_pos_x()
y = get_pos_y()
print('Final position:', x, y)
";
        
        public static readonly string TEST_GAME_HARVEST = @"
# Test: Game harvest functions
for i in range(5):
    if can_harvest():
        harvest()
        
print('Hay count:', num_items(Items.Hay))
";
        
        public static readonly string TEST_GAME_PLANT_HARVEST = @"
# Test: Plant and harvest cycle
for i in range(3):
    plant(Entities.Grass)
    move(East)
    
for i in range(3):
    move(West)
    
for i in range(3):
    if can_harvest():
        harvest()
    move(East)
";
        
        #endregion
        
        #region Edge Cases and Error Handling
        
        public static readonly string TEST_DIVISION_BY_ZERO = @"
# Test: Division by zero error handling
try:
    result = 10 / 0
except Exception as e:
    print('Division by zero caught')
";
        
        public static readonly string TEST_INDEX_OUT_OF_RANGE = @"
# Test: Index out of range error handling
items = [1, 2, 3]
try:
    x = items[10]
except Exception as e:
    print('Index error caught')
";
        
        public static readonly string TEST_UNDEFINED_VARIABLE = @"
# Test: Undefined variable error handling
try:
    x = undefined_var + 5
except Exception as e:
    print('Undefined variable caught')
";
        
        #endregion
        
        #region Integration Tests
        
        public static readonly string TEST_FULL_INTEGRATION = @"
# Test: Full integration test
def process_field(size):
    count = 0
    for i in range(size):
        for j in range(size):
            if is_even(i, j):
                count += 1
    return count

size = get_world_size()
even_tiles = process_field(size)
print('Even tiles:', even_tiles)

# Lambda with sorting
data = [(5, 'e'), (1, 'a'), (3, 'c'), (2, 'b'), (4, 'd')]
sorted_data = sorted(data, key=lambda x: x[0])
for item in sorted_data:
    print(item[1])
# Expected: a, b, c, d, e
";
        
        public static readonly string TEST_COMPLEX_GAME_LOGIC = @"
# Test: Complex game logic
def spiral_harvest(size):
    x = 0
    y = 0
    
    for layer in range(size // 2):
        for i in range(size - 2 * layer - 1):
            move(East)
            if can_harvest():
                harvest()
        
        for i in range(size - 2 * layer - 1):
            move(South)
            if can_harvest():
                harvest()
        
        for i in range(size - 2 * layer - 1):
            move(West)
            if can_harvest():
                harvest()
        
        for i in range(size - 2 * layer - 1):
            move(North)
            if can_harvest():
                harvest()

print('Complex logic test complete')
";
        
        #endregion
        
        #region All Test Scripts Collection
        
        public static readonly string[] ALL_TESTS = new string[]
        {
            TEST_LAMBDA_WITH_LIST_COMP,
            TEST_LAMBDA_SORTED_TUPLES,
            TEST_LAMBDA_MULTI_CONDITIONS,
            TEST_LAMBDA_IIFE,
            TEST_LAMBDA_NESTED_INDEXING,
            TEST_LAMBDA_MULTI_PARAMS,
            TEST_TUPLE_BASIC,
            TEST_TUPLE_ITERATION,
            TEST_TUPLE_SINGLE,
            TEST_ENUM_COMPARISON,
            TEST_ENUM_IN_FUNCTIONS,
            TEST_CONSTANTS_MOVEMENT,
            TEST_EXPONENTIATION,
            TEST_ALL_OPERATORS,
            TEST_NEGATIVE_INDEXING,
            TEST_LIST_SLICING,
            TEST_LIST_COMPREHENSION,
            TEST_NESTED_FUNCTION_CALLS,
            TEST_DEEPLY_NESTED_CALLS,
            TEST_NESTED_FOR_LOOPS,
            TEST_NESTED_WHILE_LOOPS,
            TEST_TRIPLE_NESTED_LOOPS,
            TEST_RECURSION_FACTORIAL,
            TEST_RECURSION_FIBONACCI,
            TEST_BREAK_CONTINUE,
            TEST_NESTED_BREAK,
            TEST_WHILE_WITH_CONDITIONS,
            TEST_STRING_CONCATENATION,
            TEST_STRING_INDEXING,
            TEST_COMPOUND_OPERATORS,
            TEST_LARGE_LOOP,
            TEST_NESTED_LARGE_LOOPS,
            TEST_GAME_MOVEMENT,
            TEST_GAME_HARVEST,
            TEST_GAME_PLANT_HARVEST,
            TEST_FULL_INTEGRATION,
            TEST_COMPLEX_GAME_LOGIC
        };
        
        #endregion
        
        #region Integration with Comprehensive Test Suite
        
        /// <summary>
        /// Combines original tests with comprehensive extended tests
        /// Total: 35 original + 45 extended = 80+ test cases
        /// </summary>
        public static readonly string[] ALL_TESTS_COMBINED = new string[]
        {
            // Original 35 tests
            TEST_LAMBDA_WITH_LIST_COMP,
            TEST_LAMBDA_SORTED_TUPLES,
            TEST_LAMBDA_MULTI_CONDITIONS,
            TEST_LAMBDA_IIFE,
            TEST_LAMBDA_NESTED_INDEXING,
            TEST_LAMBDA_MULTI_PARAMS,
            TEST_TUPLE_BASIC,
            TEST_TUPLE_ITERATION,
            TEST_TUPLE_SINGLE,
            TEST_ENUM_COMPARISON,
            TEST_ENUM_IN_FUNCTIONS,
            TEST_CONSTANTS_MOVEMENT,
            TEST_EXPONENTIATION,
            TEST_ALL_OPERATORS,
            TEST_NEGATIVE_INDEXING,
            TEST_LIST_SLICING,
            TEST_LIST_COMPREHENSION,
            TEST_NESTED_FUNCTION_CALLS,
            TEST_DEEPLY_NESTED_CALLS,
            TEST_NESTED_FOR_LOOPS,
            TEST_NESTED_WHILE_LOOPS,
            TEST_TRIPLE_NESTED_LOOPS,
            TEST_RECURSION_FACTORIAL,
            TEST_RECURSION_FIBONACCI,
            TEST_BREAK_CONTINUE,
            TEST_NESTED_BREAK,
            TEST_WHILE_WITH_CONDITIONS,
            TEST_STRING_CONCATENATION,
            TEST_STRING_INDEXING,
            TEST_COMPOUND_OPERATORS,
            TEST_LARGE_LOOP,
            TEST_NESTED_LARGE_LOOPS,
            TEST_GAME_MOVEMENT,
            TEST_GAME_HARVEST,
            TEST_GAME_PLANT_HARVEST,
            TEST_FULL_INTEGRATION,
            TEST_COMPLEX_GAME_LOGIC
        };
        
        public static string[] GetAllTests()
        {
            // Combine with ComprehensiveTestSuite
            var originalTests = new List<string>(ALL_TESTS);
			// originalTests.Clear();
            var extendedTests = new List<string>(ComprehensiveTestSuite.ALL_EXTENDED_TESTS);
            var combined = new List<string>();
            combined.AddRange(originalTests);
            combined.AddRange(extendedTests);
            return combined.ToArray();
        }
        
        #endregion
    }
}
