//
//  Synuit.Metadata - Metadata and Configuration Editor
//  Copyright © 2017 Synuit Software. All Rights Reserved.
//
//  This work contains trade secrets and confidential material of
//  Synuit, and its use or disclosure in whole or in part
//  without the express written permission of Synuit is prohibited.
//
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

//
namespace Precept.Metadata
{
   [Serializable]
   public class SchemaEditor
   {
      public SchemaEditor()
      {
      }

      //
      //[Editor(typeof(GenericDictionaryEditor<string, ActivityDefinition>), typeof(UITypeEditor))]
      //[GenericDictionaryEditor(ValueConverterType=typeof(ExpandableObjectConverter))]
      //[Description("A SerializableDictionary<StringAlignment, ExampleClass> with an ExpandableObjectConverter for the value")]
      //public SerializableDictionary<string, ActivityDefinition> ActivityDefinitions { get; set; }
      ////
      //[Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
      //[Description("Root folder stone images.")]
      //public string ImagesRoot { get; set; }
      ////
      //[Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
      //[Description("Output folder.")]
      //public string OutputFolder { get; set; }
      //
      //public string OutputFileName { get; set; }
      //
      public string CurrentSchemaName { get; set; } = "";

      [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
      [Description("Current Schema folder.")]
      public string CurrentSchemaFolder { get; set; } = "";

      public string CurrentSchemaFileName { get; set; } = "";

      //
      internal Context Root { get; set; } = new Context();
   }
}