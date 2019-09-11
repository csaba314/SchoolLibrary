using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using Services.DI;
using Common.DI;
using System.Web.Mvc;

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

            // Register Services module
            builder.RegisterModule<ServiceDIModule>();

            // Register Common module
            builder.RegisterModule<CommonDIModule>();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}