using System;

namespace TLPPTC.Tests
{
	public class TestExitRetriever : IExitRetriever
	{
		private Func<string, object> _conditionFunc;

		public void SetExitCondition(Func<string, object> conditionFunc)
		{
			_conditionFunc = conditionFunc;
		}

		public object GetExits(object tile, string name)
		{
			return _conditionFunc(name);
		}
	}
}