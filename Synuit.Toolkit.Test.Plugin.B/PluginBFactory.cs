using Synuit.Toolkit.Infra.Extensibility.Types;
using System.Composition;
//


namespace Synuit.Toolkit.Test.Plugins
{
   [Export(typeof(IPluginFactory))]
   public class PluginBFactory : AbstractPluginFactory
   {
      public override string Name => "B";
      //
      public override string DisplayName => "TEST PLUGIN B";

      public override IPlugin CreateInstance()
      {
         return this.CreateInstance<PluginB>();
      }
      //
      public override string GetMetadata()
      {
         return this.GetMetadata<PluginBMetadata>();
      }
   }
}
