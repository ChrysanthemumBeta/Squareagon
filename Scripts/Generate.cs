using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Generate : MonoBehaviour
{
    public int renderDistance = 3;
    public Tile tile;
    public Tilemap map;
    [SerializeField] Tilemap Decoration;
    [SerializeField] Tilemap Water;
    [SerializeField]int chunkSize = 10;
    [SerializeField] Transform player;
    List<int> LoadedChunks = new List<int>();
    [SerializeField] float frequency;
    public int seed;
    [SerializeField] float Amplitude;
    [SerializeField] int addition;
    public TileAtlas atlas;
    int distanceFromLastTree;
    [SerializeField] int snowHeight;
    [SerializeField] int waterLevel;
    public int PlayerChunkX;

    public Dictionary<int, ChunkData> Chunks = new Dictionary<int, ChunkData>();
    [HideInInspector]
    public List<Vector3Int> Tiles = new List<Vector3Int>();
    // Start is called before the first frame update
    void Start()
    {
        LoadedChunks.Clear();
        GenerateChunk(0);
        GenerateChunk(-1);
        GenerateChunk(1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int playerChunkpos = map.WorldToCell(player.position);
        PlayerChunkX = (int)playerChunkpos.x / chunkSize;
        GenerateChunk(PlayerChunkX);
        for (int i = 1; i < renderDistance; i++)
        {
            GenerateChunk(PlayerChunkX - i);
            GenerateChunk(PlayerChunkX + i);
        }
        
        
    }

    void GenerateChunk(int X)
    {
        if (!LoadedChunks.Contains(X))
        {
            Dictionary<Vector3Int, ActiveTileData> TempChunkData = new Dictionary<Vector3Int, ActiveTileData>();
            for (int x = 0; x < chunkSize; x++)
            {
                int WorldX = (X * chunkSize) + x;
                int height = (int)(Mathf.PerlinNoise((seed + WorldX) * frequency, seed * frequency) * Amplitude) + addition;
                for (int y = 0; y < height; y++)
                {
                    Vector3Int pos = new Vector3Int(WorldX, y, 0);
                    
                    if (y == height - 1)
                    {
                        if(y > snowHeight + Random.Range(-1, 1))
                        {
                            PlaceTile(pos, GetTileData.FindData("Snow", atlas).Tile, TempChunkData);
                            //map.SetTile(pos, GetTileData.FindData("Snow", atlas).Tile);
                            
                        }
                        else if(y < waterLevel)
                        {
                            PlaceTile(pos, GetTileData.FindData("Sand", atlas).Tile, TempChunkData);
                            if(y < waterLevel - 1)
                            {
                                for (int i = 0; i < waterLevel - y; i++)
                                {
                                    PlaceTile(pos + new Vector3Int(0, i, 0), GetTileData.FindData("Water", atlas).Tile, TempChunkData);
                                }
                            }
                            //map.SetTile(pos, GetTileData.FindData("Sand", atlas).Tile);
                        }
                        else
                        {
                            PlaceTile(pos, GetTileData.FindData("Grass", atlas).Tile, TempChunkData);
                            //map.SetTile(pos, GetTileData.FindData("Grass", atlas).Tile);
                        }
                        if (Random.Range(1, 5) == 1 && distanceFromLastTree > 6 && y > waterLevel)
                        {
                            Tree(pos, TempChunkData);
                            distanceFromLastTree = 0;
                        }
                        else
                        {
                            distanceFromLastTree++;
                        }
                        
                    }
                    else if(y > height - (10 + Random.Range(-2, 3)))
                    {
                        PlaceTile(pos, GetTileData.FindData("Dirt", atlas).Tile, TempChunkData);
                        //map.SetTile(pos, GetTileData.FindData("Dirt", atlas).Tile);
                    }
                    else
                    {
                        PlaceTile(pos, GetTileData.FindData("Stone", atlas).Tile, TempChunkData);
                        //map.SetTile(pos, GetTileData.FindData("Stone", atlas).Tile);
                    }
                }
            }
            LoadedChunks.Add(X);
            Chunks.Add(X, new ChunkData(TempChunkData));
        }
    }

    void PlaceTile(Vector3Int pos, Tile tile, Dictionary<Vector3Int, ActiveTileData> TempChunk)
    {
        if (!TempChunk.ContainsKey(pos))
        {
            if (tile.name == "Water")
            {
                Water.SetTile(pos, tile);
                TempChunk.Add(pos, new ActiveTileData(can: GetTileData.FindData(tile.name, atlas).CanFall));

            }
            else
            {
                map.SetTile(pos, tile);
                TempChunk.Add(pos, new ActiveTileData(can: GetTileData.FindData(tile.name, atlas).CanFall));
            }
        }

    }

    void Tree(Vector3Int pos, Dictionary<Vector3Int, ActiveTileData> TempChunk)
    {
        int TreeHeight = Random.Range(5, 9);
        for (int i = 1; i < TreeHeight; i++)
        {
            Decoration.SetTile(pos + new Vector3Int(0, i, 0), GetTileData.FindData("Wood", atlas).Tile);
        }
        Decoration.SetTile(pos + new Vector3Int(0, TreeHeight, 0), GetTileData.FindData("Leaf", atlas).Tile);
        Decoration.SetTile(pos + new Vector3Int(1, TreeHeight-1, 0), GetTileData.FindData("Leaf", atlas).Tile);
        Decoration.SetTile(pos + new Vector3Int(-1, TreeHeight - 1, 0), GetTileData.FindData("Leaf", atlas).Tile);
        Decoration.SetTile(pos + new Vector3Int(1, TreeHeight - 2, 0), GetTileData.FindData("Leaf", atlas).Tile);
        Decoration.SetTile(pos + new Vector3Int(-1, TreeHeight - 2, 0), GetTileData.FindData("Leaf", atlas).Tile);
    }

    void Pool(Vector3Int pos)
    {
        int width = Random.Range(3, 7);
        Vector3Int StartPos = pos;
        Vector3Int EndPos = pos + new Vector3Int(width, 0, 0);

        for (int i = pos.x; i < EndPos.x; i++)
        {
            Water.SetTile(new Vector3Int(i, pos.y, 0), GetTileData.FindData("Water", atlas).Tile);
        }
    }
}

public class ActiveTileData
{
    public bool canFall;

    public ActiveTileData(bool can)
    {
        canFall = can;
    }
}

public class ChunkData
{
    public Dictionary<Vector3Int, ActiveTileData> Data = new Dictionary<Vector3Int, ActiveTileData>();

    public ChunkData(Dictionary<Vector3Int, ActiveTileData> data)
    {
        Data = data;
    }
}
