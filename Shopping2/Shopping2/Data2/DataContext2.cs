﻿using Microsoft.EntityFrameworkCore;
using Shopping2.Data2.Entities;

namespace Shopping2.Data2
{
    public class DataContext2 : DbContext
    {
        public DataContext2(DbContextOptions<DataContext2> options) : base (options)
        {
        }

        public DbSet<Country2> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country2>().HasIndex(c => c.Name).IsUnique();
        }
    }
}
