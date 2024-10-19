#nullable disable
using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Core.Sequences;

namespace JupyterPreProcessor.Core.Cells
{
	public abstract class Cell
	{
		private ISegmentSequence _sequence;
		private CellMetadata _metadata;


		public static TCell Create<TCell>(ISegmentSequence segments, CellMetadata metadata) where TCell : Cell, new()
		{
			var cell = Activator.CreateInstance<TCell>();
			cell._sequence = segments;
			cell._metadata = metadata;

			cell.Validate();

			return cell;
		}


		public ISegmentSequence Segments => _sequence;

		public CellMetadata Metadata => _metadata;


		protected abstract void Validate();
	}
}
