﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab_1.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.Sqlite;

namespace Lab_1.Data
{
    public class Lab1Context : DbContext
    {
        public Lab1Context (DbContextOptions<Lab1Context> options)
            : base(options)
        {
            Database.EnsureCreated(); // создаёт базу данных
            //Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "MyDb.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            optionsBuilder.UseSqlite(connection);

            //optionsBuilder.UseSqlite("Filename=MyDatabase.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Passenger>()
                .HasMany(e => e.Flights)
                .WithMany();

            /*modelBuilder.Entity<Flight>()
            .Property(f => f.Number)
            .ValueGeneratedOnAdd();*/
        }

        public DbSet<Lab_1.Models.Passenger> Passengers { get; set; } = default!;
        public DbSet<Lab_1.Models.Flight> Flights { get; set; } = default!;
    }
}
