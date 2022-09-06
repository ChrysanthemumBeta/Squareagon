using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TileAtlas", menuName = "Tiles/TileAtlas")]
public class TileAtlas : ScriptableObject
{
    public TileData[] Data;
}
