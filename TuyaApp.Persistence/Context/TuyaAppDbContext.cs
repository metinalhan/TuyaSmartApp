using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using TuyaApp.Domain.Common;
using TuyaApp.Domain.Entities;
using TuyaApp.Persistence.Extensions;

namespace TuyaApp.Persistence.Context
{
    // This class defines the TuyaAppDbContext, which is used to connect to the database.
    public class TuyaAppDbContext : DbContext
    {
        // Get the path to the directory where the executing assembly is located.
        string newpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        // Override the OnConfiguring method to specify the SQLite database connection string.
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($@"Data Source = {newpath}\database.db ");

        // Define virtual DbSet properties for each database entity.
        public virtual DbSet<MenuProfile> Menus { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<TuyaAccount> TuyaAccounts { get; set; }
        public virtual DbSet<Dashboard> Dashboards { get; set; }

        // Override the OnModelCreating method to specify any model configurations.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configure the MenuProfile entity to use JSON conversion for the MenuSave property.
            builder.Entity<MenuProfile>().Property(e => e.MenuSave).HasJsonConversion<MenuSave>();                     

            //Define one-to-one relation between Device and Dashboard
            builder.Entity<Device>()
               .HasOne(o => o.Dashboard)
               .WithOne(c => c.Device)
               .HasForeignKey<Dashboard>(c => c.Id);

            // Call the base OnModelCreating method.
            base.OnModelCreating(builder);
        }

        // Override the SaveChangesAsync method to set IsActive = true for added entities.
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Get the change tracker for this DbContext.
            var datas = ChangeTracker.Entries<BaseEntity>();

            // Iterate through each entity in the change tracker.
            foreach (var item in datas)
            {
                // If the entity is in the Added state, set IsActive = true.
                switch (item.State)
                {
                    case EntityState.Added:
                        item.Entity.IsActive = true;
                        break;
                    default:
                        break;
                }
            }
            // Call the base SaveChangesAsync method and return the result.
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
