using JupyterPreProcessor.Core.Cells;
using JupyterPreProcessor.Core.Cells.Contracts;
using JupyterPreProcessor.Core.Configuration;
using JupyterPreProcessor.Core.Plugins;
using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Core.Segments;
using JupyterPreProcessor.Core.Sequences;

namespace JupyterPreProcessor.Core.Engine
{
	public class DefaultPreprocessorEngine : IPreprocessorEngine
	{
		private readonly List<Plugin> _plugins = [];
		private readonly Dictionary<string, TemplateCell> _templates = [];


		public void RegisterPlugin(IPluginCore pluginCore)
		{
			var plugin = new Plugin(pluginCore);
			plugin.Initialize();
			_plugins.Add(plugin);
		}

		public void SetTemplateDocument(RawDocument templateDocument)
		{
			var parser = new TemplateDocumentCellParser();
			foreach (var cell in templateDocument.Cells)
			{
				var template = parser.Parse(cell);
				_templates.Add(template.Name, template);
			}
		}

		public RawDocument Process(RawDocument sourceDocument)
		{
			// Stage 0: Preparing
			var parser = new SourceDocumentCellParser();

			var configCell = sourceDocument.Cells[0];
			var configParser = new DefaultConfigurationParser();
			var configuration = configParser.Parse(configCell);
			var defaultMods = configuration.Interpretate("DefaultMods", new DefaultModsConfigurationInterpreter());
			var parameters = configuration.Interpretate("Parameters", new KeyValueConfigurationInterpreter());

			var cells = sourceDocument.Cells.Skip(1)
				.Select(parser.Parse)
				.Select(s => 
				{
					var builder = new SegmentSequenceBuilder(s.Segments);
					builder[0] = ModsSegment.Merge(defaultMods.BeforeSegment, (ModsSegment)builder[0], defaultMods.AfterSegment);
					return Cell.Create<SourceCell>(builder.Build(), s.Metadata);
				})
				.ToArray();

			var ctx = new Context(this, parameters);

			// Stage 1: Initialization
			var states = new Dictionary<string, object>();
			foreach (var plugin in _plugins)
				states.Add(plugin.Name, plugin.Core.InitializePluginState(ctx));

			// Stage 2: Visiting
			foreach (var cell in cells)
				foreach (var plugin in _plugins)
					plugin.Core.VisitCell(cell, ctx, states[plugin.Name]);

			var document = new List<TextCell>(cells.Length);
			foreach (var cell in cells)
			{
				// Stage 3: Tag resolving
				var resolveResults = new Dictionary<TagSegment, IEnumerable<Segment>>();
				var tags = cell.Segments.OfType<TagSegment>().ToArray();

				foreach (var tag in tags)
				{
					var plugin = _plugins.First(s => s.ResolvableTags.Contains(tag.TagId));
					var result = plugin.Core.ResolveTag(tag, cell, ctx, states[plugin.Name]);
					resolveResults.Add(tag, result);
				}

				var builder = new SegmentSequenceBuilder(cell.Segments);
				builder.Foreach(s => s is TagSegment, s =>
				{
					var tag = (TagSegment)s;
					return resolveResults[tag];
				});
				
				var resolvedCell = Cell.Create<SourceCell>(builder.Build(), cell.Metadata);

				// Stage 4: Mod handling
				var intermediateCell = resolvedCell;
				while (true)
				{
					var entry = ((ModsSegment)intermediateCell.Segments[0]).Mods.FirstOrDefault();
					if (entry == default)
						break;

					var plugin = _plugins.First(s => s.HandledMods.Contains(entry.Id));

					var segments = (IEnumerable<Segment>)plugin.Core.HandleMod(entry, intermediateCell, ctx, states[plugin.Name]);
					var newModSegment = new ModsSegment(((ModsSegment)intermediateCell.Segments[0]).Mods.Skip(1).ToArray());

					intermediateCell = Cell.Create<SourceCell>(new SegmentSequence(segments.Prepend(newModSegment)), intermediateCell.Metadata);
				}

				var readyCell = Cell.Create<TextCell>(new SegmentSequence(intermediateCell.Segments.Skip(1)), intermediateCell.Metadata);
				document.Add(readyCell);
			}

			return new RawDocument(document.Select(s => new RawCell(s.Compile(), s.Metadata)).ToArray());
		}


		private class Context : IPluginContext
		{
			private readonly DefaultPreprocessorEngine _engine;
			private readonly KeyValueConfiguration _configuration;


			public Context(DefaultPreprocessorEngine engine, KeyValueConfiguration configuration)
			{
				_engine = engine;
				_configuration = configuration;
			}


			public string GetParameter(string parameterId)
			{
				return _configuration[parameterId];
			}

			public TemplateCell GetTemplate(string templateId)
			{
				return _engine._templates[templateId];
			}
		}
	}
}
