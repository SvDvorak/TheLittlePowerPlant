using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TLPPTC.Tests
{
	public class TestBlockFactory : IBlockFactory
	{
		public List<TileInfo> CreatedTiles = new List<TileInfo>();
		public object Create(object tile, Vector3 position, Vector3 rotation)
		{
			var blockInfo = new TileInfo() { Tile = tile, Position = position, Rotation = rotation };
			CreatedTiles.Add(blockInfo);
			return blockInfo;
		}

		public void Destroy(Vector3 position)
		{
			var createdTile = CreatedTiles.Single(x => x.Position == position);
			CreatedTiles.Remove(createdTile);
		}

		public class TileInfo
		{
			public Vector3 Position;
			public Vector3 Rotation;
			public object Tile;
		}
	}
}