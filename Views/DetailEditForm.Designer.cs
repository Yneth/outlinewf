namespace OutlineWF.Views
{
    partial class DetailEditForm
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
            this.components = new System.ComponentModel.Container();
            this.partImages = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openBackgroundImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editPartPage = new OutlineWF.Views.UserTabControl<OutlineWF.Views.DetailEdit>();
            this.wireframePage1 = new OutlineWF.Views.UserTabControl<OutlineWF.Views.Wireframe>();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // partImages
            // 
            this.partImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.partImages.ImageSize = new System.Drawing.Size(64, 64);
            this.partImages.TransparentColor = System.Drawing.Color.Black;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(846, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.openBackgroundImageToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.saveAsToolStripMenuItem.Text = "Save as";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // openBackgroundImageToolStripMenuItem
            // 
            this.openBackgroundImageToolStripMenuItem.Name = "openBackgroundImageToolStripMenuItem";
            this.openBackgroundImageToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.openBackgroundImageToolStripMenuItem.Text = "OpenBackgroundImage";
            // 
            // editPartPage
            // 
            this.editPartPage.BackColor = System.Drawing.Color.Transparent;
            this.editPartPage.Location = new System.Drawing.Point(4, 22);
            this.editPartPage.Name = "editPartPage";
            this.editPartPage.Padding = new System.Windows.Forms.Padding(3);
            this.editPartPage.Size = new System.Drawing.Size(840, 510);
            this.editPartPage.TabIndex = 1;
            this.editPartPage.Text = "Edit Part";
            this.editPartPage.UseVisualStyleBackColor = true;
            // 
            // wireframePage1
            // 
            this.wireframePage1.BackColor = System.Drawing.Color.Transparent;
            this.wireframePage1.Location = new System.Drawing.Point(4, 22);
            this.wireframePage1.Name = "wireframePage1";
            this.wireframePage1.Padding = new System.Windows.Forms.Padding(3);
            this.wireframePage1.Size = new System.Drawing.Size(840, 510);
            this.wireframePage1.TabIndex = 0;
            this.wireframePage1.Text = "Wireframe 1";
            this.wireframePage1.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.wireframePage1);
            this.tabControl1.Controls.Add(this.editPartPage);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(848, 550);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // DetailEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 560);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DetailEditForm";
            this.Text = "DetailEditForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList partImages;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openBackgroundImageToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private OutlineWF.Views.UserTabControl<OutlineWF.Views.DetailEdit> editPartPage;
        private OutlineWF.Views.UserTabControl<OutlineWF.Views.Wireframe> wireframePage1;
    }
}

