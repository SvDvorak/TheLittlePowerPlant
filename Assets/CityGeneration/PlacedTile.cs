public class PlacedTile
{
	public object Tile { get; private set; }
	public string AllConnections { get; private set; }
	public int Rotation { get; private set; }

	public PlacedTile(object tile, string allConnections, int rotation)
	{
		Tile = tile;
		AllConnections = allConnections;
		Rotation = rotation;
	}
}