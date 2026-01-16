using CSharpApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CSharpApi.Context
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
    }
}