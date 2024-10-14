namespace JupyterPreProcessor.Core.Segments
{
	public class TagSegment : Segment
	{
		public TagSegment(string tagId, IReadOnlyList<string> arguments)
		{
			TagId = tagId;
			Arguments = arguments;
		}


		public string TagId { get; }

		public IReadOnlyList<string> Arguments { get; }
	}
}
