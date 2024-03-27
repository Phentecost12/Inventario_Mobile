using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public ItemGrid selectedItemGrid;

    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransformItem;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] RectTransform canvas;
    [SerializeField] UIManager manager;

    InventoryHighLight inventoryHighLight;

    private Vector2Int lastPosition;

    private void Awake()
    {
        inventoryHighLight = GetComponent<InventoryHighLight>();
    }

    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            CreateRandomObject();
        }
    }

    private void Update()
    {
        if(selectedItem != null && (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)) 
        {
            Vector2 position = Input.mousePosition;
            position.x -= (rectTransformItem.sizeDelta.x / 2);
            position.y += (rectTransformItem.sizeDelta.y / 2);
            rectTransformItem.position = position;
        }

        if (selectedItemGrid == null) 
        {
            inventoryHighLight.Show(false);
            return;
        }

        HandleHighlight();

        if (Input.touchCount >0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2Int tileGridPosition = MouseOffset(touch);

            if (selectedItem == null)
            {
                selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
                if (selectedItem != null)
                {
                    rectTransformItem = selectedItem.GetComponent<RectTransform>();
                    manager.DisplayInformation(selectedItem.itemData.uiIndex);
                    lastPosition = new Vector2Int(tileGridPosition.x, tileGridPosition.y);
                }
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
                if (complete)
                {
                    selectedItem = null;
                    if (overlapItem != null)
                    {
                        selectedItem = overlapItem;
                        overlapItem = null;
                        rectTransformItem = selectedItem.GetComponent<RectTransform>();
                    }
                }
                else 
                {
                    selectedItemGrid.PlaceItem(selectedItem, lastPosition.x, lastPosition.y, ref overlapItem);
                }
            }

        }

       /* if (Input.GetMouseButtonDown(1)) 
        {
            Vector2Int tileGridPosition = MouseOffset();
            if(selectedItem == null)
            {
                InventoryItem selectedItemToShow = selectedItemGrid.GetItem(tileGridPosition.x, tileGridPosition.y);
                if (selectedItemToShow != null)
                {
                    manager.DisplayInformation(selectedItemToShow.itemData.uiIndex);
                }
            }
        }*/

    }

    private Vector2Int MouseOffset(Touch touch)
    {
        Vector2 position = touch.position;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.itemData.width - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.itemData.height - 1) * ItemGrid.tileSizeHigth / 2;
        }
        
        
        return selectedItemGrid.GetTileGridPosition(position);
    }

    public void CreateRandomObject() 
    {
        InventoryItem item  = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        item.GetComponent<RectTransform>().SetParent(canvas);
        int selectedItemID = Random.Range(0, items.Count);
        item.Set(items[selectedItemID]);

        while (true) 
        {
           bool complete = selectedItemGrid.PlaceItem(item, Random.Range(0,selectedItemGrid.gridSizeWidth-1), Random.Range(0, selectedItemGrid.gridSizeHeight - 1), ref overlapItem);
            if (complete) 
            {
                if(overlapItem == null) 
                {
                    break;
                }
                
            }
        }
    }
    private void HandleHighlight() 
    {

        
        if (selectedItem != null) 
        {
            Vector2Int positionOnGrid = MouseOffset(Input.GetTouch(0));
            inventoryHighLight.Show(selectedItemGrid.BoundryCheck(positionOnGrid.x, positionOnGrid.y, selectedItem.itemData.width, selectedItem.itemData.height));
            inventoryHighLight.SetSize(selectedItem);
            inventoryHighLight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
        else 
        {
            inventoryHighLight.Show(false);
        }
    }
}
