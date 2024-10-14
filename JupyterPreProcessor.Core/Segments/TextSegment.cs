namespace JupyterPreProcessor.Core.Segments
{
	public class TextSegment : Segment
	{
		public TextSegment(RawLines text)
		{
			Text = text;
		}


		public RawLines Text { get; }
	}
}
