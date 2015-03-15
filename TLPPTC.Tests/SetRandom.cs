using System;

namespace TLPPTC.Tests
{
	public class SetRandom : IRandom
	{
		public int Value { get; set; }

		public int Range(int min, int max)
		{
			return Value;
		}

		public float Range(float min, float max)
		{
			throw new NotImplementedException();
		}
	}
}