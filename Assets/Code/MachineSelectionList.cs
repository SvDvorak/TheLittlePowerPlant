using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineSelectionList : MonoBehaviour
{
    public Action<Turbine> MachineSelected;
    public ItemList ItemList;

	// Use this for initialization
	void Start ()
	{
	    ItemList.Items = new List<object>()
	        {
	            new Turbine() { Name = "Turbine" },
                new Turbine() { Name = "Coal" },
                new Turbine() { Name = "Nuclear" },
                new Turbine() { Name = "Wind" },
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
            MachineSelected((Turbine) ItemList.Selected);
        }
    }
}