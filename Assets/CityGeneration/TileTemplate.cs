public class TileTemplate
{
	public TileTemplate(object tile, int rotation)
	{
		Tile = tile;
		Rotation = rotation;
	}

	public object Tile { get; private set; }
	public int Rotation { get; private set; }
}