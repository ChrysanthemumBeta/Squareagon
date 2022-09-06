using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseOverlay : MonoBehaviour
{
    public Tile Highlight;
    public Tile[] BreakStates;
    int NoBreakStates;
    public Tilemap OverlapMap;
    public Tilemap Deco;
    Vector3Int Prev;
    float timer;
    public float BreakTime;
    int currentBreakstate;
    public Tilemap Terrain;
    PlaceAndBreak pab;
    public TileAtlas atlas;
    public Creative cr;
    float timer2;
    // Start is called before the first frame update
    void Start()
    {
        NoBreakStates = BreakStates.Length;
        pab = GetComponent<PlaceAndBreak>();
        if(Highlight.sprite != settings.Outline && settings.Outline != null)
        {
            Highlight.sprite = settings.Outline;
        }

    }

    // Update is called once per frame
    void Update()
    {

        OverlapMap.ClearAllTiles();
        
        //Get mouse position
        Vector3 MousePos = Input.mousePosition;
        //Connvert to world coords
        MousePos = Camera.main.ScreenToWorldPoint(MousePos);
        //Set the z to 0
        MousePos.z = 0;
        //convert to vector3Int
        Vector3Int CellPos = OverlapMap.WorldToCell(MousePos);
        
        if (!Input.GetMouseButton(0) && (!Terrain.HasTile(CellPos) || !Deco.HasTile(CellPos)))
        {
            //Clear last tile
            OverlapMap.SetTile(Prev, null);
            //Set Tile under mouse to the hightlight
            OverlapMap.SetTile(CellPos, Highlight);
        }
        else if(Input.GetMouseButtonDown(0))
        {
            //Clear last existing highlight
            OverlapMap.SetTile(Prev, null);
            //Reset current breakstate
            currentBreakstate = 0;
            //set current breakTime
            if (!cr.IsinCreative)
            {
                if (Terrain.HasTile(CellPos))
                {
                    BreakTime = GetTileData.FindData(Terrain.GetTile(CellPos).name, atlas).breakTime;

                }
                else if(Deco.HasTile(CellPos))
                {
                    BreakTime = GetTileData.FindData(Deco.GetTile(CellPos).name, atlas).breakTime;

                }


            }
            else
            {
                BreakTime = 0;
            }
        }
        else if(Terrain.HasTile(CellPos))
        {
            //Set the highlight to current breakstate
            OverlapMap.SetTile(Prev, null);
            OverlapMap.SetTile(CellPos, BreakStates[currentBreakstate]);
            if(Prev != CellPos)
            {
                currentBreakstate = 0;
                if (!cr.IsinCreative)
                {
                    BreakTime = GetTileData.FindData(Terrain.GetTile(CellPos).name, atlas).breakTime;
                }
                else
                {
                    BreakTime = 0;
                }
            }
            timer += Time.deltaTime;
            if(timer > BreakTime/NoBreakStates)
            {
                currentBreakstate++;
                if(currentBreakstate >= NoBreakStates)
                {
                    currentBreakstate = 0;
                    pab.BreakTile(CellPos);
                    OverlapMap.SetTile(CellPos, null);
                }
                timer = 0;
            }
        }
        else if(Deco.HasTile(CellPos))
        {
            //Set the highlight to current breakstate
            OverlapMap.SetTile(Prev, null);
            OverlapMap.SetTile(CellPos, BreakStates[currentBreakstate]);
            if (Prev != CellPos)
            {
                currentBreakstate = 0;
                if (!cr.IsinCreative)
                {
                    BreakTime = GetTileData.FindData(Deco.GetTile(CellPos).name, atlas).breakTime;
                }
                else
                {
                    BreakTime = 0;
                }
            }
            timer += Time.deltaTime;
            if (timer > BreakTime / NoBreakStates)
            {
                currentBreakstate++;
                if (currentBreakstate >= NoBreakStates)
                {
                    currentBreakstate = 0;
                    pab.BreakTile(CellPos);
                    OverlapMap.SetTile(CellPos, null);

                }
                timer = 0;
            }
        }
        //Store Current pos as previous pos
        Prev = CellPos;
    }
}
