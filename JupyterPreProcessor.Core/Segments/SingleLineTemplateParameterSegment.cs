namespace JupyterPreProcessor.Core.Segments
{
	public class SingleLineTemplateParameterSegment : TemplateParameterSegment
	{
		public SingleLineTemplateParameterSegment(string key, string leftText, string rightText) : base(key)
		{
			LeftText = leftText;
			RightText = rightText;
		}


		public string LeftText { get; }

		public string RightText { get; }
	}
}
