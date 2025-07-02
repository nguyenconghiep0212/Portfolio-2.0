using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BackEnd.Contexts
{
    public class IncidentContext : DbContext
    {
        public IncidentContext(DbContextOptions<IncidentContext> options)
       : base(options)
        {
        }

        public DbSet<Incident> Incident { get; set; } = null!;
        public DbSet<City> City { get; set; } = null!;
    }
}
