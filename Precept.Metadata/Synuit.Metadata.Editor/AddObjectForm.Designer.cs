namespace Synuit.Metadata.Editor
{
   partial class AddObjectForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.propertyGridPanel = new MetroFramework.Controls.MetroPanel();
         this.propertyGrid = new System.Windows.Forms.PropertyGrid();
         this.buttonPanel = new MetroFramework.Controls.MetroPanel();
         this.okButton = new MetroFramework.Controls.MetroButton();
         this.closeButton = new MetroFramework.Controls.MetroButton();
         this.propertyGridPanel.SuspendLayout();
         this.buttonPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // propertyGridPanel
         // 
         this.propertyGridPanel.Controls.Add(this.propertyGrid);
         this.propertyGridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.propertyGridPanel.HorizontalScrollbarBarColor = true;
         this.propertyGridPanel.HorizontalScrollbarHighlightOnWheel = false;
         this.propertyGridPanel.HorizontalScrollbarSize = 10;
         this.propertyGridPanel.Location = new System.Drawing.Point(20, 60);
         this.propertyGridPanel.Name = "propertyGridPanel";
         this.propertyGridPanel.Size = new System.Drawing.Size(375, 485);
         this.propertyGridPanel.TabIndex = 10;
         this.propertyGridPanel.VerticalScrollbarBarColor = true;
         this.propertyGridPanel.VerticalScrollbarHighlightOnWheel = false;
         this.propertyGridPanel.VerticalScrollbarSize = 10;
         // 
         // propertyGrid
         // 
         this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
         this.propertyGrid.Location = new System.Drawing.Point(0, 0);
         this.propertyGrid.Name = "propertyGrid";
         this.propertyGrid.Size = new System.Drawing.Size(375, 485);
         this.propertyGrid.TabIndex = 10;
         // 
         // buttonPanel
         // 
         this.buttonPanel.Controls.Add(this.okButton);
         this.buttonPanel.Controls.Add(this.closeButton);
         this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.buttonPanel.HorizontalScrollbarBarColor = true;
         this.buttonPanel.HorizontalScrollbarHighlightOnWheel = false;
         this.buttonPanel.HorizontalScrollbarSize = 10;
         this.buttonPanel.Location = new System.Drawing.Point(20, 486);
         this.buttonPanel.Name = "buttonPanel";
         this.buttonPanel.Size = new System.Drawing.Size(375, 59);
         this.buttonPanel.TabIndex = 11;
         this.buttonPanel.VerticalScrollbarBarColor = true;
         this.buttonPanel.VerticalScrollbarHighlightOnWheel = false;
         this.buttonPanel.VerticalScrollbarSize = 10;
         // 
         // okButton
         // 
         this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.okButton.Location = new System.Drawing.Point(186, 17);
         this.okButton.Name = "okButton";
         this.okButton.Size = new System.Drawing.Size(84, 23);
         this.okButton.TabIndex = 9;
         this.okButton.Text = "&Ok";
         this.okButton.UseSelectable = true;
         this.okButton.Click += new System.EventHandler(this.okButton_Click);
         // 
         // closeButton
         // 
         this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.closeButton.Location = new System.Drawing.Point(276, 17);
         this.closeButton.Name = "closeButton";
         this.closeButton.Size = new System.Drawing.Size(84, 23);
         this.closeButton.TabIndex = 10;
         this.closeButton.Text = "Close";
         this.closeButton.UseSelectable = true;
         this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
         // 
         // AddObjectForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
         this.ClientSize = new System.Drawing.Size(415, 565);
         this.Controls.Add(this.buttonPanel);
         this.Controls.Add(this.propertyGridPanel);
         this.Name = "AddObjectForm";
         this.Resizable = false;
         this.Text = "Add {$Object}";
         this.Load += new System.EventHandler(this.AboutForm_Load);
         this.propertyGridPanel.ResumeLayout(false);
         this.buttonPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }
      //
      #endregion
      private MetroFramework.Controls.MetroPanel propertyGridPanel;
      private System.Windows.Forms.PropertyGrid propertyGrid;
      private MetroFramework.Controls.MetroPanel buttonPanel;
      private MetroFramework.Controls.MetroButton okButton;
      private MetroFramework.Controls.MetroButton closeButton;
   }
}