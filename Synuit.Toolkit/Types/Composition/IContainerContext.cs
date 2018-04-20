//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
namespace Synuit.Toolkit.Types.Composition
{
   public interface IContainerContext
   {
      IContainer Container { get; }

      void SetContainer(IContainer container);
   }
}