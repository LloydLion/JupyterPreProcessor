using JupyterPreProcessor.Core.Raw;

namespace JupyterPreProcessor.Core.Configuration
{
	public interface IConfigurationParser
	{
		public DocumentConfiguration Parse(RawCell configurationCell);
	}
}
