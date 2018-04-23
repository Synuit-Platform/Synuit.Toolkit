using System;
using Synuit.Toolkit.Types.Extensibility;

namespace Synuit.Toolkit.Extensibility
{
   public class Plugin : AbstractPlugin
   {
      public override string GetMetadata()
      {
         throw new NotImplementedException();
      }

      protected override void ConfigureFromAssembly(object host, IPluginConfig config)
      {
         throw new NotImplementedException();
      }
   }
}
