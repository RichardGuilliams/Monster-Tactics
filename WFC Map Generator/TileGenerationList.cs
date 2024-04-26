using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile Generation List", menuName = "Tile Generation List")]
public class TileGenerationList : ScriptableObject{
    public List<EntropyData> possibleTiles = new List<EntropyData>();
    public List<TileGenerationData> tileGenerationList;
}