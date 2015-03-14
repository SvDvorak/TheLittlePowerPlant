using System.Collections.Generic;
using System.Linq;

public class NonStupidLookup<TKey, TValue>
{
	private readonly Dictionary<TKey, List<TValue>> _lookup;
	private readonly List<TValue> _emptyList;

	public NonStupidLookup()
	{
		_lookup = new Dictionary<TKey, List<TValue>>();
		_emptyList = new List<TValue>();
	}

	public IEnumerable<TValue> this[TKey key]
	{
		get { return _lookup.ContainsKey(key) ? _lookup[key] : _emptyList; }
	}

	public IEnumerable<TValue> GetKeyGroupByIndex(int index)
	{
		return _lookup.Values.Skip(index).First();
	} 

	public void Append(TKey key, IEnumerable<TValue> values)
	{
		if (_lookup.ContainsKey(key))
		{
			_lookup[key].AddRange(values);
		}
		else
		{
			_lookup[key] = values.ToList();
		}
	}

	public void Append(TKey key, TValue value)
	{
		if (_lookup.ContainsKey(key))
		{
			_lookup[key].Add(value);
		}
		else
		{
			_lookup[key] = value.AsList();
		}
	}

	public bool HasKey(TKey key)
	{
		return _lookup.ContainsKey(key);
	}
}