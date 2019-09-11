using System.Linq;
using Autofac;
using Autofac.Integration.Mvc;
using Services.DI;
using Common.DI;
using System.Web.Mvc;
using MVC.ViewModels;

namespace MVC.App_Start
{
    public static class DIContainerConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            // Register MVC controllers. (MvcApplication is the name of the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly)
                .Where(c => c.Name.EndsWith("Controller"));

            // Register viewModels and DTOs
            builder.RegisterType<BookIndexViewModel>().AsSelf();

            builder.RegisterType<ParameterBuilder>().As<IParameterBuilder>();

            // Register Services module
            builder.RegisterModule<ServiceDIModule>();

            // Register Common module
            builder.RegisterModule<CommonDIModule>();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}