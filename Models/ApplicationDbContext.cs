using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.Models.DBmodels.Views;

namespace ToolDistribution.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Storage> Storages { get; set; }
        public DbSet<ToolRequest> ToolRequests { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Article> Articles { get; set; }

        public DbSet<ToolsView> toolsView { get; set; }
        public DbSet<ToolRequestsView> toolRequestsView { get; set; }
        public DbSet<ToolsCount> toolsCount { get; set; }
        public DbSet<ArticlesToToolsCount> articlesToToolsCount { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ToolsCount>(pc =>
            {
                pc.HasNoKey();
                pc.ToView("ToolsCount");
            });
            modelBuilder.Entity<ToolsView>(pc =>
            {
                pc.HasNoKey();
                pc.ToView("ToolsView");
            });
            modelBuilder.Entity<ToolRequestsView>(pc =>
            {
                pc.HasNoKey();
                pc.ToView("ToolRequestsView");
            });
            modelBuilder.Entity<ArticlesToToolsCount>(pc =>
            {
                pc.HasNoKey();
                pc.ToView("ARTICLESTOTOOLSCOUNT");
            });
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
