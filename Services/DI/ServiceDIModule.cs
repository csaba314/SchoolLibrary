using System.Linq;
using Autofac;
using Services.Models;
using Services.Services;
using Services.UnitOfWork;

namespace Services.DI
{
    public class ServiceDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register all domain model objects
            builder.RegisterAssemblyTypes(typeof(Book).Assembly)
                .Where(t => t.Namespace.EndsWith("Models"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            // Register all Services
            builder.RegisterAssemblyTypes(typeof(BookServices).Assembly)
                .Where(t => t.Namespace.EndsWith("Services"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            // Register DbContext
            builder.RegisterType<SchoolLibraryDbContext>().AsSelf();

            // Register UnitOfWork
            builder.RegisterType<UnitOfWork.UnitOfWork>().As<IUnitOfWork>();
        }
    }
}
