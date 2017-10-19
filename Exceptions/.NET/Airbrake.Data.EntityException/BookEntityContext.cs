using System.Data.Entity;

namespace Airbrake.Data.EntityCommandCompilationException
{
    public class BookEntityContext : DbContext
    {
        public DbSet<BookEntity> Books { get; set; }
    }
}