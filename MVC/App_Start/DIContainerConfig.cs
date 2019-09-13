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

            // Register MVC controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly)
                .Where(c => c.Name.EndsWith("Controller"));

            // Register viewModels and DTOs
            builder.RegisterAssemblyTypes(typeof(BookDTO).Assembly)
                .Where(x => x.Name.EndsWith("DTO") && x.Namespace.EndsWith("ViewModels"))
                .As(x => x.GetInterfaces().Where(i => i.Name == "I" + x.Name));

            builder.RegisterAssemblyTypes(typeof(BookIndexViewModel).Assembly)
                .Where(x => x.Name.EndsWith("ViewModel") && x.Namespace.EndsWith("ViewModels"))
                .AsSelf();

            //builder.RegisterType<BookIndexViewModel>().AsSelf();
            //builder.RegisterType<AuthorIndexViewModel>().AsSelf();

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