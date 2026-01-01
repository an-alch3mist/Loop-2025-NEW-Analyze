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
		/// Moves player one tile in specified direction
		/// </summary>
		public IEnumerator Walk(List<object> args)
		{
			if (args.Count != 2)
			{
				throw new RuntimeError("walk() accepts just 2 argument");
			}

			string argX = args[0].ToString().ToLower();
			string argY = args[1].ToString().ToLower();
			if (float.TryParse(argX, out float distX) && float.TryParse(argY, out float distY))
			{
				int distXInt = Mathf.RoundToInt(distX);
				int distYInt = Mathf.RoundToInt(distY);
				if (true)
				{
					float duration = 0.28f;
					Transform transform = SPACE_GAME.GameStore.playerData.playerObj.transform;
					Vector3 dir = new Vector3(distXInt, distYInt, 0f).normalized;
					#region without squish
					/*
					Vector3 curr = transform.position;
					Vector3 next = curr + Vector3.right * dist;
					Vector3 scale = transform.localScale;
					for (float time = 0; time < duration; time += Time.deltaTime)
					{
						float t = time * 1f / duration;
						// Debug.Log($"t: {t}");
						Vector3 lerp = curr + (next - curr) * t;
						transform.position = lerp;
						yield return new WaitForEndOfFrame();
						// yield return null; // won't animate will run entire iteration in one go
					}
					transform.position = next;
					*/ 
					#endregion
					yield return MoveWithSquash(
						transform: transform, 
						direction: dir, 
						dist: Vector3.Magnitude(new Vector3(distXInt, distYInt, 0f)), 
						duration: duration, 
						maxStretch: 1.25f);
				}
			}
			else
			{
				throw new RuntimeError("one of argument inside walk() is not of type number");
			}
			#region move aproach reference
			/*
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
				*/
			#endregion
		}
		#region Util Walk
		/// <summary>
		/// Move transform by (direction.normalized * dist) over duration while applying squash & stretch.
		/// </summary>
		private static IEnumerator MoveWithSquash(
			Transform transform, 
			Vector3 direction, 
			float dist, float duration, float maxStretch = 1.25f, 
			bool useWaitForEndOfFrame = true) // since, yield return null shall exec loop in one go, without animating
		{
			if (transform == null) yield break;
			if (dist == 0f || duration <= 0f)
			{
				yield break;
			}

			Vector3 start = transform.position;
			Vector3 moveDir = direction.normalized;
			Vector3 target = start + moveDir * dist;

			Vector3 originalLocalScale = Vector3.one;

			// choose the dominant local axis (x=0,y=1,z=2) for stretching
			// convert world move direction to local space so stretch follows object's local axes
			Vector3 localDir = transform.InverseTransformDirection(moveDir);
			int GetDominantAxis(Vector3 v)
			{
				Vector3 a = new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
				if (a.x >= a.y && a.x >= a.z) return 0;
				if (a.y >= a.x && a.y >= a.z) return 1;
				return 2;
			}
			int dominantAxis = GetDominantAxis(localDir);

			float time = 0f;
			while (time < duration)
			{
				float dt = Time.deltaTime;
				time += dt;
				float tNorm = Mathf.Clamp01(time / duration);

				// position ease (you can choose any curve)
				float posT = Mathf.SmoothStep(0f, 1f, tNorm);
				transform.position = Vector3.Lerp(start, target, posT);

				// squash/stretch: peak in middle of motion
				// peak curve: sin(pi * t) peaks at t=0.5 (value 1)
				float peak = Mathf.Sin(tNorm * Mathf.PI);
				float stretch = Mathf.Lerp(1f, maxStretch, peak); // 1 -> maxStretch -> 1

				// apply stretch on dominant local axis, and inverse scale on other two axes for approximate volume preservation
				Vector3 newLocalScale = Vector3.one;

				if (dominantAxis == 0) // local X dominant
				{
					newLocalScale.x = originalLocalScale.x * stretch;
					float inv = 1f / Mathf.Sqrt(stretch);
					newLocalScale.y = originalLocalScale.y * inv;
					newLocalScale.z = originalLocalScale.z * inv;
				}
				else if (dominantAxis == 1) // local Y dominant
				{
					newLocalScale.y = originalLocalScale.y * stretch;
					float inv = 1f / Mathf.Sqrt(stretch);
					newLocalScale.x = originalLocalScale.x * inv;
					newLocalScale.z = originalLocalScale.z * inv;
				}
				else // local Z dominant
				{
					newLocalScale.z = originalLocalScale.z * stretch;
					float inv = 1f / Mathf.Sqrt(stretch);
					newLocalScale.x = originalLocalScale.x * inv;
					newLocalScale.y = originalLocalScale.y * inv;
				}

				transform.localScale = newLocalScale;

				// yield one frame (use WaitForEndOfFrame to be safe with your runner)
				if (useWaitForEndOfFrame)
					yield return new WaitForEndOfFrame();
				else
					yield return null;
			}

			// finalize
			transform.position = target;
			transform.localScale = originalLocalScale;
		}
		#endregion

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
