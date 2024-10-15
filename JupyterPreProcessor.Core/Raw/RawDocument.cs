namespace JupyterPreProcessor.Core.Raw
{
	public class RawDocument
	{
		public RawDocument(IReadOnlyList<RawCell> cells)
		{
			if (cells.Count == 0)
				throw new ArgumentException("Excepted at least one cell in document", nameof(cells));

			Source = cells[0].Metadata.SourceDocument;

			foreach (RawCell cell in cells)
				if (cell.Metadata.SourceDocument != Source)
					throw new ArgumentException("Raw cells from different sources", nameof(cells));

			Cells = cells;
		}


		public string Source { get; }

		public IReadOnlyList<RawCell> Cells { get; }
	}
}
