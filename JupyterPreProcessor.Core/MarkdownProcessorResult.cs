namespace JupyterPreProcessor.Core
{
	public class MarkdownProcessorResult
	{
		public MarkdownProcessorResult(Cell cell)
		{
			Cell = cell;
		}

		public MarkdownProcessorResult()
		{
			Cell = null;
		}


		public Cell? Cell { get; }


		public static MarkdownProcessorResult NoChanges()
		{
			return new MarkdownProcessorResult();
		}

		public static MarkdownProcessorResult LinesChanged(Cell cell)
		{
			return new MarkdownProcessorResult(cell);
		}
	}
}
