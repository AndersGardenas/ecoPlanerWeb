
using Microsoft.Extensions.DependencyInjection;
using Server.Server.Infrastructure;


namespace ecoPlanerWeb.IoC
{
    public static class IoC
    {
        public static EcoContext EcoContext => IoCContainer.ServiceProvider.GetService<EcoContext>();
    }

        public static class IoCContainer
    {
        public static ServiceProvider ServiceProvider {get; set; }
    }
}
