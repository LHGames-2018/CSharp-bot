using System.Collections.Generic;

namespace LHGames.DataStructures
{
    public enum ActionTypes
    {
        DefaultAction,
        MoveAction,
        AttackAction,
        CollectAction,
        UpgradeAction,
        StealAction,
        PurchaseAction,
        HealAction
    }

    public enum UpgradeType
    {
        CarryingCapacity,
        AttackPower,
        Defence,
        MaximumHealth,
        CollectingSpeed
    }

    public enum PurchasableItem
    {
        MicrosoftSword,
        UbisoftShield,
        DevolutionsBackpack,
        DevolutionsPickaxe,
        HealthPotion,
    }

    // DO NO REORDER THIS, make sure it matches the typescript tile enum.
    public enum TileContent
    {
        Empty,
        Wall,
        House,
        Lava,
        Resource,
        Shop,
        Player
    }

    public struct GameInfo
    {
        public Player Player;
        public string CustomSerializedMap;
        public List<Player> OtherPlayers;
        public int xMin;
        public int yMin;
    }

    public class Tile
    {
        public TileContent TileType { get; private set; }
        public Point Position { get; private set; }

        public Tile(byte content, int x, int y)
        {
            TileType = (TileContent)content;
            Position = new Point(x, y);
        }
        public override string ToString()
        {
            return TileType.ToString();
        }
    }

    public interface IPlayer
    {
        int Health { get; }
        int MaxHealth { get; }
        int CarriedResources { get; }
        int CarryingCapacity { get; }
        double CollectingSpeed { get; }
        int TotalResources { get; }
        int AttackPower { get; }
        int Defence { get; }
        Point Position { get; }
        Point HouseLocation { get; }
        PurchasableItem[] CarriedItems { get; }
        int Score { get; }
        string Name { get; }
    }

    public class Player : IPlayer
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int CarriedResources { get; set; }
        public int CarryingCapacity { get; set; }
        public double CollectingSpeed { get; set; }
        public int TotalResources { get; set; }
        public int AttackPower { get; set; }
        public int Defence { get; set; }
        public Point Position { get; set; }
        public Point HouseLocation { get; set; }
        public PurchasableItem[] CarriedItems { get; set; }
        public int Score { get; set; }
        public string Name { get; set; }
    }
}
