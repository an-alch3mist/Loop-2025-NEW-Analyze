using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoopLanguage
{
    /// <summary>
    /// Implements all game-specific built-in functions.
    /// Functions that yield (IEnumerator) pause Python execution until complete.
    /// Query functions return instantly.
    /// </summary>
    public class GameBuiltinMethods
    {
        #region Fields
        
        // Mock game state for testing (replace with actual game integration)
        private Vector2Int playerPosition = new Vector2Int(0, 0);
        private string currentGroundType = "soil";
        private Dictionary<string, int> inventory = new Dictionary<string, int>();
        private int worldSize = 5;
        
        #endregion
        
        #region Constructor
        
        public GameBuiltinMethods()
        {
            // Initialize inventory
            inventory["hay"] = 0;
            inventory["wood"] = 0;
            inventory["carrot"] = 0;
            inventory["pumpkin"] = 0;
            inventory["power"] = 100;
            inventory["sunflower"] = 0;
            inventory["water"] = 50;
        }
        
        #endregion
        
        #region Time Budget Dependent Functions (IEnumerator)
        
        /// <summary>
        /// Moves player one tile in specified direction
        /// </summary>
        public IEnumerator Move(List<object> args)
        {
            if (args.Count != 1)
            {
                throw new RuntimeError("move() expects 1 argument");
            }
            
            string direction = args[0].ToString().ToLower();
            
            switch (direction)
            {
                case "up":
                case "north":
                    playerPosition.y--;
                    break;
                case "down":
                case "south":
                    playerPosition.y++;
                    break;
                case "left":
                case "west":
                    playerPosition.x--;
                    break;
                case "right":
                case "east":
                    playerPosition.x++;
                    break;
                default:
                    throw new RuntimeError($"Invalid direction: {direction}");
            }
            
            // Clamp to world bounds
            playerPosition.x = Mathf.Clamp(playerPosition.x, 0, worldSize - 1);
            playerPosition.y = Mathf.Clamp(playerPosition.y, 0, worldSize - 1);
            
            Debug.Log($"Moved to ({playerPosition.x}, {playerPosition.y})");
            
            // Simulate movement animation time
            yield return new WaitForSeconds(0.3f);
        }
        
        /// <summary>
        /// Harvests entity at current position
        /// </summary>
        public IEnumerator Harvest(List<object> args)
        {
            if (args.Count != 0)
            {
                throw new RuntimeError("harvest() expects 0 arguments");
            }
            
            // Mock harvest behavior
            inventory["hay"] += 1;
            Debug.Log("Harvested! Hay count: " + inventory["hay"]);
            
            yield return new WaitForSeconds(0.2f);
        }
        
        /// <summary>
        /// Plants specified entity at current position
        /// </summary>
        public IEnumerator Plant(List<object> args)
        {
            if (args.Count != 1)
            {
                throw new RuntimeError("plant() expects 1 argument");
            }
            
            string entity = args[0].ToString();
            Debug.Log($"Planted {entity} at ({playerPosition.x}, {playerPosition.y})");
            
            yield return new WaitForSeconds(0.3f);
        }
        
        /// <summary>
        /// Tills ground at current position
        /// </summary>
        public IEnumerator Till(List<object> args)
        {
            if (args.Count != 0)
            {
                throw new RuntimeError("till() expects 0 arguments");
            }
            
            currentGroundType = "soil";
            Debug.Log("Tilled ground");
            
            yield return new WaitForSeconds(0.1f);
        }
        
        /// <summary>
        /// Uses/consumes one unit of specified item
        /// </summary>
        public IEnumerator UseItem(List<object> args)
        {
            if (args.Count != 1)
            {
                throw new RuntimeError("use_item() expects 1 argument");
            }
            
            string item = args[0].ToString();
            
            if (!inventory.ContainsKey(item))
            {
                throw new RuntimeError($"Unknown item: {item}");
            }
            
            if (inventory[item] <= 0)
            {
                throw new RuntimeError($"Not enough {item}");
            }
            
            inventory[item]--;
            Debug.Log($"Used {item}. Remaining: {inventory[item]}");
            
            yield return new WaitForSeconds(0.1f);
        }
        
        /// <summary>
        /// Player performs flip animation (easter egg)
        /// </summary>
        public IEnumerator DoAFlip(List<object> args)
        {
            if (args.Count != 0)
            {
                throw new RuntimeError("do_a_flip() expects 0 arguments");
            }
            
            Debug.Log("*FLIP*");
            
            yield return new WaitForSeconds(1.0f);
        }
        
        #endregion
        
        #region Time Budget Independent Functions (Instant Return)
        
        /// <summary>
        /// Returns true if entity at current position can be harvested
        /// </summary>
        public object CanHarvest(List<object> args)
        {
            if (args.Count != 0)
            {
                throw new RuntimeError("can_harvest() expects 0 arguments");
            }
            
            // Mock: always return true for testing
            return true;
        }
        
        /// <summary>
        /// Returns ground type at current position
        /// </summary>
        public object GetGroundType(List<object> args)
        {
            if (args.Count != 0)
            {
                throw new RuntimeError("get_ground_type() expects 0 arguments");
            }
            
            return currentGroundType;
        }
        
        /// <summary>
        /// Returns entity type at current position
        /// </summary>
        public object GetEntityType(List<object> args)
        {
            if (args.Count != 0)
            {
                throw new RuntimeError("get_entity_type() expects 0 arguments");
            }
            
            // Mock: return grass
            return "grass";
        }
        
        /// <summary>
        /// Returns current X position
        /// </summary>
        public object GetPosX(List<object> args)
        {
            if (args.Count != 0)
            {
                throw new RuntimeError("get_pos_x() expects 0 arguments");
            }
            
            return (double)playerPosition.x;
        }
        
        /// <summary>
        /// Returns current Y position
        /// </summary>
        public object GetPosY(List<object> args)
        {
            if (args.Count != 0)
            {
                throw new RuntimeError("get_pos_y() expects 0 arguments");
            }
            
            return (double)playerPosition.y;
        }
        
        /// <summary>
        /// Returns world size (width/height are same)
        /// </summary>
        public object GetWorldSize(List<object> args)
        {
            if (args.Count != 0)
            {
                throw new RuntimeError("get_world_size() expects 0 arguments");
            }
            
            return (double)worldSize;
        }
        
        /// <summary>
        /// Returns water level at current position
        /// </summary>
        public object GetWater(List<object> args)
        {
            if (args.Count != 0)
            {
                throw new RuntimeError("get_water() expects 0 arguments");
            }
            
            // Mock: return 0.5 (50% water)
            return 0.5;
        }
        
        /// <summary>
        /// Returns quantity of specified item in inventory
        /// </summary>
        public object NumItems(List<object> args)
        {
            if (args.Count != 1)
            {
                throw new RuntimeError("num_items() expects 1 argument");
            }
            
            string item = args[0].ToString();
            
            if (!inventory.ContainsKey(item))
            {
                return 0.0;
            }
            
            return (double)inventory[item];
        }
        
        /// <summary>
        /// Returns true if (x + y) is even
        /// </summary>
        public object IsEven(List<object> args)
        {
            if (args.Count != 2)
            {
                throw new RuntimeError("is_even() expects 2 arguments");
            }
            
            int x = (int)(double)args[0];
            int y = (int)(double)args[1];
            
            return (x + y) % 2 == 0;
        }
        
        /// <summary>
        /// Returns true if (x + y) is odd
        /// </summary>
        public object IsOdd(List<object> args)
        {
            if (args.Count != 2)
            {
                throw new RuntimeError("is_odd() expects 2 arguments");
            }
            
            int x = (int)(double)args[0];
            int y = (int)(double)args[1];
            
            return (x + y) % 2 == 1;
        }
        
        #endregion
    }
}
