using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickScript : MonoBehaviour
{
    public Action<ClickScript> ItemClicked;
    private Image _panelImage;

    // Use this for initialization
	void Start ()
	{
	    _panelImage = gameObject.GetComponent<Image>();
	}

    // Update is called once per frame
	void Update ()
    {
	
	}

    void OnMouseDown()
    {
        if(ItemClicked != null)
        {
            ItemClicked(this);
        }
    }

    public void Unselect()
    {
        
    }

    public void Select()
    {
        _panelImage.color = new Color(1f, 0f, 0f);
    }
}
