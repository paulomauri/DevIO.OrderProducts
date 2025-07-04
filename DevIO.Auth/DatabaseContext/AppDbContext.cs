﻿using DevIO.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Auth.DatabaseContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserClaim> Claims => Set<UserClaim>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Mappings.User());
        modelBuilder.ApplyConfiguration(new Mappings.UserClaim());

        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

    }
}
