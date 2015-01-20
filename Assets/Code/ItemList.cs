using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public object Selected
    {
        get
        {
            var selectedToggle = ItemPanel.GetComponent<ToggleGroup>().GetActive();
            return selectedToggle.GetComponent<MachineTypeData>().MachineType;
        }
    }

    private void CreatePresentationForItems()
    {
        var panelRect = ItemPanel.GetComponent<RectTransform>();
        var panelToggleGroup = ItemPanel.GetComponent<ToggleGroup>();
        var contentHeight = panelRect.sizeDelta.y/2;
        foreach (var item in _items)
        {
            var itemPresentation = (GameObject)Instantiate(ItemPresentationTemplate);
            itemPresentation.transform.SetParent(ItemPanel.transform, false);
            var itemHeight = itemPresentation.GetComponent<RectTransform>().sizeDelta.y;
            itemPresentation.transform.localPosition = new Vector3(0, contentHeight - itemHeight/2);
            contentHeight -= itemHeight + ItemPadding;
            var toggle = itemPresentation.GetComponent<Toggle>();
            toggle.group = panelToggleGroup;
            var dataContext = itemPresentation.GetComponent<MachineTypeData>();
            dataContext.SetMachineType(item);
        }

        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, panelRect.sizeDelta.y/2 - contentHeight - ItemPadding);
    }
}