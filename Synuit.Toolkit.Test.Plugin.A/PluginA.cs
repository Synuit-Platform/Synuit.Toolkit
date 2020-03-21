
using Synuit.Toolkit.Infra.Extensibility.Types;


namespace Synuit.Toolkit.Test.Plugins
{
   public class PluginA : AbstractPlugin
   {
      private PluginAMetadata _metadata = null;
      private IPluginConfig _config = null;
      protected override void ConfigureFromMetaModel(object host, IPluginConfig config)
      {
         _config = config;
         _metadata = this.Deserialize<PluginAMetadata>(_config.Metadata);
      }

      public override string GetMetadata()
      {
         return this.Serialize<PluginAMetadata>(_metadata);
      }
   }
}
