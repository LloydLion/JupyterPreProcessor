using JupyterPreProcessor.Core.Cells.Contracts;
using JupyterPreProcessor.Core.Plugins;
using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Core.Segments;
using System.Collections.Immutable;

namespace JupyterPreProcessor.Plugins
{
	internal class LiterPlugin : IPluginCore
	{
		public void Build(IPluginBuilder builder)
		{
			builder.WithName(nameof(LiterPlugin))
				.Resolves("liter")
				.Uses("Liter")
			;
		}

		public IEnumerable<TextSegment> HandleMod(ModsSegment.ModEntry mod, SourceCell cell, IPluginContext ctx, object state) =>
			throw new NotImplementedException();

		public object InitializePluginState(IPluginContext ctx) => new();

		public IEnumerable<TextSegment> ResolveTag(TagSegment tag, SourceCell cell, IPluginContext ctx, object state)
		{
			switch (tag.TagId)
			{
				case "liter":
					var lines = ctx.GetTemplate("Liter").SubstituteParameters(
						new Dictionary<string, string>()
						{
							["index"] = tag.Arguments[0],
							["url"] = tag.Arguments[1],
							["sourceName"] = tag.Arguments[2],
							["pageName"] = string.Join(" ", tag.Arguments.Skip(3)),
						},
						ImmutableDictionary<string, RawLines>.Empty
					);
					yield return new TextSegment(lines);
					break;
			}
		}

		public void VisitCell(SourceCell cell, IPluginContext ctx, object state) { }
	}
}
