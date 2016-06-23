using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OutlineWF.Models;
using OutlineWF.Views.UserControls;

namespace OutlineWF.Views
{
    public partial class DetailEditForm : Form
    {
        #region Fields
        private Detail _detail;

        private IDetailControl _editDetail;
        private IWireframeControl _wireframe1;
        #endregion

        #region MainFrom
        public DetailEditForm()
        {
            InitializeComponent();
            _editDetail = editPartPage.UserControl;
            _wireframe1 = wireframePage1.UserControl;

            _wireframe1.Image = (Bitmap)Image.FromFile(Application.StartupPath + @"\q1.bmp");

            _detail = new Detail();
            _wireframe1.Detail = _detail;
            _wireframe1.CurrentPartIndex = 0;

            _editDetail.Detail = _detail;
        }
        #endregion

        #region FormEvents
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "(*.dxf, *.dgt)|*.dxf;*.dgt",
                Title = "Save model as",
                FileName = "*.dgt"
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                var fs = dialog.OpenFile();
                var streamWriter = new StreamWriter(fs);

                if (dialog.FileName.Split('.')[1].Equals("dgt"))
                {
                    streamWriter.Write(_detail.ToDGT());
                }
                else if (dialog.FileName.Split('.')[1].Equals("dxf"))
                {
                    streamWriter.Write(_detail.ToDXF());
                }

                streamWriter.Close();
                fs.Close();
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "(*.dxf, *.dgt)|*.dxf;*.dgt"
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    using (var sr = new StreamReader(dialog.OpenFile()))
                    {
                        _detail = null;
                        if (dialog.FileName.Split('.')[1].Equals("dgt"))
                        {
                            _detail = Detail.ReadDGT(sr);
                        }
                        else if (dialog.FileName.Split('.')[1].Equals("dxf"))
                        {
                            _detail = Detail.ReadDXF(sr);
                        }
                        _wireframe1.Detail = _detail;
                        _wireframe1.CurrentPartIndex = 0;

                        _editDetail.Detail = _detail;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
