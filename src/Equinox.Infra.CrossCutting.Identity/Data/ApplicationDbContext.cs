using System.IO;
using System.Reflection;
using Equinox.Infra.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Equinox.Infra.CrossCutting.Identity.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base (options) { }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            // get the configuration from the app settings
            var config = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("appsettings.json")
                .Build ();

            // define the database to use
            optionsBuilder.UseSqlServer (config.GetConnectionString ("DefaultConnection"));
        }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext> {
        public ApplicationDbContext CreateDbContext (string[] args) {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext> ();

            var config = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("appsettings.json")
                .Build ();

            builder.UseSqlServer (config.GetConnectionString ("DefaultConnection"),
                optionsBuilder => optionsBuilder.MigrationsAssembly (typeof (ApplicationDbContext).GetTypeInfo ().Assembly.GetName ().Name));

            return new ApplicationDbContext (builder.Options);
        }
    }

}