//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
//using Unity;
//using Unity.Injection;
using Microsoft.Practices.Unity;
using System;

//
namespace Synuit.Toolkit.Infra.Composition.Types
{
   /*
   Dependency injection container that is responsible for resolving
   all of the requested dependencies at run time.
   */

   public abstract class AbstractContainer : IContainer, IDisposable
   {
      private readonly IUnityContainer _unityContainer;
      protected bool _forwardContainer = false;
      //$!!$protected IBootstrap _bootStrap;

      // --> Make the constructor private to ensure that users are always getting the same container
      protected AbstractContainer()
      {
         _unityContainer = new UnityContainer();
         /* tac
          var aggregateCatalog = new AggregateCatalog();

         // Register catalog
         _unityContainer.RegisterCatalog(aggregateCatalog);
         */
         Setup();
      }

      // --> Responsible for configuring the container and prepare it to resolve dependency requests
      abstract protected void Setup();

      // --> Register a type and the class that should be resolved to for instantiation
      public void RegisterType<TFrom, TTo>() where TTo : TFrom { _unityContainer.RegisterType<TFrom, TTo>(); }

      //$!!$
      //////// --> Register a type and the factory that will handle class resolution and instantiation.
      //////public void RegisterType<TFrom>(IObjectFactory of, IContext context) where TFrom : class
      //////{
      //////   _unityContainer.RegisterType<TFrom>(new InjectionFactory(unused_container => of.CreateObject(context)));
      //////}
      //$

      // --> abstract IoC container's resolve
      public T Resolve<T>()
      {
         T obj = _unityContainer.Resolve<T>();
         if (this._forwardContainer)
         {
            if (obj is IContainerContext)
            {
               ((IContainerContext)obj).SetContainer(this);
            }
         }
         return obj; //_unityContainer.Resolve<T>();
      }

      public void Dispose()
      {
         this.Dispose(true);
         GC.SuppressFinalize(this);
      }

      protected virtual void Dispose(bool disposing)
      {
         if (disposing)
         {
            // free managed resources
            if (_unityContainer != null)
            {
               _unityContainer.Dispose();
               //_unityContainer = null;
            }
         }
      }

      public bool ForwardContainerOnCreate
      { get { return _forwardContainer; } set { _forwardContainer = value; } }

      //$!!$
      ////////public void LinkBootstrap(IBootstrap bootStrap)
      ////////{
      ////////   this._bootStrap = bootStrap;
      ////////   this.BootstrapLinked = true;
      ////////}

      ////////public bool BootstrapLinked { get; private set; }
      //$
   }
}