using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetTileData
{
    public static TileData FindData(string name, TileAtlas atlas)
    {
        foreach(TileData data in atlas.Data)
        {
            if(data.Name == name)
            {
                return data;
            }
        }
        Debug.LogError("Attempt to access tile that doesn't exist :(.");
        return null;
    }
}
