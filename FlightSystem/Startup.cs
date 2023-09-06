using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FlightSystem.Startup))]
namespace FlightSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
