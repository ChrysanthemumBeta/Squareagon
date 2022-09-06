using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AddAndRemove : MonoBehaviour
{
    public Inventory inv;
    public Tilemap Terrain;
    public Tilemap Deco;
    public Tilemap Water;
    public Tilemap Invis;
    public TileAtlas atlas;
    public GameObject Item;
    public GameObject ItemDrop;
    public GameObject FallingBlock;
    Vector3 MousePos;
    float Timer;
    public float PlaceCooldown;
    void Start()
    {
        
    }


    void Update()
    {
        //Get mouse position
        MousePos = Input.mousePosition;
        //Connvert to world coords
        MousePos = Camera.main.ScreenToWorldPoint(MousePos);
        //Set the z to 0
        MousePos.z = 0;
        //convert to vector3Int
        Vector3Int CellPos = Terrain.WorldToCell(MousePos);
        Timer += Time.deltaTime;

        if (!inv.InventoryIsOpen)
        {
            RemoveTile(CellPos);
        }
    }

    public void RemoveTile(Vector3Int pos)
    {
        if (Input.GetMouseButton(1) && !TileAlreadyExtists(pos) && Timer > PlaceCooldown)
        {
            Timer = 0;
            InventoryItem Selected = inv.GetCurrentSelected();
            if (Selected.Data != null || Selected.ItemData != null)
            {
                if (Selected.IsItem)
                {
                    //Do item stuff
                }
                else
                {
                    if (Selected.Data.CanFall)
                    {
                        GameObject Fallingblock = Instantiate(FallingBlock, Terrain.CellToWorld(Terrain.WorldToCell(MousePos)) + new Vector3(Terrain.cellSize.x / 2, Terrain.cellSize.y / 2, 0), Quaternion.identity);
                        Fallingblock.GetComponent<SpriteRenderer>().sprite = Selected.Data.Texture;
                        Fallingblock.GetComponent<FallingSand>().Sand = Selected.Data.Tile;
                    }
                    else
                    {
                        Terrain.SetTile(pos, Selected.Data.Tile);
                    }
                    inv.RemoveTileFromInv();
                }
            }
        }
    }

     public void TryBreakTile(Vector3Int CellPos)
     {
        if (TileExistsToBreak(CellPos))
        {
            string BrokenTile = BreakTile(CellPos);
            TileData Data = GetTileData.FindData(BrokenTile, atlas);
            if (!Data.DropsItems)
            {
                //inv.AddItem(Data, null);
                GameObject itemDrop = Instantiate(Item, MousePos, Quaternion.identity);
                itemDrop.GetComponent<ItemDrop>().Data = Data;
                itemDrop.GetComponent<SpriteRenderer>().sprite = Data.Texture;
                itemDrop.name = Data.Name;
            }
            else
            {
                itemDrop Drop = FindDrop(Data.itemDrops);
                GameObject itemDrop = Instantiate(ItemDrop, MousePos, Quaternion.identity);
                itemDrop.GetComponent<Item>().Data = Drop.Drop;
                itemDrop.GetComponent<SpriteRenderer>().sprite = Drop.Drop.Sprite;
                itemDrop.name = Drop.Drop.Name;
            }

        }
     }


    public bool TileExistsToBreak(Vector3Int pos)
    {
        return Terrain.HasTile(pos) || Deco.HasTile(pos);
    }

    bool TileAlreadyExtists(Vector3Int pos)
    {
        return Terrain.HasTile(pos) || Invis.HasTile(pos) || Deco.HasTile(pos);
    }

    string BreakTile(Vector3Int pos)
    {
        string Name = "";
        if (Terrain.HasTile(pos))
        {
            Name = Terrain.GetTile(pos).name;
            Terrain.SetTile(pos, null);
            
        }
        else
        {
            Name = Deco.GetTile(pos).name;
            Deco.SetTile(pos, null);
        }
        return Name;
    }


    public itemDrop FindDrop(itemDrop[] Loottable)
    {
        float max = 0;
        foreach (itemDrop item in Loottable)
        {
            max += item.chance;
        }
        float RandomDrop = Random.Range(0, max);
        List<itemDrop> Candidates = new List<itemDrop>();
        foreach (itemDrop item in Loottable)
        {
            if(item.chance >= RandomDrop)
            {
                Candidates.Add(item);
            }
        }
        float smallestChance = Mathf.Infinity;
        int index = 0;
        foreach (itemDrop item in Candidates)
        {
            if(item.chance < smallestChance)
            {
                smallestChance = item.chance;
                index = Candidates.IndexOf(item);
            }
        }
        return Candidates[index];

    }
}
