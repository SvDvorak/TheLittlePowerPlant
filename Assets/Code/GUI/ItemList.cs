using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    public GameObject ItemPresentationTemplate;
    public GameObject ItemPanel;

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
            return selectedToggle.GetComponent<DataContext>().Data;
        }
    }

    private void CreatePresentationForItems()
    {
        var panelToggleGroup = ItemPanel.GetComponent<ToggleGroup>();

        foreach (var item in _items)
        {
            var itemPresentation = (GameObject)Instantiate(ItemPresentationTemplate);
            itemPresentation.transform.SetParent(ItemPanel.transform, false);
            
			if(panelToggleGroup != null)
			{
				var toggle = itemPresentation.GetComponent<Toggle>();
				toggle.group = panelToggleGroup;
			}

            itemPresentation.AddComponent<DataContext>().Data = item;
        }
    }
}