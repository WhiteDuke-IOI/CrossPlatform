using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab_0.Models;

namespace Lab_0.Data
{
    public class Lab_0Context : DbContext
    {
        public Lab_0Context (DbContextOptions<Lab_0Context> options)
            : base(options)
        {
        }

        public DbSet<Lab_0.Models.Movie> Movie { get; set; } = default!;
    }
}
