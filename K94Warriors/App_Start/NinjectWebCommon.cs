using System.Configuration;
using System.Data.Entity;
using K94Warriors.Data;

[assembly: WebActivator.PreApplicationStartMethod(typeof(K94Warriors.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(K94Warriors.App_Start.NinjectWebCommon), "Stop")]

namespace K94Warriors.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

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
            kernel.Bind(typeof (IRepository<>)).To(typeof (EFRepository<>));
            var useAzureSql = bool.Parse(ConfigurationManager.AppSettings["UseAzureSQL"]);
            var connectionString = useAzureSql 
                ? ConfigurationManager.ConnectionStrings["K9"].ConnectionString 
                : ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            kernel.Bind<DbContext>().To<K9DbContext>()
                  .WithConstructorArgument("nameOrConnectionString", connectionString);

            kernel.Bind<IBlobRepository>().To<K9BlobRepository>()
                  .WithConstructorArgument("connectionString",
                                           ConfigurationManager.AppSettings["StorageAccountConnectionString"])
                  .WithConstructorArgument("imageContainer",
                                           ConfigurationManager.AppSettings["ImageBlobContainerName"]);
        }        
    }
}
