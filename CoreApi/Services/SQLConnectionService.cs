namespace CoreApi2.Services
{
    public static class SQLConnectionService
    {

        static IConfiguration configuration;

        static SQLConnectionService()
        {
            configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
        }

        internal static string GetConnectionString()
        {
            return configuration.GetSection("ConnectionStrings")["DefaultConnection"];
        }

    }
}
