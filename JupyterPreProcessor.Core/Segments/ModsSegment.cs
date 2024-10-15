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
	}
}
