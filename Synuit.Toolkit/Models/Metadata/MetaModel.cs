//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
namespace Synuit.Toolkit.Models.Metadata
{
   public abstract class MetaModel : IMetaModel
   {
      public string Tag { get; set; }
      public virtual ModelType ModelType => ModelType.Unknown;
   }
}