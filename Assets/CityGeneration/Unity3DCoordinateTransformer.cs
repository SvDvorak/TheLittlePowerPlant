using UnityEngine;

public class Unity3DCoordinateTransformer : ICoordinateTransformer
{
	private Vector3 _origo = Vector3.zero;

	public void SetOrigo(Vector3 origo)
	{
		_origo = origo;
	}

	public Vector3 Transform(Vector3 vector)
	{
		return new Vector3(-vector.x, vector.z, vector.y) + _origo;
	}
}