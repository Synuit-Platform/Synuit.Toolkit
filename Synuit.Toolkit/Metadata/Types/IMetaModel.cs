//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
namespace Synuit.Toolkit.Metadata.Types
{
   public enum ModelType
   {
      Unknown = -1,
      WellKnown = 0,
      Implicit = 1
   }

   public interface IMetaModel
   {
      string Tag { get; set; }
      ModelType ModelType { get; }
   }
}