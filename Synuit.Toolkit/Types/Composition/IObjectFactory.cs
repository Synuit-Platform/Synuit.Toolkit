//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
namespace Synuit.Toolkit.Types.Composition
{
   public interface IObjectFactory
   {
      //$!!$object CreateObject(IContext context);
      object CreateObject<T>(T cls) where T: class;
   }
}