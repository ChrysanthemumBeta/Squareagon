using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour
{

    public Image[] Slots;
    public Inventory Inv;
    private void Update()
    {
        DisplayInventoryNow();
    }
    public void DisplayInventoryNow()
    {
        for (int i = 0; i < Slots.Length; i++)  
        {
            InventoryItem item = Inv.InventoryData[i];
            if (item == null)
            {
                //Debug.Log("Don't");
            }
            else
            {
                if (item.Data != null)
                {
                    Slots[i].sprite = item.Data.Texture;
                    Slots[i].color = new Color(255, 255, 255, 255);
                    Slots[i].gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = item.count.ToString();
                }
                else if (item.ItemData != null)
                {
                    Slots[i].sprite = item.ItemData.Sprite;
                    Slots[i].color = new Color(255, 255, 255, 255);
                    Slots[i].gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = item.count.ToString();
                }
                else
                {
                    Slots[i].color = new Color(255, 255, 255, 0);
                    Slots[i].gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "";
                }
            }
        }
    }
}
