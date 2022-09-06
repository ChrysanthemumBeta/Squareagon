using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public TileData Data;
    public int Count = 1;
    public int Combine;
    Inventory inv;
    int CanAdd = -1;

    private void Awake()
    {
        Combine = Random.Range(-100000, 100000);
        inv = GameObject.Find("Bg").GetComponent<Inventory>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == gameObject.tag)
        {
            
            if(collision.gameObject.GetComponent<ItemDrop>().Data == Data)
            {
                if (Combine > collision.gameObject.GetComponent<ItemDrop>().Combine)
                {
                    if (Count + collision.gameObject.GetComponent<ItemDrop>().Count <= Data.StackLimit)
                    {
                        Count += collision.gameObject.GetComponent<ItemDrop>().Count;
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        int AmountToAdd = Data.StackLimit - (Count + collision.gameObject.GetComponent<ItemDrop>().Count);
                        collision.gameObject.GetComponent<ItemDrop>().Count -= AmountToAdd;
                        Count += AmountToAdd;
                    }
                }
            }
        }
        else
        {
            if (collision.tag == "Player")
            {

                CanAdd = inv.CanAdd(Data, null, Count);
                for (int i = 0; i < CanAdd; i++)
                {
                    inv.AddItem(Data, null);
                }
                if (CanAdd == Count)
                {
                    Destroy(gameObject);
                }
                else
                {
                   if (CanAdd < Count)
                   {
                        Count -= CanAdd;
                   }
                }
            }
        }
    }
}
