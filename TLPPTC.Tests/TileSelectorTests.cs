using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace TLPPTC.Tests
{
	public class TileSelectorTests
	{
		private readonly TileSelector _sut;
		private readonly TestConnectionsFinder _connectionsFinder;

		public TileSelectorTests()
		{
			_connectionsFinder = new TestConnectionsFinder();
			_sut = new TileSelector(_connectionsFinder);
		}

		[Fact]
		public void Returns_tile_from_set_when_selecting()
		{
			var expectedTile = new object();
			_connectionsFinder.SetConnectionSet(expectedTile, new ConnectionSet("00110100", 0));
			_sut.SetTiles(expectedTile.AsList());

			var actualTile = _sut.Select(0, 0);

			actualTile.Should().Be(expectedTile);
		}

		[Fact]
		public void Returns_tiles_that_fit_with_previous_tiles()
		{
			var tile1 = new { Name = "tile1" };
			var tile2 = new { Name = "tile2" };
			var tile3 = new { Name = "tile3" };
			_connectionsFinder.SetCompleteTileConnections(tile1, "00110100");
			_connectionsFinder.SetCompleteTileConnections(tile2, "00101100");
			_connectionsFinder.SetCompleteTileConnections(tile3, "00001100");
			_connectionsFinder.SetConnectionSet(tile1, new ConnectionSet("00110100", 0));
			_connectionsFinder.SetConnectionSet(tile1, new ConnectionSet("11", 3));
			_connectionsFinder.SetConnectionSet(tile2, new ConnectionSet("10", 3));
			_connectionsFinder.SetConnectionSet(tile3, new ConnectionSet("1100", 2));
			_sut.SetTiles(new[] { tile1, tile2, tile3 });

			var placedTile1 = _sut.Select(0, 0);
			var placedTile2 = _sut.Select(1, 0);
			var placedTile3 = _sut.Select(0, 1);
			var placedTile4 = _sut.Select(1, 1);

			placedTile1.Should().Be(tile1);
			placedTile2.Should().Be(tile1);
			placedTile3.Should().Be(tile2);
			placedTile4.Should().Be(tile3);
		}
	}

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
