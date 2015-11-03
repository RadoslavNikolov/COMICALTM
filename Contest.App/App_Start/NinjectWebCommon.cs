using System;
using System.Web;
using Contests.App;
using Contests.Data;
using Contests.Data.UnitOfWork;
using Contests.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace Contests.App
{
    using Infrastructure.UserIdProvider;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IContestsData>().To<ContestsData>()
               .InRequestScope()
               .WithConstructorArgument("context", p => new ContestsDbContext());

            kernel.Bind<IUserStore<User>>().To<UserStore<User>>()
                .InRequestScope()
                .WithConstructorArgument("context", kernel.Get<ContestsDbContext>());

            kernel.Bind<IUserIdProvider>().To<AspNetUserIdProvider>()
               .InRequestScope()
               .WithConstructorArgument("context", kernel.Get<ContestsDbContext>());

            kernel.Bind<IAuthenticationManager>()
                .ToMethod<IAuthenticationManager>(context => HttpContext.Current.GetOwinContext().Authentication)
                .InRequestScope();
        }        
    }
}
