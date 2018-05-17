//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using System;

namespace Synuit.Toolkit.Metadata.Models
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