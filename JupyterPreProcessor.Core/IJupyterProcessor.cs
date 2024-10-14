namespace JupyterPreProcessor.Core
{
	public interface IJupyterProcessor
	{
		public string Id { get; }


		public void PreProcessMarkdownCell(MarkdownProcessorContext ctx);

		public MarkdownProcessorResult ProcessTagsMarkdownCell(MarkdownProcessorContext ctx);

		public MarkdownProcessorResult ProcessModsMarkdownCell(MarkdownProcessorContext ctx);
	}
}
