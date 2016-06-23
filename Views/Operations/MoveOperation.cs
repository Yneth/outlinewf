using System.Windows.Forms;
using OutlineWF.Utilities;

namespace OutlineWF.Views.Operations
{
    public class MoveOperation : Operation
    {
        private Point _to;
        private Point _from;

        public override void OnMouseUp(IOperationContext context, MouseEventArgs e)
        {
            if (_to == null || _from == null) { return; }
            context.CurrentPart.Set(_from, _to, context.Width, context.Height);
            _to = _from = null;
        }

        public override void OnMouseDown(IOperationContext context, MouseEventArgs e)
        {
            if (!e.Button.Equals(MouseButtons.Left)) { return; }
            _from = new Point(e.X, e.Y);
        }

        public override void OnMouseMove(IOperationContext context, MouseEventArgs e)
        {
            if (!e.Button.Equals(MouseButtons.Left)) { return; }
            _to = new Point(e.X, e.Y);
        }
    }
}
