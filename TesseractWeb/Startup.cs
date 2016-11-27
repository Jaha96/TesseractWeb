using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TesseractWeb.Startup))]
namespace TesseractWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
