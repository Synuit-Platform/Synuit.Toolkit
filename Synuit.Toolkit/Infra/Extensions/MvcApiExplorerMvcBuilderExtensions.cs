using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection

{
   public static class MvcApiExplorerMvcBuilderExtensions
   {
      public static IMvcBuilder AddApiExplorer(this IMvcBuilder builder)

      {
         if (builder == null)

         {
            throw new ArgumentNullException(nameof(builder));
         }

         AddApiExplorerServices(builder.Services);

         return builder;
      }

      // Internal for testing.

      internal static void AddApiExplorerServices(IServiceCollection services)

      {
         services.TryAddSingleton<IApiDescriptionGroupCollectionProvider, ApiDescriptionGroupCollectionProvider>();

         services.TryAddEnumerable(

             ServiceDescriptor.Transient<IApiDescriptionProvider, DefaultApiDescriptionProvider>());
      }
   }
}