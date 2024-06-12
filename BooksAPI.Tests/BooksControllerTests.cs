using BooksAPI.Controllers;
using BooksAPI.Entities;
using BooksAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Tests;

public class BooksControllerTests
{
       private readonly DbContextOptions<BooksDbContext> _dbContextOptions;

        public BooksControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<BooksDbContext>()
                .UseInMemoryDatabase(databaseName: "BookTestDb")
                .Options;
        }

        private BooksDbContext GetContext() => new BooksDbContext(_dbContextOptions);

        [Fact]
        public async Task GetBooks_ReturnsAllBooks()
        {
            using (var context = GetContext())
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", Author = "Author 1", Price = 9.99m });
                context.Books.Add(new Book { Id = 2, Title = "Book 2", Author = "Author 2", Price = 19.99m });
                context.SaveChanges();
            }

            using (var context = GetContext())
            {
                var controller = new BooksController(context);
                var result = await controller.GetBooks();
                var books = result.Value;

                Assert.Equal(2, books.Count());
            }
        }

        [Fact]
        public async Task GetBook_ReturnsBookById()
        {
            using (var context = GetContext())
            {
                context.Books.Add(new Book { Id = 1, Title = "Book 1", Author = "Author 1", Price = 9.99m });
                context.SaveChanges();
            }

            using (var context = GetContext())
            {
                var controller = new BooksController(context);
                var result = await controller.GetBook(1);
                var book = result.Value;

                Assert.NotNull(book);
                Assert.Equal(1, book.Id);
            }
        }

        [Fact]
        public async Task PostBook_CreatesNewBook()
        {
            using (var context = GetContext())
            {
                var controller = new BooksController(context);
                var newBook = new Book { Title = "New Book", Author = "New Author", Price = 29.99m };

                var result = await controller.PostBook(newBook);
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
                var createdBook = Assert.IsType<Book>(createdAtActionResult.Value);

                Assert.Equal("New Book", createdBook.Title);
                Assert.Equal("New Author", createdBook.Author);
            }

            using (var context = GetContext())
            {
                Assert.Equal(1, context.Books.Count());
            }
        }

        [Fact]
        public async Task PutBook_UpdatesExistingBook()
        {
            using (var context = GetContext())
            {
                context.Books.Add(new Book { Id = 1, Title = "Old Book", Author = "Old Author", Price = 9.99m });
                context.SaveChanges();
            }

            using (var context = GetContext())
            {
                var controller = new BooksController(context);
                var updatedBook = new Book { Id = 1, Title = "Updated Book", Author = "Updated Author", Price = 19.99m };

                var result = await controller.PutBook(1, updatedBook);
                Assert.IsType<NoContentResult>(result);
            }

            using (var context = GetContext())
            {
                var book = context.Books.Find(1);
                Assert.Equal("Updated Book", book.Title);
                Assert.Equal("Updated Author", book.Author);
            }
        }

        [Fact]
        public async Task DeleteBook_RemovesBook()
        {
            using (var context = GetContext())
            {
                context.Books.Add(new Book { Id = 1, Title = "Book to Delete", Author = "Author", Price = 9.99m });
                context.SaveChanges();
            }

            using (var context = GetContext())
            {
                var controller = new BooksController(context);
                var result = await controller.DeleteBook(1);
                Assert.IsType<NoContentResult>(result);
            }

            using (var context = GetContext())
            {
                Assert.Equal(0, context.Books.Count());
            }
        }
    }
