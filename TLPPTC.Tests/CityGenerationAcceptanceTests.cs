using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using UnityEngine;
using Xunit;
using Object = System.Object;

namespace TLPPTC.Tests
{
	public class CityGenerationAcceptanceTests
	{
		[Fact]
		public void Adds_blocks_that_fit_with_surrounding_ones()
		{
			var random = new IncrementingRandom();
			var testBlockFactory = new TestBlockFactory();
			var exitRetriever = new TestExitRetriever();

			var tileTemplate1 = new {name = "Tile1"};
			var tileTemplate2 = new {name = "Tile2"};

			var twoDimensionalCollection = new TwoDimensionalCollection<PlacedTile>();
			var sut = new CityGeneration(testBlockFactory, new TileSelector(new ConnectionsFinder(exitRetriever), random, twoDimensionalCollection), twoDimensionalCollection)
			{
				NrOfTiles = 2,
				TileDimension = 2,
			};

			exitRetriever.SetExitCondition(name => name.Contains("_S_") ? new object() : null);

			sut.SetTiles(new List<object>()
				{
					tileTemplate1,
					tileTemplate2
				});

			sut.Generate();

			var block1 = testBlockFactory.CreatedTiles[0];
			var block2 = testBlockFactory.CreatedTiles[1];
			var block3 = testBlockFactory.CreatedTiles[2];
			var block4 = testBlockFactory.CreatedTiles[3];

			block1.Tile.Should().Be(tileTemplate2);
			block2.Tile.Should().Be(tileTemplate1);
			block3.Tile.Should().Be(tileTemplate2);
			block4.Tile.Should().Be(tileTemplate1);

			block1.Position.Should().Be(new Vector3(0, 0, 0));
			block2.Position.Should().Be(new Vector3(0, 0, 2));
			block3.Position.Should().Be(new Vector3(2, 0, 0));
			block4.Position.Should().Be(new Vector3(2, 0, 2));

			block1.Rotation.Should().Be(new Vector3(0, 0, 0));
			block2.Rotation.Should().Be(new Vector3(0, 180, 0));
			block3.Rotation.Should().Be(new Vector3(0, 270, 0));
			block4.Rotation.Should().Be(new Vector3(0, 270, 0));
		}
	}

	public class TestTileSelector : ITileSelector
	{
		private Queue<PlacedTile> _tileQueue = new Queue<PlacedTile>();

		public void SetTiles(IEnumerable<object> tiles)
		{
			throw new NotImplementedException();
		}

		public void SetSelectedTiles(IEnumerable<PlacedTile> tiles)
		{
			_tileQueue = new Queue<PlacedTile>(tiles);
		}

		public PlacedTile Select(int x, int y)
		{
			return _tileQueue.Dequeue();
		}
	}

	public class IncrementingRandom : IRandom
	{
		private int _incrementingValue;

		public int Range(int min, int max)
		{
			return _incrementingValue++ % max;
		}

		public float Range(float min, float max)
		{
			throw new NotImplementedException();
		}
	}

	public class SetRandom : IRandom
	{
		public int Value { get; set; }

		public int Range(int min, int max)
		{
			return Value;
		}

		public float Range(float min, float max)
		{
			throw new NotImplementedException();
		}
	}

	public class TestBlockFactory : IBlockFactory
	{
		public List<TileInfo> CreatedTiles = new List<TileInfo>();
		public object Create(object tile, Vector3 position, Vector3 rotation)
		{
			var blockInfo = new TileInfo() { Tile = tile, Position = position, Rotation = rotation };
			CreatedTiles.Add(blockInfo);
			return blockInfo;
		}

		public class TileInfo
		{
			public Vector3 Position;
			public Vector3 Rotation;
			public object Tile;
		}
	}
}
