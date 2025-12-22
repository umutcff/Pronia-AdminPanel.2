using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using ProniaUmut.Models;

namespace ProniaUmut.Contexts
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions options):base(options)
        {
        }
        
        public DbSet<ShippingItem> ShippingItems { get; set; }

    }
}
