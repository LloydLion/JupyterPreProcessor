using JupyterPreProcessor.Core.Raw;

namespace JupyterPreProcessor.Core.Segments
{
    public abstract class Segment
	{
		public RawLines? BareForm { get; init; }


		public override string ToString()
		{
			int count = BareForm?.Lines.Count ?? -1;
			string firstLine = count > 0 ? BareForm!.Value.Lines[0] : "";
			string lastLine = count > 0 ? BareForm!.Value.Lines[^1] : "";
			return $"|{GetType().Name}|: BareForm.Lines.Count={count}, FirstLine={firstLine}, LastLine={lastLine}";
		}
	}
}
