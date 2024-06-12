namespace BooksAPI.Entities;

public class Book:BaseEntity
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public decimal Price { get; set; }
}