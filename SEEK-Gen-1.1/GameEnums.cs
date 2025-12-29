namespace LoopLanguage
{
    /// <summary>
    /// Ground/terrain type enum accessed as Grounds.Soil, Grounds.Turf, etc.
    /// Internally stored as strings for compatibility with game functions.
    /// </summary>
    public static class Grounds
    {
        public static readonly string Soil = "soil";
        public static readonly string Turf = "turf";
        public static readonly string Grassland = "grassland";
    }
    
    /// <summary>
    /// Item type enum accessed as Items.Hay, Items.Carrot, etc.
    /// Used for inventory operations and item management.
    /// </summary>
    public static class Items
    {
        public static readonly string Hay = "hay";
        public static readonly string Wood = "wood";
        public static readonly string Carrot = "carrot";
        public static readonly string Pumpkin = "pumpkin";
        public static readonly string Power = "power";
        public static readonly string Sunflower = "sunflower";
        public static readonly string Water = "water";
    }
    
    /// <summary>
    /// Entity type enum accessed as Entities.Grass, Entities.Carrot, etc.
    /// Used for planting and entity management.
    /// </summary>
    public static class Entities
    {
        public static readonly string Grass = "grass";
        public static readonly string Bush = "bush";
        public static readonly string Tree = "tree";
        public static readonly string Carrot = "carrot";
        public static readonly string Pumpkin = "pumpkin";
        public static readonly string Sunflower = "sunflower";
    }
}
