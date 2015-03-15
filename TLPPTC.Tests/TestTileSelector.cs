using System;
using System.Collections.Generic;

namespace TLPPTC.Tests
{
	public class TestTileSelector : ITileSelector
	{
		private Queue<TileInstance> _tileQueue = new Queue<TileInstance>();

		public void SetTiles(IEnumerable<object> tiles)
		{
			throw new NotImplementedException();
		}

		public void SetSelectedTiles(IEnumerable<TileInstance> tiles)
		{
			_tileQueue = new Queue<TileInstance>(tiles);
		}

		public TileInstance Select(int x, int y)
		{
			var placedTile = _tileQueue.Dequeue();
			if (placedTile == null)
			{
				throw new NoTileWithConnections("");
			}
			return placedTile;
		}
	}
}