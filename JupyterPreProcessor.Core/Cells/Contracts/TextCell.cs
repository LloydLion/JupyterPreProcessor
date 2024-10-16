using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Core.Segments;
using JupyterPreProcessor.Core.Sequences;

namespace JupyterPreProcessor.Core.Cells.Contracts
{
	public class TextCell : Cell
	{
		protected override void Validate()
		{
			foreach (var segment in Segments)
				if (segment is not TextSegment)
					throw new CellValidationException("Only text segments allowed",
						typeof(TextCell), GetType(), segment);
		}


		public RawLines Compile()
		{
			return Segments.JoinToText();
		}
	}
}
