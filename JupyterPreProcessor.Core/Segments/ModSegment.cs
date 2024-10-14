namespace JupyterPreProcessor.Core.Segments
{
	public class ModSegment : Segment
	{
		public ModSegment(string modId, IReadOnlyList<string> arguments)
		{
			ModId = modId;
			Arguments = arguments;
		}


		public string ModId { get; }

		public IReadOnlyList<string> Arguments { get; }
	}
}
