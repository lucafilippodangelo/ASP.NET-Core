using Microsoft.EntityFrameworkCore;

namespace OdeToFood.Entities
{
    public class OdeToFoodDbContext : DbContext
    {

        public OdeToFoodDbContext(DbContextOptions options) : base(options)
        {}
        
        //LD STEP20
        public DbSet<Restaurant> Restaurants { get; set; }
    }
}
