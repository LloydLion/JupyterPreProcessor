using JupyterPreProcessor.Core.Cells.Contracts;
using JupyterPreProcessor.Core.Plugins;
using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Core.Segments;
using System.Collections.Immutable;

namespace JupyterPreProcessor.Plugins
{
	internal class ContentTablePlugin : IPluginCore
	{
		public void Build(IPluginBuilder builder)
		{
			builder.WithName(nameof(ContentTablePlugin))
				.Resolves("header")
				.Resolves("contentTable")
				.Uses("ContentTable")
				.Uses("ContentTableItem")
				.Uses("Header")
			;
		}

		public void VisitCell(SourceCell cell, IPluginContext ctx, object state)
		{
			var headers = ((State)state).Headers;

			foreach (var header in cell.Segments.OfType<TagSegment>().Where(s => s.TagId == "header"))
			{
				var index = header.Arguments[0];
				var depth = 0;
				while (index.StartsWith('-'))
				{
					index = index[1..];
					depth++;
				}

				var name = string.Join(" ", header.Arguments.Skip(1));

				headers.Add(new State.Header(header, depth, index, name));
			}
		}

		public IEnumerable<TextSegment> ResolveTag(TagSegment tag, SourceCell cell, IPluginContext ctx, object state)
		{
			var headers = ((State)state).Headers;

			switch (tag.TagId)
			{
				case "header":
					{
						var header = headers.Single(s => s.Tag == tag);
						var lines = ctx.GetTemplate("Header").SubstituteParameters(
							new Dictionary<string, string>()
							{
								["index"] = header.Index,
								["name"] = header.Name,
								["hashes"] = new string('#', header.Depth)
							},
							ImmutableDictionary<string, RawLines>.Empty
						);
						yield return new TextSegment(lines);
						break;
					}

				case "contentTable":
					{
						var tableItem = ctx.GetTemplate("ContentTableItem");
						var contentTableItemsList = new List<RawLines>();
						foreach (var header in headers)
						{
							var lines = tableItem.SubstituteParameters(
								new Dictionary<string, string>()
								{
									["intend"] = new string(Enumerable.Range(0, header.Depth).SelectMany(s => "&emsp;").ToArray()),
									["index"] = header.Index,
									["name"] = header.Name,
								},
								ImmutableDictionary<string, RawLines>.Empty
							);
							contentTableItemsList.Add(lines);
						}

						var contentTableItemsLines = new RawLines(contentTableItemsList.SelectMany(s => s.Lines).ToArray());

						var result = ctx.GetTemplate("ContentTable").SubstituteParameters(
							ImmutableDictionary<string, string>.Empty,
							new Dictionary<string, RawLines>()
							{
								["items"] = contentTableItemsLines
							}
						);

						yield return new TextSegment(result);
						break;
					}
			}
		}

		public IEnumerable<TextSegment> HandleMod(ModsSegment.ModEntry mod, SourceCell cell, IPluginContext ctx, object state) =>
			throw new NotImplementedException();

		public object InitializePluginState(IPluginContext ctx) => new State();


		private record State()
		{
			public List<Header> Headers { get; } = [];


			public record struct Header(TagSegment Tag, int Depth, string Index, string Name);
		}
	}
}
