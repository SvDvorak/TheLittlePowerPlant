using FluentAssertions;
using UnityEngine;
using Xunit;

namespace TLPPTC.Tests
{
	public class CityGenerationTests
	{
		private readonly CityGeneration _sut;
		private readonly TwoDimensionalCollection<PlacedTile> _collection;
		private readonly TestBlockFactory _testBlockFactory;
		private readonly TestTileSelector _tileSelector;

		public CityGenerationTests()
		{
			_testBlockFactory = new TestBlockFactory();
			_tileSelector = new TestTileSelector();

			_collection = new TwoDimensionalCollection<PlacedTile>();
			_sut = new CityGeneration(_testBlockFactory, _tileSelector, _collection)
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
				new PlacedTile(new object(), "1234", 0),
				new PlacedTile(new object(), "5678", 1),
				new PlacedTile(new object(), "9012", 1),
				new PlacedTile(new object(), "3456", 3)
			};
			_tileSelector.SetSelectedTiles(selectTiles);
			_sut.Generate();

			_testBlockFactory.CreatedTiles.Count.Should().Be(4);
			var block1 = _testBlockFactory.CreatedTiles[0];
			var block2 = _testBlockFactory.CreatedTiles[1];
			var block3 = _testBlockFactory.CreatedTiles[2];
			var block4 = _testBlockFactory.CreatedTiles[3];

			block1.Position.Should().Be(new Vector3(0, 0, 0));
			block2.Position.Should().Be(new Vector3(0, 0, 2));
			block3.Position.Should().Be(new Vector3(2, 0, 0));
			block4.Position.Should().Be(new Vector3(2, 0, 2));

			block1.Rotation.Should().Be(new Vector3(0, 0, 0));
			block2.Rotation.Should().Be(new Vector3(0, 90, 0));
			block3.Rotation.Should().Be(new Vector3(0, 90, 0));
			block4.Rotation.Should().Be(new Vector3(0, 270, 0));
		}

		[Fact]
		public void Places_selected_tiles_into_collection()
		{
			var tile = new PlacedTile(new object(), "", 0);
			_tileSelector.SetSelectedTiles(new[] { tile, tile, tile, tile });
			_sut.Generate();

			_collection[0, 0].Should().NotBeNull();
			_collection[1, 0].Should().NotBeNull();
			_collection[0, 1].Should().NotBeNull();
			_collection[1, 1].Should().NotBeNull();
		}
	}
}