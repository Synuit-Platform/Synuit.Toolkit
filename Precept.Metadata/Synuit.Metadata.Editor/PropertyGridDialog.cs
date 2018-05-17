//
//  Synuit.Metadata - Metadata and Configuration Editor
//  Copyright © 2017 Synuit Software. All Rights Reserved.
//
//  This work contains trade secrets and confidential material of
//  Synuit, and its use or disclosure in whole or in part
//  without the express written permission of Synuit is prohibited.
//
using Precept.Toolkit;
using System;
using System.Windows.Forms;

namespace Precept.Metadata.Editor
{
   public partial class PropertyGridDialog<T> : Form where T : class
   {
      public PropertyGridDialog()
      {
         InitializeComponent();
      }

      internal DialogResult ShowDialog(ref T obj)
      {
         this.ObjectPropertyGrid.SelectedObject = Utilities.Deserialize<T>(Utilities.Serialize<T>(obj));
         var res = this.ShowDialog();
         if (res == DialogResult.OK)
         {
            obj = Utilities.Deserialize<T>(Utilities.Serialize<T>((T)this.ObjectPropertyGrid.SelectedObject));
         }

         return res;
      }

      //
      private void oKToolStripMenuItem_Click(object sender, EventArgs e)
      {
         this.DialogResult = DialogResult.OK;
         this.Close();
      }

      public static bool EditObject<P>(string title, ref P obj) where P : class
      {
         PropertyGridDialog<P> diag = new PropertyGridDialog<P>();
         diag.Text = diag.Text.Replace("$", title);
         return (diag.ShowDialog(ref obj) == DialogResult.OK);
      }

      private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
      {
         this.DialogResult = DialogResult.Cancel;
         this.Close();
      }
   }
}