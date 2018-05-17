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

//$!!$using Synuit.Metadata.Editor.Resources;

namespace Synuit.Metadata.Editor
{
	/// <summary>
	/// Show the About form
	/// </summary>
	public partial class AboutForm : ResourceForm
	{
      //$!!$
		///////// <summary>
		///////// Current config object
		///////// </summary>
		//////public BossConfig Config { get; set; }

		/// <summary>
		/// Create the form
		/// </summary>
		public AboutForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Load the about form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AboutForm_Load(object sender, EventArgs e)
		{
			// get the version of the application
			Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			string debug = string.Empty;

#if BETA
			debug += " (BETA)";
#endif
#if DEBUG
			debug += " (DEBUG)";
#endif
			this.aboutLabel.Text = string.Format(this.aboutLabel.Text, version.ToString(3) + debug, DateTime.Today.Year);
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

		/// <summary>
		/// Click the report button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void reportButton_Click(object sender, EventArgs e)
		{
         //$!!$
			////////// display the error form, loading it with current Authenticator data
			////////DiagnosticForm errorreport = new DiagnosticForm();
			////////errorreport.Config = Config;
			////////if (string.IsNullOrEmpty(errorreport.Config.Filename) == false)
			////////{
			////////	errorreport.ConfigFileContents = File.ReadAllText(errorreport.Config.Filename);
			////////}
			////////else
			////////{
			////////	using (MemoryStream ms = new MemoryStream())
			////////	{
			////////		XmlWriterSettings settings = new XmlWriterSettings();
			////////		settings.Indent = true;
			////////		using (XmlWriter writer = XmlWriter.Create(ms, settings))
			////////		{
			////////			Config.WriteXmlString(writer);
			////////		}
			////////		ms.Position = 0;
			////////		errorreport.ConfigFileContents = new StreamReader(ms).ReadToEnd();
			////////	}
			////////}
			////////errorreport.ShowDialog(this);
		}
	}
}
