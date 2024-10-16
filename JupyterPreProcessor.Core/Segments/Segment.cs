using JupyterPreProcessor.Core.Raw;

namespace JupyterPreProcessor.Core.Segments
{
    public abstract class Segment
	{
		public RawLines? BareForm { get; init; }
	}
}
