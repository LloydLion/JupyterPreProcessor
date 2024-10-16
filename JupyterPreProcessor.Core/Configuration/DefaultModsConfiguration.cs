using JupyterPreProcessor.Core.Segments;

namespace JupyterPreProcessor.Core.Configuration
{
	public class DefaultModsConfiguration
	{
		public DefaultModsConfiguration(ModsSegment beforeSegment, ModsSegment afterSegment)
		{
			BeforeSegment = beforeSegment;
			AfterSegment = afterSegment;
		}


		public ModsSegment BeforeSegment { get; }

		public ModsSegment AfterSegment { get; }
	}
}
