// PackagingStrategies.cs
namespace Strategy
{
    /// <summary>
    /// Default packaging strategy.
    /// 
    /// Uses box, bubble wrap, and tape.
    /// </summary>
    internal class DefaultStrategy : PackagingStrategy { }

    /// <summary>
    /// Strategy for flat objects, such as letters.
    /// 
    /// Uses envelope.
    /// </summary>
    internal class FlatStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Envelope);
        }
    }

    /// <summary>
    /// Strategy for fragile objects, such as glassware.
    /// 
    /// Uses box, bubble wrap, foam, and tape.
    /// </summary>
    internal class FragileStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Box);
            package.Packaging.Add(PackingMaterials.BubbleWrap);
            package.Packaging.Add(PackingMaterials.Foam);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }

    /// <summary>
    /// Strategy for perishables, such as food.
    /// 
    /// Uses box, dry ice, foam, and tape.
    /// </summary>
    internal class PerishableStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Box);
            package.Packaging.Add(PackingMaterials.DryIce);
            package.Packaging.Add(PackingMaterials.Foam);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }

    /// <summary>
    /// Strategy for pliable objects, such as clothing.
    /// 
    /// Uses envelope and tape.
    /// </summary>
    internal class PliableStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Envelope);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }

    /// <summary>
    /// Strategy for oversized objects, such as furniture.
    /// 
    /// Uses large box, foam, and tape.
    /// </summary>
    internal class OversizedStrategy : PackagingStrategy
    {
        public override void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.LargeBox);
            package.Packaging.Add(PackingMaterials.Foam);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }
}
