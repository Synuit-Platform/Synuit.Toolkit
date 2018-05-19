//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using System;
using System.Linq;
using Synuit.Toolkit.Metadata.Models;
using Synuit.Toolkit.Types.Metadata;
using Synuit.Toolkit.Utilities;
//
namespace Precept.Metadata
{
   public class Workspace //$!!$: IAppEngine
   {
      //$!!$
      ////////protected _paramters
      //////public string AppDirectory { get; set; } = "";
      //////public object Parameters { get; }
      //$
   }

   //
   public class Workspace<C, I> : IWorkspace<C, I> where C : Context where I : Item
   {
      public C Context { get; set; }

      IContext IWorkspace<C, I>.Context
      {
         get { return this.Context; }
         set { Context = ( C ) value; }
      }

      //
      private void StepTreeWrite(C node, string value, int step, ref int top, ref string[] nodes, string description = "")
      {
         string key = nodes[step];

         if ((top - step) != 0)
         {
            node.Contexts[key] = (node.Contexts.Keys.Contains(key)) ? node.Contexts[key] : (C) Activator.CreateInstance(typeof(C), new object[] { key, "" });

            StepTreeWrite((C)node.Contexts[key], value, ++step, ref top, ref nodes, description);
         }
         else
         {
            node.Items[key] = (node.Items.Keys.Contains(key)) ? node.Items[key] : (I) Activator.CreateInstance(typeof(I), new object[] { key, description, value });
         }
      }

      // --> Write an item to the repository (if path (associated contexts and item) does not exist, they are created).
      public void WriteItem(string path, string value, string name = "", string description = "")
      {
         string[] nodes = path.Split('.');
         int top = nodes.Count() - 1;
         //
         StepTreeWrite(Context, value, 0, ref top, ref nodes, description);
      }

      //
      private string StepTreeRead(C node, int step, ref int top, ref string[] nodes, string defaultValue = "")
      {
         string key = nodes[step];
         //
         if ((top - step) != 0)
         {
            return (node.Contexts.Keys.Contains(key)) ? StepTreeRead((C)node.Contexts[key], ++step, ref top, ref nodes, defaultValue) : (defaultValue == "") ? "ERROR" : defaultValue;
         }
         else
         {
            return (node.Items.Keys.Contains(key)) ? node.Items[key].Value : (defaultValue == "") ? "ERROR" : defaultValue;
         }
      }

      // -->
      public string ReadItem(string path, string defaultValue = "")
      {
         string[] nodes = path.Split('.');
         int top = nodes.Count() - 1;
         //
         return StepTreeRead(Context, 0, ref top, ref nodes, defaultValue);
      }

      //
      public static Workspace<C, I> CreateWorkspace(string name, C context)
      {
         return new Workspace<C, I>() { Context = context };
      }

      //
      public static void Save(C context, string filepath) //where C : Context
      {
         Common.SaveObject<C>(context, filepath);
      }

      //
      public static C Load(string filepath)// where C : Context
      {
         return Common.LoadObject<C>(filepath);
      }
   }
}