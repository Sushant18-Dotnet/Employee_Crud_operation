using Employee_Crud_operation;
using System.Collections.Generic;


// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using Employee_Crud_operation.Models;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;

namespace Employee_Crud_operation.Data
{

    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
           

        }

        public DbSet<EmployeeMaster> EmployeeMaster { get; set; }
        public DbSet<Dropdowns> Dropdowns { get; set; }
        
  

    }
}
