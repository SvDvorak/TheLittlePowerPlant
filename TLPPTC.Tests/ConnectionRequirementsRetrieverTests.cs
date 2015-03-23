using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TLPPTC.Tests
{
	public class ConnectionRequirementsRetrieverTests
	{
		private readonly ConnectionRequirementsRetriever _sut;
		private TwoDimensionalCollection<TileInstance> _tiles;

		public ConnectionRequirementsRetrieverTests()
		{
			_tiles = new TwoDimensionalCollection<TileInstance>();
			_sut = new ConnectionRequirementsRetriever(_tiles);
		}

		[Fact]
		public void No_connection_required_with_no_tiles_around()
		{
			var requiredConnection = _sut.GetRequiredConnection(0, 0);

			requiredConnection.Connections.Should().BeEmpty();
		}

		[Fact]
		public void Retrieves_connections_for_single_tile_when_in_collection()
		{
			_tiles[0, 1] = new TileInstance("tile", "11000000", 0);
			var requiredConnection = _sut.GetRequiredConnection(0, 0);

			requiredConnection.Connections.Should().Be("11");
			requiredConnection.Rotation.Should().Be(2);
		}

		[Fact]
		public void Multiple_tile_connections_are_chained_with_rotation_of_first_tile_in_connections()
		{
			_tiles[-1, 0] = new TileInstance("tile", "00010000", 0);
			_tiles[0, -1] = new TileInstance("tile", "00001000", 0);
			var requiredConnection = _sut.GetRequiredConnection(0, 0);

			requiredConnection.Connections.Should().Be("1001");
			requiredConnection.Rotation.Should().Be(3);
		}

		[Fact]
		public void Surrounding_tile_connections_are_read_with_correct_rotation()
		{
			_tiles[-1, 0] = new TileInstance("tile", "10000000", 1);
			var requiredConnection = _sut.GetRequiredConnection(0, 0);

			requiredConnection.Connections.Should().Be("01");
			requiredConnection.Rotation.Should().Be(3);
		}

		[Fact]
		public void Stops_looking_for_surrounding_when_gone_through_all_directions()
		{
			_tiles[-1, 0] = new TileInstance("tile", "00110000", 0);
			_tiles[0, -1] = new TileInstance("tile", "00000100", 0);
			_tiles[1, 0] = new TileInstance("tile", "00000000", 0);
			_tiles[0, 1] = new TileInstance("tile", "10000000", 0);
			var requiredConnection = _sut.GetRequiredConnection(0, 0);

			requiredConnection.Connections.Should().Be("10000111");
			requiredConnection.Rotation.Should().Be(0);
		}
	}
}
