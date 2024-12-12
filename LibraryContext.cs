using Microsoft.EntityFrameworkCore;

public class LibraryContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LibraryDB;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfigurera Books-tabellen
        modelBuilder.Entity<Book>()
            .ToTable("Books")
            .Property(b => b.Id)
            .ValueGeneratedOnAdd(); // Anger att `Id` ska autogenereras av databasen.

        // Konfigurera Users-tabellen (om du behöver anpassningar)
        modelBuilder.Entity<User>().ToTable("Users");
    }
}
