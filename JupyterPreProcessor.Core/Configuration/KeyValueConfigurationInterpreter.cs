namespace JupyterPreProcessor.Core.Configuration
{
	public class KeyValueConfigurationInterpreter : IConfigurationInterpreter<KeyValueConfiguration>
	{
		public KeyValueConfiguration Interpretate(ConfigurationSection section)
		{
			return new KeyValueConfiguration(section.Lines.Select(s =>
			{
				var split = s.Line.Split('=');
				return KeyValuePair.Create(split[0].TrimEnd(), split[1].TrimStart());
			}).ToDictionary());
		}
	}
}
