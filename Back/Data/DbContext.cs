using Microsoft.EntityFrameworkCore;
using RealTimeForum.Data.Entities;

namespace RealTimeForum.Data;

public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
}
