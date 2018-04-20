using Synuit.Toolkit.Extensibility;
using Synuit.Toolkit.Types.Extensibility;

namespace Synuit.Toolkit.Test.Plugins
{
   public class PluginB : AbstractPlugin
   {
      private PluginBMetadata _metadata = null;
      private IPluginConfig _config = null;

      public override string GetMetadata()
      {
         return this.Serialize<PluginBMetadata>(_metadata);
      }

      protected override void ConfigureFromMetaModel(object host, IPluginConfig config)
      {
         _config = config;
         _metadata = this.Deserialize<PluginBMetadata>(_config.Metadata);
      }
   }
}
