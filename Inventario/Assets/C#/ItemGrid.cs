using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 32;
    public const float tileSizeHigth = 32;

    InventoryItem[,] inventoryItemsSlot;

    RectTransform rectTransform;

    public int gridSizeWidth = 20;
    public int gridSizeHeight = 30;

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();



    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
    }

    private void Init(int widthm ,int height) 
    {
        inventoryItemsSlot = new InventoryItem[widthm,height];
        Vector2 size = new Vector2(widthm * tileSizeWidth, height * tileSizeHigth);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetTileGridPosition( Vector2 mousePosition) 
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHigth);

        return tileGridPosition;
    }

    public bool PlaceItem(InventoryItem item, int x, int y, ref InventoryItem overlap)
    {
        if (!BoundryCheck(x, y, item.itemData.width, item.itemData.height)) return false;

        if (!OverlapHeck(x, y, item.itemData.width, item.itemData.height, ref overlap)) { overlap = null; return false; }

        if (overlap != null) CleanGridReference(overlap);

        RectTransform rectTransformItem = item.GetComponent<RectTransform>();
        rectTransformItem.SetParent(rectTransform);

        for (int i = 0; i < item.itemData.width; i++)
        {
            for (int j = 0; j < item.itemData.height; j++)
            {
                inventoryItemsSlot[x + i, y + j] = item;
            }
        }

        item.onGridPositionX = x;
        item.onGridPositionY = y;
        Vector2 position = CalculatePositionGRid(item, x, y);

        rectTransformItem.localPosition = position;

        return true;
    }

    public Vector2 CalculatePositionGRid(InventoryItem item, int x, int y)
    {
        Vector2 position = new Vector2();
        position.x = x * tileSizeWidth + item.itemData.width / 2;
        position.y = -(y * tileSizeHigth + item.itemData.height / 2);
        return position;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toRetur = inventoryItemsSlot[x, y];

        if (toRetur == null) return null;

        CleanGridReference(toRetur);

        return toRetur;
    }

    private void CleanGridReference(InventoryItem item)
    {
        for (int i = 0; i < item.itemData.width; i++)
        {
            for (int j = 0; j < item.itemData.height; j++)
            {
                inventoryItemsSlot[item.onGridPositionX + i, item.onGridPositionY + j] = null;
            }
        }
    }

    public InventoryItem GetItem(int x, int y) 
    {
        return inventoryItemsSlot[x, y];
    }

    bool PositionCheck(int x, int y) 
    {
        if(x<0 || y < 0) 
        {
            return false;
        }
        else if(x>=gridSizeWidth || y >= gridSizeHeight) 
        {
            return false;
        }
        else 
        {
            return true;
        }
    }

    public bool BoundryCheck(int x, int y,  int width, int height) 
    {
        if (!PositionCheck(x,y)) return false;

        x += width -1;
        y += height -1;

        if(!PositionCheck(x,y)) return false;


        return true;
    }

    bool OverlapHeck(int x, int y, int width, int height, ref InventoryItem overlap) 
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (inventoryItemsSlot[x+i,y+j] != null) 
                {
                    if(overlap == null) 
                    {
                        overlap = inventoryItemsSlot[x + i, y + j];
                    }
                    else 
                    {
                        if(overlap != inventoryItemsSlot[x + i, y + j]) return false;
                    }
                }
            }
        }

        return true;
    }

    public void FilterItems(Color col, int i) 
    {
        foreach(InventoryItem item in inventoryItemsSlot) 
        {
            if (item == null) continue;

            if((int)item.itemData.type != i) 
            {
                item.GetComponent<Image>().color = col;
            }
            else 
            {
                item.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void CleanFilter() 
    {
        foreach (InventoryItem item in inventoryItemsSlot)
        {
            if (item == null) continue;
            item.GetComponent<Image>().color = Color.white;
        }
    }
}
