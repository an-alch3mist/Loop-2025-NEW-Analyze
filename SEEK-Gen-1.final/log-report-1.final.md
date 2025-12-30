=======================================
STARTING COMPREHENSIVE TEST SUITE
Total tests: 86
(35 original + 45 extended tests)
========================================

[TEST 1] Running: Lambda with list comprehension inside
[16, 36, 64]
[TEST 1] ✓ PASSED: Lambda with list comprehension inside

[TEST 2] Running: Sorted with lambda accessing tuple elements
[[1, 'b'], [2, 'c'], [3, 'a']]
[TEST 2] ✓ PASSED: Sorted with lambda accessing tuple elements

[TEST 3] Running: Lambda with multiple conditions
[2, 50, 88]
[TEST 3] ✓ PASSED: Lambda with multiple conditions

[TEST 4] Running: Immediately invoked lambda expression (IIFE)
10
[6, 10, 14, 18]
[TEST 4] ✓ PASSED: Immediately invoked lambda expression (IIFE)

[TEST 5] Running: Lambda with nested indexing
[1, 2, 3]
[4, 5, 6]
[7, 8, 9]
[TEST 5] ✓ PASSED: Lambda with nested indexing

[TEST 6] Running: Lambda with multiple parameters
20
[5, 10]
[TEST 6] ✓ PASSED: Lambda with multiple parameters

[TEST 7] Running: Tuple creation and indexing
1
a
3.14
3.14
[TEST 7] ✓ PASSED: Tuple creation and indexing

[TEST 8] Running: Tuple in list, iteration
3
7
11
[TEST 8] ✓ PASSED: Tuple in list, iteration

[TEST 9] Running: Single element tuple
1
42
[TEST 9] ✓ PASSED: Single element tuple

[TEST 10] Running: Enum member access and comparison
Standing on soil
[TEST 10] ✓ PASSED: Enum member access and comparison

[TEST 11] Running: Using enum in function calls
Planted carrot at (0, 0)
Harvested! Hay count: 1
[TEST 11] ✓ PASSED: Using enum in function calls

[TEST 12] Running: Directional constants in movement
Moved to (0, 0)
Moved to (1, 0)
Moved to (1, 1)
Moved to (0, 1)
Movement complete
[TEST 12] ✓ PASSED: Directional constants in movement

[TEST 13] Running: Exponentiation operator precedence
512
18
64
[TEST 13] ✓ PASSED: Exponentiation operator precedence

[TEST 14] Running: Comprehensive operator test
13
7
30
3.33333333333333
1
1000
False
True
False
True
False
True
False
True
False
1
7
6
-6
10
2
[TEST 14] ✓ PASSED: Comprehensive operator test

[TEST 15] Running: Negative indexing
50
40
10
[TEST 15] ✓ PASSED: Negative indexing

[TEST 16] Running: List slicing
[2, 3, 4]
[0, 1, 2]
[7, 8, 9]
[0, 2, 4, 6, 8]
[1, 3, 5, 7]
[TEST 16] ✓ PASSED: List slicing

[TEST 17] Running: List comprehension
[2, 4, 6, 8, 10]
[2, 4]
[4, 16]
[TEST 17] ✓ PASSED: List comprehension

[TEST 18] Running: Nested function calls as arguments
12
[TEST 18] ✓ PASSED: Nested function calls as arguments

[TEST 19] Running: Deeply nested function calls
26
[TEST 19] ✓ PASSED: Deeply nested function calls

[TEST 20] Running: Nested for loops
0
1
2
3
4
5
6
7
8
[TEST 20] ✓ PASSED: Nested for loops

[TEST 21] Running: Nested while loops
0
1
2
3
4
5
6
7
8
[TEST 21] ✓ PASSED: Nested while loops

[TEST 22] Running: Triple nested loops
8
[TEST 22] ✓ PASSED: Triple nested loops
[TEST 23] Running: Recursion with factorial

[TEST 23] ✓ PASSED: Recursion with factorial
[TEST 24] Running: Recursion with Fibonacci

[TEST 24] ✓ PASSED: Recursion with Fibonacci

[TEST 25] Running: Break and continue in loops
0
1
2
4
5
6
[TEST 25] ✓ PASSED: Break and continue in loops

[TEST 26] Running: Break in nested loops
0
1
3
4
6
7
[TEST 26] ✓ PASSED: Break in nested loops

[TEST 27] Running: While loop with complex conditions
0
2
4
6
[TEST 27] ✓ PASSED: While loop with complex conditions

[TEST 28] Running: String concatenation
Hello, Alice!
Count: 42
[TEST 28] ✓ PASSED: String concatenation

[TEST 29] Running: String indexing
P
n
yth
[TEST 29] ✓ PASSED: String indexing

[TEST 30] Running: Compound assignment operators
15
12
24
6
[TEST 30] ✓ PASSED: Compound assignment operators

[TEST 31] Running: Large loop (tests instruction budget / time slicing)
1000
[TEST 31] ✓ PASSED: Large loop (tests instruction budget / time slicing)

[TEST 32] Running: Nested large loops (tests frame distribution)
2500
[TEST 32] ✓ PASSED: Nested large loops (tests frame distribution)

[TEST 33] Running: Game movement functions
Starting position: 0 0
Moved to (0, 0)
Moved to (1, 0)
Moved to (1, 1)
Moved to (0, 1)
Final position: 0 1
[TEST 33] ✓ PASSED: Game movement functions

[TEST 34] Running: Game harvest functions
Harvested! Hay count: 1
Harvested! Hay count: 2
Harvested! Hay count: 3
Harvested! Hay count: 4
Harvested! Hay count: 5
Hay count: 5
[TEST 34] ✓ PASSED: Game harvest functions

[TEST 35] Running: Plant and harvest cycle
Planted grass at (0, 0)
Moved to (1, 0)
Planted grass at (1, 0)
Moved to (2, 0)
Planted grass at (2, 0)
Moved to (3, 0)
Moved to (2, 0)
Moved to (1, 0)
Moved to (0, 0)
Harvested! Hay count: 1
Moved to (1, 0)
Harvested! Hay count: 2
Moved to (2, 0)
Harvested! Hay count: 3
Moved to (3, 0)
[TEST 35] ✓ PASSED: Plant and harvest cycle

[TEST 36] Running: Full integration test
Even tiles: None
a
b
c
d
e
[TEST 36] ✓ PASSED: Full integration test

[TEST 37] Running: Complex game logic
[TEST 37] ✗ FAILED: Complex game logic

rror: Parse error at line 7: Parse error at line 7: Expected ')' after arguments at line 7, got '
'

[TEST 38] Running: sleep() with integer argument
Before sleep
After 2 second sleep
[TEST 38] ✓ PASSED: sleep() with integer argument

[TEST 39] Running: sleep() with float argument
Before sleep
After 2.0 second sleep
[TEST 39] ✓ PASSED: sleep() with float argument

[TEST 40] Running: sleep() with decimal values
Starting
After 0.5 seconds
After 1.5 more seconds
After 0.1 more seconds
[TEST 40] ✓ PASSED: sleep() with decimal values

[TEST 41] Running: sleep() inside loop (should pause each iteration)
Iteration 0
Iteration 1
Iteration 2
Loop complete
[TEST 41] ✓ PASSED: sleep() inside loop (should pause each iteration)

[TEST 42] Running: sleep(0) - should still yield once
Before zero sleep
After zero sleep
[TEST 42] ✓ PASSED: sleep(0) - should still yield once

[TEST 43] Running: Move in all 4 directions with constants
Starting position: 0 0
Moved to (0, 0)
After North: 0 0
Moved to (1, 0)
After East: 1 0
Moved to (1, 1)
After South: 1 1
Moved to (0, 1)
After West: 0 1
[TEST 43] ✓ PASSED: Move in all 4 directions with constants

[TEST 44] Running: Move using string directions
Moved to (0, 0)
Moved up
Moved to (0, 1)
Moved down
Moved to (0, 1)
Moved left
Moved to (1, 1)
Moved right
[TEST 44] ✓ PASSED: Move using string directions

[TEST 45] Running: Harvest in a loop (time budget dependent)
Harvested! Hay count: 1
Harvested: 1
Harvested! Hay count: 2
Harvested: 2
Harvested! Hay count: 3
Harvested: 3
Harvested! Hay count: 4
Harvested: 4
Harvested! Hay count: 5
Harvested: 5
Total harvested: 5
[TEST 45] ✓ PASSED: Harvest in a loop (time budget dependent)

[TEST 46] Running: Plant different entities in sequence
Planting sequence
Planted grass at (0, 0)
Planted Grass
Planted carrot at (0, 0)
Planted Carrot
Planted pumpkin at (0, 0)
Planted Pumpkin
[TEST 46] ✓ PASSED: Plant different entities in sequence

[TEST 47] Running: Till ground before planting
Ground type: soil
Tilled ground
Tilled ground
Ground type after till: soil
Planted carrot at (0, 0)
Planted carrot
[TEST 47] ✓ PASSED: Till ground before planting

[TEST 48] Running: Use items from inventory
Water: 50
Used water. Remaining: 49
Used water
Water remaining: 49
Power: 100
[TEST 48] ✓ PASSED: Use items from inventory

[TEST 49] Running: Easter egg function (1 second animation)
Before flip
*FLIP*
After flip
[TEST 49] ✓ PASSED: Easter egg function (1 second animation)

[TEST 50] Running: can_harvest() returns instantly
Can harvest at iteration 0
Can harvest at iteration 1
Can harvest at iteration 2
Can harvest at iteration 3
Can harvest at iteration 4
Can harvest at iteration 5
Can harvest at iteration 6
Can harvest at iteration 7
Can harvest at iteration 8
Can harvest at iteration 9
[TEST 50] ✓ PASSED: can_harvest() returns instantly

[TEST 51] Running: get_ground_type() returns instantly
Ground type: soil
Standing on soil
[TEST 51] ✓ PASSED: get_ground_type() returns instantly

[TEST 52] Running: get_entity_type() returns instantly
Entity type: grass
Found grass
[TEST 52] ✓ PASSED: get_entity_type() returns instantly

[TEST 53] Running: Position functions return instantly
Final position: 0 0
[TEST 53] ✓ PASSED: Position functions return instantly

[TEST 54] Running: get_world_size() returns instantly
World size: 10
Total tiles: 100
[TEST 54] ✓ PASSED: get_world_size() returns instantly

[TEST 55] Running: get_water() returns instantly (0.0 to 1.0)
Water level: 0.5
Low water
[TEST 55] ✓ PASSED: get_water() returns instantly (0.0 to 1.0)

[TEST 56] Running: num_items() for all item types (instant)
=== INVENTORY ===
Hay: 0
Wood: 0
Carrot: 0
Pumpkin: 0
Power: 100
Sunflower: 0
Water: 50
===============
[TEST 56] ✓ PASSED: num_items() for all item types (instant)

[TEST 57] Running: is_even() and is_odd() helpers (instant)
0 0 is even
0 1 is odd
0 2 is even
0 3 is odd
0 4 is even
1 0 is odd
1 1 is even
1 2 is odd
1 3 is even
1 4 is odd
2 0 is even
2 1 is odd
2 2 is even
2 3 is odd
2 4 is even
3 0 is odd
3 1 is even
3 2 is odd
3 3 is even
3 4 is odd
4 0 is even
4 1 is odd
4 2 is even
4 3 is odd
4 4 is even
[TEST 57] ✓ PASSED: is_even() and is_odd() helpers (instant)

[TEST 58] Running: Mix of instant and yielding operations
Position: 0 0
Moved to (0, 0)
New position: 0 0
Harvested! Hay count: 1
Harvested!
Done
[TEST 58] ✓ PASSED: Mix of instant and yielding operations

[TEST 59] Running: Fast queries in loop with occasional yields
[TEST 59] ✗ FAILED: Fast queries in loop with occasional yields

rror: Parse error at line 4: Expected indented block after for at line 5, got '
'

[TEST 60] Running: Scan entire grid (mix of instant queries and movement)
[TEST 60] ✗ FAILED: Scan entire grid (mix of instant queries and movement)

rror: Parse error at line 8: Parse error at line 8: Expected indented block after for at line 9, got '
'

[TEST 61] Running: Instruction budget triggers (no sleep/game commands)
Sum: 499500
[TEST 61] ✓ PASSED: Instruction budget triggers (no sleep/game commands)

[TEST 62] Running: Small operations complete in one frame
Sum: 1225
[TEST 62] ✓ PASSED: Small operations complete in one frame

[TEST 63] Running: Instruction budget + explicit sleep
Checkpoint: 0
Checkpoint: 100
Checkpoint: 200
Checkpoint: 300
Checkpoint: 400
Final sum: 124750
[TEST 63] ✓ PASSED: Instruction budget + explicit sleep

[TEST 64] Running: Integer automatically converts to float for sleep
All sleep types work
[TEST 64] ✓ PASSED: Integer automatically converts to float for sleep

[TEST 65] Running: Number type handling (all stored as double)
x: 10
y: 10.5
z: 20.5
int(y): 10
float(x): 10
[TEST 65] ✓ PASSED: Number type handling (all stored as double)

[TEST 66] Running: String to number conversion
Parsed: 42
Parsed float: 3.14
Result: 45.14
[TEST 66] ✓ PASSED: String to number conversion

[TEST 67] Running: Complete farming cycle
[TEST 67] ✗ FAILED: Complete farming cycle

rror: Parse error at line 3: Expected indented block after function definition at line 4, got '
'

[TEST 68] Running: Farm a grid pattern
[TEST 68] ✗ FAILED: Farm a grid pattern

rror: Parse error at line 5: Parse error at line 5: Parse error at line 5: Expected indented block after for at line 6, got '
'

[TEST 69] Running: Spiral harvest pattern
[TEST 69] ✗ FAILED: Spiral harvest pattern

rror: Parse error at line 7: Parse error at line 7: Expected ')' after arguments at line 7, got '
'

[TEST 70] Running: Resource management logic
Has sufficient resources: None
[TEST 70] ✓ PASSED: Resource management logic

[TEST 71] Running: sleep() with 0 should still yield once
Iteration: 1
Iteration: 2
Iteration: 3
Iteration: 4
Iteration: 5
[TEST 71] ✓ PASSED: sleep() with 0 should still yield once

[TEST 72] Running: Negative sleep (should probably error or treat as 0)
[TEST 72] ✗ FAILED: Negative sleep (should probably error or treat as 0)
Error: Parse error at line 3: Expected newline at line 3

[TEST 73] Running: Invalid direction should error
[TEST 73] ✗ FAILED: Invalid direction should error
Error: Parse error at line 3: Expected newline at line 3

