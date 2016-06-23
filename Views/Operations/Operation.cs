using System.Drawing;
using System.Windows.Forms;

namespace OutlineWF.Views.Operations
{
    public abstract class Operation : IOperationState
    {
        public virtual void Draw(IOperationContext context, Graphics g) { }

        public virtual void OnButtonClick(IOperationContext context) { }

        public virtual void OnMouseUp(IOperationContext context, MouseEventArgs e) { }

        public virtual void OnMouseDrag(IOperationContext context, MouseEventArgs e) { }

        public virtual void OnMouseMove(IOperationContext context, MouseEventArgs e) { }

        public virtual void OnMouseDown(IOperationContext context, MouseEventArgs e) { }

        public virtual void OnMouseClick(IOperationContext context, MouseEventArgs e) { }
    }
}
