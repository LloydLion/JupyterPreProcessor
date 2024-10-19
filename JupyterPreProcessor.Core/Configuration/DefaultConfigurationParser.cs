using JupyterPreProcessor.Core.Raw;

namespace JupyterPreProcessor.Core.Configuration
{
	public class DefaultConfigurationParser : IConfigurationParser
	{
		public DocumentConfiguration Parse(RawCell configurationCell)
		{
			var sections = new List<ConfigurationSection>();
			var lines = configurationCell.Lines.Lines;
			List<ConfigurationLine>? sectionLines = null;
			var sectionName = "";

            for (int i = 0; i < lines.Count; i++)
            {
				var line = lines[i].Trim();
				if (string.IsNullOrWhiteSpace(line))
					continue;

				if (line.StartsWith('[') && line.EndsWith(']'))
				{
					if (sectionLines is not null)
						sections.Add(new ConfigurationSection(sectionName, sectionLines));

					sectionLines = [];
					sectionName = line[1..^1];
					continue;
				}

				if (sectionLines is null)
					throw new ArgumentException("Configuration lines cannot be before first section declaration", nameof(configurationCell));

				sectionLines.Add(new ConfigurationLine(line));
            }

			if (sectionLines is not null)
				sections.Add(new ConfigurationSection(sectionName, sectionLines));

			return new DocumentConfiguration(sections);
        }
	}
}
