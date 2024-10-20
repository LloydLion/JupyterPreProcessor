using JupyterPreProcessor.Core.Cells.Contracts;
using JupyterPreProcessor.Core.Plugins;
using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Core.Segments;
using System.Collections.Immutable;

namespace JupyterPreProcessor.Plugins
{
    internal class TitlePagePlugin : IPluginCore
    {
        public void Build(IPluginBuilder builder)
        {
            builder.WithName(nameof(TitlePagePlugin))
                .Resolves("titlePage")
                .Uses("TitlePage")
            ;
        }


        public IEnumerable<TextSegment> ResolveTag(TagSegment tag, SourceCell cell, IPluginContext ctx, object state)
        {
            switch (tag.TagId)
            {
                case "titlePage":
                    var lines = ctx.GetTemplate("TitlePage").SubstituteParameters(
                        new Dictionary<string, string>()
                        {
                            ["number"] = tag.Arguments[0],
                            ["name"] = string.Join(" ", tag.Arguments.Skip(1)),
                        },
                        ImmutableDictionary<string, RawLines>.Empty
                    );

                    yield return new TextSegment(lines);
                    break;
            }
        }

        public void VisitCell(SourceCell cell, IPluginContext ctx, object state) { }

        public object InitializePluginState(IPluginContext ctx) => new();

        public IEnumerable<TextSegment> HandleMod(ModsSegment.ModEntry mod, SourceCell cell, IPluginContext ctx, object state) =>
            throw new NotImplementedException();
    }
}
