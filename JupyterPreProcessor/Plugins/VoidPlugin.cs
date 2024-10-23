using JupyterPreProcessor.Core.Cells.Contracts;
using JupyterPreProcessor.Core.Plugins;
using JupyterPreProcessor.Core.Segments;

namespace JupyterPreProcessor.Plugins
{
	internal class VoidPlugin : IPluginCore
	{
		public void Build(IPluginBuilder builder)
		{
			builder.WithName(nameof(VoidPlugin))
				.Handles("void");
		}

		public IEnumerable<TextSegment> HandleMod(ModsSegment.ModEntry mod, SourceCell cell, IPluginContext ctx, object state)
		{
			return mod.Id switch
			{
				"void" => [],
				_ => cell.Segments.OfType<TextSegment>(),
			};
		}

		public object InitializePluginState(IPluginContext ctx) => new();

		public IEnumerable<TextSegment> ResolveTag(TagSegment tag, SourceCell cell, IPluginContext ctx, object state) =>
			throw new NotImplementedException();

		public void VisitCell(SourceCell cell, IPluginContext ctx, object state) { }
	}
}
