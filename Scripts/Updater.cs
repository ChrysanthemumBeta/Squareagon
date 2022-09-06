using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Linq;
using System.Threading;

public class Updater : MonoBehaviour
{
    public int updateDistance;
    public Generate Gen;
    public Tilemap map;
    List<Vector3Int> TilesToRemove = new List<Vector3Int>();
    bool hasCleared;

    private void Start()
    {
        Thread Main = new Thread(Deal);
        Main.Start();
    }

    public void Deal()
    {
        Debug.Log("Started");
        while (true)
        {
            updateChunk(Gen.PlayerChunkX);
            for (int i = 1; i < updateDistance; i++)
            {
                updateChunk(Gen.PlayerChunkX - i);
                updateChunk(Gen.PlayerChunkX + i);
            }
        }
    }

    private void Update()
    {
        if (!hasCleared)
        {
            foreach (Vector3Int pos in TilesToRemove)
            {
                map.SetTile(pos, null);
                hasCleared = true;
            }
        }

    }

    void removeChunks()
    {

    }


    public void updateChunk(int X)
    {
        if (Gen.Chunks.ContainsKey(X))
        {
            TilesToRemove.Clear();
            Dictionary<Vector3Int, ActiveTileData> Temp = new Dictionary<Vector3Int, ActiveTileData>();
            Temp = Gen.Chunks[X].Data.ToDictionary(entry => entry.Key, entry => new ActiveTileData(entry.Value.canFall));
            foreach (KeyValuePair<Vector3Int, ActiveTileData> item in Gen.Chunks[X].Data)
            {
                if (item.Value.canFall)
                {
                    //map.SetTile(item.Key, null);
                    TilesToRemove.Add(item.Key);
                    Temp.Remove(item.Key);
                }
            }
            while (hasCleared)
            {

            }
            hasCleared = false;
            removeChunks();
            Gen.Chunks[X].Data = Temp.ToDictionary(entry => entry.Key, entry => new ActiveTileData(entry.Value.canFall));
        }

    }
}
