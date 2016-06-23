using System.Drawing;
using OutlineWF.Models;

namespace OutlineWF.Views.Operations
{
    public interface IOperationContext
    {
        Part CurrentPart { get; }
        
        int Width { get; }
        int Height { get; }
        Color Color { get; }

        void SetState(IOperationState state);
    }
}
