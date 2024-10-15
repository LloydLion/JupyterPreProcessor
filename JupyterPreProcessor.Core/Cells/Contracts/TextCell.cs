using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Core.Segments;

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
			var lines = Segments.SelectMany(s => ((TextSegment)s).Text.Lines).ToArray();
			return new RawLines(lines);
		}
	}
}
