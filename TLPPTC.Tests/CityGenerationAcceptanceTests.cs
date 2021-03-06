﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using UnityEngine;
using Xunit;

namespace TLPPTC.Tests
{
	public class CityGenerationAcceptanceTests
	{
		private CityGeneration _sut;
		private TestExitRetriever _exitRetriever;
		private TestBlockFactory _testBlockFactory;

		public CityGenerationAcceptanceTests()
		{
			CreateSut(new IncrementingRandom());
		}

		private void CreateSut(IRandom random)
		{
			_testBlockFactory = new TestBlockFactory();
			_exitRetriever = new TestExitRetriever();

			var twoDimensionalCollection = new TwoDimensionalCollection<TileInstance>();
			_sut = new CityGeneration(_testBlockFactory,
				new TileAligner(
					new ConnectionsFinder(_exitRetriever),
					new EdgeConnections(twoDimensionalCollection),
					random),
				twoDimensionalCollection,
				new DoNothingLogger(), new TestCoordinateTransformer())
				{
					NrOfTiles = 2,
					TileDimension = 2,
				};
		}

		[Fact]
		public void Adds_blocks_around_center_point_that_fit_with_previous_ones()
		{
			var tileTemplate1 = new { name = "Tile1" };
			var tileTemplate2 = new { name = "Tile2" };

			_exitRetriever.SetExitCondition(name => name.Contains("_S_") ? new object() : null);

			_sut.SetTiles(new List<object>()
				{
					tileTemplate1,
					tileTemplate2
				});

			_sut.Generate(new Vector3(3, 3, 0));

			var block1 = _testBlockFactory.CreatedTiles[0];
			var block2 = _testBlockFactory.CreatedTiles[1];
			var block3 = _testBlockFactory.CreatedTiles[2];
			var block4 = _testBlockFactory.CreatedTiles[3];
			var block5 = _testBlockFactory.CreatedTiles[4];

			block1.Tile.Should().Be(tileTemplate2);
			block2.Tile.Should().Be(tileTemplate2);
			block3.Tile.Should().Be(tileTemplate1);
			block4.Tile.Should().Be(tileTemplate2);
			block5.Tile.Should().Be(tileTemplate1);

			block1.Position.Should().Be(new Vector3(2, 0, 0));
			block2.Position.Should().Be(new Vector3(0, 2, 0));
			block3.Position.Should().Be(new Vector3(2, 2, 0));
			block4.Position.Should().Be(new Vector3(4, 2, 0));
			block5.Position.Should().Be(new Vector3(2, 4, 0));

			block1.Rotation.Should().Be(new Vector3(0, 0, 0));
			block2.Rotation.Should().Be(new Vector3(0, 180, 0));
			block3.Rotation.Should().Be(new Vector3(0, 180, 0));
			block4.Rotation.Should().Be(new Vector3(0, 0, 0));
			block5.Rotation.Should().Be(new Vector3(0, 0, 0));
		}

		[Fact]
		public void Adds_new_tiles_and_removes_invalid_tiles_when_generating_at_new_point()
		{
			_exitRetriever.SetExitCondition(name => new object());

			_sut.SetTiles(new List<object>()
				{
					new { name = "Tile1" },
					new { name = "Tile2" }
				});

			_sut.Generate(new Vector3(0, 0, 0));
			_sut.Generate(new Vector3(2, 2, 0));

			_testBlockFactory.CreatedTiles.Should().HaveCount(5);
			var block1 = _testBlockFactory.CreatedTiles[0];
			var block2 = _testBlockFactory.CreatedTiles[1];
			var block3 = _testBlockFactory.CreatedTiles[2];
			var block4 = _testBlockFactory.CreatedTiles[3];
			var block5 = _testBlockFactory.CreatedTiles[4];

			block1.Position.Should().Be(new Vector3(2, 0, 0));
			block2.Position.Should().Be(new Vector3(0, 2, 0));
			block3.Position.Should().Be(new Vector3(2, 2, 0));
			block4.Position.Should().Be(new Vector3(4, 2, 0));
			block5.Position.Should().Be(new Vector3(2, 4, 0));
		}

		[Fact]
		public void Fits_blocks_when_center_point_has_moved()
		{
			var setRandom = new SetRandom { Value = 0 };
			CreateSut(setRandom);

			var tileTemplate1 = new { name = "Tile1" };

			_exitRetriever.SetExitCondition(name => name.Contains("_W_") ? new object() : null);

			_sut.SetTiles(tileTemplate1.AsList());

			_sut.Generate(new Vector3(0, 0, 0));
			_sut.Generate(new Vector3(-2, 0, 0));

			var leftBlock1 = _testBlockFactory.CreatedTiles.Single(x => x.Position == new Vector3(-2, 0, 0));
			var leftBlock2 = _testBlockFactory.CreatedTiles.Single(x => x.Position == new Vector3(-4, 0, 0));

			leftBlock1.Tile.Should().Be(tileTemplate1);
			leftBlock2.Tile.Should().Be(tileTemplate1);

			leftBlock1.Rotation.Should().Be(new Vector3(0, 0, 0));
			leftBlock2.Rotation.Should().Be(new Vector3(0, 180, 0));
		}
	}
}
