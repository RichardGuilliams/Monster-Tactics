using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell{
    public TileGenerationData tile = new TileGenerationData();
    public TileBase tileBase;
    public Vector3Int cellPosition;
    public int entropy;
    public List<EntropyData> possibleTiles = new List<EntropyData>(); 
    public List<Cell> neighbors = new List<Cell>();
    public List<int> weightList = new List<int>();
    public Cell leftCell;
    public Cell rightCell;
    public Cell upCell;
    public Cell downCell;
    public Cell aboveCell;
    public bool assigned = false;
    public bool blank = false;
    public int blankCellWeight;

    public Cell(Vector3Int position, TileGenerationList tileList){
        cellPosition = position;
        possibleTiles = new List<EntropyData>(tileList.possibleGenerationTiles);
        tile = tileList.tileGenerationList[0];
        entropy = possibleTiles.Count;
        SetupWeightList();
    }

    public void SetupWeightList(){
        weightList = new List<int>();
        foreach(EntropyData data in possibleTiles){
            weightList.Add(data.weight);
        };
    }

    public List<Cell> NeighboringCells(){
        List<Cell> list = new List<Cell>();
        if(leftCell != null) list.Add(leftCell);
        if(rightCell != null) list.Add(rightCell);
        if(upCell != null) list.Add(upCell);
        if(downCell != null) list.Add(downCell);
        if(aboveCell != null) list.Add(aboveCell);
        return list;
    }
}