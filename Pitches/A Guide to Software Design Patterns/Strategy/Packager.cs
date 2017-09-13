// Packager.cs
using Utility;

namespace Strategy
{
    /// <summary>
    /// Client that routes all packages to the passed packaging strategy.
    /// </summary>
    internal class Packager
    {
        protected IPackagingStrategy Strategy;

        public Packager(IPackagingStrategy strategy)
        {
            Strategy = strategy;
        }

        /// <summary>
        /// Packs the passed Package, using the existing strategy.
        /// </summary>
        /// <param name="package"></param>
        public void Pack(Package package)
        {
            // Output the current strategy to the log.
            Logging.LineSeparator(Strategy.GetType().Name);

            // Pack the package using current strategy.
            Strategy.Pack(package);
        }

        /// <summary>
        /// Packs the passed Package, using the passed strategy.
        /// </summary>
        /// <param name="package">Package to pack.</param>
        /// <param name="strategy">Strategy to use.</param>
        public void Pack(Package package, IPackagingStrategy strategy)
        {
            // Assign to local strategy.
            Strategy = strategy;

            // Pass to default Pack method.
            Pack(package);
        }
    }
}