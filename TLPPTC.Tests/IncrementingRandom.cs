using System;

namespace TLPPTC.Tests
{
	public class IncrementingRandom : IRandom
	{
		private int _incrementingValue;

		public int Range(int min, int max)
		{
			return _incrementingValue++ % max;
		}

		public float Range(float min, float max)
		{
			throw new NotImplementedException();
		}
	}
}