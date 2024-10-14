using JupyterPreProcessor.Core;

namespace JupyterPreProcessor.Processors
{
	public class TestProcessor : IJupyterProcessor
	{
		public string Id => "TestProcessor";


		public void PreProcessMarkdownCell(MarkdownProcessorContext ctx)
		{
		
		}

		public MarkdownProcessorResult ProcessModsMarkdownCell(MarkdownProcessorContext ctx)
		{
			if (ctx.Cell.HasMod("boilerPlate"))
			{
				var lines = ctx.Cell.Lines.ToList();
				lines.AddRange(["1", "2", "3"]);
				lines.InsertRange(0, ["-1", "-2", "-3"]);
				return MarkdownProcessorResult.LinesChanged();
			}
			else return MarkdownProcessorResult.NoChanges();
		}

		public MarkdownProcessorResult ProcessTagsMarkdownCell(MarkdownProcessorContext ctx)
		{
			throw new NotImplementedException();
		}
	}
}
