//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using Synuit.Toolkit.Infra.Composition.Types;

//
namespace Synuit.Toolkit.Infra.Composition
{
   public class ContainerContext : IContainerContext
   {
      protected IContainer _container;

      public void SetContainer(IContainer container)
      {
         this._container = container;
      }

      //
      public IContainer Container { get { return this._container; } }
   }
}