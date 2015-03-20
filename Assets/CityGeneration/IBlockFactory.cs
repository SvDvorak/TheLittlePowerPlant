using UnityEngine;

public interface IBlockFactory
{
	object Create(object block, Vector3 position, Vector3 rotation);
	void Destroy(Vector3 position);
}