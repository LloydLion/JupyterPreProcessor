using JupyterPreProcessor.Core.Raw;

namespace JupyterPreProcessor.Core.Segments
{
	public class ModsSegment : Segment
	{
		public ModsSegment(IReadOnlyList<ModEntry> mods)
		{
			Mods = mods;
		}


		public IReadOnlyList<ModEntry> Mods { get; }


		public record struct ModEntry(string Id, IReadOnlyList<string> Arguments);


		public static ModsSegment Merge(params ModsSegment[] segments)
		{
			return new ModsSegment(segments.SelectMany(s => s.Mods).ToArray())
			{
				BareForm = new RawLines(
					segments
						.Select(s => s.BareForm)
						.Where(s => s is not null)
						.SelectMany(s => s!.Value.Lines)
						.ToArray()
				)
			};
		}
	}
}
