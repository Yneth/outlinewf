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
    public partial class SerialGraduationForm : Form
    {
        #region PrivateFields
        private int _initialSize;
        private int _outputSize;

        private Part _currentPart;

        private string _detailFilePath;

        private Part _graduationPart;
        private List<Amplifier> _amplifiers;

        private Point _clickedPoint;

        private readonly Bitmap _partImage;
        #endregion

        #region Properties
        public Detail Detail { get; set; }
        public PartDrawContext Drawing { get; set; }
        public Color Color { get; set; }
        #endregion

        #region PublicMethods
        public SerialGraduationForm()
        {
            InitializeComponent();
            _partImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Drawing = new PartDrawContext(Color.Red, pictureBox1.Width, pictureBox1.Height);
            _amplifiers = new List<Amplifier>();

            // Comment if it throws an exception
            string path = Application.StartupPath + "\\bruki4.dgt";
            OpenFile(path);
            // EndComment
        }
        #endregion

        #region PrivateMethods
        private void OpenFile(string path)
        {
            Detail = null;
            _detailFilePath = Path.GetDirectoryName(path);
            if (path.Split('.')[1].Equals("dgt"))
            {
                Detail = Detail.ReadDGT(File.OpenText(path));
                partListView1.Detail = Detail;
                _currentPart = Detail.Parts.FirstOrDefault();
                OpenImage(_currentPart);
                Draw();
            }
        }

        private void OpenImage(Part part)
        {
            string partName = part.Name;
            partImagePicturebox.Image = Image.FromFile(_detailFilePath + "\\" + partName + ".bmp");
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
                        OpenFile(dialog.FileName);
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

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            _clickedPoint = new Point(e.X, e.Y * -1.0f + pictureBox1.Height);
            DrawEditor();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_clickedPoint == null) return;

            var index = _currentPart.IndexOfClosestLocalPoint(_clickedPoint, Drawing.Width, Drawing.Height);
            var amp = new Amplifier(index, new Point(Convert.ToSingle(textBox1.Text), Convert.ToSingle(textBox2.Text)));

            var existing = _amplifiers.FirstOrDefault(amplifier => amplifier.Id == index);
            if (existing != null) existing.Delta = amp.Delta;
            else _amplifiers.Add(amp);

            DrawEditor();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_amplifiers.Count <= 1) { return; }

            _amplifiers.Sort(new AmplifierIdComparer());
            _amplifiers.Add(_amplifiers.First());
            _graduationPart = _currentPart.SerialGraduation(_initialSize, _outputSize, _amplifiers);

            DrawEditor();
            _amplifiers.RemoveAt(_amplifiers.Count - 1);
        }

        private void partListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            _currentPart = partListView1.CurrentPart;

            OpenImage(_currentPart);

            _amplifiers = new List<Amplifier>();
            _graduationPart = null;
            _clickedPoint = null;

            DrawEditor();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _initialSize = Convert.ToInt32(textBox3.Text);
            _outputSize = Convert.ToInt32(textBox4.Text);
            button1.Visible = true;
            button2.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
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

                _currentPart.Draw(graphics, Drawing);
                if (_graduationPart != null)
                {
                    Drawing.Color = Color.Blue;
                    var mxy = _currentPart.CalculateMaxScale(Drawing.Width, Drawing.Height);
                    _graduationPart.Draw(graphics, Drawing, _currentPart.Center, mxy);
                    Drawing.Color = Color.Red;
                }

                DrawSelectedPoint(graphics);
                DrawAmplifiers(graphics);
            }
            pictureBox1.Image = _partImage;
        }

        private void DrawAmplifiers(Graphics graphics)
        {
            int w = Drawing.Width;
            int h = Drawing.Height;
            foreach (var amp in _amplifiers)
            {
                var point0 = _currentPart.Vertices[amp.Id];
                var point1 = _currentPart.Vertices[amp.Id] + amp.Delta * (_initialSize - _outputSize) / 2;

                var dpoint0 = _currentPart.ToScreenCoordinates(point0, w, h);
                var dpoint1 = _currentPart.ToScreenCoordinates(point1, w, h);
                _currentPart.DrawLine(graphics, Drawing.Color, dpoint0, dpoint1);
            }
        }

        private void DrawSelectedPoint(Graphics graphics)
        {
            if (_clickedPoint == null) return;

            int w = Drawing.Width;
            int h = Drawing.Height;

            var point = _currentPart.ClosestLocalPointTo(_clickedPoint, w, h);
            var p = _currentPart.ToScreenCoordinates(point, w, h);
            _currentPart.DrawPoint(graphics, Color.Blue, p);
        }
        #endregion
    }

    class AmplifierIdComparer : IComparer<Amplifier>
    {
        public int Compare(Amplifier x, Amplifier y)
        {
            return x.Id.CompareTo(y.Id);
        }
    }
}
