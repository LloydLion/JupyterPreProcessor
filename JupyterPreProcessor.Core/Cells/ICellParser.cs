using JupyterPreProcessor.Core.Raw;

namespace JupyterPreProcessor.Core.Cells
{
    public interface ICellParser<TCell> where TCell : Cell
	{
		public TCell Parse(RawCell rawCell);
	}
}
