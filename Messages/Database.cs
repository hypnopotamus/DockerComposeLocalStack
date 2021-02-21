using DbUp;

namespace Messages
{
    public static class Database
    {
        public static void Create(string connectionString)
        {
            var deployment = DeployChanges.To.SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(typeof(Database).Assembly)
                .Build();

            EnsureDatabase.For.SqlDatabase(connectionString);
            deployment.PerformUpgrade();
        }
    }
}