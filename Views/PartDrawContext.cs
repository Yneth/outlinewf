using System.Drawing;

namespace OutlineWF.Views
{
    public class PartDrawContext
    {
        public Color Color { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public PartDrawContext(Color color, int width, int height)
        {
            Color = color;
            Width = width;
            Height = height;
        }
    }
}
