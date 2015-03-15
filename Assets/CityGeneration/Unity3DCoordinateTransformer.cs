using UnityEngine;

public class Unity3DCoordinateTransformer : ICoordinateTransformer
{
	private Vector3 _origo;

	public void SetOrigo(Vector3 origo)
	{
		_origo = origo;
	}

	public Vector3 Transform(Vector3 vector)
	{
		return new Vector3(-vector.x, 0, vector.y) + _origo;
	}
}