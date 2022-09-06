using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Overlay : MonoBehaviour
{
    //public Overlay lay;
    public Tile Highlight;
    public Tilemap OverlayMap;
    public Tilemap Terrain;
    public Tilemap Deco;
    public AddAndRemove addAndRemove;
    public TileAtlas atlas;
    public Tile[] Breakstates;
    public int NoBreakStates;
    Vector3Int Prev;
    bool IsBreaking;
    public float timer;
    float breakTime = 0;
    int currentBreakState;
    float gap;


    // Start is called before the first frame update
    void Start()
    {
        NoBreakStates = Breakstates.Length;
        //Debug.Log(settings.Outline.name);
        if(settings.Outline == null)
        {
            //Other won't work
        }
        else
        {
            Highlight.sprite = settings.Outline;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Get mouse position
        Vector3 MousePos = Input.mousePosition;
        //Connvert to world coords
        MousePos = Camera.main.ScreenToWorldPoint(MousePos);
        //Set the z to 0
        MousePos.z = 0;
        //convert to vector3Int
        Vector3Int CellPos = OverlayMap.WorldToCell(MousePos);
        OverlayMap.SetTile(Prev, null);
        if((Input.GetMouseButtonDown(0) || Prev != CellPos) && addAndRemove.TileExistsToBreak(CellPos) && !addAndRemove.inv.InventoryIsOpen)
        {
            currentBreakState = 0;
            string name = GetNameOfTile(CellPos);
            TileData Data = GetTileData.FindData(name, atlas);
            //breakTime = GetTileData.FindData(Terrain.GetTile(CellPos).name, atlas).breakTime;
            breakTime = Data.breakTime;
            //Debug.Log(breakTime);
            gap = breakTime / NoBreakStates;
            //Debug.Log(gap);
            
           
        }
        if (Input.GetMouseButton(0) && addAndRemove.TileExistsToBreak(CellPos) && !addAndRemove.inv.InventoryIsOpen)
        {
            OverlayMap.SetTile(CellPos, Breakstates[currentBreakState]);   
            timer += Time.deltaTime;
            if(timer > gap)
            {
                timer = 0;
                currentBreakState++;
                if(currentBreakState >= NoBreakStates)
                {
                    currentBreakState = 0;
                    addAndRemove.TryBreakTile(CellPos);
                }
            }
        }
        else
        {
            OverlayMap.SetTile(CellPos, Highlight);
        }
        


        Prev = CellPos;
    }

    string GetNameOfTile(Vector3Int pos)
    {
        if (Terrain.HasTile(pos))
        {
            return Terrain.GetTile(pos).name;
        }
        else
        {
            return Deco.GetTile(pos).name;
        }
    }
}
