//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
namespace Synuit.Toolkit.Models.Metadata
{
   public enum MetaModelType
   {
      Unknown = -1,
      Explicit = 0,
      Implicit = 1
   }
    public class MetaModel
    {
      public string Tag { get; set; }
      public virtual MetaModelType Type => MetaModelType.Unknown;
    }
}
