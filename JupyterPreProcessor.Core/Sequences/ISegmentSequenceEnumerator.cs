using JupyterPreProcessor.Core.Segments;

namespace JupyterPreProcessor.Core.Sequences
{
	public interface ISegmentSequenceEnumerator : IEnumerator<Segment>
	{
		public bool MovePrevious();
	}
}
