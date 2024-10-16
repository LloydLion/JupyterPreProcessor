namespace JupyterPreProcessor.Core.Configuration
{
	public class DocumentConfiguration
	{
		public DocumentConfiguration(IEnumerable<ConfigurationSection> sections)
		{
			Sections = sections.ToDictionary(s => s.Name);
		}


		public IReadOnlyDictionary<string, ConfigurationSection> Sections { get; }


		public TOutput Interpretate<TOutput>(string section, IConfigurationInterpreter<TOutput> interpreter)
		{
			return interpreter.Interpretate(Sections[section]);
		}

		public TOutput? TryInterpretate<TOutput>(string section, IConfigurationInterpreter<TOutput> interpreter)
		{
			if (Sections.TryGetValue(section, out var result))
			{
				return interpreter.Interpretate(result);
			}
			else return default;
		}
	}
}
