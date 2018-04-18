//
//  Synuit.Toolkit - Application Architecture Tools - Patterns, Types, and Components 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using System;
using Synuit.Toolkit.Types.Platform.Plugins;

namespace Synuit.Toolkit.Platform.Plugins
{
   public abstract class AbstractPluginFactory : IPluginFactory
   {
      public virtual string Name => throw new NotImplementedException();

      public virtual string DisplayName => throw new NotImplementedException();

      public abstract void Configure(string json);

      public abstract IPlugin CreateNewPluginInstance();

      public abstract string GetMetadata();

   }
}
