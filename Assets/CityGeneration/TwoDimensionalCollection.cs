using System;
using System.Collections.Generic;

public class TwoDimensionalCollection<TValue> where TValue : class
{
	private readonly Dictionary<int, Dictionary<int, TValue>> _collection;

	public TwoDimensionalCollection()
	{
		_collection = new Dictionary<int, Dictionary<int, TValue>>();
	}

	public TValue this[int x, int y]
	{
		get
		{
			if (!_collection.ContainsKey(x) || !_collection[x].ContainsKey(y))
			{
				return null;
			}

			return _collection[x][y];
		}
		set
		{
			if (!_collection.ContainsKey(x))
			{
				_collection[x] = new Dictionary<int, TValue>();
			}

			_collection[x][y] = value;
		}
	}
}