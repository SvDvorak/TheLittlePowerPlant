using System.Collections.Generic;

namespace TLPPTC.Tests
{
	public class DoNothingLogger : ILogger
	{
		public List<string> Warnings { get; private set; }

		public DoNothingLogger()
		{
			Warnings = new List<string>();
		}

		public void LogWarning(string message)
		{
			Warnings.Add(message);
		}
	}
}