using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData : ScriptableObject
{
    // Start is called before the first frame update
    public TileBase tile;
    public List<TileType> types;
    public List<EntropyData> possibleRightTiles;
    public int currentEntropy;
    public Vector3Int pos;
}
public enum TileType {
    Forest, Water, Grass, Dirt, Rock, Cliff, Door, Plant, Crystal, Lava, Fire, Earth, Air 
}
public class EntropyData{
    public TileData tile;
    public int entropy;
    public  PlacementDirection direction;
}

public enum PlacementDirection {
    Left, Right, Up, Down, Above
}
