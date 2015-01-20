using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineSelectionList : MonoBehaviour
{
    public Action<MachineType> MachineSelected;
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
	void Update ()
    {
	
	}

    public void Build()
    {
        if (MachineSelected != null)
        {
            MachineSelected((MachineType) ItemList.Selected);
        }
    }
}

public class MachineType
{
    public string Name { get; set; }

    public MachineType(string name)
    {
        Name = name;
    }
}
