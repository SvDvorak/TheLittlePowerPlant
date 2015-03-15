using System.Collections.Generic;

namespace TLPPTC.Tests
{
	public class TestConnectionsFinder : IConnectionsFinder
	{
		private readonly Dictionary<object, string> _completeConnections = new Dictionary<object, string>();
		private readonly NonStupidLookup<object, ConnectionSet> _connectionSets = new NonStupidLookup<object, ConnectionSet>();

		public void SetCompleteTileConnections(object tile, string connections)
		{
			_completeConnections.Add(tile, connections);
		}

		public string GetCompleteConnectionsOriented(object tile, int rotation)
		{
			var connections = "11111111";
			if (_completeConnections.ContainsKey(tile))
			{
				connections = _completeConnections[tile];
			}
			return connections;
		}

		public void SetConnectionSet(object tile, ConnectionSet connectionSet)
		{
			_connectionSets.Append(tile, connectionSet);
		}

		public IEnumerable<ConnectionSet> FindConnectionSets(object tile)
		{
			return _connectionSets[tile];
		}
	}
}