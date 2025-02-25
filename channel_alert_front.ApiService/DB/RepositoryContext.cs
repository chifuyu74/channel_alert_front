#pragma warning disable 8618

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Design;
using channel_alert_front.ApiService.Defines;
using channel_alert_front.ApiService.Entities;

namespace channel_alert_front.ApiService.DB;

public class RepositoryContext : DbContext
{
    private readonly IConfiguration _configuration;

    public RepositoryContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // TODO: Create Secrets
        optionsBuilder
            .UseSqlServer("Server=localhost;Database=channel_alert_front;Trusted_Connection=True;Integrated Security=True;TrustServerCertificate=True;")
            .UseSeeding((context, _) =>
            {
                UserSeeding userConfig = new();
                foreach (User user in userConfig.Seeding())
                {
                    var findUser = context.Set<User>().FirstOrDefault(u => u.UserName == user.UserName);
                    if (findUser == null)
                    {
                        context.Set<User>().Add(user);
                    }

                }

                AlertHistorySeeding alertHistoryConfiguration = new();
                foreach (AlertHistory history in alertHistoryConfiguration.Seeding())
                {
                    User? foundUser = context.Set<User>().FirstOrDefault((user) => user.Id == history.UserId);
                    if (foundUser == null)
                        continue;

                    if (foundUser.AlertHistories.Count > 0)
                        continue;

                    foundUser.AlertHistories.Add(history);
                }

                context.SaveChanges();
            });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(x => x.AlertHistories)
            .WithOne(y => y.User)
            .HasForeignKey(y => y.UserId);

        modelBuilder.Entity<User>()
            .HasMany(x => x.Subscriptions)
            .WithOne(y => y.User)
            .HasForeignKey(y => y.UserId);
    }

    public DbSet<User> User { get; set; }
    public DbSet<AlertHistory> AlertHistory { get; set; }
    public DbSet<Subscription> Subscription { get; set; }
}

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseSqlServer(
                configuration.GetConnectionString(DbConnections.SqlServer)
            );

        return new RepositoryContext(builder.Options);
    }
}
#pragma warning restore