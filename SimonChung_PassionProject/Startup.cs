using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SimonChung_PassionProject.Startup))]
namespace SimonChung_PassionProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
