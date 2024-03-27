using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;

    public int onGridPositionX;
    public int onGridPositionY;

    public void Set(ItemData data) 
    {
        this.itemData = data;

        GetComponent<Image>().sprite = data.texture;

        Vector2 size = new Vector2();

        size.x = itemData.width * ItemGrid.tileSizeWidth;
        size.y = itemData.height * ItemGrid.tileSizeHigth;

        GetComponent<RectTransform>().sizeDelta = size;
    }
}
