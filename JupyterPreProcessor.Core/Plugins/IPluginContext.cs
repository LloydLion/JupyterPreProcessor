using JupyterPreProcessor.Core.Cells.Contracts;

namespace JupyterPreProcessor.Core.Plugins
{
	public interface IPluginContext
	{
		public TemplateCell GetTemplate(string templateId);

		public string GetParameter(string parameterId);
	}
}
