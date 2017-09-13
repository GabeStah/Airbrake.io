// PackagingStrategy.cs
namespace Strategy
{
    /// <summary>
    /// Strategy interface used to pack all packages.
    /// </summary>
    internal interface IPackagingStrategy
    {
        void Pack(Package package);
    }

    /// <summary>
    /// Default packaging strategy.
    /// 
    /// Abstract class forces this class to be inherited.
    /// </summary>
    internal abstract class PackagingStrategy : IPackagingStrategy
    {
        public virtual void Pack(Package package)
        {
            package.Packaging.Add(PackingMaterials.Box);
            package.Packaging.Add(PackingMaterials.BubbleWrap);
            package.Packaging.Add(PackingMaterials.Tape);
        }
    }
}