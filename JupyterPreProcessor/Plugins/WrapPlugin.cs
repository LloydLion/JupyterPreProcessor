using JupyterPreProcessor.Core.Cells.Contracts;
using JupyterPreProcessor.Core.Plugins;
using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Core.Segments;
using System.Collections.Immutable;

namespace JupyterPreProcessor.Plugins
{
	internal class WrapPlugin : IPluginCore
	{
		public void Build(IPluginBuilder builder)
		{
			builder.WithName(nameof(WrapPlugin))
				.Handles("wrap")
				.Uses("WrapBeforeMarkdown")
				.Uses("WrapAfterMarkdown")
				.Uses("WrapBeforeCode")
				.Uses("WrapAfterCode")
			;
		}

		public IEnumerable<TextSegment> HandleMod(ModsSegment.ModEntry mod, SourceCell cell, IPluginContext ctx, object state)
		{
			switch (mod.Id)
			{
				case "wrap":
					var before = ctx.GetTemplate("WrapBefore" + cell.Metadata.Type)
						.SubstituteParameters(ImmutableDictionary<string, string>.Empty, ImmutableDictionary<string, RawLines>.Empty);
					yield return new TextSegment(before);

					foreach (var item in cell.Segments.Skip(1))
						yield return (TextSegment)item;

					var after = ctx.GetTemplate("WrapAfter" + cell.Metadata.Type)
						.SubstituteParameters(ImmutableDictionary<string, string>.Empty, ImmutableDictionary<string, RawLines>.Empty);
					yield return new TextSegment(after);
					break;
			}
		}

		public object InitializePluginState(IPluginContext ctx) => new();

		public IEnumerable<TextSegment> ResolveTag(TagSegment tag, SourceCell cell, IPluginContext ctx, object state) =>
			throw new NotImplementedException();

		public void VisitCell(SourceCell cell, IPluginContext ctx, object state) { }
	}
}
