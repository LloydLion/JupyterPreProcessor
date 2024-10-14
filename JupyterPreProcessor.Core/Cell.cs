using OneOf;
using System.Diagnostics.CodeAnalysis;

namespace JupyterPreProcessor.Core
{
	public class Cell
	{
		public const char TagPrefix = '@';
		public const char ModPrefix = '.';


		private Cell(IReadOnlyList<string?> lines, IReadOnlyCollection<ModEntry> mods, IReadOnlyDictionary<int, TagEntry> tags)
		{
			Lines = lines;
			Mods = mods;
			Tags = tags;
		}


		public IReadOnlyList<string?> Lines { get; }

		public IReadOnlyCollection<ModEntry> Mods { get; }

		public IReadOnlyDictionary<int, TagEntry> Tags { get; }


		public IEnumerable<OneOf<string, TagEntry>> Enumerate()
		{
			int i = -1;
			foreach (var item in Lines)
			{
				i++;
				if (item is null)
					yield return Tags[i];
				else
					yield return item;
			}
		}

		public Cell ReplaceTags(IEnumerable<KeyValuePair<TagEntry, string>> replacements)
		{
			var wrappedLines = Lines.ToList();
			var tags = Tags.ToDictionary();

			foreach (var replacement in replacements)
			{
				var lineIndex = replacement.Key.LineIndex;
				wrappedLines[lineIndex] = replacement.Value.ReplaceLineEndings("\n");
				tags.Remove(lineIndex);
			}

			var lines = wrappedLines.SelectMany(s => ((string?[]?)s?.Split('\n')) ?? [null]).ToArray();

			var orderedTagsEnumerator = tags.OrderBy(s => s.Key).GetEnumerator();
			var newTags = new Dictionary<int, TagEntry>();

			for (int i = 0;	i < lines.Length; i++)
			{
				if (lines[i] is null)
				{
					orderedTagsEnumerator.MoveNext();
					var oldTag = orderedTagsEnumerator.Current.Value;
					newTags.Add(i, new TagEntry(i, oldTag.Tag, oldTag.Arguments));
				}
			}

			return new Cell(lines, Mods, newTags);
		}

		public static Cell Parse(IEnumerable<string> cell)
		{
			cell = cell.SelectMany(s => s.Split("\n"));

			var mods = new List<ModEntry>();
			while (cell.Any() && cell.First().StartsWith('.'))
			{
				var rawMod = cell.First();

				var split = rawMod[1..].Split(' ');
				var mod = split[0];
				var arguments = string.Join(' ', split[1..]);

				mods.Add(new ModEntry(mod, arguments));
				cell = cell.Skip(1);
			}

			var lines = new List<string?>();
			var tags = new Dictionary<int, TagEntry>();

			int i = -1;
			foreach (var item in cell)
			{
				i++;

				if (item.StartsWith(TagPrefix))
				{
					var split = item[1..].Split(' ');
					var tag = split[0];
					var arguments = string.Join(' ', split[1..]);

					tags.Add(i, new TagEntry(i, tag, arguments));
					lines.Add(null);
				}
				else
				{
					lines.Add(item);
				}
			}

			return new Cell(lines, mods, tags);
		}

		public bool HasMod(string modName, [NotNullWhen(true)] out string? arguments)
		{
			var modEntry = Mods.Where(s => s.Mod == modName).SingleOrDefault();
			if (modEntry == default)
			{
				arguments = null;
				return false;
			}
			else
			{
				arguments = modEntry.Arguments;
				return true;
			}
		}

		public bool HasMod(string modName) => HasMod(modName, out _);
	}
}
