using System.Windows.Forms;
using OutlineWF.Utilities;

namespace OutlineWF.Views.Operations
{
    public class DeleteOperation : Operation
    {
        public override void OnMouseClick(IOperationContext context, MouseEventArgs e)
        {
            if (!e.Button.Equals(MouseButtons.Left)) { return; }
            var mouseClick = new Point(e.X, e.Y);
            context.CurrentPart.Remove(mouseClick, context.Width, context.Height);
        }
    }
}
