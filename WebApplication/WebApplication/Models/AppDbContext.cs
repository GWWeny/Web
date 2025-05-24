using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=SQLServerConnectionString") { }

        public DbSet<Custom> Customs { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}