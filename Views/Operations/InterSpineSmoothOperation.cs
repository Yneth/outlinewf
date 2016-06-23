namespace OutlineWF.Views.Operations
{
    public class InterSpineSmoothOperation : SmoothOperation
    {
        public override void OnButtonClick(IOperationContext context)
        {
            if (To == null || From == null) { return; }
            context.CurrentPart.InterSplineSmoothRange(From, To);
            From = null;
            To = null;
        }
    }
}
