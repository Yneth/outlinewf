using System;
using System.Drawing;
using System.Windows.Forms;
using OutlineWF.Models;

namespace OutlineWF.Views.UserControls
{
    public delegate void ItemSelectionChangedHandler(object sender, ListViewItemSelectionChangedEventArgs e);
    public partial class PartListView : UserControl
    {
        public Detail Detail { get; set; }
        public Color DetailColor { get; set; }
        public Part CurrentPart { get; private set; }
        public event ItemSelectionChangedHandler ItemSelectionChangedEvent;

        public int CurrentPartIndex { get { return _currentPartIndex; } }

        private int _currentPartIndex = 0;

        public PartListView()
        {
            InitializeComponent();
            SizeChanged += (sender, args) => listView.Size = Size;
        }

        private void DrawListViewParts()
        {
            if (Detail == null || partImages == null || listView == null) { return; }

            partImages.Images.Clear();
            listView.Items.Clear();
            int i = 0;
            foreach (var p in Detail.Parts)
            {
                var size = partImages.ImageSize;
                var bitmap = new Bitmap(size.Width, size.Height);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    var container = graphics.BeginContainer();

                    graphics.ScaleTransform(1.0f, -1.0f);
                    graphics.TranslateTransform(0.0f, -bitmap.Height);

                    p.Draw(graphics, new PartDrawContext(DetailColor, size.Width, size.Height));

                    graphics.EndContainer(container);
                }
                partImages.Images.Add(bitmap);
                listView.Items.Add(new ListViewItem { ImageIndex = i++ });
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            DrawListViewParts();
        }

        private void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (Detail == null || Detail.Parts.Count == 0) { return; }

            _currentPartIndex = e.ItemIndex;
            CurrentPart = Detail.Parts[_currentPartIndex % Detail.Parts.Count];

            if (ItemSelectionChangedEvent != null)
            {
                ItemSelectionChangedEvent(this, e);
            }
        }
    }
}
