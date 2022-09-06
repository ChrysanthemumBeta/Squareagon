using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Creative : MonoBehaviour
{
    public bool IsinCreative = true;
    public TMP_Text indicator;
    public float index;
    public int current;
    public float scale;
    public TileAtlas atlas;
    public GameObject[] slots;
    public List<int> indexes = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            indexes.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsinCreative)
        {
            foreach (GameObject slot in slots)
            {
                slot.SetActive(true);
            }
            current = (int)index;
            indexes[0] = current - 1;
            if (indexes[0] < 0)
            {
                indexes[0] = atlas.Data.Length - 1;
            }
            indexes[1] = current;
            indexes[2] = current + 1;
            if (indexes[2] > atlas.Data.Length - 1)
            {
                indexes[2] = 0;
            }


            for (int i = 0; i < slots.Length; i++)
            {

                slots[i].GetComponent<Image>().sprite = atlas.Data[indexes[i]].Texture;
            }

            indicator.text = atlas.Data[current].Name;

            float mouseScroll = Input.mouseScrollDelta.y;
            if (mouseScroll > 0)
            {
                Debug.Log("up");
                index += mouseScroll * scale;
                if (index > atlas.Data.Length - 1)
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
                    index = atlas.Data.Length - 1;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            IsinCreative = !IsinCreative;
        }


        
    }

    public TileData GetCurrentSelected()
    {
        return atlas.Data[current];
    }
}
