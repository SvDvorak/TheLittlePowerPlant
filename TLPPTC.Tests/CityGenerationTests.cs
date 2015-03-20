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
		private readonly TileInstance _emptyTile = new TileInstance(new object(), "", 0);

		public CityGenerationTests()
		{
			_testBlockFactory = new TestBlockFactory();
			_tileAligner = new TestTileAligner();
			_tileAligner.SetAlignedTiles(new[] { _emptyTile });

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
		public void Generates_tiles_centered_according_to_size_using_Manhattan_distance()
		{
			var selectTiles = new[]
			{
				new TileInstance(new object(), "1234", 0),
				new TileInstance(new object(), "5678", 1),
				new TileInstance(new object(), "9012", 1),
				new TileInstance(new object(), "3456", 3),
				new TileInstance(new object(), "7890", 4)
			};
			_tileAligner.SetAlignedTiles(selectTiles);
			_sut.Generate(Vector3.zero);

			_testBlockFactory.CreatedTiles.Count.Should().Be(5);
			var block1 = _testBlockFactory.CreatedTiles[0];
			var block2 = _testBlockFactory.CreatedTiles[1];
			var block3 = _testBlockFactory.CreatedTiles[2];
			var block4 = _testBlockFactory.CreatedTiles[3];
			var block5 = _testBlockFactory.CreatedTiles[4];

			block1.Position.Should().Be(new Vector3(0, -2, 0));
			block2.Position.Should().Be(new Vector3(-2, 0, 0));
			block3.Position.Should().Be(new Vector3(0, 0, 0));
			block4.Position.Should().Be(new Vector3(2, 0, 0));
			block5.Position.Should().Be(new Vector3(0, 2, 0));

			block1.Rotation.Should().Be(new Vector3(0, 0, 0));
			block2.Rotation.Should().Be(new Vector3(0, 90, 0));
			block3.Rotation.Should().Be(new Vector3(0, 90, 0));
			block4.Rotation.Should().Be(new Vector3(0, 270, 0));
			block5.Rotation.Should().Be(new Vector3(0, 360, 0));
		}

		[Fact]
		public void Places_selected_tiles_into_collection()
		{
			_tileAligner.SetAlignedTiles(new[] { _emptyTile, _emptyTile, _emptyTile, _emptyTile, _emptyTile });

			_sut.Generate(new Vector3(_sut.TileDimension, _sut.TileDimension));

			_collection[1, 0].Should().NotBeNull();
			_collection[0, 1].Should().NotBeNull();
			_collection[1, 1].Should().NotBeNull();
			_collection[2, 1].Should().NotBeNull();
			_collection[1, 2].Should().NotBeNull();
		}

		[Fact]
		public void Logs_that_select_cant_find_tile_and_continues()
		{
			_tileAligner.SetAlignedTiles(new[] { _emptyTile, null, _emptyTile, _emptyTile, _emptyTile });
			_sut.Generate(Vector3.zero);

			_logger.Warnings.Should().HaveCount(1);

			_collection[0, -1].Should().NotBeNull();
			_collection[-1, 0].Should().BeNull();
			_collection[0, 0].Should().NotBeNull();
			_collection[1, 0].Should().NotBeNull();
			_collection[0, 1].Should().NotBeNull();
		}

		[Fact]
		public void Transforms_coordinates_before_sending_to_block_factory()
		{
			_sut.NrOfTiles = 1;
			_coordinateTransformer.SetTransformation(x => new Vector3(1, 0, 0));

			_sut.Generate(Vector3.zero);

			_testBlockFactory.CreatedTiles[0].Position.Should().Be(new Vector3(1, 0, 0));
		}

		[Fact]
		public void Places_tiles_according_to_center_point_and_removes_tiles_outside()
		{
			_sut.NrOfTiles = 1;
			_sut.TileDimension = 2;
			_tileAligner.SetAlignedTiles(new[] { _emptyTile, _emptyTile });

			_sut.Generate(new Vector3(0, 0, 0));
			_testBlockFactory.CreatedTiles.Should().HaveCount(1);
			_testBlockFactory.CreatedTiles[0].Position.Should().Be(new Vector3(0, 0, 0));

			_sut.Generate(new Vector3(3, 3, 0));

			_testBlockFactory.CreatedTiles.Should().HaveCount(1);
			_testBlockFactory.CreatedTiles[0].Position.Should().Be(new Vector3(2, 2, 0));
		}

		[Fact]
		public void Does_not_create_tile_on_existing_tile()
		{
			_sut.NrOfTiles = 1;
			_tileAligner.SetAlignedTiles(new[] { _emptyTile, _emptyTile });

			_sut.Generate(Vector3.zero);
			_sut.Generate(Vector3.zero);

			_testBlockFactory.CreatedTiles.Should().HaveCount(1);
		}
	}
}