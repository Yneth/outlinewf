namespace OutlineWF.Views.Operations
{
    public class BasicSplineSmoothOperation : SmoothOperation
    {
        public override void OnButtonClick(IOperationContext context)
        {
            if (To == null || From == null) { return; }
            context.CurrentPart.BSplineSmoothRange(From, To);
            From = null;
            To = null;
        }
    }
}
