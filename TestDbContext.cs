using Microsoft.EntityFrameworkCore;
using StudyBuddy;
using StudyBuddy.Models;

public class TestDbContext : IDisposable
{
    public ApplicationDbContext Context { get; private set; }

    public TestDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDbForTesting")
            .Options;

        Context = new ApplicationDbContext(options);
        Context.Database.EnsureCreated();
        InitializeEntities();
    }

    private void InitializeEntities()
    {
        // Optionally add test data here, such as:
        Context.Students.Add(new Student { FirstName = "Test", LastName = "User", Email = "test@example.com" });
        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
