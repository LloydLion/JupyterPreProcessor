using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace JupyterPreProcessor.Core.Configuration
{
	public class KeyValueConfiguration : IReadOnlyDictionary<string, string>
	{
		private readonly Dictionary<string, string> _configuration;


		public KeyValueConfiguration(Dictionary<string, string> configuration)
		{
			_configuration = configuration;
		}


		public string this[string key] => ((IReadOnlyDictionary<string, string>)_configuration)[key];


		public IEnumerable<string> Keys => ((IReadOnlyDictionary<string, string>)_configuration).Keys;

		public IEnumerable<string> Values => ((IReadOnlyDictionary<string, string>)_configuration).Values;

		public int Count => ((IReadOnlyCollection<KeyValuePair<string, string>>)_configuration).Count;


		public bool ContainsKey(string key)
		{
			return ((IReadOnlyDictionary<string, string>)_configuration).ContainsKey(key);
		}

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<string, string>>)_configuration).GetEnumerator();
		}

		public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
		{
			return ((IReadOnlyDictionary<string, string>)_configuration).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_configuration).GetEnumerator();
		}
	}
}
