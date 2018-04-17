//
//  Synuit.Toolkit - Application Architecture Tools - Patterns, Types and Components 
//  Copyright © 2012-2018 The Dude.  All Rights Reserved.
//
using Synuit.Toolkit.Types.Platform.DI;
//
namespace Synuit.Toolkit.Platform.DI
﻿{
   public class ContainerContext : IContainerContext
   {
      protected IContainer _container;

      public void SetContainer(IContainer container)
      {
         this._container = container;
      }

      public IContainer Container { get { return this._container; } }
   }
}