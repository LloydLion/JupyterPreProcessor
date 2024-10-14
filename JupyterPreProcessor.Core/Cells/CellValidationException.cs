using JupyterPreProcessor.Core.Segments;

namespace JupyterPreProcessor.Core.Cells
{
	public class CellValidationException : Exception
	{
		public CellValidationException(string message, Type currentContractType, Type masterContractType)
			: base($"[{currentContractType.Name}/{masterContractType.Name}] {message}")
		{
		
		}

		public CellValidationException(string message, Type currentContractType, Type masterContractType, Segment segment)
			: base($"[{currentContractType.Name}/{masterContractType.Name}] during processing {{{segment}}}: {message}")
		{

		}
	}
}
