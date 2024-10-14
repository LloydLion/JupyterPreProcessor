namespace JupyterPreProcessor.Core
{
	public class MarkdownProcessorContext
	{
		public const char TagPrefix = '@';


		public Cell Cell { get; }


		public MarkdownProcessorContext(Cell cell)
		{
			Cell = cell;
		}
	}
}
