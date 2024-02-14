using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HierarchicalCatalog.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<MenuItem> MenuItems { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>().HasData(
                    new MenuItem { Id = 1, Header = "1лвл Заглушка 1", Order = 1 },
            new MenuItem { Id = 2, Header = "1лвл Заглушка 2", Order = 2 },
            new MenuItem { Id = 3, Header = "1лвл Заглушка 3", Order = 3 },
            new MenuItem { Id = 4, Header = "2лвл Заглушка 4", Order = 1, ParentId = 2 },
            new MenuItem { Id = 5, Header = "2лвл Заглушка 5", Order = 2, ParentId = 2 },
            new MenuItem { Id = 6, Header = "2лвл Заглушка 6", Order = 3, ParentId = 2 },
            new MenuItem { Id = 7, Header = "3лвл Заглушка 7", Order = 1, ParentId = 4 },
            new MenuItem { Id = 8, Header = "3лвл Заглушка 8", Order = 2, ParentId = 4 },
            new MenuItem { Id = 9, Header = "3лвл Заглушка 9", Order = 3, ParentId = 4 },
            new MenuItem { Id = 10, Header = "3лвл Заглушка 10", Order = 1, ParentId = 5 },
            new MenuItem { Id = 11, Header = "3лвл Заглушка 11", Order = 2, ParentId = 5 },
            new MenuItem { Id = 12, Header = "3лвл Заглушка 12", Order = 3, ParentId = 5 }
            );
        }
    }
}