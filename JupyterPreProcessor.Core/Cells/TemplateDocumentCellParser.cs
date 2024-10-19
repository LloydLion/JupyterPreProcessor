using JupyterPreProcessor.Core.Cells.Contracts;
using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Core.Segments;
using JupyterPreProcessor.Core.Sequences;
using System.Text.RegularExpressions;

namespace JupyterPreProcessor.Core.Cells
{
	public class TemplateDocumentCellParser : ICellParser<TemplateCell>
	{
		public TemplateCell Parse(RawCell rawCell)
		{
			var segments = new List<Segment>();
			var lines = rawCell.Lines.Lines.ToArray();
			var i = 1;

			segments.Add(new TemplateSegment(lines[0]) { BareForm = new RawLines([lines[0]]) });

			var parameterEntry = i;
			while (parameterEntry < lines.Length)
			{
				var line = lines[parameterEntry];
				var slMatch = Regex.Match(line, @"!!{(\w+)}");
				var mlMatch = Regex.Match(line, @"^!!{\.\.\.(\w+)}$");
				var isMatch = slMatch.Success || mlMatch.Success;

				if (isMatch)
				{
					if (parameterEntry > i)
					{
						var previousText = new RawLines(lines[i..parameterEntry]);
						segments.Add(new TextSegment(previousText) { BareForm = previousText });
					}

					Segment parameterSegment;
					if (mlMatch.Success)
					{
						parameterSegment = new MultiLineTemplateParameterSegment(mlMatch.Groups[1].Value)
							{ BareForm = new RawLines([line]) };
					}
					else // slMatch.Success
					{
						parameterSegment = new SingleLineTemplateParameterSegment(
							slMatch.Groups[1].Value,
							line[..slMatch.Index],
							line[(slMatch.Index + slMatch.Length)..]
						) { BareForm = new RawLines([line]) };
					}
                    
					segments.Add(parameterSegment);

				}

				parameterEntry++;

				if (isMatch)
					i = parameterEntry;
            }

			if (parameterEntry > i)
			{
				var previousText = new RawLines(lines[i..parameterEntry]);
				segments.Add(new TextSegment(previousText) { BareForm = previousText });
			}

			return Cell.Create<TemplateCell>(new SegmentSequence(segments), rawCell.Metadata);
		}
	}
}
