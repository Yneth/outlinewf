using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OutlineWF.Models;
using Point = OutlineWF.Utilities.Point;

namespace OutlineWF.Views
{
    public partial class EquidistantForm : Form
    {
        #region PrivateFiedls
        private Part _currentPart;

        private readonly Bitmap _partImage;

        private Point _point0;
        private Point _point1;

        private bool _loop = false;
        private bool _isDrawn = true;

        private int _equidistantLength;
        private List<Point> _equidistant;
        #endregion

        #region Properties
        public Detail Detail { get; set; }
        public PartDrawContext Drawing { get; set; }
        public Color Color { get; set; }
        #endregion

        #region Form
        public EquidistantForm()
        {
            InitializeComponent();
            _partImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Drawing = new PartDrawContext(Color.Red, pictureBox1.Width, pictureBox1.Height);

            _equidistantLength = ReadEquidistantLength();
            string path = Application.StartupPath + "\\testVupk.dgt";
            OpenFile(path);
        }
        #endregion

        #region PrivateMethods
        private int ReadEquidistantLength()
        {
            return Convert.ToInt32(textBox1.Text);
        }

        private void OpenFile(string path, StreamReader sr = null)
        {
            Detail = null;
            if (path.Split('.')[1].Equals("dgt"))
            {
                if (sr == null) { Detail = Detail.ReadDGT(File.OpenText(path)); }
                else { Detail = Detail.ReadDGT(sr); }

                _currentPart = Detail.Parts.FirstOrDefault();
                partListView1.Detail = Detail;
                Draw();
            }
        }
        #endregion

        #region FormEvents
        private void opendgtToolStripMenuItem_Click(object sender, System.EventArgs e)
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
                        OpenFile(dialog.FileName, sr);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void savedgtToolStripMenuItem_Click(object sender, System.EventArgs e)
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
                    streamWriter.Write(Detail.ToDGT());
                }

                streamWriter.Close();
                fs.Close();
            }
        }

        private void partListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            _point0 = null;
            _point1 = null;
            _currentPart = partListView1.CurrentPart;
            DrawEditor();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                _point0 = new Point(e.X, e.Y * -1.0f + pictureBox1.Height);
            }
            else if (e.Button.Equals(MouseButtons.Right))
            {
                _point1 = new Point(e.X, e.Y * -1.0f + pictureBox1.Height);
            }
            Draw();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_point0 == null || _point1 == null) return;
            _loop = radioButton1.Checked;
            _isDrawn = !_isDrawn;
            _equidistantLength = ReadEquidistantLength();
            Draw();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _loop = true;
            if (_point0 == null || _point1 == null) return;
            _equidistantLength = ReadEquidistantLength();
            Draw();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _loop = false;
            if (_point0 == null || _point1 == null) return;
            _equidistantLength = ReadEquidistantLength();
            Draw();
        }
        #endregion

        #region DrawMethods
        private void Draw()
        {
            if (Detail == null) return;

            DrawEditor();
        }

        private void DrawEditor()
        {
            if (Detail == null) return;

            pictureBox1.Refresh();
            using (var graphics = Graphics.FromImage(_partImage))
            {
                graphics.Clear(Color.White);
                if (_isDrawn)
                {
                    _currentPart.Draw(graphics, Drawing);
                    DrawPath(graphics);
                }
                else
                {
                    DrawPath(graphics);
                }
            }
            pictureBox1.Image = _partImage;
        }

        private void DrawPath(Graphics graphics)
        {
            int w = Drawing.Width;
            int h = Drawing.Height;

            Point localPoint0 = null;
            Point drawPoint0 = null;
            if (_point0 != null)
            {
                _currentPart.AddPoint(_point0, w, h);
                localPoint0 = _currentPart.ClosestLocalPointTo(_point0, w, h);
                drawPoint0 = _currentPart.ToScreenCoordinates(localPoint0, w, h);
                _currentPart.DrawPoint(graphics, Color.Blue, drawPoint0);
            }
            Point localPoint1 = null;
            Point drawPoint1 = null;
            if (_point1 != null)
            {
                _currentPart.AddPoint(_point1, w, h);
                localPoint1 = _currentPart.ClosestLocalPointTo(_point1, w, h);
                drawPoint1 = _currentPart.ToScreenCoordinates(localPoint1, w, h);
                _currentPart.DrawPoint(graphics, Color.Blue, drawPoint1);
            }

            if (_point0 == null || _point1 == null) return;
            var index0 = _currentPart.IndexOfClosestPoint(localPoint0);
            var index1 = _currentPart.IndexOfClosestPoint(localPoint1);
            if (index0 > index1)
            {
                var tempI = index0;
                index0 = index1;
                index1 = tempI;

                var tempP = localPoint0;
                localPoint0 = localPoint1;
                localPoint1 = tempP;

                tempP = drawPoint0;
                drawPoint0 = drawPoint1;
                drawPoint1 = tempP;
            }
            Point prev = null;

            _equidistant = _currentPart.GetEquidistant(index0, index1, _equidistantLength, _loop);
            foreach (var p in _equidistant)
            {
                if (prev == null)
                {
                    prev = p;
                }
                else
                {
                    var dpPrev = _currentPart.ToScreenCoordinates(prev, w, h);
                    var dpThis = _currentPart.ToScreenCoordinates(p, w, h);
                    _currentPart.DrawLine(graphics, Color.Blue, dpPrev, dpThis);
                    prev = p;
                }
            }
            _currentPart.Remove(localPoint0);
            _currentPart.Remove(localPoint1);
        }
        #endregion
    }
}
