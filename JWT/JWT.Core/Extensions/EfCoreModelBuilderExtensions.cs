using Microsoft.EntityFrameworkCore;

namespace JWT.Core.Extensions
{
    public static class EfCoreModelBuilderExtensions
    {
        public static void Indexes(this ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Order>().HasIndex(p => p.Id);
        }

        public static void DefaultValues(this ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Order>().Property(x => x.PlatformType).HasDefaultValue(PlatformType.Website);
        }
    }
}
