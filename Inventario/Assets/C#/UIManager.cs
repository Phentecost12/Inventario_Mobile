using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] Information;
    private int infoDiplaying;
    private int index = -1;
    public ItemGrid grid;

    public void DisplayInformation(int i) 
    {
        Information[i].SetActive(true);
        Information[infoDiplaying].SetActive(false);
        infoDiplaying = i;
    }

    public void ChangeFilter() 
    {
        if (index == 2) 
        { 
            index = -1;
            grid.CleanFilter();
            return;

        }

        index++;
        grid.FilterItems(new Color(1, 1, 1, 0.2f), index);
    }
}
