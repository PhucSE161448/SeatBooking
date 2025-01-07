using SeatBooking.Domain.Common;
using Microsoft.Identity.Client;

namespace SeatBooking.API.Configurations
{
    public static class AppSettings
    {
        public static void SettingsBinding(this IConfiguration configuration)
        {
            AppConfiguration.ConnectionString = new ConnectionString();
            AppConfiguration.JWTSection = new JwtSection();
            AppConfiguration.EmailConfiguration = new EmailConfiguration();
            AppConfiguration.PayOSConfig = new PayOSConfig();
            AppConfiguration.VnPayConfig = new VnPayConfig();

            configuration.Bind("ConnectionStrings", AppConfiguration.ConnectionString);
            configuration.Bind("JwtSection", AppConfiguration.JWTSection);
            configuration.Bind("EmailConfiguration", AppConfiguration.EmailConfiguration);
            configuration.Bind("PayOSConfigs", AppConfiguration.PayOSConfig);
            configuration.Bind("VnPay", AppConfiguration.VnPayConfig);
        }
    }
}
