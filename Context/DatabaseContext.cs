using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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