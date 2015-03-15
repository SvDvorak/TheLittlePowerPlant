using System;
using System.Collections.Generic;

namespace TLPPTC.Tests
{
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
			var placedTile = _tileQueue.Dequeue();
			if (placedTile == null)
			{
				throw new NoTileWithConnections("");
			}
			return placedTile;
		}
	}
}