using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OutlineWF.Models;
using OutlineWF.Views.UserControls;
using Point = OutlineWF.Utilities.Point;

namespace OutlineWF.Views
{
    public partial class Wireframe : UserControl, IWireframeControl
    {
        private Detail _detail;
        private Part _part;

        private Bitmap _image;
        private Point _imageScale;

        public Detail Detail
        {
            get
            {
                return _detail;
            }
            set
            {
                _detail = value;
                _part = new Part();
                CurrentPartIndex = 0;
            }
        }

        public PartDrawContext Drawing { get; set; }
        public Color Color { get; set; }

        public int CurrentPartIndex { get; set; }

        public Bitmap Image
        {
            get { return _image; }
            set
            {
                _image = value;
                SetImage(_image);
            }
        }

        public Point ImageScale
        {
            get { return _imageScale; }
        }

        public Wireframe()
        {
            InitializeComponent();
            CurrentPartIndex = 0;
            _part = new Part();
            SizeChanged += (sender, args) => picturePanel.Size = Size;
        }

        private void SetImage(Bitmap image)
        {
            picture.Image = image;
            var bump = new Bitmap(image.Width, image.Height);

            _imageScale = new Point(
                1000 / (bump.HorizontalResolution * 39.37007874016f),
                1000 / (bump.VerticalResolution * 39.37007874016f)
                );
            var r = File.ReadAllBytes(Application.StartupPath + @"\q1.bmp").Skip(38).Take(8).ToArray();
            var p = new Point(
                (float)(1000 / (r[0] + 256 * r[1] + 256 * 256 * r[2] + Math.Pow(256, 3) * r[3])),
                (float)(1000 / (r[4] + 256 * r[5] + 256 * 256 * r[6] + Math.Pow(256, 3) * r[7]))
            );
        }

        private void PicturePanel_MouseWheel(object sender, MouseEventArgs e)
        {
            Draw();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            Draw();
        }

        private void picture_MouseClick(object sender, MouseEventArgs e)
        {
            if (Detail == null) { return; }

            var clickPosition = new Point(e.X, e.Y);
            if (e.Button.Equals(MouseButtons.Left))
            {
                _part.AddPoint(clickPosition * _imageScale);
            }
            else if (e.Button.Equals(MouseButtons.Middle))
            {
                Detail.AddPart(_part);
                _part = new Part();
                CurrentPartIndex++;
            }
            else if (e.Button.Equals(MouseButtons.Right))
            {
                _part.Remove(clickPosition * _imageScale);
            }
            Draw();
        }

        private void Draw()
        {
            picture.Refresh();
            using (var graphics = picture.CreateGraphics())
            {
                Detail.Parts.ForEach(p => DrawPart(graphics, p));
                DrawPart(graphics, _part);
            }
        }

        private void DrawPart(Graphics graphics, Part part)
        {
            var vertices = part.Vertices;

            if (vertices.Count <= 0) { return; }

            if (vertices.Count == 1)
            {
                DrawPoint(graphics, vertices[0] / _imageScale);
                return;
            }

            for (var i = 0; i < vertices.Count - 1; i++)
            {
                var p0 = vertices[i] / _imageScale;
                var p1 = vertices[i + 1] / _imageScale;
                DrawPoint(graphics, p0);
                DrawLine(graphics, p0, p1);
            }

            var last = vertices[vertices.Count - 1] / _imageScale;
            DrawPoint(graphics, last);
            DrawLine(graphics, last, vertices[0] / _imageScale);
        }

        private void DrawPoint(Graphics graphics, Point position)
        {
            const int width = 10;
            const int height = 10;
            graphics.FillEllipse(Brushes.Red,
                position.X - width / 2.0f, position.Y - height / 2.0f,
                width, height);
        }

        private void DrawLine(Graphics graphics, Point p1, Point p2)
        {
            graphics.DrawLine(Pens.Red, p1.X, p1.Y, p2.X, p2.Y);
        }

        private void picturePanel_Scroll(object sender, ScrollEventArgs e)
        {
            Draw();
        }
    }
}