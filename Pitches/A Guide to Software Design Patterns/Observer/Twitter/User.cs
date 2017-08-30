using System;
using System.Collections.Generic;
using Utility;

namespace Observer.Twitter
{
    public class User : IObserver<Post>
    {
        private readonly string _name;
        private List<string> flightInfos = new List<string>();
        private IDisposable cancellation;
        private const string fmt = "{0,-20} {1,5}  {2, 3}";

        public User(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("The observer must be assigned a name.");

            _name = name;
        }

        public virtual void Subscribe(Service provider)
        {
            cancellation = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            cancellation.Dispose();
            flightInfos.Clear();
        }

        public virtual void OnCompleted()
        {
            flightInfos.Clear();
        }

        // No implementation needed: Method is not called by the BaggageHandler class.
        public virtual void OnError(Exception exception)
        {
            // No implementation.
        }

        // Update information.
        public virtual void OnNext(Post post)
        {
            bool updated = false;

            // Flight has unloaded its baggage; remove from the monitor.
            if (string.IsNullOrEmpty(post.Content))
            {
                var flightsToRemove = new List<string>();
                string flightNo = $"{post.Author,5}";

                foreach (var flightInfo in flightInfos)
                {
                    if (flightInfo.Substring(21, 5).Equals(flightNo))
                    {
                        flightsToRemove.Add(flightInfo);
                        updated = true;
                    }
                }
                foreach (var flightToRemove in flightsToRemove)
                    flightInfos.Remove(flightToRemove);

                flightsToRemove.Clear();
            }
            else
            {
                // Add flight if it does not exist in the collection.
                string flightInfo = String.Format(fmt, post.Author, post.Title, post.Content);
                if (!flightInfos.Contains(flightInfo))
                {
                    flightInfos.Add(flightInfo);
                    updated = true;
                }
            }
            if (updated)
            {
                flightInfos.Sort();
                Logging.Log("Arrivals information from {0}", this._name);
                foreach (var flightInfo in flightInfos)
                    Logging.Log(flightInfo);

            }
        }
    }

}
