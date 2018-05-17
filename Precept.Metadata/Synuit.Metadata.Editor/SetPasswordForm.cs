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
	/// Form used to get a password used to protect an Authenticator
	/// </summary>
	public partial class SetPasswordForm : ResourceForm
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SetPasswordForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Current password
		/// </summary>
		public string Password { get; protected set; }

		/// <summary>
		/// Click the Show checkbox to unmask the password fields
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void showCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.showCheckbox.Checked == true)
			{
				this.passwordField.UseSystemPasswordChar = false;
				this.passwordField.PasswordChar = (char)0;
				this.verifyField.UseSystemPasswordChar = false;
				this.verifyField.PasswordChar = (char)0;
			}
			else
			{
				this.passwordField.UseSystemPasswordChar = true;
				this.passwordField.PasswordChar = '*';
				this.verifyField.UseSystemPasswordChar = true;
				this.verifyField.PasswordChar = '*';
			}
		}

		/// <summary>
		/// Click the OK button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
         //$!!$
			////string password = this.passwordField.Text.Trim();
			////string verify = this.verifyField.Text.Trim();
			////if (password != verify)
			////{
			////	//BossForm.ErrorDialog(this, "Your passwords do not match.");
			////	this.errorLabel.Text = strings.PasswordsDontMatch;
			////	this.errorLabel.Visible = true;
			////	this.errorTimer.Enabled = true;
			////	this.DialogResult = System.Windows.Forms.DialogResult.None;
			////	return;
			////}

			////this.Password = password;
		}

		/// <summary>
		/// Timer fired to clear error message
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void errorTimer_Tick(object sender, EventArgs e)
		{
			this.errorTimer.Enabled = false;
			this.errorLabel.Text = string.Empty;
			this.errorLabel.Visible = false;
		}
	}
}
