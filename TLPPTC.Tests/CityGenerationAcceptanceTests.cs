using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using UnityEngine;
using Xunit;

namespace TLPPTC.Tests
{
	public class CityGenerationAcceptanceTests
	{
		[Fact]
		public void Adds_blocks_that_fit_with_surrounding_ones()
		{
			var random = new SetRandom();
			var testBlockFactory = new TestBlockFactory();
			var exitRetriever = new TestExitRetriever();

			var blockTemplate1 = new object();
			var blockTemplate2 = new object();

			var sut = new CityGeneration(random, testBlockFactory, new TileSelector(new ConnectionsFinder(exitRetriever)))
			{
				NrOfTiles = 2,
				TileDimension = 2,
			};

			sut.SetTiles(new List<object>()
				{
					blockTemplate1,
					blockTemplate2
				});

			random.Value = 0;

			sut.Generate();

			var block1 = testBlockFactory.CreatedBlocks[0];
			var block2 = testBlockFactory.CreatedBlocks[1];
			var block3 = testBlockFactory.CreatedBlocks[2];
			var block4 = testBlockFactory.CreatedBlocks[3];

			block1.Block.Should().Be(blockTemplate1);
			block2.Block.Should().Be(blockTemplate1);
			block3.Block.Should().Be(blockTemplate2);
			block4.Block.Should().Be(blockTemplate2);

			block1.Position.Should().Be(new Vector3(0, 0, 0));
			block2.Position.Should().Be(new Vector3(0, 0, 2));
			block3.Position.Should().Be(new Vector3(2, 0, 0));
			block4.Position.Should().Be(new Vector3(2, 0, 2));

			block1.Rotation.Should().Be(new Vector3(0, 0, 0));
			block2.Rotation.Should().Be(new Vector3(0, 90, 0));
			block3.Rotation.Should().Be(new Vector3(0, 180, 0));
			block4.Rotation.Should().Be(new Vector3(0, 270, 0));
		}
	}

	public class CityGenerationTests
	{
		[Fact]
		public void Generates_tiles_according_to_size()
		{
			var random = new IncrementingRandom();
			var testBlockFactory = new TestBlockFactory();
			var tileSelector = new TestTileSelector();

			var sut = new CityGeneration(random, testBlockFactory, tileSelector)
			{
				NrOfTiles = 2,
				TileDimension = 2
			};

			sut.Generate();

			testBlockFactory.CreatedBlocks.Count.Should().Be(4);
			var block1 = testBlockFactory.CreatedBlocks[0];
			var block2 = testBlockFactory.CreatedBlocks[1];
			var block3 = testBlockFactory.CreatedBlocks[2];
			var block4 = testBlockFactory.CreatedBlocks[3];

			block1.Position.Should().Be(new Vector3(0, 0, 0));
			block2.Position.Should().Be(new Vector3(0, 0, 2));
			block3.Position.Should().Be(new Vector3(2, 0, 0));
			block4.Position.Should().Be(new Vector3(2, 0, 2));

			block1.Rotation.Should().Be(new Vector3(0, 0, 0));
			block2.Rotation.Should().Be(new Vector3(0, 90, 0));
			block3.Rotation.Should().Be(new Vector3(0, 180, 0));
			block4.Rotation.Should().Be(new Vector3(0, 270, 0));
		}

		[Fact]
		public void Selects_tile_using_selector_with_previously_set_tiles()
		{
			var random = new IncrementingRandom();
			var testBlockFactory = new TestBlockFactory();
			var tileSelector = new TestTileSelector();

			var sut = new CityGeneration(random, testBlockFactory, tileSelector)
			{
				NrOfTiles = 1,
				TileDimension = 1
			};

			var tile1 = new object();
			var tile2 = new object();
			sut.SetTiles(new List<object> { tile1, tile2 });

			sut.Generate();

			testBlockFactory.CreatedBlocks[0].Block.Should().Be(tile2);
		}
	}

	public class TestTileSelector : ITileSelector
	{
		private IEnumerable<object> _tiles = new List<object>();

		public void SetTiles(IEnumerable<object> tiles)
		{
			_tiles = tiles;
		}

		public object Select(int x, int z)
		{
			return _tiles.LastOrDefault();
		}
	}

	public class IncrementingRandom : IRandom
	{
		private int IncrementingValue;

		public int Range(int min, int max)
		{
			return IncrementingValue++;
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
		public List<BlockInfo> CreatedBlocks = new List<BlockInfo>();
		public object Create(object block, Vector3 position, Vector3 rotation)
		{
			var blockInfo = new BlockInfo() { Block = block, Position = position, Rotation = rotation };
			CreatedBlocks.Add(blockInfo);
			return blockInfo;
		}

		public class BlockInfo
		{
			public Vector3 Position;
			public Vector3 Rotation;
			public object Block;
		}
	}
}
