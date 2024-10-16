namespace JupyterPreProcessor.Core.Configuration
{
	public class ConfigurationSection
	{
		public ConfigurationSection(string name, IReadOnlyCollection<ConfigurationLine> lines)
		{
			Lines = lines;
			Name = name;
		}


		public IReadOnlyCollection<ConfigurationLine> Lines { get; }

		public string Name { get; }
	}
}
