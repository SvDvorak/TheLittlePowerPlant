using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FillMachineTypeList : MonoBehaviour
{
    public ItemList ItemList;

	// Use this for initialization
	void Start ()
	{
	    ItemList.Items = new List<object>()
	        {
	            new MachineType("Turbine"),
                new MachineType("Coal"),
                new MachineType("Nuclear"),
                new MachineType("Wind"),
	        };
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public class MachineType
{
    private string _name;

    public MachineType(string name)
    {
        _name = name;
    }
}
