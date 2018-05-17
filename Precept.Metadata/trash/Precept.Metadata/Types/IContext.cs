using System;

namespace Precept.Metadata.Types
{
   [Serializable]
   public enum ContextType
   {
      Config = 0,
      Schema = 1,
      Template = 2
   }

   [Serializable]
   public enum ModeType
   {
      Runtime = 0,
      DesignTime = 1,
      DeployTime = 2
   }

   public interface IContext
   {
      //ContextType Type { get; set; }
      ModeType Mode { get; set; }
   }
}