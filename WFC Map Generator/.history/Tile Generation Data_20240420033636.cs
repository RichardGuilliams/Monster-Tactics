using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile Generation Data", menuName = "Tile Generation Data")]
public class TileGenerationData : ScriptableObject
{
    // Start is called before the first frame update
    public TileData tile;
    public List<EntropyData> possibleTiles;
}

[System.Serializable]
public class EntropyData{
    public TileData tile;

    public int weight;
    public  List<PlacementDirection> direction;
}

public enum PlacementDirection {
    Left, Right, Up, Down, Above
}
