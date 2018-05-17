namespace Precept.Metadata.Editor
{
   partial class PropertyGridDialog<T> where T : class
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyGridDialog<T>));
         this.menuStrip1 = new System.Windows.Forms.MenuStrip();
         this.oKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.ObjectPropertyGrid = new System.Windows.Forms.PropertyGrid();
         this.menuStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // menuStrip1
         // 
         this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oKToolStripMenuItem,
            this.cancelToolStripMenuItem});
         this.menuStrip1.Location = new System.Drawing.Point(0, 0);
         this.menuStrip1.Name = "menuStrip1";
         this.menuStrip1.Size = new System.Drawing.Size(626, 24);
         this.menuStrip1.TabIndex = 0;
         this.menuStrip1.Text = "menuStrip1";
         // 
         // oKToolStripMenuItem
         // 
         this.oKToolStripMenuItem.Name = "oKToolStripMenuItem";
         this.oKToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
         this.oKToolStripMenuItem.Text = "OK";
         this.oKToolStripMenuItem.Click += new System.EventHandler(this.oKToolStripMenuItem_Click);
         // 
         // cancelToolStripMenuItem
         // 
         this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
         this.cancelToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
         this.cancelToolStripMenuItem.Text = "CANCEL";
         this.cancelToolStripMenuItem.Click += new System.EventHandler(this.cancelToolStripMenuItem_Click);
         // 
         // ObjectPropertyGrid
         // 
         this.ObjectPropertyGrid.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
         this.ObjectPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
         this.ObjectPropertyGrid.Location = new System.Drawing.Point(0, 24);
         this.ObjectPropertyGrid.Name = "ObjectPropertyGrid";
         this.ObjectPropertyGrid.Size = new System.Drawing.Size(626, 323);
         this.ObjectPropertyGrid.TabIndex = 2;
         // 
         // PropertyGridDialog
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(626, 347);
         this.Controls.Add(this.ObjectPropertyGrid);
         this.Controls.Add(this.menuStrip1);
         this.MainMenuStrip = this.menuStrip1;
         this.Name = "PropertyGridDialog";
         this.Text = "Properties [$]";
         this.menuStrip1.ResumeLayout(false);
         this.menuStrip1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.MenuStrip menuStrip1;
      private System.Windows.Forms.ToolStripMenuItem oKToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
      private System.Windows.Forms.PropertyGrid ObjectPropertyGrid;
   }
}