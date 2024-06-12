using BooksAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Infrastructure;

public class BooksDbContext : DbContext 
{
    public BooksDbContext(DbContextOptions<BooksDbContext> options):base(options){}
    public DbSet<Book> Books { get; set; }
}