using System;
using FluentAssertions;
using Xunit;

namespace TLPPTC.Tests
{
	public class TileAlignerTests
	{
		private readonly TileAligner _sut;
		private readonly TestConnectionsFinder _connectionsFinder;
		private readonly SetRandom _setRandom;
		private readonly TwoDimensionalCollection<TileInstance> _placedTiles;
		private readonly TestEdgeConnections _connectionRequirements;

		public TileAlignerTests()
		{
			_connectionsFinder = new TestConnectionsFinder();
			_setRandom = new SetRandom();
			_placedTiles = new TwoDimensionalCollection<TileInstance>();
			_connectionRequirements = new TestEdgeConnections();
			_sut = new TileAligner(_connectionsFinder, _connectionRequirements, _setRandom);
		}

		[Fact]
		public void Returns_tile_from_set_when_aligning()
		{
			var expectedTile = new object();
			_connectionsFinder.SetConnectionSet(expectedTile, new ConnectionSet("", 0));
			_sut.SetTiles(expectedTile.AsList());

			var actualTile = _sut.GetAlignedTile(0, 0);

			actualTile.Tile.Should().Be(expectedTile);
		}

		[Fact]
		public void Returns_tiles_that_fit_with_previous_tiles()
		{
			var tile1 = new { Name = "tile1" };
			var tile2 = new { Name = "tile2" };
			_connectionsFinder.SetConnectionSet(tile1, new ConnectionSet("00", 1));
			_connectionsFinder.SetConnectionSet(tile2, new ConnectionSet("11", 0));
			_sut.SetTiles(new[] { tile1, tile2 });

			_connectionRequirements.SetRequiredConnection("00", 1);
			var placedTile1 = _sut.GetAlignedTile(0, 0);
			_connectionRequirements.SetRequiredConnection("11", 3);
			var placedTile2 = _sut.GetAlignedTile(0, 1);

			placedTile1.Tile.Should().Be(tile1);
			placedTile2.Tile.Should().Be(tile2);

			placedTile1.Rotation.Should().Be(2);
			placedTile2.Rotation.Should().Be(3);
		}

		[Fact]
		public void Selects_single_tile_from_multiple_options_using_random()
		{
			var tile1 = new { Name = "tile1" };
			var tile2 = new { Name = "tile2" };
			_connectionsFinder.SetConnectionSet(tile1, new ConnectionSet("00110100", 0));
			_connectionsFinder.SetConnectionSet(tile1, new ConnectionSet("11", 3));
			_connectionsFinder.SetConnectionSet(tile2, new ConnectionSet("00110100", 0));
			_connectionsFinder.SetConnectionSet(tile2, new ConnectionSet("11", 2));
			_sut.SetTiles(new[] { tile1, tile2 });

			_setRandom.Value = 1;

			var placedTile1 = _sut.GetAlignedTile(0, 0);
			var placedTile2 = _sut.GetAlignedTile(1, 0);

			placedTile1.Tile.Should().Be(tile2);
			placedTile2.Tile.Should().Be(tile2);
		}

		[Fact]
		public void Throws_exception_when_tile_with_matching_connections_does_not_exist()
		{
			_connectionRequirements.SetRequiredConnection("11", 0);

			Action invalidSelect = () => _sut.GetAlignedTile(0, 0);
			invalidSelect.ShouldThrow<NoTileWithConnections>();
		}

		[Fact]
		public void Sets_rotation_and_complete_connections_on_aligned_tile()
		{
			const string expectedConnections = "00110100";
			const int expectedRotation = 1;

			var tile = new { Name = "tile" };
			_connectionsFinder.SetCompleteTileConnections(tile, expectedConnections);
			_connectionsFinder.SetConnectionSet(tile, new ConnectionSet("", expectedRotation));
			_sut.SetTiles(tile.AsList());

			var tileInstance = _sut.GetAlignedTile(0, 0);
			tileInstance.AllConnections.Should().Be(expectedConnections);
			tileInstance.Rotation.Should().Be(expectedRotation);
		}
	}

	public class TestEdgeConnections : IEdgeConnections
	{
		private ConnectionSet _connectionSet;

		public void SetRequiredConnection(string connections, int rotation)
		{
			_connectionSet = new ConnectionSet(connections, rotation);
		}

		public ConnectionSet GetEdgeConnections(int x, int y)
		{
			return _connectionSet ?? new ConnectionSet("", 0);
		}
	}
}
