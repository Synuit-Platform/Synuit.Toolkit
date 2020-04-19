//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using Synuit.Toolkit.Metadata.Types;

namespace Synuit.Toolkit.Metadata.Models
{
   public abstract class MetaModel : IMetaModel
   {
      public string Tag { get; set; }
      public virtual ModelType ModelType => ModelType.Unknown;
   }
}