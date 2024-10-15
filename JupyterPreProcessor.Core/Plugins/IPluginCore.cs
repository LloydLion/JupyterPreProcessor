using JupyterPreProcessor.Core.Cells.Contracts;
using JupyterPreProcessor.Core.Segments;
using JupyterPreProcessor.Core.Sequences;

namespace JupyterPreProcessor.Core.Plugins
{
	public interface IPluginCore
	{
		public void Build(IPluginBuilder builder);


		public object InitializePluginState(PluginContext ctx);

		public void VisitCell(SourceCell cell, PluginContext ctx, object state);

		public SegmentSequence ResolveTag(TagSegment tag, SourceCell cell, PluginContext ctx, object state);

		public SegmentSequence HandleMod(
			ModsSegment.ModEntry mod, SourceCell cell, PluginContext ctx, object state);
	}
}
