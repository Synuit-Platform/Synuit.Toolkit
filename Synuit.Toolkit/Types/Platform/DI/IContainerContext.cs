//
//  Synuit.Toolkit - Application Architecture Tools - Patterns, Types and Components 
//  Copyright © 2012-2018 The Dude.  All Rights Reserved.
//
namespace Synuit.Toolkit.Types.Platform.DI
{
   public interface IContainerContext
   {
      IContainer Container { get; }

      void SetContainer(IContainer container);
   }
}