namespace JupyterPreProcessor.Core.Segments
{
	public class TemplateSegment : Segment
	{
		public TemplateSegment(string name)
		{
			Name = name;
		}


		public string Name { get; }
	}
}
