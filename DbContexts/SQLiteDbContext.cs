using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Q3Books.Models;

namespace Q3Books.DbContexts
{
    public class SQLiteDbContext : DbContext
    {
        public SQLiteDbContext(DbContextOptions<SQLiteDbContext> options) : base(options)
        {

        }
        
        public DbSet<UserModel> user { get; set; }
        public DbSet<BookModel> book { get; set; }
        public DbSet<ConfigModel> config { get; set; }

    }
}
