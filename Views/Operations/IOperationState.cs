using System.Drawing;
using System.Windows.Forms;

namespace OutlineWF.Views.Operations
{
    public interface IOperationState
    {
        void Draw(IOperationContext context, Graphics g);

        void OnButtonClick(IOperationContext context);

        void OnMouseUp(IOperationContext context, MouseEventArgs e);

        void OnMouseDrag(IOperationContext context, MouseEventArgs e);

        void OnMouseMove(IOperationContext context, MouseEventArgs e);

        void OnMouseDown(IOperationContext context, MouseEventArgs e);

        void OnMouseClick(IOperationContext context, MouseEventArgs e);
    }
}
