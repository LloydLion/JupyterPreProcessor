using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Core.Segments;

namespace JupyterPreProcessor.Core.Sequences
{
	public static class SequenceExtensions
	{
		public static RawLines JoinToText(this ISegmentSequence sequence)
		{
			var lines = sequence.SelectMany(s => ((TextSegment)s).Text.Lines).ToArray();
			return new RawLines(lines);
		}
	}
}
