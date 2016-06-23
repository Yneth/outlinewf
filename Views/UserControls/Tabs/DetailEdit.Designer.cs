namespace OutlineWF.Views
{
    partial class DetailEdit
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        ///         private System.Windows.Forms.Button smoothButton;
        private void InitializeComponent()
        {
            this.conjugationButton = new System.Windows.Forms.Button();
            this.iSplineSmoothButton = new System.Windows.Forms.Button();
            this.smoothButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.moveButton = new System.Windows.Forms.Button();
            this.compressButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.partListView1 = new OutlineWF.Views.UserControls.PartListView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // conjugationButton
            // 
            this.conjugationButton.Location = new System.Drawing.Point(657, 255);
            this.conjugationButton.Name = "conjugationButton";
            this.conjugationButton.Size = new System.Drawing.Size(175, 36);
            this.conjugationButton.TabIndex = 9;
            this.conjugationButton.Text = "Conjugate";
            this.conjugationButton.UseVisualStyleBackColor = true;
            this.conjugationButton.Click += new System.EventHandler(this.conjugationButton_Click);
            // 
            // iSplineSmoothButton
            // 
            this.iSplineSmoothButton.Location = new System.Drawing.Point(657, 210);
            this.iSplineSmoothButton.Name = "iSplineSmoothButton";
            this.iSplineSmoothButton.Size = new System.Drawing.Size(175, 38);
            this.iSplineSmoothButton.TabIndex = 8;
            this.iSplineSmoothButton.Text = "ISpline";
            this.iSplineSmoothButton.UseVisualStyleBackColor = true;
            this.iSplineSmoothButton.Click += new System.EventHandler(this.iSplineSmoothButton_Click);
            // 
            // smoothButton
            // 
            this.smoothButton.Location = new System.Drawing.Point(658, 166);
            this.smoothButton.Name = "smoothButton";
            this.smoothButton.Size = new System.Drawing.Size(175, 38);
            this.smoothButton.TabIndex = 7;
            this.smoothButton.Text = "BSpline";
            this.smoothButton.UseVisualStyleBackColor = true;
            this.smoothButton.Click += new System.EventHandler(this.smoothButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(658, 124);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(175, 35);
            this.addButton.TabIndex = 5;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(658, 83);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(175, 34);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // moveButton
            // 
            this.moveButton.Location = new System.Drawing.Point(658, 43);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(175, 34);
            this.moveButton.TabIndex = 3;
            this.moveButton.Text = "Move";
            this.moveButton.UseVisualStyleBackColor = true;
            this.moveButton.Click += new System.EventHandler(this.moveButton_Click);
            // 
            // compressButton
            // 
            this.compressButton.Location = new System.Drawing.Point(658, 6);
            this.compressButton.Name = "compressButton";
            this.compressButton.Size = new System.Drawing.Size(176, 31);
            this.compressButton.TabIndex = 1;
            this.compressButton.Text = "Compress";
            this.compressButton.UseVisualStyleBackColor = true;
            this.compressButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(646, 498);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // partListView1
            // 
            this.partListView1.Detail = null;
            this.partListView1.DetailColor = System.Drawing.Color.Red;
            this.partListView1.Location = new System.Drawing.Point(658, 297);
            this.partListView1.Name = "partListView1";
            this.partListView1.Size = new System.Drawing.Size(174, 207);
            this.partListView1.TabIndex = 10;
            this.partListView1.ItemSelectionChangedEvent += new OutlineWF.Views.UserControls.ItemSelectionChangedHandler(this.partListView_ItemSelectionChanged);
            // 
            // DetailEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.partListView1);
            this.Controls.Add(this.conjugationButton);
            this.Controls.Add(this.iSplineSmoothButton);
            this.Controls.Add(this.smoothButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.moveButton);
            this.Controls.Add(this.compressButton);
            this.Controls.Add(this.pictureBox1);
            this.Location = new System.Drawing.Point(4, 22);
            this.Name = "DetailEdit";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(840, 510);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Button iSplineSmoothButton;
        private System.Windows.Forms.Button conjugationButton;
        private System.Windows.Forms.Button compressButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button moveButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button smoothButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        #endregion

        private UserControls.PartListView partListView1;
    }
}
