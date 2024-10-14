using JupyterPreProcessor.Core.Cells.Contracts;
using JupyterPreProcessor.Core.Segments;
using JupyterPreProcessor.Core.Sequences;

namespace JupyterPreProcessor.Core.Plugins
{
	public interface IPluginCore
	{
		public void Build(IPluginBuilder builder);


		public object InitializePluginState();

		public void VisitCell(SourceCell cell, object state);

		public SegmentSequence ResolveTag(TagSegment tag, SourceCell cell, object state);

		public SegmentSequence HandleMod(ModSegment mod, SourceCell cell, object state);
	}
}
