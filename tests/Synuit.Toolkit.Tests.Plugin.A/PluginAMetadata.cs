
using Synuit.Toolkit.Models.Metadata;
//
namespace Synuit.Toolkit.Test.Plugins
{
   public class PluginAMetadata : ExplicitModel
   {
      public string Name { get; set; } = "";
      public int Value { get; set; } = 0;
      public string SpecificToPluginA { get; set; } = "";
   }
}
