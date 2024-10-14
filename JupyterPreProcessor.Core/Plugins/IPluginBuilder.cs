namespace JupyterPreProcessor.Core.Plugins
{
	public interface IPluginBuilder
	{
		public IPluginBuilder WithName(string name);

		public IPluginBuilder Handles(string modId);

		public IPluginBuilder Resolves(string tagId);

		public IPluginBuilder Uses(string templateId);
	}
}
