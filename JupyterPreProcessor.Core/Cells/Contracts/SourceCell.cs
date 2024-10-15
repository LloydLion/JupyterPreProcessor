using JupyterPreProcessor.Core.Segments;

namespace JupyterPreProcessor.Core.Cells.Contracts
{
	public class SourceCell : Cell
	{
		protected override void Validate()
		{
			var enumerator = Segments.GetEnumerator();
			if (enumerator.MoveNext() == false)
				throw new CellValidationException("Expected at least one segment",
					typeof(SourceCell), GetType());

			if (enumerator.Current is not ModsSegment)
				throw new CellValidationException("First segment must be ModsSegment",
					typeof(SourceCell), GetType(), enumerator.Current);


			while (enumerator.MoveNext())
			{
				if (enumerator.Current is not TextSegment and not TagSegment)
					throw new CellValidationException("Unexpected segment type",
						typeof(SourceCell), GetType(), enumerator.Current);
			}
		}
	}
}
