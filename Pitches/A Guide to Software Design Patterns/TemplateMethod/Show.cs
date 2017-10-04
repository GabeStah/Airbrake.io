using Utility;

namespace TemplateMethod
{
    internal abstract class Show
    {
        public BroadcastSlot BroadcastSlot { get; set; }
        public string Name { get; set; }
        public string Network { get; set; }

#region Abstract methods.
        // These methods must be overriden.
        public abstract void AssignBroadcastSlot();
        public abstract void FindNetwork();
#endregion

#region Default methods.
        // These methods can be left as their default implementations.
        public virtual void CastActors() => Logging.Log($"Casting actors for {Name}.");
        public virtual void ShootPilot() => Logging.Log($"Shooting pilot for {Name}.");
        public virtual void WriteScript() => Logging.Log($"Writing script for {Name}.");
#endregion

        protected Show(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Broadcasts show according to assigned properties.
        /// 
        /// Acts as 'Template Method' in Template Method pattern.
        /// </summary>
        public void Broadcast()
        {
            WriteScript();
            FindNetwork();
            CastActors();
            ShootPilot();
            AssignBroadcastSlot();

            // Output broadcasting message.
            Logging.Log($"Broadcasting {this}.");
        }

        /// <summary>
        /// Get formatted string representation of Show.
        /// </summary>
        /// <returns>Formatted Show information.</returns>
        public override string ToString()
        {
            return $"'{Name}' on {Network} at {BroadcastSlot}";
        }
    }
}