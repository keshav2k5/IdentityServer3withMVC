using System;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin;
using Owin;
using Identity.Server.Config;
using IdentityServer3.Core.Models;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;

[assembly: OwinStartupAttribute(typeof(Identity.Server.Startup))]
namespace Identity.Server
{
    public partial class Startup
    {
        //public void Configuration(IAppBuilder app)
        //{
        //    ConfigureAuth(app);
        //}

        public void Configuration(IAppBuilder app)
        {
            app.Map("/identity", idsrvApp =>
            {
            idsrvApp.UseIdentityServer(new IdentityServerOptions
            {
                SiteName = "Embedded IdentityServer",
                SigningCertificate = LoadCertificate(),

                Factory = new IdentityServerServiceFactory()
                            .UseInMemoryUsers(Users.Get())
                            .UseInMemoryClients(Clients.Get())
                            .UseInMemoryScopes(Scopes.Get())
                });
            });
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://localhost:44396/identity",
                ClientId = "mvc",
                RedirectUri = "https://localhost:44396/",
                ResponseType = "id_token",

                SignInAsAuthenticationType = "Cookies"
            });
        }

        private X509Certificate2 LoadCertificate()
        {
            var certificate = Convert.FromBase64String(ConfigurationManager.AppSettings["SigningCertificate"]);
            return new X509Certificate2(certificate,"password");

        }
    }
}
