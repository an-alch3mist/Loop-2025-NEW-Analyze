namespace LoopLanguage
{
    /// <summary>
    /// COMPREHENSIVE TEST SUITE - Extended Edition
    /// Covers all edge cases for Farmer Was Replaced clone
    /// Including time budget dependent/independent functions
    /// </summary>
    public static class ComprehensiveTestSuite
    {
        #region Sleep Function Tests (Time Budget Independent - ALWAYS Yields)
        
        public static readonly string TEST_SLEEP_INTEGER = @"
# Test: sleep() with integer argument
print('Before sleep')
sleep(2)  # Should work with integer (converts to float)
print('After 2 second sleep')
";
        
        public static readonly string TEST_SLEEP_FLOAT = @"
# Test: sleep() with float argument
print('Before sleep')
sleep(2.0)  # Should work with float
print('After 2.0 second sleep')
";
        
        public static readonly string TEST_SLEEP_DECIMAL = @"
# Test: sleep() with decimal values
print('Starting')
sleep(0.5)
print('After 0.5 seconds')
sleep(1.5)
print('After 1.5 more seconds')
sleep(0.1)
print('After 0.1 more seconds')
";
        
        public static readonly string TEST_SLEEP_IN_LOOP = @"
# Test: sleep() inside loop (should pause each iteration)
for i in range(3):
    print('Iteration', i)
    sleep(1)  # Pause 1 second between iterations
print('Loop complete')
";
        
        public static readonly string TEST_SLEEP_ZERO = @"
# Test: sleep(0) - should still yield once
print('Before zero sleep')
sleep(0)
print('After zero sleep')
";
        
        #endregion
        
        #region Time Budget Dependent Functions (IEnumerator - Yields for Animation)
        
        public static readonly string TEST_MOVE_ALL_DIRECTIONS = @"
# Test: Move in all 4 directions with constants
print('Starting position:', get_pos_x(), get_pos_y())

move(North)
print('After North:', get_pos_x(), get_pos_y())

move(East)
print('After East:', get_pos_x(), get_pos_y())

move(South)
print('After South:', get_pos_x(), get_pos_y())

move(West)
print('After West:', get_pos_x(), get_pos_y())
";
        
        public static readonly string TEST_MOVE_WITH_STRINGS = @"
# Test: Move using string directions
move('up')
print('Moved up')

move('down')
print('Moved down')

move('left')
print('Moved left')

move('right')
print('Moved right')
";
        
        public static readonly string TEST_HARVEST_LOOP = @"
# Test: Harvest in a loop (time budget dependent)
harvested = 0
for i in range(5):
    if can_harvest():
        harvest()
        harvested += 1
        print('Harvested:', harvested)
    sleep(0.1)  # Small delay between harvests

print('Total harvested:', harvested)
";
        
        public static readonly string TEST_PLANT_SEQUENCE = @"
# Test: Plant different entities in sequence
print('Planting sequence')

plant(Entities.Grass)
print('Planted Grass')
sleep(0.2)

plant(Entities.Carrot)
print('Planted Carrot')
sleep(0.2)

plant(Entities.Pumpkin)
print('Planted Pumpkin')
sleep(0.2)
";
        
        public static readonly string TEST_TILL_BEFORE_PLANT = @"
# Test: Till ground before planting
print('Ground type:', get_ground_type())

till()
print('Tilled ground')

print('Ground type after till:', get_ground_type())

plant(Entities.Carrot)
print('Planted carrot')
";
        
        public static readonly string TEST_USE_ITEM_SEQUENCE = @"
# Test: Use items from inventory
print('Water:', num_items(Items.Water))

if num_items(Items.Water) > 0:
    use_item(Items.Water)
    print('Used water')
    print('Water remaining:', num_items(Items.Water))

print('Power:', num_items(Items.Power))
";
        
        public static readonly string TEST_DO_A_FLIP = @"
# Test: Easter egg function (1 second animation)
print('Before flip')
do_a_flip()
print('After flip')
";
        
        #endregion
        
        #region Time Budget Independent Functions (Instant Return)
        
        public static readonly string TEST_CAN_HARVEST_CHECK = @"
# Test: can_harvest() returns instantly
for i in range(10):
    if can_harvest():
        print('Can harvest at iteration', i)
    else:
        print('Cannot harvest at iteration', i)
";
        
        public static readonly string TEST_GET_GROUND_TYPE = @"
# Test: get_ground_type() returns instantly
ground = get_ground_type()
print('Ground type:', ground)

if ground == Grounds.Soil:
    print('Standing on soil')
elif ground == Grounds.Turf:
    print('Standing on turf')
elif ground == Grounds.Grassland:
    print('Standing on grassland')
else:
    print('Unknown ground type')
";
        
        public static readonly string TEST_GET_ENTITY_TYPE = @"
# Test: get_entity_type() returns instantly
entity = get_entity_type()
print('Entity type:', entity)

if entity == Entities.Grass:
    print('Found grass')
elif entity == Entities.Carrot:
    print('Found carrot')
elif entity == None:
    print('No entity here')
";
        
        public static readonly string TEST_POSITION_FUNCTIONS = @"
# Test: Position functions return instantly
for i in range(100):
    x = get_pos_x()
    y = get_pos_y()
    # This loop should complete instantly
    
print('Final position:', get_pos_x(), get_pos_y())
";
        
        public static readonly string TEST_GET_WORLD_SIZE = @"
# Test: get_world_size() returns instantly
size = get_world_size()
print('World size:', size)

# Use world size in calculations
total_tiles = size * size
print('Total tiles:', total_tiles)
";
        
        public static readonly string TEST_GET_WATER = @"
# Test: get_water() returns instantly (0.0 to 1.0)
water = get_water()
print('Water level:', water)

if water > 0.8:
    print('High water')
elif water > 0.5:
    print('Medium water')
elif water > 0.2:
    print('Low water')
else:
    print('Very low water')
";
        
        public static readonly string TEST_NUM_ITEMS_ALL = @"
# Test: num_items() for all item types (instant)
print('=== INVENTORY ===')
print('Hay:', num_items(Items.Hay))
print('Wood:', num_items(Items.Wood))
print('Carrot:', num_items(Items.Carrot))
print('Pumpkin:', num_items(Items.Pumpkin))
print('Power:', num_items(Items.Power))
print('Sunflower:', num_items(Items.Sunflower))
print('Water:', num_items(Items.Water))
print('===============')
";
        
        public static readonly string TEST_IS_EVEN_IS_ODD = @"
# Test: is_even() and is_odd() helpers (instant)
for i in range(5):
    for j in range(5):
        if is_even(i, j):
            print(i, j, 'is even')
        if is_odd(i, j):
            print(i, j, 'is odd')
";
        
        #endregion
        
        #region Mixed Time Budget Operations
        
        public static readonly string TEST_MIXED_OPERATIONS_1 = @"
# Test: Mix of instant and yielding operations
print('Position:', get_pos_x(), get_pos_y())  # Instant

move(North)  # Yields ~0.3s

print('New position:', get_pos_x(), get_pos_y())  # Instant

if can_harvest():  # Instant
    harvest()  # Yields ~0.2s
    print('Harvested!')

sleep(1)  # Yields exactly 1s

print('Done')
";
        
        public static readonly string TEST_MIXED_OPERATIONS_2 = @"
# Test: Fast queries in loop with occasional yields
count = 0
for i in range(100):
    # These are instant (no yield)
    x = get_pos_x()
    y = get_pos_y()
    ground = get_ground_type()
    
    count += 1
    
    # Yield every 10 iterations
    if i % 10 == 0:
        sleep(0.1)
        print('Progress:', i)

print('Completed', count, 'iterations')
";
        
        public static readonly string TEST_GRID_SCAN = @"
# Test: Scan entire grid (mix of instant queries and movement)
size = get_world_size()
print('Scanning', size, 'x', size, 'grid')

scanned = 0
for i in range(size):
    for j in range(size):
        # Instant queries
        ground = get_ground_type()
        entity = get_entity_type()
        scanned += 1
        
        # Move to next tile (yields)
        if j < size - 1:
            move(East)
    
    # Move to next row
    if i < size - 1:
        move(South)
        # Move back to start of row
        for k in range(size - 1):
            move(West)

print('Scanned', scanned, 'tiles')
";
        
        #endregion
        
        #region Instruction Budget vs Time Budget Tests
        
        public static readonly string TEST_INSTRUCTION_BUDGET = @"
# Test: Instruction budget triggers (no sleep/game commands)
# This should distribute across multiple frames
sum = 0
for i in range(1000):
    sum += i
    # No yield commands, but instruction budget will cause yields
    
print('Sum:', sum)  # Should be 499500
";
        
        public static readonly string TEST_NO_BUDGET_NEEDED = @"
# Test: Small operations complete in one frame
sum = 0
for i in range(50):
    sum += i

print('Sum:', sum)  # Should complete instantly
";
        
        public static readonly string TEST_BUDGET_WITH_SLEEP = @"
# Test: Instruction budget + explicit sleep
sum = 0
for i in range(500):
    sum += i
    if i % 100 == 0:
        sleep(0.1)  # Explicit yield
        print('Checkpoint:', i)

print('Final sum:', sum)
";
        
        #endregion
        
        #region Type Conversion Tests
        
        public static readonly string TEST_INT_TO_FLOAT_SLEEP = @"
# Test: Integer automatically converts to float for sleep
sleep(1)    # int -> should work
sleep(2)    # int -> should work
sleep(3.5)  # float -> should work

print('All sleep types work')
";
        
        public static readonly string TEST_NUMBER_TYPES = @"
# Test: Number type handling (all stored as double)
x = 10      # Integer literal
y = 10.5    # Float literal
z = x + y   # Mixed arithmetic

print('x:', x)
print('y:', y)
print('z:', z)

# Should all work with functions
print('int(y):', int(y))
print('float(x):', float(x))
";
        
        public static readonly string TEST_STRING_NUMBER_CONVERSION = @"
# Test: String to number conversion
num_str = '42'
num = int(num_str)
print('Parsed:', num)

float_str = '3.14'
float_val = float(float_str)
print('Parsed float:', float_val)

# Use in arithmetic
result = num + float_val
print('Result:', result)
";
        
        #endregion
        
        #region Complex Game Scenarios
        
        public static readonly string TEST_FARMING_CYCLE = @"
# Test: Complete farming cycle
def farming_cycle():
    # Check ground
    if get_ground_type() != Grounds.Soil:
        till()
    
    # Plant
    plant(Entities.Carrot)
    print('Planted carrot')
    
    # Wait for growth (simulated)
    sleep(2)
    
    # Harvest
    if can_harvest():
        harvest()
        print('Harvested carrot')
        print('Carrots in inventory:', num_items(Items.Carrot))

farming_cycle()
";
        
        public static readonly string TEST_GRID_FARMING = @"
# Test: Farm a grid pattern
def farm_grid(size):
    for i in range(size):
        for j in range(size):
            # Till and plant
            till()
            plant(Entities.Grass)
            
            # Move to next tile
            if j < size - 1:
                move(East)
        
        # Move to next row
        if i < size - 1:
            move(South)
            for k in range(size - 1):
                move(West)

farm_grid(3)
print('Grid farming complete')
";
        
        public static readonly string TEST_HARVEST_SPIRAL = @"
# Test: Spiral harvest pattern
def spiral_harvest(size):
    x = 0
    y = 0
    
    for layer in range(size // 2):
        # Move right
        for i in range(size - 2*layer - 1):
            if can_harvest():
                harvest()
            move(East)
        
        # Move down
        for i in range(size - 2*layer - 1):
            if can_harvest():
                harvest()
            move(South)
        
        # Move left
        for i in range(size - 2*layer - 1):
            if can_harvest():
                harvest()
            move(West)
        
        # Move up
        for i in range(size - 2*layer - 1):
            if can_harvest():
                harvest()
            move(North)

# spiral_harvest(5)  # Uncomment to test
print('Spiral pattern ready')
";
        
        public static readonly string TEST_RESOURCE_MANAGEMENT = @"
# Test: Resource management logic
def check_resources():
    hay = num_items(Items.Hay)
    water = num_items(Items.Water)
    power = num_items(Items.Power)
    
    print('=== RESOURCES ===')
    print('Hay:', hay)
    print('Water:', water)
    print('Power:', power)
    
    # Resource logic
    if hay < 10:
        print('WARNING: Low hay!')
    
    if water < 5:
        print('WARNING: Low water!')
    
    if power < 20:
        print('WARNING: Low power!')
    
    return hay >= 10 and water >= 5 and power >= 20

has_resources = check_resources()
print('Has sufficient resources:', has_resources)
";
        
        #endregion
        
        #region Edge Cases and Error Conditions
        
        public static readonly string TEST_EMPTY_SLEEP = @"
# Test: sleep() with 0 should still yield once
count = 0
for i in range(5):
    sleep(0)
    count += 1
    print('Iteration:', count)
";
        
        public static readonly string TEST_NEGATIVE_SLEEP = @"
# Test: Negative sleep (should probably error or treat as 0)
try:
    sleep(-1)
    print('Negative sleep completed')
except:
    print('Negative sleep error (expected)')
";
        
        public static readonly string TEST_MOVE_INVALID_DIRECTION = @"
# Test: Invalid direction should error
try:
    move('invalid')
    print('Invalid move succeeded (unexpected)')
except:
    print('Invalid direction error (expected)')
";
        
        public static readonly string TEST_USE_ITEM_NOT_ENOUGH = @"
# Test: Using item when not enough in inventory
# First check inventory
print('Water:', num_items(Items.Water))

# Try to use more than available
try:
    for i in range(100):
        use_item(Items.Water)
except:
    print('Ran out of water (expected error)')
";
        
        public static readonly string TEST_PLANT_INVALID_ENTITY = @"
# Test: Planting with invalid entity
try:
    plant('invalid_entity')
    print('Invalid plant succeeded (unexpected)')
except:
    print('Invalid entity error (expected)')
";
        
        #endregion
        
        #region Performance Tests
        
        public static readonly string TEST_MANY_INSTANT_CALLS = @"
# Test: Many instant function calls (should be fast)
total_checks = 0
for i in range(1000):
    can_harvest()
    get_ground_type()
    get_pos_x()
    get_pos_y()
    total_checks += 4

print('Completed', total_checks, 'instant function calls')
";
        
        public static readonly string TEST_MIXED_PERFORMANCE = @"
# Test: Performance with mixed operations
instant_ops = 0
yield_ops = 0

for i in range(50):
    # Instant operations
    x = get_pos_x()
    y = get_pos_y()
    instant_ops += 2
    
    # Occasional yield operation
    if i % 10 == 0:
        move(North)
        yield_ops += 1

print('Instant ops:', instant_ops)
print('Yield ops:', yield_ops)
";
        
        #endregion
        
        #region Lambda with Game Functions
        
        public static readonly string TEST_LAMBDA_WITH_GAME_FUNCTIONS = @"
# Test: Lambda functions using game queries
positions = []

# Collect positions using lambda
collect_pos = lambda: (get_pos_x(), get_pos_y())

for i in range(3):
    pos = collect_pos()
    positions.append(pos)
    print('Position:', pos)
    move(East)

print('All positions:', positions)
";
        
        public static readonly string TEST_SORTED_BY_POSITION = @"
# Test: Sort entities by distance from origin
entities = [
    (3, 4),  # Distance 5
    (1, 1),  # Distance 1.41
    (0, 5),  # Distance 5
    (2, 2)   # Distance 2.83
]

# Sort by distance from origin
sorted_entities = sorted(entities, key=lambda pos: (pos[0]**2 + pos[1]**2)**0.5)

for entity in sorted_entities:
    print('Position:', entity)
";
        
        #endregion
        
        #region Comprehensive Integration Test
        
        public static readonly string TEST_FULL_GAME_SIMULATION = @"
# Test: Complete game simulation
print('=== GAME SIMULATION START ===')

# 1. Check starting state
print('Starting position:', get_pos_x(), get_pos_y())
print('Starting inventory:', num_items(Items.Hay))

# 2. Move around and check ground
moves = [North, East, South, West]
for direction in moves:
    move(direction)
    ground = get_ground_type()
    print('Moved, ground type:', ground)
    sleep(0.1)

# 3. Farming operations
print('--- Farming Phase ---')
till()
plant(Entities.Carrot)
sleep(1)

if can_harvest():
    harvest()
    print('Harvested! Carrots:', num_items(Items.Carrot))

# 4. Resource check
print('--- Resource Check ---')
water = num_items(Items.Water)
power = num_items(Items.Power)
print('Water:', water, 'Power:', power)

# 5. Complex logic
print('--- Complex Logic ---')
size = get_world_size()
center_x = size // 2
center_y = size // 2

current_x = get_pos_x()
current_y = get_pos_y()

print('Center:', center_x, center_y)
print('Current:', current_x, current_y)

# 6. Lambda usage
check_inventory = lambda item: num_items(item) > 0
print('Has hay:', check_inventory(Items.Hay))
print('Has water:', check_inventory(Items.Water))

print('=== GAME SIMULATION END ===')
";
        
        #endregion
        
        #region Python-Style Number Handling Tests
        
        public static readonly string TEST_NUMBER_EQUALITY = @"
# Test: Python-style number equality
# 1 == 1.0 should be True (unlike C#/Java)
x = 1
y = 1.0

if x == y:
    print('PASS: 1 == 1.0 is True')
else:
    print('FAIL: 1 == 1.0 is False')

# Test with expressions
a = 10 / 2  # 5.0
b = 5       # 5

if a == b:
    print('PASS: 10/2 == 5 is True')
else:
    print('FAIL: 10/2 == 5 is False')
";
        
        public static readonly string TEST_LIST_INDEX_REQUIRES_INTEGER = @"
# Test: List indexing requires integers (not floats)
items = [10, 20, 30, 40, 50]

# This should work
print(items[0])    # 10
print(items[2])    # 30

# This should work (1.0 is integer value)
print(items[int(1.0)])  # 20

# This should ERROR (1.5 is not integer)
try:
    x = items[1.5]
    print('FAIL: list[1.5] should error')
except:
    print('PASS: list[1.5] correctly raised error')
";
        
        public static readonly string TEST_RANGE_REQUIRES_INTEGER = @"
# Test: range() requires integers
# This should work
nums = range(5)
print('range(5) works:', len(nums))

# This should work (5.0 is integer value)
nums2 = range(int(5.0))
print('range(5.0 as int) works:', len(nums2))

# This should ERROR (5.5 is not integer)
try:
    nums3 = range(5.5)
    print('FAIL: range(5.5) should error')
except:
    print('PASS: range(5.5) correctly raised error')
";
        
        public static readonly string TEST_SLICE_REQUIRES_INTEGER = @"
# Test: Slicing requires integer indices
items = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# This should work
print(items[1:5])  # [2, 3, 4, 5]

# This should ERROR (1.5 is not integer)
try:
    x = items[1.5:5]
    print('FAIL: slice with float start should error')
except:
    print('PASS: slice with float correctly raised error')
";
        
        public static readonly string TEST_NUMBER_DISPLAY = @"
# Test: Number display (Python format)
# Integers display without decimal point
x = 1.0
y = 1.5
z = 2.0

print(x)  # Should display '1' not '1.0'
print(y)  # Should display '1.5'
print(z)  # Should display '2' not '2.0'

# Expressions
a = 10 / 2  # 5.0
print(a)    # Should display '5'

b = 10 / 3  # 3.333...
print(b)    # Should display '3.333...'
";
        
        public static readonly string TEST_NUMBER_MIXED_ARITHMETIC = @"
# Test: Mixed int/float arithmetic (Python behavior)
x = 5      # Integer
y = 2.5    # Float

# All operations work
print(x + y)   # 7.5
print(x - y)   # 2.5
print(x * y)   # 12.5
print(x / y)   # 2

# Comparisons work
print(x > y)   # True
print(x < y)   # False
print(x == 5.0)  # True (equality works)
";
        
        #endregion
        
        #region All Extended Tests Collection
        
        public static readonly string[] ALL_EXTENDED_TESTS = new string[]
        {
            // Sleep tests
            TEST_SLEEP_INTEGER,
            TEST_SLEEP_FLOAT,
            TEST_SLEEP_DECIMAL,
            TEST_SLEEP_IN_LOOP,
            TEST_SLEEP_ZERO,
            
            // Time budget dependent
            TEST_MOVE_ALL_DIRECTIONS,
            TEST_MOVE_WITH_STRINGS,
            TEST_HARVEST_LOOP,
            TEST_PLANT_SEQUENCE,
            TEST_TILL_BEFORE_PLANT,
            TEST_USE_ITEM_SEQUENCE,
            TEST_DO_A_FLIP,
            
            // Time budget independent
            TEST_CAN_HARVEST_CHECK,
            TEST_GET_GROUND_TYPE,
            TEST_GET_ENTITY_TYPE,
            TEST_POSITION_FUNCTIONS,
            TEST_GET_WORLD_SIZE,
            TEST_GET_WATER,
            TEST_NUM_ITEMS_ALL,
            TEST_IS_EVEN_IS_ODD,
            
            // Mixed operations
            TEST_MIXED_OPERATIONS_1,
            TEST_MIXED_OPERATIONS_2,
            TEST_GRID_SCAN,
            
            // Instruction vs time budget
            TEST_INSTRUCTION_BUDGET,
            TEST_NO_BUDGET_NEEDED,
            TEST_BUDGET_WITH_SLEEP,
            
            // Type conversions
            TEST_INT_TO_FLOAT_SLEEP,
            TEST_NUMBER_TYPES,
            TEST_STRING_NUMBER_CONVERSION,
            
            // Complex game scenarios
            TEST_FARMING_CYCLE,
            TEST_GRID_FARMING,
            TEST_HARVEST_SPIRAL,
            TEST_RESOURCE_MANAGEMENT,
            
            // Edge cases
            TEST_EMPTY_SLEEP,
            TEST_NEGATIVE_SLEEP,
            TEST_MOVE_INVALID_DIRECTION,
            TEST_USE_ITEM_NOT_ENOUGH,
            TEST_PLANT_INVALID_ENTITY,
            
            // Performance
            TEST_MANY_INSTANT_CALLS,
            TEST_MIXED_PERFORMANCE,
            
            // Lambda with game functions
            TEST_LAMBDA_WITH_GAME_FUNCTIONS,
            TEST_SORTED_BY_POSITION,
            
            // Integration
            TEST_FULL_GAME_SIMULATION,
            
            // Python-style number handling (NEW)
            TEST_NUMBER_EQUALITY,
            TEST_LIST_INDEX_REQUIRES_INTEGER,
            TEST_RANGE_REQUIRES_INTEGER,
            TEST_SLICE_REQUIRES_INTEGER,
            TEST_NUMBER_DISPLAY,
            TEST_NUMBER_MIXED_ARITHMETIC
        };
        
        #endregion
        
        #region Test Descriptions
        
        public static readonly string[] TEST_DESCRIPTIONS = new string[]
        {
            "Sleep with integer argument (2 converts to 2.0)",
            "Sleep with float argument (2.0)",
            "Sleep with decimal values (0.5, 1.5, 0.1)",
            "Sleep inside loop - pauses each iteration",
            "Sleep with zero - still yields once",
            
            "Move in all 4 directions using constants",
            "Move using string directions ('up', 'down', etc)",
            "Harvest in loop with time delays",
            "Plant different entities in sequence",
            "Till ground before planting",
            "Use items from inventory",
            "Do a flip (easter egg, 1s animation)",
            
            "can_harvest() instant check in loop",
            "get_ground_type() instant with enum comparison",
            "get_entity_type() instant check",
            "Position functions instant (get_pos_x/y)",
            "get_world_size() instant",
            "get_water() instant (0.0 to 1.0)",
            "num_items() for all item types",
            "is_even() and is_odd() helpers",
            
            "Mix of instant queries and yielding commands",
            "Fast queries in loop with occasional yields",
            "Grid scan with instant queries and movement",
            
            "Instruction budget triggers on 1000 iterations",
            "Small operations complete in one frame",
            "Instruction budget + explicit sleep",
            
            "Integer to float conversion for sleep()",
            "Number type handling (int/float)",
            "String to number conversion",
            
            "Complete farming cycle (till, plant, harvest)",
            "Grid farming pattern",
            "Spiral harvest pattern",
            "Resource management logic",
            
            "Empty sleep(0) still yields",
            "Negative sleep error handling",
            "Invalid move direction error",
            "Using item when not enough in inventory",
            "Planting invalid entity error",
            
            "1000 instant function calls performance",
            "Mixed instant and yield operations",
            
            "Lambda with game function calls",
            "Sorted by distance using lambda",
            
            "Full game simulation integration test"
        };
        
        #endregion
    }
}
