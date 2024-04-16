using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile Group", menuName = "Tile Group")]
public class TileGroupPrefab : ScriptableObject
{
    public List<TileGroup> tileGroups;
}

    [System.Serializable]
    public class TileGenerationData{
        public TileBase tile;

        public int minCellWidth;
        public int maxCellWidth;
        public int minCellLength;
        public int maxCellLength;

        public int minCellCount;
        public int maxCellCount;
        public int index;
        public int minPathSize;
        public int maxPathSize;

        public int minPathCount;
        public int maxPathCount;
        public bool vertical;
        public int weight;
    }

[System.Serializable]
public class TileGroup{
    public string tileGroupName;
    public List<TileGenerationData> baseTiles;
    public int minBaseTiles;
    public int maxBaseTiles;
}