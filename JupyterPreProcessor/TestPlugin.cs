using JupyterPreProcessor.Core.Cells.Contracts;
using JupyterPreProcessor.Core.Plugins;
using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Core.Segments;

namespace JupyterPreProcessor
{
	internal class TestPlugin : IPluginCore
	{
		public void Build(IPluginBuilder builder)
		{
			builder.WithName("Test")
				.Resolves("test")
				.Resolves("testTemplate")
				.Handles("demo")
				.Uses("Example")
			;
		}


		public object InitializePluginState(IPluginContext ctx) => new();

		public void VisitCell(SourceCell cell, IPluginContext ctx, object state) { }


		public IEnumerable<TextSegment> ResolveTag(TagSegment tag, SourceCell cell, IPluginContext ctx, object state)
		{
			switch (tag.TagId)
			{
				case "test":
					yield return new TextSegment(new RawLines(["[Replate text for @test]"]));
					break;

				case "testTemplate":
					var lines = ctx.GetTemplate("Example").SubstituteParameters(
						new Dictionary<string, string>() { { "data", tag.Arguments[0] } },
						new Dictionary<string, RawLines>() { }
					);
					yield return new TextSegment(lines);
					break;
			}
		}

		public IEnumerable<TextSegment> HandleMod(ModsSegment.ModEntry mod, SourceCell cell, IPluginContext ctx, object state)
		{
			switch (mod.Id)
			{
				case "demo":
					yield return new TextSegment(new RawLines(["wrap start"]));

					foreach (var item in cell.Segments.OfType<TextSegment>())
						yield return item;

					yield return new TextSegment(new RawLines(["wrap end"]));

					break;
			}
		}
	}
}
