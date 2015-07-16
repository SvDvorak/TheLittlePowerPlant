using UnityEngine;
using System.Collections;
using Assets.Code;

public class DurabilityPresentation : MonoBehaviour
{
    private Hydro _hydro;
    private DurabilityData _durabilityData;

    void Start ()
	{
	    _hydro = gameObject.GetDataContext<Hydro>();
	    var newDataContext = gameObject.AddComponent<DataContext>();
	    _durabilityData = new DurabilityData();
	    newDataContext.Data = _durabilityData;
	}

	void Update ()
	{
	    _durabilityData.DurabilityInPercent = _hydro.Durability*100;
	}

    public class DurabilityData
    {
        public float DurabilityInPercent { get; set; }
    }
}
