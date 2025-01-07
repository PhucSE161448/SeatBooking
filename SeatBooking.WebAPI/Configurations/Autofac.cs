using Autofac.Extensions.DependencyInjection;
using Autofac;
using SeatBooking.Infrastructure;

namespace SeatBooking.API.Configurations
{
    public static class Autofac
    {
        public static void ConfigureAutofacContainer(this WebApplicationBuilder builder)
        {
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(container =>
            {
                container.RegisterModule(new AutofacModule());
            });
        }
    }
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddDbContext();
            builder.RegisterRepository();
            builder.RegisterServices();
            builder.RegisterMapster();
            base.Load(builder);
        }
    }
}
