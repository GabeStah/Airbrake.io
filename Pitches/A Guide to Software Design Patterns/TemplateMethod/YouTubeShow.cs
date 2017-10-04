using Utility;

namespace TemplateMethod
{
    internal class YouTubeShow : Show
    {
        public override void AssignBroadcastSlot()
        {
            // Assign a default BroadcastSlot, 
            // which ensures the show is always available.
            BroadcastSlot = new BroadcastSlot();
            Logging.Log($"{Name} is a {BroadcastSlot.Type} broadcast.");
        }

        public override void FindNetwork()
        {
            Network = "YouTube";
            Logging.Log($"Network ({Network}) found for {Name}.");
        }

        /// <summary>
        /// Create new YouTube show.
        /// </summary>
        /// <param name="name">Name of show.</param>
        public YouTubeShow(string name) : base(name)
        {
        }
    }
}