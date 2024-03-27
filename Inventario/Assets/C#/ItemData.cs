using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public int  width = 1;
    public int height = 1;
    public int uiIndex;

    public Sprite texture;

    public enum Type 
    {
        valuable, consumable, weapon
    }

    public Type type;
}
