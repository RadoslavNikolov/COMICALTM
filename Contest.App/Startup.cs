using Contests.App;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Contests.App
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
