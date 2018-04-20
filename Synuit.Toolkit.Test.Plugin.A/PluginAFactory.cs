using System.Composition;
using Synuit.Toolkit.Extensibility;
using Synuit.Toolkit.Types.Extensibility;

namespace Synuit.Toolkit.Test.Plugins
{
   [Export(typeof(IPluginFactory))]
   public class PluginAFactory : AbstractPluginFactory
   {
      public override string Name => "A";
      //
      public override string DisplayName => "TEST PLUGIN A";

      public override IPlugin CreateInstance()
      {
         return this.CreateInstance<PluginA>();
      }
      public override string GetMetadata()
      {
         return this.GetMetadata<PluginAMetadata>();
      }
   }
}
