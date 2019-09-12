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
            var serviceAssembly = typeof(Book).Assembly;

            // Register all domain model objects
            builder.RegisterAssemblyTypes(serviceAssembly)
                .Where(t => t.Namespace.EndsWith("Models"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            //Register all Services
            //builder.RegisterAssemblyTypes(serviceAssembly)
            //    .Where(t => t.Namespace.EndsWith("Services"))
            //    .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            builder.RegisterType<BookServices>().As<IBookServices>();
            builder.RegisterType<AuthorServices>().As<IAuthorServices>();
            builder.RegisterType<FileServices>().As<IFileServices>();
            builder.RegisterType<GenreServices>().As<IGenreServices>();
            builder.RegisterType<CustomerServices>().As<ICustomerServices>();
            builder.RegisterType<RentalServices>().As<IRentalServices>();


            // Register DbContext
            builder.RegisterType<SchoolLibraryDbContext>().AsSelf();

            // Register UnitOfWork
            builder.RegisterType<UnitOfWork.UnitOfWork>().As<IUnitOfWork>();
        }
    }
}
