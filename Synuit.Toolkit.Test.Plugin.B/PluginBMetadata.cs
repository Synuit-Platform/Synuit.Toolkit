using Synuit.Toolkit.Models.Metadata;

namespace Synuit.Toolkit.Test.Plugins
{
   public class PluginBMetadata : ExplicitModel
   {
      public string Name { get; set; } = "";
      public int Value { get; set; } = 0;
      public string SpecificToPluginB { get; set; } = "";
   }
}
