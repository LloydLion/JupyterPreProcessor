namespace JupyterPreProcessor.Core.Plugins
{
	public class Plugin
	{
		private readonly List<string> _resolvableTags = [];
		private readonly List<string> _handledMods = [];
		private readonly List<string> _usedTemplates = [];
		private string? _name = null;
		private bool _initialized = false;


		public Plugin(IPluginCore pluginCore)
		{
			Core = pluginCore;
		}


		public void Initialize()
		{
			Core.Build(new Builder(this));
			if (_name is null)
				throw new Exception($"Plugin core [{Core.GetType().FullName}] does not set plugin's name");
			_initialized = true;
		}


		public string Name => IfInitialized(_name);

		public IReadOnlyCollection<string> ResolvableTags => IfInitialized(_resolvableTags);

		public IReadOnlyCollection<string> HandledMods => IfInitialized(_handledMods);

		public IReadOnlyCollection<string> UsedTemplates => IfInitialized(_usedTemplates);

		public IPluginCore Core { get; }


		private TValue IfInitialized<TValue>(TValue? value) =>
			_initialized ? value! : throw new InvalidOperationException("Plugin has not initialized");


		private class Builder : IPluginBuilder
		{
			private readonly Plugin _plugin;


			public Builder(Plugin plugin)
			{
				_plugin = plugin;
			}


			public IPluginBuilder Handles(string modId)
			{
				_plugin._handledMods.Add(modId);
				return this;
			}

			public IPluginBuilder Resolves(string tagId)
			{
				_plugin._resolvableTags.Add(tagId);
				return this;
			}

			public IPluginBuilder Uses(string templateId)
			{
				_plugin._usedTemplates.Add(templateId);
				return this;
			}

			public IPluginBuilder WithName(string name)
			{
				_plugin._name = name;
				return this;
			}
		}
	}
}
