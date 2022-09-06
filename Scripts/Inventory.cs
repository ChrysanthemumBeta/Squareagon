using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int MaxSlots;
    public int HotbarLength;
    public List<Image> Hotbar = new List<Image>();
    public InventoryItem[] InventoryData;
    public bool CanScroll;
    int scale = 1;
    public int Current;
    public bool InventoryIsOpen;
    public GameObject DisplayInv;
    public TileAtlas atlas;


    private void Awake()
    {
        InventoryData = new InventoryItem[MaxSlots];
        for (int i = 0; i < InventoryData.Length; i++)
        {
            InventoryData[i] = new InventoryItem(null, 0, null);
        }
        int x = 0;
        foreach (InventorySaveItem item in PlayerValues.Inventory)
        {
            if(item != null)
            {
                if(item.Type != null)
                {
                    InventoryData[x] = new InventoryItem(GetTileData.FindData(item.Type, atlas), item.Count, null);
                }
            }
            x++;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (InventoryIsOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
        if (CanScroll)
        {
            Vector2 Scroll = Input.mouseScrollDelta;
            if(Scroll.y < 0)
            {
                Current--;
                if(Current < 0)
                {
                    Current = HotbarLength - 1;
                }

            }
            else if(Scroll.y > 0)
            {
                Current++;
                if(Current > HotbarLength - 1)
                {
                    Current = 0;
                }
            }
        }
        if (!InventoryIsOpen)
        {
            foreach (Image item in Hotbar)
            {
                item.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }

            Hotbar[Current].gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        for (int i = 0; i < HotbarLength; i++)
        {
            InventoryItem item = InventoryData[i];
            if(item == null)
            {
                //Debug.Log("Don't");
            }
            else
            {
                if(item.Data != null)
                {
                    Hotbar[i].sprite = item.Data.Texture;
                    Hotbar[i].color = new Color(255, 255, 255, 255);
                    Hotbar[i].gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = item.count.ToString();
                }
                else if(item.ItemData != null)
                {
                    Hotbar[i].sprite = item.ItemData.Sprite;
                    Hotbar[i].color = new Color(255, 255, 255, 255);
                    Hotbar[i].gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = item.count.ToString();
                }
                else
                {
                    Hotbar[i].color = new Color(255, 255, 255, 0);
                    Hotbar[i].gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "";
                }
            }
        }
    }

    public void AddItem(TileData data, ItemData itemData)
    {
        //Check if item exists in inventory
        InventoryItem ToAdd = new InventoryItem(data: data, Count: 1, itemData: itemData);
        ToAdd.IsItem = itemData;
        if (ItemExists(ToAdd) && CanBeAdded(ToAdd))
        {
            //Get availibe index
            int Index = GetIndexOf(ToAdd);
            Debug.Log(Index);
            if(Index != -1)
            {
                InventoryData[Index].count++;
            }
            else
            {
                int index = GetFirstFree();
                if(index != -1)
                {
                    InventoryData[index] = ToAdd;
                }
            }
        }
        else
        {
            int Index = GetFirstFree();
            if(Index != -1)
            {
                InventoryData[Index] = ToAdd;
            }
        }
    }

    int GetIndexOf(InventoryItem Check)
    {
        int index = 0;
        foreach (InventoryItem item in InventoryData)
        {
            if(item != null && (item.Data != null || item.ItemData != null))
            {
                if ((item.Data == Check.Data || item.Data == Check.ItemData))
                {
                    if (item.IsItem)
                    {
                        if (item.ItemData.StackLimit > item.count)
                        {
                            return index;

                        }
                    }
                    else
                    {
                        if (Check.Data.StackLimit > item.count)
                        {
                            return index;

                        }
                    }
                }
            }
            index++;
        }
        return -1;
    }

    bool ItemExists(InventoryItem check)
    {
        foreach (InventoryItem item in InventoryData)
        {
            if(item != null)
            {
                if (check.IsItem)
                {
                    if (item.ItemData == check.ItemData)
                    {
                        return true;
                    }
                }
                else
                {
                    if (item.Data == check.Data)
                    {
                        return true;
                    }
                }
            }

        }
        return false;
    }

    bool CanBeAdded(InventoryItem Check)
    {
        List<bool> CanBeAdded = new List<bool>();
        foreach (InventoryItem item in InventoryData)
        {
            if(item != null)
            {
                if (!item.IsItem)
                {
                    if (item.Data == Check.Data)
                    {
                        CanBeAdded.Add(item.count < item.Data.StackLimit);
                    }
                }
                else
                {
                    if(item.ItemData == Check.ItemData)
                    {
                        CanBeAdded.Add(item.count < item.Data.StackLimit);
                    }
                }

            }
        }
        return CanBeAdded.Contains(true);
    }

    int GetFirstFreeSlot(InventoryItem ItemToCheck)
    {
        if (ItemToCheck == null)
        {
            int index = 0;
            foreach (InventoryItem item in InventoryData)
            {
                if(item != null)
                {
                    if (item.count == 0)
                    {
                        return index;
                    }
                }
                index++;
            }
            return -1;
        }
        else
        {
            int index = 0;
            foreach (InventoryItem item in InventoryData)
            {
                if (item != null)
                {
                    if (item.Data == ItemToCheck.Data || item.ItemData == ItemToCheck.ItemData)
                    {
                        return index;
                    }
                }

                index++;
            }
            return -1;
        }
    }

    int GetFirstFree()
    {
        int index = 0;
        foreach (InventoryItem item in InventoryData)
        {
            if (item != null)
            {
                if (item.count == 0)
                {
                    return index;
                }
            }
            index++;
        }
        return -1;
    }

    public void RemoveTileFromInv()
    {
        Debug.Log("!");
        InventoryData[Current].count--;
        if(InventoryData[Current].count <= 0)
        {
            InventoryData[Current].Data = null;
            InventoryData[Current].ItemData = null;
            InventoryData[Current].count = 0;
        }
    }

    public InventoryItem GetCurrentSelected()
    {
        return InventoryData[Current];
    }

    public void OpenInventory()
    {
        Hotbar[Current].gameObject.transform.GetChild(0).gameObject.SetActive(false);
        InventoryIsOpen = true;
        CanScroll = false;
        DisplayInv.SetActive(true);
    }

    public void CloseInventory()
    {
        Hotbar[Current].gameObject.transform.GetChild(0).gameObject.SetActive(true);
        InventoryIsOpen = false;
        CanScroll = true;
        DisplayInv.SetActive(false);
    }

    public int CanAdd(TileData data, ItemData itemData, int amount)
    {
        InventoryItem Check = new InventoryItem(data: data, itemData: itemData, Count: 1);
        
        List<bool> CanBeAdded = new List<bool>();
        foreach (InventoryItem item in InventoryData)
        {
            if (item != null)
            {
                if (!item.IsItem)
                {
                    //Debug.Log("IsTile");
                    if (item.Data == data)
                    {
                        //Debug.Log("Is Tile");
                        if (item.count < item.Data.StackLimit)
                        {
                            if (item.count + amount <= item.Data.StackLimit)
                            {
                                return amount;
                            }
                            else
                            {
                                return (item.count + amount) - item.Data.StackLimit;
                            }
                        }
                    }
                    else if(item.Data == null)
                    {
                        return amount;
                    }
                }
                else
                {
                    if (item.ItemData == itemData)
                    {
                        if (item.count < item.ItemData.StackLimit)
                        {
                            if (item.count + amount <= item.ItemData.StackLimit)
                            {
                                return amount;
                            }
                            else
                            {
                                return (item.count + amount) - item.ItemData.StackLimit;
                            }
                        }
                    }
                }
            }
            else
            { 
                Debug.Log("Empty");
                return amount;
            }
        }
        return -1;
    }

    public InventoryItem DeleteFrom(int Slot)
    {
        InventoryItem Item = new InventoryItem(data: InventoryData[Slot].Data, Count: InventoryData[Slot].count, itemData: InventoryData[Slot].ItemData);
        Item.IsItem = InventoryData[Slot].IsItem;

        InventoryData[Slot].count = 0;
        InventoryData[Slot].Data = null;
        InventoryData[Slot].ItemData = null;
        Debug.Log(Item.count);
        return Item;
    }

    public InventoryItem GetInvItem(int slot)
    {
        InventoryItem Item = InventoryData[slot];
        return Item;
    }
    public void SetItem(int slot, InventoryItem item)
    {
        InventoryData[slot] = item;
    }
    

}