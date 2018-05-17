//
//  Synuit.Metadata - Metadata and Configuration Editor
//  Copyright © 2017 Synuit Software. All Rights Reserved.
//
//  This work contains trade secrets and confidential material of
//  Synuit, and its use or disclosure in whole or in part
//  without the express written permission of Synuit is prohibited.
//
using Precept.Architecture.Common;
using Precept.Metadata;
using Precept.Metadata.Editor;
using Precept.Toolkit;
using Synuit.Metadata.Editor;
using System;
using System.Windows.Forms;

namespace Precept.Editor
{
   public partial class SchemaEditorView : Form
   {
      private SchemaEditor _editor = new SchemaEditor();
      private Workspace<Context, Item> _workspace = new Workspace<Context, Item>();

      //
      public SchemaEditorView()
      {
         InitializeComponent();
      }

      //
      private void SchemaEditorView_Load(object sender, EventArgs e)
      {
         try
         {
            LoadConfiguration();
            try
            {
               LoadSchema();
            }
            catch (Exception)
            {
               try
               {
                  NewSchema();
               }
               catch (Exception)
               {
                  throw;
               }
            }
         }
         catch (Exception x)
         {
            MessageBox.Show(x.Message);
            throw;
         }
      }

      //
      private void openToolStripMenuItem_Click(object sender, EventArgs e)
      {
         //OpenFileDialog diag = new OpenFileDialog();

         //if (diag.ShowDialog() == DialogResult.OK )
         //{
         //   _editor.CurrentSchemaFileName = diag.FileName;
         //   SaveEditorProperties();
         //}
      }

      //
      private void SaveEditorProperties()
      {
         Utilities.SaveObject<SchemaEditor>(_editor, Utilities.FormatFilePath(Application.ExecutablePath + ".Schema" + ".xml"));
      }

      //
      private void SaveSchema(string folder, string file, Context context)
      {
         string filepath = Utilities.FormatFilePath(folder + '\\' + file);

         Workspace<Context, Item>.Save(context, filepath);
         //
         _editor.CurrentSchemaFolder = folder;
         _editor.CurrentSchemaFileName = file;
         SaveEditorProperties();
      }

      //
      private void SaveSchemaAs(string folder, string file, string oldfolder, string oldfile)
      {
         NewSchema("Save As ... ENTER NEW CONFIG FILE AND FOLDER NAME!");
      }

      //
      private void SchemaPropertyGrid_Click(object sender, EventArgs e)
      {
      }

      //
      private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (PropertyGridDialog<Context>.EditObject<SchemaEditor>(this.Text, ref _editor))
         {
            //MessageBox.Show(Application.ExecutablePath);
            //if (Utilities.FileExists(Application.ExecutablePath + ".xml"))
            //{
            //}
            SaveEditorProperties();
         }
      }

      // --> 1
      private void LoadConfiguration()
      {
         //MessageBox.Show(Application.ExecutablePath);
         if (Utilities.FileExists(Application.ExecutablePath + ".Schema" + ".xml"))
         {
            _editor = Utilities.LoadObject<SchemaEditor>(Application.ExecutablePath + ".Schema" + ".xml");
         }
         //else
         // {
         //}
      }

      // --> 2
      private void LoadSchema()
      {
         string file = _editor.CurrentSchemaFileName;
         string folder = _editor.CurrentSchemaFolder;
         string filepath = Utilities.FormatFilePath(folder + '\\' + file);

         if (Utilities.FileExists(filepath))
         {
            Context context = Workspace<Context, Item>.Load(filepath);
            if (context == null)
            {
               context = new Context();
               context.Name = (_editor.CurrentSchemaName != "") ? _editor.CurrentSchemaName : _editor.CurrentSchemaName = "ROOT";
               SaveSchema(folder, file, context);
            }
            SyncSchema(context);
         }
         else
         {
            throw new Exception("No schema file specified and/or created, or file has been deleted or moved!");
         }
      }

      // --> 3 (if load fails, or new menu item is selected)
      private void NewSchema(string prompt = "Enter new config folder and filename!")
      {
         string oldfile = _editor.CurrentSchemaFileName;
         string oldfolder = _editor.CurrentSchemaFolder;
         _editor.CurrentSchemaFileName = "";
         _editor.CurrentSchemaFolder = "";

         try
         {
            if (PropertyGridDialog<Context>.EditObject<SchemaEditor>(prompt, ref _editor))
            {
               string newfile = _editor.CurrentSchemaFileName;
               string newfolder = _editor.CurrentSchemaFolder;
               //
               if (newfile != "")
               {
                  var Old = oldfolder + oldfile;
                  var New = newfolder + newfile;

                  if ((Old) != (New))
                  {
                     // --> create new instance, save to file, sync pointers
                     Context context = new Context();
                     context.Name = (_editor.CurrentSchemaName != "") ? _editor.CurrentSchemaName : "Root";
                     SaveSchema(newfolder, newfile, context);
                     SyncSchema(context);
                  }
               }
            }
            else
            {
               throw new Exception();
            }
         }
         catch (Exception)
         {
            _editor.CurrentSchemaFileName = oldfile;
            _editor.CurrentSchemaFolder = oldfolder;
            if (oldfile == "")
               throw new Exception("No valid context is set for schema editor!");
         }
      }

      //
      private void SyncSchema(Context context)
      {
         _workspace.Context = context;
         _editor.Root = _workspace.Context;
         this.SchemaPropertyGrid.SelectedObject = context;
      }

      //
      private void newToolStripMenuItem_Click(object sender, EventArgs e)
      {
         NewSchema();
      }

      //
      private void saveToolStripMenuItem_Click(object sender, EventArgs e)
      {
         SaveSchema(_editor.CurrentSchemaFolder, _editor.CurrentSchemaFileName, _workspace.Context);
      }

      //
      private void testToolStripMenuItem_Click(object sender, EventArgs e)
      {
         _workspace.WriteItem("Write.Item.Test", "Value");
      }

      private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
      {
         SaveSchemaAs(null, null, null, null);
      }

      private void toolsToolStripMenuItem_Click(object sender, EventArgs e)
      {
      }
   }
}