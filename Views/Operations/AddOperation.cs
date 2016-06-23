using System;
using System.Windows.Forms;
using OutlineWF.Utilities;

namespace OutlineWF.Views.Operations
{
    public class AddOperation : Operation
    {
        public override void OnMouseClick(IOperationContext context, MouseEventArgs e)
        {
            if (!e.Button.Equals(MouseButtons.Left)) { return; }
            var mouseClick = new Point(e.X, e.Y);
            Console.WriteLine(mouseClick);
            context.CurrentPart.AddPoint(mouseClick, context.Width, context.Height);
        }
    }
}
