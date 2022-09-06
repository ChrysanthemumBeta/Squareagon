using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string Name;
    public float[] Colour;
    public InventorySaveItem[] Inv;

    public CharacterData(string name, float[] colour, InventorySaveItem[] Inventory)
    {
        Name = name;
        Colour = colour;
        Inv = Inventory;

    }
}

[System.Serializable]
public class InventorySaveItem
{
    public string Type;
    public int Count;

    public InventorySaveItem(string type, int count)
    {
        Type = type;
        Count = count;
    }
}
