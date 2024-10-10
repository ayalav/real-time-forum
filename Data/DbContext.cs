using Microsoft.EntityFrameworkCore;
using RealTimeForum.Data.Entities; 

namespace RealTimeForum.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts  { get; set; }
    public DbSet<Comment> Comments  { get; set; }   

}
