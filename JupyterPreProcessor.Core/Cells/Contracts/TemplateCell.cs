using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Core.Segments;
using JupyterPreProcessor.Core.Sequences;

namespace JupyterPreProcessor.Core.Cells.Contracts
{
	public class TemplateCell : Cell
	{
		protected override void Validate()
		{
			var enumerator = Segments.GetEnumerator();
			if (enumerator.MoveNext() == false)
				throw new CellValidationException("Expected at least one segment",
					typeof(TemplateCell), GetType());

			if (enumerator.Current is not TemplateSegment)
				throw new CellValidationException("First segment must be TemplateSegment",
					typeof(TemplateCell), GetType(), enumerator.Current);


			while (enumerator.MoveNext())
			{
				if (enumerator.Current is not TextSegment and not TemplateParameterSegment)
					throw new CellValidationException("Unexpected segment type",
						typeof(TemplateCell), GetType(), enumerator.Current);
			}
		}


		public RawLines SubstituteParameters(
			IReadOnlyDictionary<string, string> singleLineParameters,
			IReadOnlyDictionary<string, RawLines> multiLineParameters
		)
		{
			var builder = new SegmentSequenceBuilder(Segments);

			builder.Foreach(s => s is SingleLineTemplateParameterSegment, s =>
			{
				var sl = (SingleLineTemplateParameterSegment)s;
				var newLine = sl.LeftText + singleLineParameters[sl.Key] + sl.RightText;
				return new TextSegment(new RawLines([newLine]));
			});

			builder.Foreach(s => s is MultiLineTemplateParameterSegment, s =>
			{
				var ml = (MultiLineTemplateParameterSegment)s;
				return new TextSegment(multiLineParameters[ml.Key]);
			});

			builder.RemoveAt(0); // Remove TemplateSegment

			var sequence = builder.Build();

			return sequence.JoinToText();
		}
	}
}
