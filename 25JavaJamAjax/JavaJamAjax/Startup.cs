using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JavaJamAjax.Startup))]
namespace JavaJamAjax
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
