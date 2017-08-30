using System;
using System.Collections.Generic;

namespace Observer.Twitter
{
    public class Service : IObservable<Post>
    {
        private readonly List<IObserver<Post>> _observers;
        private readonly List<Post> _posts;

        public Service()
        {
            _observers = new List<IObserver<Post>>();
            _posts = new List<Post>();
        }

        public IDisposable Subscribe(IObserver<Post> observer)
        {
            // Check whether observer is already registered. If not, add it
            if (_observers.Contains(observer)) return new Unsubscriber<Post>(_observers, observer);

            _observers.Add(observer);
            // Provide observer with existing data.
            foreach (var post in _posts)
            {
                observer.OnNext(post);
            }

            return new Unsubscriber<Post>(_observers, observer);
        }

        // Called to indicate all baggage is now unloaded.
        public void CreatePost(string author)
        {
            CreatePost(author, string.Empty, string.Empty);
        }

        public void CreatePost(string author, string title, string content)
        {
            var post = new Post(author, title, content);

            // Carousel is assigned, so add new info object to list.
            if (!_posts.Contains(post))
            {
                _posts.Add(post);
                foreach (var observer in _observers)
                {
                    observer.OnNext(post);
                }
            }
            //else if (content == 0)
            //{
            //    // Baggage claim for flight is done
            //    var flightsToRemove = new List<Post>();
            //    foreach (var flight in _posts)
            //    {
            //        if (post.Author == flight.Author)
            //        {
            //            flightsToRemove.Add(flight);
            //            foreach (var observer in _observers)
            //                observer.OnNext(post);
            //        }
            //    }
            //    foreach (var flightToRemove in flightsToRemove)
            //        _posts.Remove(flightToRemove);

            //    flightsToRemove.Clear();
            //}
        }

        public void Cleanup()
        {
            foreach (var observer in _observers)
                observer.OnCompleted();

            _observers.Clear();
        }
    }
}