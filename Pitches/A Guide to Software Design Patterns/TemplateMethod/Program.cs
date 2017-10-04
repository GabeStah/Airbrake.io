using System;
using Utility;

namespace TemplateMethod
{
    internal class Program
    {
        private static void Main()
        {
            Logging.LineSeparator("THE OFFICE");
            var theOffice = new NBCShow("The Office", DayOfWeek.Thursday, 21);
            theOffice.Broadcast();

            Logging.LineSeparator("COMPUTERPHILE");
            var computerphile = new YouTubeShow("Computerphile");
            computerphile.Broadcast();

            Logging.LineSeparator("STRANGER THINGS");
            var strangerThings = new NetflixShow("Stranger Things");
            strangerThings.Broadcast();
        }
    }
}
