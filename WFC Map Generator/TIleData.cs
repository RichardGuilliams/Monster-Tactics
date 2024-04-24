using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile Data", menuName = "Tile Data")]
public class TileData : ScriptableObject{
    public TileBase tileBase;
    public List<TileType> types;
    public Vector3Int position;
}

public enum TileType {
    Forest, Water, Grass, Dirt, Rock, Ice, Cliff, Door, Plant, Crystal, Lava, Fire, Earth, Air 
}