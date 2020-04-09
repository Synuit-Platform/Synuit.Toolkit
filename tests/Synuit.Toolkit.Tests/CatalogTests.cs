using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Synuit.Toolkit.Infra.Extensibility;
using Synuit.Toolkit.Infra.Extensibility.Models;
using Synuit.Toolkit.Test.Plugins;
using System.IO;
using Xunit;

namespace Synuit.Toolkit.Tests
{
   public class CatalogTests
   {
      private readonly string PathA = "Synuit.Toolkit.Test.Plugin.A.dll";
      private readonly string PathB = "Synuit.Toolkit.Test.Plugin.B.dll";
      private readonly IConfigurationRoot _config;

      public CatalogTests()
      {
         _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
         //_config = config;
         //var envVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

         //Console.WriteLine($"env: {envVariable}");
         //var config = new ConfigurationBuilder()
         //    .AddJsonFile("appsettings.json")
         //     .AddJsonFile($"appsettings.{envVariable}.json", optional: true)
         //    .Build();
         //var conn = config.GetConnectionString("BloggingDatabase");
         //var otherSettings = config["OtherSettings:UserName"];
         //Console.WriteLine(conn);
         //Console.WriteLine(otherSettings);
      }

      [Fact]
      public void ConfigurationFileTest()
      {
         var test = _config["OtherSettings:test"];
         Assert.True(test == "test");
      }

      //
      [Fact]
      public void TestPluginRepoLocation()
      {
         var repo = Path.GetDirectoryName(typeof(CatalogTests).Assembly.Location);
         var pluginA = Path.Combine(repo, PathA);
         var pluginB = Path.Combine(repo, PathB);
         Assert.True(File.Exists(pluginA));
         Assert.True(File.Exists(pluginB));
      }

      //
      [Fact]
      public void TestCatalogComposition()
      {
         var repo = Path.GetDirectoryName(typeof(CatalogTests).Assembly.Location);
         //var pluginA = Path.Combine(repo, PathA);
         //var pluginB = Path.Combine(repo, PathB);
         //Assert.True(File.Exists(pluginA));
         //Assert.True(File.Exists(pluginB));

         var catalog = new PluginCatalog("");
         Assert.True(catalog.Composed == false);
         catalog.Compose(repo, "Synuit.Toolkit.Test.Plugin.*.dll");
         Assert.True(catalog.Instances.Count == 2);
         Assert.True(catalog.Composed == true);
      }

      //
      [Fact]
      public void TestFactories()
      {
         var repo = Path.GetDirectoryName(typeof(CatalogTests).Assembly.Location);

         var catalog = new PluginCatalog("");
         catalog.Compose(repo, "Synuit.Toolkit.Test.Plugin.*.dll");
         //
         var factory = catalog.Instances["A"];
         Assert.True((factory.Name == "A") && (factory.DisplayName == "TEST PLUGIN A") && (factory.GetMetadata() != ""));
         var metadataA = factory.GetMetadata();
         //
         factory = catalog.Instances["B"];
         Assert.True((factory.Name == "B") && (factory.DisplayName == "TEST PLUGIN B") && (factory.GetMetadata() != ""));
         var metadataB = factory.GetMetadata();
         //
         Assert.True(metadataA != metadataB);
      }

      [Fact]
      public void TestBasicPluginConfigurations()
      {
         // --> load and compose the catalog
         var repo = Path.GetDirectoryName(typeof(CatalogTests).Assembly.Location);
         var catalog = new PluginCatalog("");
         catalog.Compose(repo, "Synuit.Toolkit.Test.Plugin.*.dll");

         // --> reference factories and pull their default metadata/templates
         var factoryA = catalog.Instances["A"];
         Assert.True((factoryA.Name == "A") && (factoryA.DisplayName == "TEST PLUGIN A") && (factoryA.GetMetadata() != ""));
         var metadataA = factoryA.GetMetadata();
         //
         var factoryB = catalog.Instances["B"];
         Assert.True((factoryB.Name == "B") && (factoryB.DisplayName == "TEST PLUGIN B") && (factoryB.GetMetadata() != ""));
         var metadataB = factoryB.GetMetadata();
         //
         Assert.True(metadataA != metadataB);
         /////////////////////////////////////////////////////////////////////////////////////////
         // --> create new metamodels from metadata templates and populate plugin configuration //
         /////////////////////////////////////////////////////////////////////////////////////////
         var metamodelA = JsonConvert.DeserializeObject<PluginAMetadata>(metadataA);
         // --> stuff values as if being entered or read out of a db
         metamodelA.Name = "PC-A";
         metamodelA.SpecificToPluginA = "SpecificToPluginA";
         metamodelA.Value = 1;
         metamodelA.Tag = "A";
         // --> reserialize
         metadataA = JsonConvert.SerializeObject(metamodelA, Formatting.Indented);

         var configA = new PluginConfig()
         {
            ID = 1,
            Name = factoryA.Name + "-TEST-1",
            DisplayName = factoryA.DisplayName + "-TEST-1",
            DriverName = factoryA.Name,
            Metadata = metadataA,
            PluginType = Infra.Extensibility.Types.PluginType.Configuration,
            Enabled = true
         };
         //
         var metamodelB = JsonConvert.DeserializeObject<PluginBMetadata>(metadataB);
         // --> stuff values as if being entered or read out of a db
         metamodelB.Name = "PC-B";
         metamodelB.SpecificToPluginB = "SpecificToPluginB";
         metamodelB.Value = 2;
         metamodelB.Tag = "B";
         // --> reserialize
         metadataB = JsonConvert.SerializeObject(metamodelB, Formatting.Indented);

         var configB = new PluginConfig()
         {
            ID = 2,
            Name = factoryB.Name + "-TEST-1",
            DisplayName = factoryB.DisplayName + "-TEST-1",
            DriverName = factoryB.Name,
            Metadata = metadataB,
            PluginType = Infra.Extensibility.Types.PluginType.Configuration,
            Enabled = true
         };
         // --> use factory to create instance of and configure plugin
         var pluginA = factoryA.CreateInstance();
         pluginA.Configure(this, configA);
         Assert.True(metadataA == pluginA.GetMetadata());
         var pluginB = factoryB.CreateInstance();
         pluginB.Configure(this, configB);
         Assert.True(metadataB == pluginB.GetMetadata());
      }
   }
}