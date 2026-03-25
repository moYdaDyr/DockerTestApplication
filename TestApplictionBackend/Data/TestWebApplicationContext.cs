using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TestWebApplication.Models;

namespace TestWebApplication.Data
{
    public class TestWebApplicationContext : DbContext
    {
        public TestWebApplicationContext (DbContextOptions<TestWebApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<TestWebApplication.Models.AccountModel> AccountModel { get; set; } = default!;
        public DbSet<TestWebApplication.Models.ReportModel> ReportModel { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
