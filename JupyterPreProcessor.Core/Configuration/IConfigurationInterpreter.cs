namespace JupyterPreProcessor.Core.Configuration
{
	public interface IConfigurationInterpreter<TOutput>
	{
		public TOutput Interpretate(ConfigurationSection section);
	}
}
