using JupyterPreProcessor.Core.Plugins;
using JupyterPreProcessor.Core.Raw;

namespace JupyterPreProcessor.Core.Engine
{
	public interface IPreprocessorEngine
	{
		public void RegisterPlugin(IPluginCore pluginCore);

		public void SetTemplateDocument(RawDocument templateDocument);

		public RawDocument Process(RawDocument sourceDocument);
	}
}
