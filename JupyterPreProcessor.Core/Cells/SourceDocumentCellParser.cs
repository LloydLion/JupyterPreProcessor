using JupyterPreProcessor.Core.Cells.Contracts;
using JupyterPreProcessor.Core.Raw;
using JupyterPreProcessor.Core.Segments;
using JupyterPreProcessor.Core.Sequences;

namespace JupyterPreProcessor.Core.Cells
{
	public class SourceDocumentCellParser : ICellParser<SourceCell>
	{
		public SourceCell Parse(RawCell rawCell)
		{
			var lines = rawCell.Lines.Lines.ToArray();
			int i = 0;

			if (lines.Length == 0)
				return Cell.Create<SourceCell>(new SegmentSequence([new ModsSegment([])]), rawCell.Metadata);

			var segments = new List<Segment>();

			var mods = new List<ModsSegment.ModEntry>();
			while (lines[i].StartsWith('.'))
			{
				var (id, args) = ParseName(lines[i]);
				mods.Add(new ModsSegment.ModEntry(id, args));
				i++;
			}

			segments.Add(new ModsSegment(mods) { BareForm = new RawLines(lines[..i]) });

			int tagEntry = i;
			while (tagEntry < lines.Length)
			{
				if (lines[tagEntry].StartsWith('@'))
				{
					if (tagEntry > i)
					{
						var segmentLines = lines[i..tagEntry];
						segments.Add(new TextSegment(new RawLines(segmentLines))
							{ BareForm = new RawLines(segmentLines) });
					}

					var (id, args) = ParseName(lines[tagEntry]);
					segments.Add(new TagSegment(id, args) { BareForm = new RawLines([lines[tagEntry]]) });

					tagEntry++;
					i = tagEntry;
				}
				else tagEntry++;
			}

			if (tagEntry > i)
			{
				var segmentLines = lines[i..tagEntry];
				segments.Add(new TextSegment(new RawLines(segmentLines))
					{ BareForm = new RawLines(segmentLines) });
			}

			return Cell.Create<SourceCell>(new SegmentSequence(segments), rawCell.Metadata);
		}


		private (string, IReadOnlyList<string>) ParseName(string line)
		{
			var subLines = line[1..].Split(' ');
			var name = subLines[0];
			var arguments = subLines[1..];
			return (name, arguments);
		}
	}
}
