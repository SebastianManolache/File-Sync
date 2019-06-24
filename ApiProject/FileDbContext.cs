using Microsoft.EntityFrameworkCore;
using File = Data.Models.File;

namespace ApiProject
{
    public class FileDbContext:DbContext
    {
        public DbSet<File> File { get; set; }

        public FileDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server = tcp:dotnetinternserver.database.windows.net, 1433; Initial Catalog = FileSyncDb; Persist Security Info = False; User ID = dotnet; Password = Pa$$w0rd; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ///modelBuilder.Entity<File>().ToTable("File");
            base.OnModelCreating(modelBuilder);
        }
    }
}
