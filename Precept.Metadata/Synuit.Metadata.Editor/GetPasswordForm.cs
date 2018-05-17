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
using System.Linq;
using System.Text;
using System.Windows.Forms;

//$!!$ using Synuit.Metadata.Editor.Resources;

namespace Synuit.Metadata.Editor
{
	/// <summary>
	/// Class for form that prompts for password and unprotects Authenticator
	/// </summary>
	public partial class GetPasswordForm : ResourceForm
	{
		/// <summary>
		/// Create new form
		/// </summary>
		public GetPasswordForm()
			: base()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Password
		/// </summary>
		public string Password { get; private set; }

		public bool InvalidPassword { get; set; }

		/// <summary>
		/// Load the form and make it topmost
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GetPasswordForm_Load(object sender, EventArgs e)
		{
			// force this window to the front and topmost
			// see: http://stackoverflow.com/questions/278237/keep-window-on-top-and-steal-focus-in-winforms
			var oldtopmost = this.TopMost;
			this.TopMost = true;
			this.TopMost = oldtopmost;
			this.Activate();

			if (InvalidPassword == true)
			{
				//$!!$invalidPasswordLabel.Text = strings.InvalidPassword;
				invalidPasswordLabel.Visible = true;
				invalidPasswordTimer.Enabled = true;
			}
		}

		/// <summary>
		/// Click the OK button to unprotect the Authenticator with given password
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			// it isn't empty
			string password = this.passwordField.Text;
			if (password.Length == 0)
			{
				//$!!$ invalidPasswordLabel.Text = strings.EnterPassword;
				invalidPasswordLabel.Visible = true;
				invalidPasswordTimer.Enabled = true;
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}

			this.Password = password;
		}

		/// <summary>
		/// Display error message for couple seconds
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void invalidPasswordTimer_Tick(object sender, EventArgs e)
		{
			invalidPasswordTimer.Enabled = false;
			invalidPasswordLabel.Visible = false;
		}

	}
}
