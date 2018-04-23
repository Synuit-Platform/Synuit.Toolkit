using System;
using Synuit.Toolkit.Types.Extensibility;

namespace Synuit.Toolkit.Extensibility
{
   public class ScriptablePlugin : AbstractPlugin
   {
      public override string GetMetadata()
      {
         throw new NotImplementedException();
      }

      protected override void ConfigureFromMetaModel(object host, IPluginConfig config)
      {
         throw new NotImplementedException();
      }
   }
}
