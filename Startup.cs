using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyMasterOrange.Startup))]
namespace MyMasterOrange
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
