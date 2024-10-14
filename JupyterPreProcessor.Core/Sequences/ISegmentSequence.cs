using JupyterPreProcessor.Core.Segments;

namespace JupyterPreProcessor.Core.Sequences
{
	public interface ISegmentSequence : IReadOnlyList<Segment>
	{
		public new ISegmentSequenceEnumerator GetEnumerator();

		public ISegmentSequenceEnumerator GetEnumerator(int index);

		public ISegmentSequenceEnumerator? GetEnumerator(Predicate<Segment> condition);
	}
}
