using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Survival : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public Generate gen;
    //public InventoryItem[] inventory;
    public GameObject[] slots;
    [HideInInspector]
    public int current;
    float index;
    List<int> indexes = new List<int>();
    [SerializeField]TMP_Text indicator;
    float scale = 1;
    [SerializeField] Creative cr;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            indexes.Add(0);
        }
    }

    private void Update()
    {
        if (!cr.IsinCreative)
        {
            current = (int)index;
            indexes[0] = current - 1;
            if (indexes[0] < 0)
            {
                indexes[0] = inventory.Count - 1;
            }
            indexes[1] = current;
            indexes[2] = current + 1;
            if (indexes[2] > inventory.Count - 1)
            {
                indexes[2] = 0;
            }

            if(inventory.Count > 1)
            {
                foreach (GameObject slot in slots)
                {
                    slot.SetActive(true);
                }
                
                for (int i = 0; i < slots.Length; i++)
                {
                    if (indexes[i] < 0)
                    {
                        indexes[i] = 0;
                    }
                    else if(indexes[i] > inventory.Count)
                    {
                        indexes[i] = inventory.Count;
                    }
                    if ((indexes[i] >= inventory.Count))
                    {
                        current--;
                        index--;
                        indexes[i] = inventory.Count - 1;

                    }
                    if(current > inventory.Count)
                    {
                        current--;
                        index--;
                    }
                    if (inventory[indexes[i]].Data)
                    {
                        slots[i].GetComponent<Image>().sprite = inventory[indexes[i]].Data.Texture;
                    }
                    else
                    {
                        slots[i].GetComponent<Image>().sprite = inventory[indexes[i]].ItemData.Sprite;
                    }


                    //slots[i].GetComponent<Image>().sprite = inventory[0].Data.Texture;
                }
            }
            else if(inventory.Count > 0)
            {

                if (current > inventory.Count)
                {
                    current--;
                    index--;
                }
                slots[0].SetActive(false);
                slots[1].SetActive(true);
                if (!inventory[indexes[current]].IsItem)
                {
                    slots[1].GetComponent<Image>().sprite = inventory[indexes[current]].Data.Texture;
                }
                else
                {
                    slots[1].GetComponent<Image>().sprite = inventory[indexes[current]].ItemData.Sprite;
                }
                //slots[1].GetComponent<Image>().sprite = inventory[0].Data.Texture;
                slots[2].SetActive(false);
            }
            else
            {
                foreach (GameObject slot in slots)
                {
                    slot.SetActive(false);
                }
            }

            if (inventory.Count > 0)
            {
                if (current < inventory.Count)
                {
                    if (inventory[indexes[1]].Data)
                    {
                        indicator.text = inventory[current].Data.Name + ": " + inventory[current].count;
                    }
                    else if (inventory[indexes[1]].count == 1)
                    {
                        indicator.text = inventory[current].ItemData.Name;
                    }
                    else
                    {
                        indicator.text = inventory[current].ItemData.Name + ": " + inventory[current].count;
                    }

                }
                else
                {
                    if (inventory[indexes[1]].Data)
                    {
                        indicator.text = inventory[current].Data.Name + ": " + inventory[current].count;
                    }
                    else if(inventory[indexes[1]].count == 1)
                    {
                        indicator.text = inventory[current].ItemData.Name;
                    }
                    else
                    {
                        indicator.text = inventory[current].ItemData.Name + ": " + inventory[current].count;
                    }
                }
            }
            else
            {
                indicator.text = "";
            }

            float mouseScroll = Input.mouseScrollDelta.y;
            if (mouseScroll > 0)
            {
                Debug.Log("up");
                index += mouseScroll * scale;
                if (index > inventory.Count - 1)
                {
                    index = 0;
                }
            }
            else if (mouseScroll < 0)
            {
                Debug.Log("Down");
                index += mouseScroll * scale;
                if (index < 0)
                {
                    index = inventory.Count - 1;
                }
            }
        }
        
    }
    public TileData GetCurrentSelected()
    {
        if(inventory.Count > 0)
        {
            if(inventory.Count > current)
            {
                return inventory[current].Data;

            }
            else
            {
                return inventory[0].Data;
            }
        }
        return null;

    }

    public int GetIndexOf(TileData data)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].Data == data)
            {
                return i;
            }
        }
        return 0;
    }
}


[System.Serializable]
public class InventoryItem
{
    public TileData Data;
    public ItemData ItemData;
    public int count;
    public bool IsItem;

    public InventoryItem(TileData data, int Count, ItemData itemData)
    {
        Data = data;
        count = Count;
        ItemData = itemData;
    }
}

