using System.Data;
using Autofac;
using SeatBooking.Domain.Common;
using SeatBooking.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SeatBooking.API.Configurations
{
    public static class DBContext
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            services.AddDbContext<SeatBookingContext>(options => options.UseSqlServer(AppConfiguration.ConnectionString.DefaultConnection));
            return services;
        }

        public static ContainerBuilder AddDbContext(this ContainerBuilder builder)
        {
            builder.Register(c => new SqlConnection(AppConfiguration.ConnectionString.DefaultConnection))
                .As<IDbConnection>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SeatBookingContext>().As<DbContext>().InstancePerLifetimeScope();
            return builder;
        }
    }
}
