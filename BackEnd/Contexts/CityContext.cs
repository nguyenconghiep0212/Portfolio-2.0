using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BackEnd.Contexts;

public class CityContext : DbContext
{
    public CityContext(DbContextOptions<CityContext> options)
        : base(options)
    {
    }

    public DbSet<City> City { get; set; } = null!;
}