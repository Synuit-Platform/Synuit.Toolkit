using Precept.Metadata.Types;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using Wexman.Design;

//
namespace Precept.Metadata
{
   //
   [Serializable]
   public class Contexts : SerializableDictionary<string, Context> { }

   //
   [Serializable]
   public class Context : Node, Types.IContext
   {
      //
      [Editor(typeof(GenericDictionaryEditor<string, Context>), typeof(UITypeEditor))]
      [GenericDictionaryEditor(ValueConverterType = typeof(ExpandableObjectConverter))]
      [Description("Contexts Dictionary.")]
      public Contexts Contexts { get; set; } = new Contexts();

      //
      [Editor(typeof(GenericDictionaryEditor<string, Item>), typeof(UITypeEditor))]
      [GenericDictionaryEditor(ValueConverterType = typeof(ExpandableObjectConverter))]
      [Description("Items Dictionary.")]
      public Items Items { get; set; } = new Items();

      public ModeType Mode { get; set; } = ModeType.Runtime;

      //public ContextType Type { get; set; } =

      ////[Display(Order = -1000)]
      //public new string Name { get; set; } = "";
      ////[Display(Order = -999)]
      //public new string Description { get; set; } = "";

      public Context() : base()
      {
      }

      public Context(string name, string description) : base(name, description)
      {
      }
   }
}