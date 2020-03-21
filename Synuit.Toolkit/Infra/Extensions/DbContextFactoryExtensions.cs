using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Synuit.Toolkit.Infra.Composition;
using Synuit.Toolkit.Infra.Composition.Types;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
   /// <summary>
   /// Extensions to add AddDbContextFactory
   /// </summary>
   public static class DbContextFactoryExtensions
   {
      /// <summary>
      /// Configures the resolution of <typeparamref name="TDataContext"/>'s factory.
      /// </summary>
      /// <typeparam name="TDataContext">The DbContext.</typeparam>
      /// <param name="services"></param>
      /// <param name="nameOrConnectionString">Name or connection string of the context. (Optional)</param>
      /// <param name="logger">The <see cref="ILoggerFactory"/>implementation.</param>
      public static void AddSqlServerDbContextFactory<TDataContext>(this IServiceCollection services, string nameOrConnectionString = null, ILoggerFactory logger = null, ServiceLifetime lifetime = ServiceLifetime.Singleton)
          where TDataContext : DbContext
      {
         if (string.IsNullOrEmpty(nameOrConnectionString))
         {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            nameOrConnectionString = configuration.GetConnectionString("DefaultConnection");
         }

         AddDbContextFactory<TDataContext>(services, (provider, builder) =>
             builder.UseSqlServer(nameOrConnectionString)
                 .UseLoggerFactory(logger), lifetime
         );
      }

      /// <summary>
      /// Configures the resolution of <typeparamref name="TDataContext"/>'s factory.
      /// </summary>
      /// <typeparam name="TDataContext">The DbContext.</typeparam>
      /// <param name="services"></param>
      /// <param name="options">The DbContext options.</param>
      public static void AddDbContextFactory<TDataContext>
         (
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> options,
            ServiceLifetime lifetime = ServiceLifetime.Singleton,
            IServiceProvider provider = null
         )
         where TDataContext : DbContext
         => AddDbContextFactory<TDataContext>(services, (provider, builder) => options.Invoke(builder), lifetime, provider);

      /// <summary>
      /// Configures the resolution of <typeparamref name="TDataContext"/>'s factory.
      /// </summary>
      /// <typeparam name="TDataContext">The DbContext.</typeparam>
      /// <param name="services"></param>
      /// <param name="optionsAction">Service provider and DbContext options.</param>
      public static IServiceCollection AddDbContextFactory<TDataContext>
         (
            this IServiceCollection services,
            Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
            ServiceLifetime lifetime = ServiceLifetime.Singleton,
            IServiceProvider provider = null
         )
         where TDataContext : DbContext
      {
         AddCoreServices<TDataContext>(services, optionsAction, lifetime);
         //
         var serviceProvider = services.BuildServiceProvider() ;
         var options = serviceProvider.GetService<DbContextOptions<TDataContext>>();
         var parms = (provider == null) ? new object[] { options } : new object[] { options, serviceProvider };
         //
         Func<TDataContext> func(IServiceProvider ctx) => () => (TDataContext)Activator.CreateInstance(typeof(TDataContext), parms);
         // services.AddSingleton<Func<TDataContext>>(ctx => () => (TDataContext)Activator.CreateInstance(typeof(TDataContext), new object[] { options, serviceProvider }));

         return (lifetime == ServiceLifetime.Singleton) ? services.AddFactoryClassForSingleton(func)
            : (lifetime == ServiceLifetime.Scoped) ? services.AddScoped(func)
               : services.AddTransient(func);
      }

      private static IServiceCollection AddFactoryClassForSingleton<TDataContext>(
         this IServiceCollection services,
         Func<IServiceProvider, Func<TDataContext>> func)
      where TDataContext : DbContext
      {
         services.AddSingleton(func);
         return services.AddSingleton<IFactory<TDataContext>, Factory<TDataContext>>();
      }

      private static void AddCoreServices<TContextImplementation>(
          IServiceCollection serviceCollection,
          Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
          ServiceLifetime optionsLifetime)
          where TContextImplementation : DbContext
      {
         serviceCollection
             .AddMemoryCache()
             .AddLogging();

         serviceCollection.TryAdd(
             new ServiceDescriptor(
                 typeof(DbContextOptions<TContextImplementation>),
                 p => DbContextOptionsFactory<TContextImplementation>(p, optionsAction),
                 optionsLifetime));

         serviceCollection.Add(
             new ServiceDescriptor(
                 typeof(DbContextOptions),
                 p => p.GetRequiredService<DbContextOptions<TContextImplementation>>(),
                 optionsLifetime));
      }

      private static DbContextOptions<TContext> DbContextOptionsFactory<TContext>(
          IServiceProvider applicationServiceProvider,
          Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
          where TContext : DbContext
      {
         var builder = new DbContextOptionsBuilder<TContext>(
             new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>()));

         builder.UseApplicationServiceProvider(applicationServiceProvider);
         optionsAction?.Invoke(applicationServiceProvider, builder);

         return builder.Options;
      }
   }
}