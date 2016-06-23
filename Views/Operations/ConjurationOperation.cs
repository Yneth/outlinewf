using System;
using System.Drawing;
using System.Windows.Forms;
using Point = OutlineWF.Utilities.Point;

namespace OutlineWF.Views.Operations
{
    public class ConjurationOperation : Operation
    {
        private Point _point;

        public override void OnButtonClick(IOperationContext context)
        {
            if (_point == null) { return; }

            var d0Text = "";
            InputBox("Input d0", "Input d0", ref d0Text);
            context.CurrentPart.CoupleAngleAt(
                _point,
                context.Width,
                context.Height,
                Convert.ToSingle(d0Text)
            );
            _point = null;
        }

        public override void OnMouseClick(IOperationContext context, MouseEventArgs e)
        {
            var mouseClick = new Point(e.X, e.Y);
            _point = context.CurrentPart.ClosestLocalPointTo(mouseClick, context.Width, context.Height);
        }

        public override void Draw(IOperationContext context, Graphics g)
        {
            if (_point == null) { return; }

            var width = context.Width;
            var height = context.Height;

            var currentPart = context.CurrentPart;

            var point = currentPart.ToScreenCoordinates(_point, width + 20, height + 20);
            g.FillEllipse(Brushes.Blue, point.X - 4, point.Y - 4, 8, 8);
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }




    }
}
