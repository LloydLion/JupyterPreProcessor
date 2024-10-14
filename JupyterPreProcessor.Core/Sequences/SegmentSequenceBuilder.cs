using JupyterPreProcessor.Core.Segments;

namespace JupyterPreProcessor.Core.Sequences
{
	public class SegmentSequenceBuilder
	{
		private readonly List<Segment> _segments;


		public SegmentSequenceBuilder(ISegmentSequence baseSequence)
		{
			_segments = new List<Segment>(baseSequence);
		}

		public SegmentSequenceBuilder()
		{
			_segments = [];
		}


		public ISegmentSequence Build()
		{
			return new SegmentSequence(_segments);
		}
	}
}
