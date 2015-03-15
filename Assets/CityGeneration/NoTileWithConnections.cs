using System;

public class NoTileWithConnections : Exception
{
	public NoTileWithConnections(string connections) : base("No tile with following connections: " + connections)  { }
}