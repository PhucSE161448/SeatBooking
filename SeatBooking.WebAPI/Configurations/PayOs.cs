using SeatBooking.Domain.Common;
using Microsoft.Identity.Client;
using Net.payOS;

namespace SeatBooking.API.Configurations
{
    public static class PayOs
    {
        public static IServiceCollection AddPayOs(this IServiceCollection services)
        {
            PayOS payOs = new PayOS(AppConfiguration.PayOSConfig.PAYOS_CLIENT_ID,
                AppConfiguration.PayOSConfig.PAYOS_API_KEY,
                AppConfiguration.PayOSConfig.PAYOS_CHECKSUM_KEY);
            services.AddSingleton(payOs);
            return services;
        }
    }
}
