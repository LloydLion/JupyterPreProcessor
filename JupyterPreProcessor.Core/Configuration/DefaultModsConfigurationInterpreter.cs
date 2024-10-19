using JupyterPreProcessor.Core.Segments;

namespace JupyterPreProcessor.Core.Configuration
{
	public class DefaultModsConfigurationInterpreter : IConfigurationInterpreter<DefaultModsConfiguration>
	{
		public DefaultModsConfiguration Interpretate(ConfigurationSection section)
		{
			var afterMods = section.Lines
				.Where(s => s.Line.StartsWith("after "))
				.Select(parse)
				.ToArray();


			var beforeMods = section.Lines
				.Where(s => s.Line.StartsWith("before "))
				.Select(parse)
				.ToArray();

			return new DefaultModsConfiguration(new ModsSegment(beforeMods), new ModsSegment(afterMods));


			ModsSegment.ModEntry parse(ConfigurationLine line)
			{
				var subLines = line.Line.Split(' ');
				var name = subLines[1];
				var arguments = subLines[2..];
				return new ModsSegment.ModEntry(name, arguments);
			}
		}
	}
}
