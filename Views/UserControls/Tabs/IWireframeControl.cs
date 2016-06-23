using System.Drawing;
using Point = OutlineWF.Utilities.Point;

namespace OutlineWF.Views.UserControls
{
    public interface IWireframeControl : IDetailControl
    {
        int CurrentPartIndex { get; set; }
        Bitmap Image { get; set; }
        Point ImageScale { get; }
    }
}
