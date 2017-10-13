using System;
using System.Web.UI;
using Utility;

namespace Airbrake.Web.HttpException
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Creates a new Book using Book form.
        /// </summary>
        /// <returns>Created Book.</returns>
        private Book CreateBook()
        {
            try
            {
                // Ensure that book title is present.
                if (BookTitle.Text == "")
                {
                    throw new System.Web.HttpException($"Book Title cannot be empty.");
                }

                // Ensure that book author is present.
                if (BookAuthor.Text == "")
                {
                    throw new System.Web.HttpException($"Book Author cannot be empty.");
                }

                // Check if no publication date was selected.
                if (BookPublicationDate.SelectedDate.Date == DateTime.MinValue)
                {
                    return new Book(
                        BookTitle.Text,
                        BookAuthor.Text,
                        BookPageCount.Text == "" ? 0 : Convert.ToInt32(BookPageCount.Text)
                    );
                }

                // Instantiate new Book.
                return new Book(
                    BookTitle.Text,
                    BookAuthor.Text,
                    BookPageCount.Text == "" ? 0 : Convert.ToInt32(BookPageCount.Text),
                    BookPublicationDate.SelectedDate
                );
            }
            catch (FormatException exception)
            {
                // Output expected FormatExceptions.
                Logging.Log(exception);
                // Throw new HttpException.
                throw new System.Web.HttpException(exception.Message);
            }
        }

        protected void BookSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Get generated Book.
                var book = CreateBook();
                // Generate output message.
                var output = $"New Book: {book}";
                // Output to log.
                Logging.Log(output);
                // Modify label for Standard output.
                BookLabel.Text = output;
                BookLabel.ForeColor = System.Drawing.Color.Black;
            }
            catch (System.Web.HttpException exception)
            {
                // Output expected HttpExceptions.
                Logging.Log(exception);
                // Modify label for Error output.
                BookLabel.Text = $"HttpException: {exception.Message}";
                BookLabel.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}