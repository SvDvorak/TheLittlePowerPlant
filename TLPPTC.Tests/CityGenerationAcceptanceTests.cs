using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using UnityEngine;
using Xunit;

namespace TLPPTC.Tests
{
	public class CityGenerationAcceptanceTests
	{
		[Fact]
		public void Generates_tiles_according_to_size()
		{
			var random = new TestRandom();
			var testBlockFactory = new TestBlockFactory();

			var sut = new CityGeneration(random, testBlockFactory)
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
	}

	public class TestRandom : IRandom
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

	public class TestBlockFactory : IBlockFactory
	{
		public List<BlockInfo> CreatedBlocks = new List<BlockInfo>();
		public void Create(Vector3 position, Vector3 rotation)
		{
			CreatedBlocks.Add(new BlockInfo() { Position = position, Rotation = rotation });
		}

		public class BlockInfo
		{
			public Vector3 Position;
			public Vector3 Rotation;
		}
	}
}
