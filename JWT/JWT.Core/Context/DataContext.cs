using JWT.Core.Entity;
using JWT.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace JWT.Core.Context
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public void SetGlobalQuery<T>(ModelBuilder builder) where T : BaseEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entity.ClrType))
                {
                    var method = typeof(DataContext)
                        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery")
                        .MakeGenericMethod(entity.ClrType);
                    method.Invoke(this, new object[] { modelBuilder });
                }
            }

            //modelBuilder.Entity<Setting>().HasIndex(x => x.Name).IsUnique();
            base.OnModelCreating(modelBuilder);
            modelBuilder.Indexes();
            modelBuilder.DefaultValues();
        }



        #region Dbsets
        //public DbSet<User> Users { get; set; }

        #endregion

    }
}