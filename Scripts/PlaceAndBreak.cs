using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceAndBreak : MonoBehaviour
{
    public TileAtlas atlas;
    public Tilemap Terrain;
    public Tilemap Deco;
    public Tilemap Invis;
    public Tile tile;
    public Creative cr;
    public Survival sr;
    [SerializeField]GameObject destroyParticle;
    [SerializeField] Transform player;
    [SerializeField] Tilemap overLay;
    List<Vector3Int> PlayerCovers = new List<Vector3Int>();
    public List<string> OnWater = new List<string>();
    public Tilemap water;
    [SerializeField]
    GameObject FallingSand;
    public List<string> Interactable = new List<string>();
    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            PlayerCovers.Add(Vector3Int.zero);
        }
    }
    private void Update()
    {
        //Get mouse position
        Vector3 MousePos = Input.mousePosition;
        //Connvert to world coords
        MousePos = Camera.main.ScreenToWorldPoint(MousePos);
        //Set the z to 0
        MousePos.z = 0;
        //convert to vector3Int
        Vector3Int CellPos = Terrain.WorldToCell(MousePos);
        if (Input.GetMouseButton(1))
        {
            if (cr.IsinCreative)
            {
                PlaceTile(CellPos, sr.GetCurrentSelected().Tile);
            }
            else
            {
                if (sr.inventory.Count > 0)
                {
                    if (!sr.inventory[sr.current].IsItem)
                    {
                        PlaceTile(CellPos, sr.GetCurrentSelected().Tile);
                    }

                }

            }

        }

    
        PlayerCovers[0] = Terrain.WorldToCell(player.position);
        PlayerCovers[1] = Terrain.WorldToCell(player.position) + new Vector3Int(0, 1, 0);
        //overLay.SetTile(Terrain.WorldToCell(player.position), tile);
        //overLay.SetTile(Terrain.WorldToCell(player.position) + new Vector3Int(0, 1, 0), tile);
    }

    void PlaceTile(Vector3Int pos, Tile tile)
    {
        if (!Terrain.HasTile(pos) && !PlayerCovers.Contains(pos) && !Deco.HasTile(pos) && !Invis.HasTile(pos))
        {
            if (cr.IsinCreative)
            {
                if (OnWater.Contains(cr.GetCurrentSelected().Name))
                {                    
                    water.SetTile(pos, tile);
                }
                else
                {
                    Terrain.SetTile(pos, tile);
                    if (water.HasTile(pos))
                    {
                        water.SetTile(pos, null);
                    }
                }
            }
            else
            {
                if (sr.inventory.Count > sr.current)
                {
                    if (sr.inventory[sr.current].count > 0)
                    {
                        sr.inventory[sr.current].count--;
                    }
                    if (sr.inventory[sr.current].count == 0)
                    {
                        sr.inventory.RemoveAt(sr.current);
                    }
                }
                else
                {
                    if (sr.inventory[0].count > 0)
                    {
                        sr.inventory[0].count--;
                    }
                    if (sr.inventory[0].count == 0)
                    {
                        sr.inventory.RemoveAt(0);
                    }
                }
                if(sr.current <= sr.inventory.Count)
                {
                    Debug.Log(sr.current);
                    if (OnWater.Contains(sr.inventory[sr.current].Data.Name))
                    {
                        water.SetTile(pos, tile);
                    }
                    else
                    {
                        if (!Terrain.HasTile(pos + new Vector3Int(0, -1, 0)) && !Invis.HasTile(pos + new Vector3Int(0, -1, 0)) && tile.name == "Sand")
                        {
                            Instantiate(FallingSand, Terrain.CellToWorld(pos) + new Vector3(Terrain.cellSize.x / 2, Terrain.cellSize.y / 2, 0), Quaternion.identity);
                        }
                        else if (tile.name != "Sand")
                        {
                            Terrain.SetTile(pos, tile);
                            if (water.HasTile(pos))
                            {
                                water.SetTile(pos, null);
                            }
                        }
                        else if (tile.name == "Sand" && Terrain.HasTile(pos + new Vector3Int(0, -1, 0)))
                        {
                            Terrain.SetTile(pos, tile);
                            if (water.HasTile(pos))
                            {
                                water.SetTile(pos, null);
                            }
                        }

                    }
                }



            }

        }
        else
        {
            if (Terrain.HasTile(pos))
            {
                if (Interactable.Contains(Terrain.GetTile(pos).name))
                {
                    Debug.Log("Boom");
                    
                }
            }
        }
    }

    public void BreakTile(Vector3Int pos)
    {
        //GameObject desP = Instantiate(destroyParticle, Terrain.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 10), Quaternion.identity);
        //Color particleColour = GetTileData.FindData(Terrain.GetTile(pos).name, atlas).Colour;
        //ParticleSystem.MainModule settings = desP.GetComponent<ParticleSystem>().main;
        //settings.startColor = new ParticleSystem.MinMaxGradient(particleColour);
        //desP.GetComponent<ParticleSystem>().Play();
        if (Terrain.HasTile(pos))
        {
            string name = Terrain.GetTile(pos).name;
            if (!cr.IsinCreative)
            {
                if (InvContains(GetTileData.FindData(name, atlas)))
                {
                    if(sr.inventory[sr.GetIndexOf(GetTileData.FindData(name, atlas))].count >= GetTileData.FindData(name, atlas).StackLimit)
                    {
                        sr.inventory.Add(new InventoryItem(GetTileData.FindData(name, atlas), 1, null));
                    }
                    else
                    {
                        sr.inventory[sr.GetIndexOf(GetTileData.FindData(name, atlas))].count++;
                    }
                }
                else
                {
                    sr.inventory.Add(new InventoryItem(GetTileData.FindData(name, atlas), 1, null));
                }
            }
            Terrain.SetTile(pos, null);
        }
        if(Deco.HasTile(pos))
        {
            string name = Deco.GetTile(pos).name;
            if (!cr.IsinCreative)
            {
                if (InvContains(GetTileData.FindData(name, atlas)))
                {
                    sr.inventory[sr.GetIndexOf(GetTileData.FindData(name, atlas))].count++;
                }
                else
                {
                    sr.inventory.Add(new InventoryItem(GetTileData.FindData(name, atlas), 1, null));
                }
            }
            Deco.SetTile(pos, null);
        }

    }

    bool InvContains(TileData data)
    {
        foreach (InventoryItem item in sr.inventory)
        {
            if(item.Data == data)
            {
                return true;
            }
        }
        return false;
    }

    bool InvContainsItem(ItemData data)
    {
        foreach(InventoryItem item in sr.inventory)
        {
            if(item.ItemData == data)
            {
                return true;
            }
        }
        return false;
    }
}
