// <News/>Newspaper.cs
using System;
using System.Collections.Generic;
using Utility;

namespace Observer.News
{
    /// <summary>
    /// Receives Articles from Agencies for publication.
    /// 
    /// Acts as 'Observer' in Observer pattern.
    /// </summary>
    public class Newspaper : IObserver<Article>
    {
        private readonly List<Article> _articles = new List<Article>();
        private readonly SortedList<Agency, IDisposable> _cancellations = new SortedList<Agency, IDisposable>();

        public string Name { get; }

        public Newspaper(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Fires when provider has finished.
        /// Destroys all saved articles.
        /// </summary>
        public virtual void OnCompleted()
        {
            _articles.Clear();
        }
        
        public void OnError(Exception exception)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called by provider with newly-added articles.
        /// </summary>
        /// <param name="article">New notification.</param>
        public virtual void OnNext(Article article)
        {
            // If article already exists, ignore.
            if (_articles.Contains(article)) return;

            // Add article.
            _articles.Add(article);

            // Output article.
            Logging.Log($"{Name} received {article}");
        }

        /// <summary>
        /// Subscribe to passed Agency notifications.
        /// </summary>
        /// <param name="agency">Agency to subscribe to.</param>
        public virtual void Subscribe(Agency agency)
        {
            // Add cancellation token.
            _cancellations.Add(agency, agency.Subscribe(this));

            // Output subscription info.
            Logging.LineSeparator($"{Name} SUBSCRIBED TO {agency.Name}", 70);
        }

        /// <summary>
        /// Unsubscribes from passed Agency notifications.
        /// </summary>
        /// <param name="agency">Agency to unsubscribe from.</param>
        public virtual void Unsubscribe(Agency agency)
        {
            // Dispose.
            _cancellations[agency].Dispose();

            // Remove all articles from agency.
            _articles.RemoveAll(x => x.Agency == agency);

            // Output subscription info.
            Logging.LineSeparator($"{Name} UNSUBSCRIBED TO {agency.Name}", 70);
        }
    }
}