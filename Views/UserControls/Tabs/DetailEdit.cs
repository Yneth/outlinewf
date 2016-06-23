using System;
using System.Drawing;
using System.Windows.Forms;
using OutlineWF.Models;
using OutlineWF.Views.Operations;
using OutlineWF.Views.UserControls;

namespace OutlineWF.Views
{
    public partial class DetailEdit : UserControl, IDetailControl, IOperationContext
    {
        private int _currentPartTabIndex;
        private IOperationState _operationState;
        public Color Color { get; }
        private readonly Bitmap _partImage;
        private Detail _detail;
        public new int Width
        {
            get { return pictureBox1.Width; }
        }
        public new int Height
        {
            get { return pictureBox1.Height; }
        }

        public Detail Detail
        {
            get { return _detail; }
            set
            {
                _detail = value;
                partListView1.Detail = _detail;
            }
        }

        public PartDrawContext Drawing { get; set; }
        public Part CurrentPart
        {
            get
            {
                if (Detail == null) return null;
                return Detail.Parts[_currentPartTabIndex];
            }
        }

        public DetailEdit()
        {
            InitializeComponent();
            _partImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Drawing = new PartDrawContext(Color.Red, pictureBox1.Width, pictureBox1.Height);
            _operationState = new AddOperation();
            _currentPartTabIndex = 0;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            Draw();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (Detail == null) return;
            _operationState.OnMouseClick(this, e);
            Draw();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (Detail == null) return;
            _operationState.OnMouseDown(this, e);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Detail == null) return;
            _operationState.OnMouseMove(this, e);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (Detail == null) return;
            _operationState.OnMouseUp(this, e);
            Draw();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Detail == null) return;
            Detail.Parts[_currentPartTabIndex].Compress();
            Draw();
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            if (Detail == null) return;
            SetState(new MoveOperation());
            Draw();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (Detail == null) return;
            SetState(new DeleteOperation());
            Draw();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (Detail == null) return;
            SetState(new AddOperation());
            Draw();
        }

        private void smoothButton_Click(object sender, EventArgs e)
        {
            if (Detail == null) return;
            SetState(new BasicSplineSmoothOperation());
            _operationState.OnButtonClick(this);
            Draw();
        }

        private void iSplineSmoothButton_Click(object sender, EventArgs e)
        {
            if (Detail == null) return;
            SetState(new InterSpineSmoothOperation());
            _operationState.OnButtonClick(this);
            Draw();
        }

        private void conjugationButton_Click(object sender, EventArgs e)
        {
            if (Detail == null) return;
            SetState(new ConjurationOperation());
            _operationState.OnButtonClick(this);
            Draw();
        }

        private void partListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            _currentPartTabIndex = e.ItemIndex;
            Draw();
        }

        public void SetState(IOperationState state)
        {
            if (_operationState.GetType() == state.GetType()) { return; }
            _operationState = state;
        }

        private void Draw()
        {
            if (Detail == null || Detail.Parts.Count == 0) return;

            DrawPart();
        }

        private void DrawPart()
        {
            pictureBox1.Refresh();
            using (var graphics = Graphics.FromImage(_partImage))
            {
                pictureBox1.Image = _partImage;
                graphics.Clear(Color.White);
                var container = graphics.BeginContainer();

                graphics.ScaleTransform(1.0f, -1.0f);
                graphics.TranslateTransform(0.0f, -pictureBox1.Height);

                Detail.Parts[_currentPartTabIndex].Draw(graphics, Drawing);
                _operationState.Draw(this, graphics);

                graphics.EndContainer(container);
            }
        }
    }
}
