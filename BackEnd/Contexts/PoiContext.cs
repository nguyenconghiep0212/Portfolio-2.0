using Microsoft.EntityFrameworkCore;
using BackEnd.Models;

namespace BackEnd.Contexts
{
    public class PoiContext : DbContext
    {
        public PoiContext(DbContextOptions<PoiContext> options) : base(options) { }

        public DbSet<Poi> Poi { get; set; }
    }
}