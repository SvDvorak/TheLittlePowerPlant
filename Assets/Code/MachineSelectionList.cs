using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineSelectionList : MonoBehaviour
{
    public Action<IMachineType> MachineSelected;
    public ItemList ItemList;

	// Use this for initialization
	void Start ()
	{
	    ItemList.Items = new List<object>()
	        {
                new Coal() { Name = "Coal", Cost = 10000 },
	            new Turbine() { Name = "Hydro", Cost = 20000 },
                new Nuclear() { Name = "Nuclear", Cost = 30000},
	        };
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void Build()
    {
        if (MachineSelected != null)
        {
            MachineSelected((IMachineType)ItemList.Selected);
        }
    }
}