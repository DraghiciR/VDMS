using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VDMS.Startup))]
namespace VDMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app); 
        }
    }
}
