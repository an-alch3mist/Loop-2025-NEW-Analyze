using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LOOPLanguage
{
    /// <summary>
    /// Implements game-specific built-in functions ("Farmer Was Replaced" style).
    /// Some methods yield (take time), others return instantly.
    /// </summary>
    public class GameBuiltinMethods : MonoBehaviour
    {
        #region Mock Game State (Replace with real game implementation)
        
        private Vector2Int playerPos = new Vector2Int(0, 0);
        private int worldSize = 10;
        private Dictionary<Vector2Int, string> groundTypes = new Dictionary<Vector2Int, string>();
        private Dictionary<Vector2Int, string> entities = new Dictionary<Vector2Int, string>();
        private Dictionary<string, int> inventory = new Dictionary<string, int>();
        
        void Start()
        {
            // Initialize mock state
            for (int x = 0; x < worldSize; x++)
            {
                for (int y = 0; y < worldSize; y++)
                {
                    groundTypes[new Vector2Int(x, y)] = Grounds.Soil;
                }
            }
            
            inventory[Items.Hay] = 100;
            inventory[Items.Water] = 50;
        }
        
        #endregion
        
        #region Method Dispatcher
        
        /// <summary>
        /// Dispatches Python function calls to appropriate C# methods.
        /// Returns IEnumerator for yielding functions, null for instant functions.
        /// </summary>
        public IEnumerator CallMethod(string name, List<object> arguments)
        {
            switch (name)
            {
                // Movement (yields)
                case "move":
                    yield return Move(arguments.Count > 0 ? arguments[0] : null);
                    break;
                    
                // Farming (yields)
                case "harvest":
                    yield return Harvest();
                    break;
                    
                case "plant":
                    yield return Plant(arguments.Count > 0 ? arguments[0] : null);
                    break;
                    
                case "till":
                    yield return Till();
                    break;
                    
                case "use_item":
                    yield return UseItem(arguments.Count > 0 ? arguments[0] : null);
                    break;
                    
                // Utility (yields)
                case "do_a_flip":
                    yield return DoAFlip();
                    break;
                    
                case "sleep":
                    float seconds = arguments.Count > 0 ? (float)(double)arguments[0] : 0f;
                    yield return new WaitForSeconds(seconds);
                    break;
                    
                // Query functions (instant - no yield)
                case "can_harvest":
                    // Instant functions don't yield - they're handled synchronously in interpreter
                    break;
                    
                case "get_ground_type":
                case "get_entity_type":
                case "get_pos_x":
                case "get_pos_y":
                case "get_world_size":
                case "get_water":
                case "num_items":
                case "is_even":
                case "is_odd":
                    // These are instant functions - handled via global scope
                    break;
                    
                default:
                    Debug.LogWarning("Unknown game method: " + name);
                    break;
            }
        }
        
        #endregion
        
        #region Movement Commands (Yielding)
        
        private IEnumerator Move(object direction)
        {
            string dir = direction.ToString().ToLower();
            
            Vector2Int newPos = playerPos;
            
            switch (dir)
            {
                case "north":
                case "up":
                    newPos.y = Mathf.Max(0, playerPos.y - 1);
                    break;
                case "south":
                case "down":
                    newPos.y = Mathf.Min(worldSize - 1, playerPos.y + 1);
                    break;
                case "east":
                case "right":
                    newPos.x = Mathf.Min(worldSize - 1, playerPos.x + 1);
                    break;
                case "west":
                case "left":
                    newPos.x = Mathf.Max(0, playerPos.x - 1);
                    break;
            }
            
            playerPos = newPos;
            
            Debug.Log($"Moved to ({playerPos.x}, {playerPos.y})");
            
            // Simulate movement animation
            yield return new WaitForSeconds(0.3f);
        }
        
        #endregion
        
        #region Farming Commands (Yielding)
        
        private IEnumerator Harvest()
        {
            if (entities.ContainsKey(playerPos))
            {
                string entityType = entities[playerPos];
                entities.Remove(playerPos);
                
                // Add to inventory
                string itemType = entityType; // Simplified mapping
                if (!inventory.ContainsKey(itemType))
                {
                    inventory[itemType] = 0;
                }
                inventory[itemType]++;
                
                Debug.Log($"Harvested {entityType}");
            }
            else
            {
                Debug.LogWarning("Nothing to harvest");
            }
            
            yield return new WaitForSeconds(0.2f);
        }
        
        private IEnumerator Plant(object entity)
        {
            string entityType = entity.ToString();
            entities[playerPos] = entityType;
            
            Debug.Log($"Planted {entityType} at ({playerPos.x}, {playerPos.y})");
            
            yield return new WaitForSeconds(0.3f);
        }
        
        private IEnumerator Till()
        {
            groundTypes[playerPos] = Grounds.Soil;
            
            Debug.Log($"Tilled ground at ({playerPos.x}, {playerPos.y})");
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private IEnumerator UseItem(object item)
        {
            string itemType = item.ToString();
            
            if (inventory.ContainsKey(itemType) && inventory[itemType] > 0)
            {
                inventory[itemType]--;
                Debug.Log($"Used {itemType}");
            }
            else
            {
                Debug.LogWarning($"Item {itemType} not available");
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        #endregion
        
        #region Query Functions (Instant)
        
        public bool CanHarvest()
        {
            return entities.ContainsKey(playerPos);
        }
        
        public string GetGroundType()
        {
            if (groundTypes.ContainsKey(playerPos))
            {
                return groundTypes[playerPos];
            }
            return Grounds.Soil;
        }
        
        public object GetEntityType()
        {
            if (entities.ContainsKey(playerPos))
            {
                return entities[playerPos];
            }
            return null;
        }
        
        public int GetPosX()
        {
            return playerPos.x;
        }
        
        public int GetPosY()
        {
            return playerPos.y;
        }
        
        public int GetWorldSize()
        {
            return worldSize;
        }
        
        public float GetWater()
        {
            // Mock water level
            return 0.5f;
        }
        
        public int NumItems(object item)
        {
            string itemType = item.ToString();
            if (inventory.ContainsKey(itemType))
            {
                return inventory[itemType];
            }
            return 0;
        }
        
        public bool IsEven(int x, int y)
        {
            return (x + y) % 2 == 0;
        }
        
        public bool IsOdd(int x, int y)
        {
            return (x + y) % 2 == 1;
        }
        
        #endregion
        
        #region Utility Commands
        
        private IEnumerator DoAFlip()
        {
            Debug.Log("🤸 *does a flip*");
            yield return new WaitForSeconds(1.0f);
        }
        
        #endregion
        
        #region Registration Helper
        
        /// <summary>
        /// Registers all game functions in the interpreter's global scope.
        /// Call this from PythonInterpreter.RegisterBuiltins().
        /// </summary>
        public void RegisterInScope(Scope scope)
        {
            // Register yielding functions as special markers
            scope.Define("move", "move");
            scope.Define("harvest", "harvest");
            scope.Define("plant", "plant");
            scope.Define("till", "till");
            scope.Define("use_item", "use_item");
            scope.Define("do_a_flip", "do_a_flip");
            scope.Define("sleep", "sleep");
            
            // Register instant query functions
            scope.Define("can_harvest", new BuiltinFunction("can_harvest", args => CanHarvest(), 0, 0));
            scope.Define("get_ground_type", new BuiltinFunction("get_ground_type", args => GetGroundType(), 0, 0));
            scope.Define("get_entity_type", new BuiltinFunction("get_entity_type", args => GetEntityType(), 0, 0));
            scope.Define("get_pos_x", new BuiltinFunction("get_pos_x", args => (double)GetPosX(), 0, 0));
            scope.Define("get_pos_y", new BuiltinFunction("get_pos_y", args => (double)GetPosY(), 0, 0));
            scope.Define("get_world_size", new BuiltinFunction("get_world_size", args => (double)GetWorldSize(), 0, 0));
            scope.Define("get_water", new BuiltinFunction("get_water", args => (double)GetWater(), 0, 0));
            scope.Define("num_items", new BuiltinFunction("num_items", args => (double)NumItems(args[0]), 1, 1));
            scope.Define("is_even", new BuiltinFunction("is_even", args => IsEven((int)(double)args[0], (int)(double)args[1]), 2, 2));
            scope.Define("is_odd", new BuiltinFunction("is_odd", args => IsOdd((int)(double)args[0], (int)(double)args[1]), 2, 2));
        }
        
        #endregion
    }
}