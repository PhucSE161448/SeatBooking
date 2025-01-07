using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatBooking.Domain.Common
{
    public class ConnectionString
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }
    public class JwtSection
    {
        public string SecretKey { get; set; } =
            "This is my custom Secret key for authentication MoreThan128bitsSecretKey";
        public bool ValidateIssuerSigningKey { get; set; }
        public string? IssuerSigningKey { get; set; }
        public bool ValidateIssuer { get; set; } = true;
        public string? ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; } = true;
        public string? ValidAudience { get; set; }
        public bool RequireExpirationTime { get; set; }
        public bool ValidateLifetime { get; set; } = true;
    }
    public class EmailConfiguration
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string EmailSender { get; set; }
        public string Password { get; set; }
    }

    public class PayOSConfig
    {
        public string PAYOS_CHECKSUM_KEY { get; set; } = string.Empty;
        public string PAYOS_API_KEY { get; set; } = string.Empty;
        public string PAYOS_CLIENT_ID { get; set; } = string.Empty;
    }
    public class VnPayConfig
    {
        public string Url { get; set; } = string.Empty;
        public string TmnCode { get; set; } = string.Empty;
        public string HashSecret { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public string ReturnAppointmentUrl { get; set; } = string.Empty;
    }

    public class AppConfiguration
    {
        public static ConnectionString ConnectionString { get; set; }
        public static EmailConfiguration EmailConfiguration { get; set; }
        public static PayOSConfig PayOSConfig { get; set; }
        public static VnPayConfig VnPayConfig { get; set; }
        public static JwtSection JWTSection { get; set; }

    }
}
