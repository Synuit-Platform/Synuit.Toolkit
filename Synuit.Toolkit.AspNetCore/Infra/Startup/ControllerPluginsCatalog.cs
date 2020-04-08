using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Synuit.Toolkit.Infra.Configuration;

namespace Synuit.Toolkit.Infra.Startup
{
   public class ControllerPluginsCatalog : PluginsCatalog
   {
      public ControllerPluginsCatalog(string name) : base(name)
      {
      }

      public void Configure(IMvcBuilder builder)
      {
         // Requires using System.Reflection;
         // Requires using Microsoft.AspNetCore.Mvc.ApplicationParts;
         foreach (var assembly in this.Assemblies)
         {
            // This creates an AssemblyPart, but does not create any related parts for items such as views.
            var part = new AssemblyPart(assembly);
            builder.ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(part));
         }
         this.Configure();
      }
   }
}