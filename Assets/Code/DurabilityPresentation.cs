using UnityEngine;
using System.Collections;
using Assets.Code;

public class DurabilityPresentation : MonoBehaviour
{
    private Turbine _turbine;
    private DurabilityData _durabilityData;

    void Start ()
	{
	    _turbine = gameObject.GetDataContext<Turbine>();
	    var newDataContext = gameObject.AddComponent<DataContext>();
	    _durabilityData = new DurabilityData();
	    newDataContext.Data = _durabilityData;
	}

	void Update ()
	{
	    _durabilityData.DurabilityInPercent = _turbine.Durability*100;
	}

    public class DurabilityData
    {
        public float DurabilityInPercent { get; set; }
    }
}
