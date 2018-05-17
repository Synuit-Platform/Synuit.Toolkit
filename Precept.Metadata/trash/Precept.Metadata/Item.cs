using System;

namespace Precept.Metadata
{
   [Serializable]
   public class Items : SerializableDictionary<string, Item> { }

   [Serializable]
   public class Item : Node
   {
      public string Value { get; set; } = "";

      //
      public Item() : base() { }

      public Item(string name, string description, string value) : base(name, description)
      {
         Value = value;
      }
   }

   //
   [Serializable]
   public class Item<T> : Item where T : class
   {
      public new T Value { get; set; }
      public string AsString { get { return Value.ToString(); } }
   }
}