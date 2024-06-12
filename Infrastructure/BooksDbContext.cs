using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Infrastructure;

public class BooksDbContext : DbContext 
{
    public BooksDbContext(DbContextOptions<BooksDbContext> options):base(options){}
    
}