using FluentAssertions;
using UnityEngine;
using Xunit;

namespace TLPPTC.Tests
{
	public class CityGenerationTests
	{
		private readonly CityGeneration _sut;
		private readonly TwoDimensionalCollection<TileInstance> _collection;
		private readonly TestBlockFactory _testBlockFactory;
		private readonly TestTileAligner _tileAligner;
		private readonly DoNothingLogger _logger;
		private readonly TestCoordinateTransformer _coordinateTransformer;

		public CityGenerationTests()
		{
			_testBlockFactory = new TestBlockFactory();
			_tileAligner = new TestTileAligner();

			_collection = new TwoDimensionalCollection<TileInstance>();
			_logger = new DoNothingLogger();
			_coordinateTransformer = new TestCoordinateTransformer();
			_sut = new CityGeneration(_testBlockFactory, _tileAligner, _collection, _logger, _coordinateTransformer)
			{
				NrOfTiles = 2,
				TileDimension = 2
			};
		}

		[Fact]
		public void Generates_tiles_according_to_size()
		{
			var selectTiles = new[]
			{
				new TileInstance(new object(), "1234", 0),
				new TileInstance(new object(), "5678", 1),
				new TileInstance(new object(), "9012", 1),
				new TileInstance(new object(), "3456", 3)
			};
			_tileAligner.SetAlignedTiles(selectTiles);
			_sut.Generate();

			_testBlockFactory.CreatedTiles.Count.Should().Be(4);
			var block1 = _testBlockFactory.CreatedTiles[0];
			var block2 = _testBlockFactory.CreatedTiles[1];
			var block3 = _testBlockFactory.CreatedTiles[2];
			var block4 = _testBlockFactory.CreatedTiles[3];

			block1.Position.Should().Be(new Vector3(0, 0, 0));
			block2.Position.Should().Be(new Vector3(2, 0, 0));
			block3.Position.Should().Be(new Vector3(0, 2, 0));
			block4.Position.Should().Be(new Vector3(2, 2, 0));

			block1.Rotation.Should().Be(new Vector3(0, 0, 0));
			block2.Rotation.Should().Be(new Vector3(0, 90, 0));
			block3.Rotation.Should().Be(new Vector3(0, 90, 0));
			block4.Rotation.Should().Be(new Vector3(0, 270, 0));
		}

		[Fact]
		public void Places_selected_tiles_into_collection()
		{
			var tile = new TileInstance(new object(), "", 0);
			_tileAligner.SetAlignedTiles(new[] { tile, tile, tile, tile });
			_sut.Generate();

			_collection[0, 0].Should().NotBeNull();
			_collection[1, 0].Should().NotBeNull();
			_collection[0, 1].Should().NotBeNull();
			_collection[1, 1].Should().NotBeNull();
		}

		[Fact]
		public void Logs_that_select_cant_find_tile_and_continues()
		{
			var tile = new TileInstance(new object(), "", 0);
			_tileAligner.SetAlignedTiles(new[] { tile, null, tile, tile });
			_sut.Generate();

			_logger.Warnings.Should().HaveCount(1);

			_collection[0, 0].Should().NotBeNull();
			_collection[1, 0].Should().BeNull();
			_collection[0, 1].Should().NotBeNull();
			_collection[1, 1].Should().NotBeNull();
		}

		[Fact]
		public void Transforms_coordinates_before_sending_to_block_factory()
		{
			_sut.NrOfTiles = 1;
			_tileAligner.SetAlignedTiles(new[] { new TileInstance(new object(), "", 0) });
			_coordinateTransformer.SetTransformation(x => new Vector3(1, 0, 0));

			_sut.Generate();

			_testBlockFactory.CreatedTiles[0].Position.Should().Be(new Vector3(1, 0, 0));
		}
	}
}