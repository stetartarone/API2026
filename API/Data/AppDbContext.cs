using API.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) :DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
}