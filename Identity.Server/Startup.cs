using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Identity.Server.Startup))]
namespace Identity.Server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
