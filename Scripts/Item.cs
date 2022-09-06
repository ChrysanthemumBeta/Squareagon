using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData Data;
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
        if (collision.tag == gameObject.tag)
        {

            if (collision.gameObject.GetComponent<Item>().Data == Data)
            {
                if (Combine > collision.gameObject.GetComponent<Item>().Combine)
                {
                    if (Count + collision.gameObject.GetComponent<Item>().Count <= Data.StackLimit)
                    {
                        Count += collision.gameObject.GetComponent<Item>().Count;
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        int AmountToAdd = Data.StackLimit - (Count + collision.gameObject.GetComponent<Item>().Count);
                        collision.gameObject.GetComponent<Item>().Count -= AmountToAdd;
                        Count += AmountToAdd;
                    }
                }
            }
        }
        else
        {
            if (collision.tag == "Player")
            {

                CanAdd = inv.CanAdd(null, Data, Count);
                for (int i = 0; i < CanAdd; i++)
                {
                    inv.AddItem(null, Data);
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
