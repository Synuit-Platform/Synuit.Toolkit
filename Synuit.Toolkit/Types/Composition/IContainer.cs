//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
namespace Synuit.Toolkit.Types.Composition
{
   /*
   Dependency injection container Interface that is responsible for resolving
   all of the requested dependencies at run time.
   */

   public interface IContainer
   {
      bool ForwardContainerOnCreate { get; set; }
      
      //$!!$
      //bool BootstrapLinked { get; }
      //void LinkBootstrap(IBootstrap bootStrap);
      //$

      // --> Register a type and the class that should be resolved to for instantiation
      void RegisterType<TFrom, TTo>() where TTo : TFrom;

      // --> Register a type and the factory that will handle class resolution and instantiation.
      //$!!$ void RegisterType<TFrom>(IObjectFactory of, IContext context) where TFrom : class;

      // --> abstract IoC container's resolve
      T Resolve<T>();
   }
}
