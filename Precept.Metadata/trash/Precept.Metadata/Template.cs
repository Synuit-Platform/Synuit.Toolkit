using Precept.Toolkit;
using System;
using System.Linq;

namespace Precept.Metadata
{
   [Serializable]
   public class Template : Node { }

   //
   [Serializable]
   public class Template<I, D> : Template where I : Metadata.Item where D : Metadata.SerializableDictionary<string, I>
   {
      public D Items { get; set; }

      // --> Write an item to the repository (if path (associated contexts and item) does not exist, they are created).
      public void WriteItem(string key, string value, string name = "", string description = "")
      {
         Items[key] = (Items.Keys.Contains(key)) ? Items[key] : (I) Activator.CreateInstance(typeof(I), new object[] { key, description, value });
      }

      // -->
      public string ReadItem(string key, string defaultValue = "")
      {
         return (Items.Keys.Contains(key)) ? Items[key].Value : (defaultValue == "") ? "ERROR" : defaultValue;
      }

      //
      public static Template<I, D> CreateTemplate(string name, D items)
      {
         return new Template<I, D>() { Items = items };
      }

      //
      public static void Save(Template<I, D> template, string filepath)
      {
         Utilities.SaveObject<Template<I, D>>(template, filepath);
      }

      //
      public static Template<I, D> Load(string filepath)
      {
         return Utilities.LoadObject<Template<I, D>>(filepath);
      }
   }
}