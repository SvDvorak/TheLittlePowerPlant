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
			var random = new IncrementingRandom();
			var testBlockFactory = new TestBlockFactory();
			var exitRetriever = new TestExitRetriever();

			var tileTemplate1 = new {name = "Tile1"};
			var tileTemplate2 = new {name = "Tile2"};

			var twoDimensionalCollection = new TwoDimensionalCollection<TileInstance>();
			var sut = new CityGeneration(testBlockFactory, new TileSelector(new ConnectionsFinder(exitRetriever), random, twoDimensionalCollection), twoDimensionalCollection, new DoNothingLogger(), new TestCoordinateTransformer())
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
			block2.Position.Should().Be(new Vector3(2, 0, 0));
			block3.Position.Should().Be(new Vector3(0, 2, 0));
			block4.Position.Should().Be(new Vector3(2, 2, 0));

			block1.Rotation.Should().Be(new Vector3(0, 0, 0));
			block2.Rotation.Should().Be(new Vector3(0, 0, 0));
			block3.Rotation.Should().Be(new Vector3(0, 180, 0));
			block4.Rotation.Should().Be(new Vector3(0, 180, 0));
		}
	}
}
