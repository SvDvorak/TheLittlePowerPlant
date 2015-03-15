using System;
using UnityEngine;

namespace TLPPTC.Tests
{
	public class TestCoordinateTransformer : ICoordinateTransformer
	{
		private Func<Vector3, Vector3> _transformFunc = vector => vector;

		public void SetTransformation(Func<Vector3, Vector3> transformFunc)
		{
			_transformFunc = transformFunc;
		}

		public Vector3 Transform(Vector3 vector)
		{
			return _transformFunc(vector);
		}
	}
}