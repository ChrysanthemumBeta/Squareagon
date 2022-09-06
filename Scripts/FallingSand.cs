 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingSand : MonoBehaviour
{
    Tilemap Map;
    Tilemap Invis;
    Tilemap Deco;
    public Tile Sand;
    [SerializeField]
    Tile InvisTile;

    List<Vector3Int> Pos = new List<Vector3Int>();

    Vector3Int Prev;
    void Start()
    {
        Map = GameObject.Find("Terrain").GetComponent<Tilemap>();
        Invis = GameObject.Find("Invis").GetComponent<Tilemap>();
        Deco = GameObject.Find("Decoration").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int CurrentPos = Map.WorldToCell(transform.position);
        Invis.SetTile(Prev, null);

        Invis.SetTile(CurrentPos, InvisTile);
            
        
        if(Map.HasTile(CurrentPos + new Vector3Int(0, -1, 0)) || Deco.HasTile(CurrentPos + new Vector3Int(0, -1, 0)))
        {

            
            Map.SetTile(CurrentPos, Sand);
            Invis.SetTile(CurrentPos, null);
            Destroy(gameObject);
        }
        Prev = CurrentPos;
    }
}
