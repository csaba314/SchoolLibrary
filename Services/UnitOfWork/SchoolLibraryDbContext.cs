using Services.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Services.UnitOfWork
{
    public class SchoolLibraryDbContext : DbContext
    {
        
        public SchoolLibraryDbContext()
            : base("SchoolLibraryDb")
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}