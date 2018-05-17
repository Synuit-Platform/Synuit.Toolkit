//
//  Synuit.Metadata - Metadata and Configuration Editor
//  Copyright © 2017 Synuit Software. All Rights Reserved.
//
//  This work contains trade secrets and confidential material of
//  Synuit, and its use or disclosure in whole or in part
//  without the express written permission of Synuit is prohibited.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Precept.Architecture;

//$!!$using Synuit.Metadata.Editor.Resources;

namespace Synuit.Metadata.Editor
{
	/// <summary>
	/// Show the About form
	/// </summary>
	public partial class AddObjectForm : ResourceForm
	{
      //$!!$
		///////// <summary>
		///////// Current config object
		///////// </summary>
		//////public BossConfig Config { get; set; }

		/// <summary>
		/// Create the form
		/// </summary>
		public AddObjectForm( )
		{
			InitializeComponent( );
		}
      //
      public static T AddObject<T>( Precept.Architecture.Types.DI.IContainer container, IWin32Window owner = null ) 
      {
         T obj =  container.Resolve<T>( );
         var frm = new AddObjectForm( );
         frm.propertyGrid.SelectedObject = obj;
         if ( frm.ShowDialog( owner) == DialogResult.OK )
         {
            return obj;
         }
         else
         {
            return default( T );
         }
      }
      //
      public static T EditObject<T>( T obj, IWin32Window owner = null )
      {
         var frm = new AddObjectForm();
         frm.propertyGrid.SelectedObject = obj;
         if (frm.ShowDialog( owner ) == DialogResult.OK)
         {
            return obj;
         }
         else
         {
            return default(T);
         }
      }
      //
      /// <summary>
      /// Load the about form
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void AboutForm_Load(object sender, EventArgs e)
		{	
		}

		/// <summary>
		/// Click the close button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void closeButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
      //
      private void okButton_Click(object sender, EventArgs e)
      {
         this.DialogResult = DialogResult.OK;
      }
   }
}
