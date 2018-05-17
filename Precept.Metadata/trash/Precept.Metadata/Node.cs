using System;

namespace Precept.Metadata
{
   [Serializable]
   public class Node
   {
      public int Id { get; protected set; } = -999999999;
      public string Name { get; set; } = "";
      public string Description { get; set; } = "";

      //
      public Node() { }

      public Node(string name, string description) : base()
      {
         Name = name; Description = description;
      }

      //
   }
}