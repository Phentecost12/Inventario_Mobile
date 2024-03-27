using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryHighLight : MonoBehaviour
{
    [SerializeField] RectTransform highLighter;

    public void SetSize(InventoryItem item) 
    {
        Vector2 size = new Vector2();

        size.x = item.itemData.width * ItemGrid.tileSizeWidth;
        size.y = item.itemData.height * ItemGrid.tileSizeHigth;

        highLighter.sizeDelta = size;
    }

    public void SetPosition(ItemGrid grid, InventoryItem item) 
    {
        highLighter.SetParent(grid.GetComponent<RectTransform>());

        Vector2 position = grid.CalculatePositionGRid(item, item.onGridPositionX, item.onGridPositionY);

        position.x += ItemGrid.tileSizeWidth *item.itemData.width / 2;
        position.y -= ItemGrid.tileSizeHigth*item.itemData.height / 2;
        highLighter.localPosition = position;
    }

    public void SetPosition(ItemGrid grid, InventoryItem item, int x, int y) 
    {
        highLighter.SetParent(grid.GetComponent<RectTransform>());

        Vector2 pos = grid.CalculatePositionGRid(item,x, y);

        pos.x += ItemGrid.tileSizeWidth * item.itemData.width / 2;
        pos.y -= ItemGrid.tileSizeHigth * item.itemData.height / 2;


        highLighter.localPosition = pos;
    }

    public void Show(bool b) 
    {
        highLighter.gameObject.SetActive(b);
    }
}
