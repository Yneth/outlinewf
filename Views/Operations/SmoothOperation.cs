using System.Drawing;
using System.Windows.Forms;
using Point = OutlineWF.Utilities.Point;

namespace OutlineWF.Views.Operations
{
    public abstract class SmoothOperation : Operation
    {
        protected Point To;
        protected Point From;

        public override void OnMouseClick(IOperationContext context, MouseEventArgs e)
        {
            if (!e.Button.Equals(MouseButtons.Left)) { return; }

            var mouseClick = new Point(e.X, e.Y);
            var partPoint = context.CurrentPart.ClosestLocalPointTo(mouseClick,
                context.Width, context.Height);

            if (From == null) { From = partPoint; }
            else if (To == null) { To = partPoint; }
        }

        public override void Draw(IOperationContext context, Graphics g)
        {
            if (From == null) { return; }
            //var from = context.CurrentPart.ToScreenCoordinates(From, context.Width + 20, context.Height + 20);
            var from = context.CurrentPart.ToScreenCoordinates(From, context.Width, context.Height);
            g.FillEllipse(Brushes.Blue, from.X - 4, from.Y - 4, 8, 8);
            if (To == null) { return; }
            //var to = context.CurrentPart.ToScreenCoordinates(To, context.Width + 20, context.Height + 20);
            var to = context.CurrentPart.ToScreenCoordinates(To, context.Width, context.Height);
            g.FillEllipse(Brushes.Blue, to.X - 4, to.Y - 4, 8, 8);
            Point previous = null;
            foreach (var p in context.CurrentPart.Range(From, To))
            {
                if (null == previous)
                {
                    previous = p;
                    continue;
                }
                var p0 = context.CurrentPart.ToScreenCoordinates(previous, context.Width, context.Height);
                //var p0 = context.CurrentPart.ToScreenCoordinates(previous, context.Width + 20, context.Height + 20);
                //var p1 = context.CurrentPart.ToScreenCoordinates(p, context.Width + 20, context.Height + 20);
                var p1 = context.CurrentPart.ToScreenCoordinates(p, context.Width, context.Height);
                g.DrawLine(Pens.Blue, p0.X, p0.Y, p1.X, p1.Y);
                previous = p;
            }
        }
    }
}
