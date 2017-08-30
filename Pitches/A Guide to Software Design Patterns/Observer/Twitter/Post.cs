namespace Observer.Twitter
{
    public class Post
    {
        internal Post(string author, string title, string content)
        {
            Author = author;
            Title = title;
            Content = content;
        }

        public string Author { get; }

        public string Title { get; }

        public string Content { get; }

    }
}