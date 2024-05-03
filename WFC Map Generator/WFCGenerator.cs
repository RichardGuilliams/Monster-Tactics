using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Data.Common;
public class WFCGenerator{
    public TileGenerationList tiles { get; set;}
    public Tilemap tilemap { get; set;}
    public List<Cell> cells = new List<Cell>();
    public Cell currentCell;
    public int mapWidth;
    public int mapLength;
    public int mapHeight;

    public bool loop = true;

    public void InitRandomSeed(int seed){
        Random.InitState(seed);
    }

    public void setup(Tilemap map, TileGenerationList tileList){
        tiles = tileList;
        tilemap = map;
        InitializeCells();
        SetCellNeighbors();
    }

    public void InitializeCells(){
        for(int i = 0; i < mapHeight - 1; i++){
            for(int j = 0; j < mapLength - 1; j++){
                for(int k = 0; k < mapWidth - 1; k++){
                    cells.Add(new Cell(new Vector3Int(j, k, i), tiles));
                }
            }
        }
    }

    public void SetCellNeighbors(){
        foreach(var cell in cells){
            cell.leftCell = cells.Find(c => c.cellPosition == new Vector3Int(cell.cellPosition.x - 1, cell.cellPosition.y, cell.cellPosition.z));
            cell.rightCell = cells.Find(c => c.cellPosition == new Vector3Int(cell.cellPosition.x + 1, cell.cellPosition.y, cell.cellPosition.z));
            cell.upCell = cells.Find(c => c.cellPosition == new Vector3Int(cell.cellPosition.x, cell.cellPosition.y + 1, cell.cellPosition.z));
            cell.downCell = cells.Find(c => c.cellPosition == new Vector3Int(cell.cellPosition.x, cell.cellPosition.y - 1, cell.cellPosition.z));
            cell.aboveCell = cells.Find(c => c.cellPosition == new Vector3Int(cell.cellPosition.x, cell.cellPosition.y, cell.cellPosition.z - 1));
        }
    }


    public List<Cell> CellsWithLowestEntropy(){
        int minEntropy = tiles.tileGenerationList.Count;
        foreach(Cell cell in cells){
            if(cell.entropy > 0 && cell.entropy < minEntropy) minEntropy = cell.entropy;
        }
            List<Cell> cellsWithLowestEntropy = cells.Where(cell => cell.entropy == minEntropy).ToList();
        return cellsWithLowestEntropy;
    }
    
    //Pick random cell from list of cells with lowest entropy as long as entropy is not 0;
    public void GetRandomCellWithLowestEntropy(){
        if(CellsWithLowestEntropy().Count > 0){
            currentCell = CellsWithLowestEntropy()[Random.Range(0, CellsWithLowestEntropy().Count)];
        }
    }
    // pick random tile from current tiles possibleTiles.
    public EntropyData GetRandomTile(){
        int weightSum = 0;
        foreach(int weight in currentCell.weightList){
            // Run logic checks to determine weight modifiers for each possible tile.
            // Based on neighbors.
            weightSum += weight;
        }
        int currentWeight = Random.Range(0, weightSum);
        for(int i = 0; i < currentCell.possibleTiles.Count; i++){
            currentWeight -= currentCell.weightList[i];
            if(currentWeight <= 0) return currentCell.possibleTiles[i];
        }
        return currentCell.possibleTiles[Random.Range(0, currentCell.possibleTiles.Count - 1)];
    }
    //Set the random tile from the map
    public void SetTile(){
        // Get A list of TileBases from the neighboring tiles
        List<TileBase> tileBases = new List<TileBase>();
        foreach(Cell cell in currentCell.NeighboringCells()){
            if(cell.tileBase != null){
                tileBases.Add(cell.tileBase);
            }
        }
        // iterate over the possible tiles and adjust their weight according to the count
        // of matching tiles in tileBases
        for(int i = 0; i < currentCell.possibleTiles.Count; i++){
            EntropyData data = currentCell.possibleTiles[i]; 
            if(tileBases.Contains(data.tile.tileData.tileBase)){
            int weight = tileBases.Count(item => item == data.tile.tileData.tileBase);
                currentCell.weightList[i] *= weight * data.neighborModifier;
            }
        }
        currentCell.tile = GetRandomTile().tile;
        currentCell.tileBase = currentCell.tile.tileData.tileBase;
        tilemap.SetTile(currentCell.cellPosition, currentCell.tile.tileData.tileBase);
    }
    // recalculate the neighboring cells entropy.
    public void RecalculateEntropyData(){
        //set current cells entropy to 0;
        currentCell.entropy = 0;

        //get list of neighboring cells.
        List<Cell> neighboringCells = currentCell.NeighboringCells();
        // for each neighboring cell 
        foreach(Cell cell in currentCell.NeighboringCells()){
            // - get the possible tiles list for that tiles neighbors.
            List<List<EntropyData>> possibleTiles = new List<List<EntropyData>>();  
            List<EntropyData> commonTiles = new List<EntropyData>();
            foreach(Cell neighborCell in cell.NeighboringCells()){
                // add the possibleTiles of the neighborCell to the possibleTiles
                possibleTiles.Add(neighborCell.possibleTiles);
                // - get the common tiles from each list of possible tiles.
            }
            // get list of all possible tiles that exist in all lists
            commonTiles = possibleTiles[0];
            for(int i = 1; i < possibleTiles.Count - 1; i++){
                commonTiles = commonTiles.Intersect(possibleTiles[i]).ToList();
            }
            // - assign the common tiles to the possiible tiles list of cell in loop.
            cell.possibleTiles = commonTiles;
            cell.entropy = cell.possibleTiles.Count;
            // - assign the entropy data of the cell in loop to its possibleTiles.Count.
        }

    }

    public bool AllTilesAssigned(){
        bool allAssigned = true;
        foreach(Cell cell in cells){
            if(!cell.blank && !cell.assigned){
                allAssigned = false;
            }
        }
        return allAssigned;
    }

    public void collapseFirstCell(){
        // 
        List<Cell> lowestHeightCells = cells.Where(cell => cell.cellPosition.z == 0).ToList();
        // get a random cell from the new List
        currentCell = lowestHeightCells[Random.Range(0, lowestHeightCells.Count - 1)];
        currentCell.tile = GetRandomTile().tile;
        tilemap.SetTile(currentCell.cellPosition, currentCell.tile.tileData.tileBase);
        currentCell.entropy = 0;
        currentCell.assigned = true;
        RecalculateEntropyData();

    }    // repeat cycle until all cells have an entropy of 0;
    public void collapseAllCells(){
        //
        collapseFirstCell();
        // iterate through the list until all tiles have been assigned
        while(!AllTilesAssigned() && loop){
            // loop = false;
            GetRandomCellWithLowestEntropy();
            currentCell.SetupWeightList();
            SetTile();
            RecalculateEntropyData();
            currentCell.entropy = 0;
            currentCell.assigned = true;
        }
        
    }

    public WFCGenerator(TileGenerationList list, Tilemap map, int maxWidth, int maxLength, int maxHeight){
        tiles = list;
        tilemap = map;
        mapWidth = maxWidth;
        mapLength = maxLength;
        mapHeight = maxHeight;
    }
}