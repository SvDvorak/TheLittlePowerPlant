using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemList : MonoBehaviour
{
    public GameObject ItemPresentationTemplate;
    public GameObject ItemPanel;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private const int ItemPadding = 10;
    private ClickScript _selectedItemScript;

    private IEnumerable<object> _items;
    public IEnumerable<object> Items
    {
        get { return _items; }
        set
        {
            _items = value;
            CreatePresentationForItems();
        }
    }

    private void CreatePresentationForItems()
    {
        var panelRect = ItemPanel.GetComponent<RectTransform>();
        var contentHeight = panelRect.sizeDelta.y/2;
        foreach (var item in _items)
        {
            var itemPresentation = (GameObject)Instantiate(ItemPresentationTemplate);
            itemPresentation.GetComponent<ClickScript>().ItemClicked += OnItemClicked;
            itemPresentation.transform.SetParent(ItemPanel.transform, false);
            var itemHeight = itemPresentation.GetComponent<RectTransform>().sizeDelta.y;
            itemPresentation.transform.localPosition = new Vector3(0, contentHeight - itemHeight/2);
            contentHeight -= itemHeight + ItemPadding;
        }

        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, panelRect.sizeDelta.y/2 - contentHeight - ItemPadding);
    }

    private void OnItemClicked(ClickScript clickedScript)
    {
        _selectedItemScript.Unselect();
        clickedScript.Select();
        _selectedItemScript = clickedScript;
    }
}