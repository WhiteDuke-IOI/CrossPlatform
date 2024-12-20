﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab_1.Models;
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

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Passenger>()
                .HasMany(ps => ps.Flights)
                .WithMany();

            /*modelBuilder.Entity<Flight>()
            .Property(f => f.Number)
            .ValueGeneratedOnAdd();*/
        }

        public DbSet<Lab_1.Models.Passenger> Passengers { get; set; } = default!;
        public DbSet<Lab_1.Models.Flight> Flights { get; set; } = default!;
        public DbSet<Lab_1.Models.Route> Routes { get; set; } = default!;
    }
}
