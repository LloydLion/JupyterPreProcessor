namespace JupyterPreProcessor.Core.Segments
{
	public abstract class TemplateParameterSegment : Segment
	{
		protected TemplateParameterSegment(string key)
		{
			Key = key;
		}


		public string Key { get; }
	}
}
