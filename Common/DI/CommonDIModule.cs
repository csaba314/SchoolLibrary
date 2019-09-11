using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Common.Parameters;

namespace Common.DI
{
    public class CommonDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register all parameters
            builder.RegisterAssemblyTypes(typeof(Options).Assembly)
                .Where(t => t.Namespace.EndsWith("Parameters"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));
        }
    }
}
