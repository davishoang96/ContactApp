using ContactApp.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Repository;

public class DatabaseContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
          : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships and keys
        modelBuilder.Entity<PhoneNumber>()
            .HasOne(p => p.Contact)
            .WithMany(c => c.PhoneNumbers)
            .HasForeignKey(p => p.Contact_Id);

        modelBuilder.Entity<Contact>()
        .HasMany(c => c.PhoneNumbers)
        .WithOne(p => p.Contact)
        .HasForeignKey(p => p.Contact_Id)
        .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete

        base.OnModelCreating(modelBuilder);
    }
}
