namespace CoreApi2.Services
{
    /// <summary>
    /// A
    /// </summary>
    public static class KeyService
    {
        static IConfiguration configuration;

        static KeyService() {

            configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

        }


        internal static string? GetMapApiKey()
        {
            return configuration["GoogleMapsApiKey"];
        }



        internal static List<(string baseUrl, string authKey, string authValue)> GetTechPOS()
        {
            var environments = new[] { "Demo", "Dev" }; // Your environments
            var baseAPIs = new List<(string baseUrl, string authKey, string authValue)>();

            foreach (var env in environments)
            {
                var techPosSection = configuration.GetSection("TechPOSKeys").GetSection(env);

                string? url = techPosSection["Url"];
                string? techPosApiKey = techPosSection["TechPOSAPIKey"];

                baseAPIs.Add((url, "TechPOSAPIKey", techPosApiKey!));
            }

            return baseAPIs;
        }


        internal static string? GetSigningKey()
        {
            return configuration.GetSection("Jwt")["SigningKey"];
        }


    }
}
