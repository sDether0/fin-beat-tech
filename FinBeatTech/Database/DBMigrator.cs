using Microsoft.EntityFrameworkCore;

namespace FinBeatTech.Database
{
    public static class DBMigrator
    {
        public static void InitAndMigrate(string conString)
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>();
            contextOptions.UseNpgsql(conString);
            var tempContext = new AppDbContext(contextOptions.Options);
            if (tempContext.Database.GetPendingMigrations().Any())
            {
                tempContext.Database.Migrate();
            }

        }
    }
}
