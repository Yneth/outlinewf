using System.Drawing;
using OutlineWF.Models;

namespace OutlineWF.Views.UserControls
{
    public interface IDetailControl
    {
        Detail Detail { get; set; }
        PartDrawContext Drawing { get; set; }
    }
}
