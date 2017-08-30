// <News/>Agency.cs
using System;
using System.Collections.Generic;

namespace Observer.News
{
    /// <summary>
    /// News agency that publishes articles, 
    /// which can be picked up by other outlets like Newspapers.
    /// 
    /// Acts as 'Provider' in Observer pattern.
    /// </summary>
    public class Agency : IObservable<Article>, IComparable
    {
        private readonly List<Article> _articles = new List<Article>();
        private readonly List<IObserver<Article>> _observers = new List<IObserver<Article>>();

        public string Name { get; }

        public Agency(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Invoked by observers that wish to subscribe to Agency notifications.
        /// </summary>
        /// <param name="observer">Observer to add.</param>
        /// <returns>IDisposable reference, which allows observers to unsubscribe.</returns>
        public IDisposable Subscribe(IObserver<Article> observer)
        {
            // Check if list contains observer.
            if (!_observers.Contains(observer))
            {
                // Add observer to list.
                _observers.Add(observer);
            }

            // Return a new Unsubscriber<Article> instance.
            return new Unsubscriber<Article>(_observers, observer);
        }

        /// <summary>
        /// Comparison method for IComparison interface, used for sorting.
        /// </summary>
        /// <param name="agency">Agency to be compared.</param>
        /// <returns>Comparison result.</returns>
        public int CompareTo(object agency)
        {
            if (agency is null) return 1;

            var other = agency as Agency;

            // Check that parameter is Article.
            if (other is null) throw new ArgumentException("Compared object must be an Agency instance.", nameof(agency));

            // Sort by name.
            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        /// <summary>
        /// Publishes a new article with title and author.
        /// </summary>
        /// <param name="title">Article title.</param>
        /// <param name="author">Article author.</param>
        public void Publish(string title, string author)
        {
            // Create new Article.
            var article = new Article(title, author, this);

            // If article already exists, abort.
            if (_articles.Contains(article)) return;

            // Add article to list.
            _articles.Add(article);

            // Invoke OnNext for every subscribed observer.
            foreach (var observer in _observers)
            {
                observer.OnNext(article);
            }
        }

        /// <summary>
        /// Halts all notification pushes, invokes OnCompleted for all observers,
        /// and removes all subscribed observers.
        /// </summary>
        public void Shutdown()
        {
            foreach (var observer in _observers)
                observer.OnCompleted();

            _observers.Clear();
        }
    }
}