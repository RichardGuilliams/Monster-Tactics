
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class RandomMapGeneratorWindow : EditorWindow
{
    public WFCGenerator wFCGenerator;

    private TileGenerationList tiles;
    // private List<Cell> cells;
    // private TileGenerationData currentTile;
    private int seed;
    private Tilemap tilemap;
    private int maxWidth;
    private int maxLength;
    // private Vector3Int selectedPosition = new Vector3Int(0, 0, 0);
    private int maxHeight;
    // private int[,,] map;
    
    [MenuItem("Window/RandomMapGenerator")]
    public static void ShowWindow(){
        EditorWindow.GetWindow<RandomMapGeneratorWindow>("RandomMapGenerator");
    }

    void OnGUI(){
        tiles = EditorGUILayout.ObjectField("TileGenerationLists", tiles, typeof(TileGenerationList), false) as TileGenerationList;
        maxLength = EditorGUILayout.IntField("MaxLength", maxLength);
        maxWidth = EditorGUILayout.IntField("MaxWidth", maxWidth);
        maxHeight = EditorGUILayout.IntField("MaxHeight", maxHeight);
        seed = EditorGUILayout.IntField("Seed", seed);
        tilemap = FindObjectOfType<Tilemap>();
        SetupGenerator();
        if(GUILayout.Button("Generate")){
            SetupTilemap();
            wFCGenerator.setup(tilemap, tiles);
            wFCGenerator.collapseAllCells();
            // AssignEntropyMap();
            // ChooseRandomPosition();
            // CollapseFirstCell();
            // CollapseCells();
        }
    }

    void SetupGenerator(){
        wFCGenerator = new WFCGenerator(tiles, tilemap, maxWidth, maxLength, maxHeight);
        wFCGenerator.InitRandomSeed(seed);
    }

    void SetupTilemap(){
        tilemap = FindObjectOfType<Tilemap>();
        if(tilemap != null){
            DestroyImmediate(tilemap.gameObject);
        }
        GameObject tilemapObject = new GameObject("Tilemap");
        tilemapObject.AddComponent<Grid>();
        tilemapObject.GetComponent<Grid>().cellSize = new Vector3(1, 0.5f, 1);
        tilemapObject.GetComponent<Grid>().cellLayout = GridLayout.CellLayout.IsometricZAsY;// Optional: Adds a collider to the tilemap

        tilemap = tilemapObject.AddComponent<Tilemap>();
        tilemap.gameObject.AddComponent<TilemapRenderer>();
        tilemap.gameObject.GetComponent<TilemapRenderer>().mode = TilemapRenderer.Mode.Individual;
        tilemap.gameObject.GetComponent<TilemapRenderer>().sortOrder = TilemapRenderer.SortOrder.TopRight;
        tilemap.GetComponent<Grid>().cellLayout = GridLayout.CellLayout.IsometricZAsY;
        DestroyImmediate(tilemapObject.GetComponent<Rigidbody2D>());
        DestroyImmediate(tilemapObject.GetComponent<CompositeCollider2D>());
    }

    // public void AssignEntropyMap(){
    //     map = new int[maxWidth, maxLength, maxHeight];
    //     Random.InitState(seed);
    //     for(int i = 0; i < maxWidth; i++){
    //         for(int j = 0; j < maxLength; j++){
    //             for(int k = 0; k < maxHeight; k++){
    //                 // Locking the generation to the lowest z axis
    //                 if(k == 0){
    //                     map[i, j, k] = tiles.tileGenerationList.Count;
    //                 }
    //             }   
    //         }       
    //     }
    // }

    // public void ChooseRandomPosition(){
    //     selectedPosition.x = Random.Range(0, maxHeight - 1);
    //     selectedPosition.y = Random.Range(0, maxWidth - 1);
    // }

    // public TileGenerationData GetDataByWeight(){
    //     int weightSum = 0;
    //     foreach(TileGenerationData tile in tiles.tileGenerationList) {
    //         weightSum += tile.weight;
    //     }
    //     int currentWeight = Random.Range(0, weightSum);
    //     for(int i = 0;  i < tiles.tileGenerationList.Count; i++){
    //         currentWeight -= tiles.tileGenerationList[i].weight;
    //         if(currentWeight <= 0) return tiles.tileGenerationList[i];
    //     }
    //     return new TileGenerationData();
    // }

    // public void GetRandomTile(){
        
    // }

    // public void GetEntropyData(){
        
    // }

    // public Vector3Int GetRandomMapPosition(){
    //     int x = Random.Range(0, maxWidth);
    //     int y = Random.Range(0, maxLength);
    //     int z = Random.Range(0, 0);
    //     return new Vector3Int(x, y, z);
    // }
    // public void CollapseFirstCell(){
    //     Vector3Int position = GetRandomMapPosition();
    //     currentTile = GetDataByWeight();
    //     tilemap.SetTile(position, currentTile.tileData.tileBase);
    //     map[position.x, position.y, position.z] = 0;
    //     RecalculateEntropyData();
    // }

    // public void AssignEntropyAtPosition(Vector3Int position, int entropy){
    //     map[position.x, position.y, position.z] = entropy;
    // }
    // public TileGenerationList RecalculateEntropyData(){
    //     // from selected position, readjust the entropy of the surrounding cells that have not been collapsed.;
    //     Vector3Int left = new Vector3Int(selectedPosition.x - 1, selectedPosition.y, 0);
    //     Vector3Int right = new Vector3Int(selectedPosition.x + 1, selectedPosition.y, 0);
    //     Vector3Int up = new Vector3Int(selectedPosition.x, selectedPosition.y - 1, 0);
    //     Vector3Int down = new Vector3Int(selectedPosition.x, selectedPosition.y + 1, 0);
    //     if(left.x > 0){
    //         AssignEntropyAtPosition(left, currentTile.possibleTiles.Count);
    //     }
    //     if(right.x < maxWidth){
    //         AssignEntropyAtPosition(right, currentTile.possibleTiles.Count);
    //     }
    //     if(down.y > 0){
    //         AssignEntropyAtPosition(up, currentTile.possibleTiles.Count);
    //     }
    //     if(up.y < maxHeight){
    //         AssignEntropyAtPosition(down, currentTile.possibleTiles.Count);
    //     }
    //     TileGenerationList newGenerationList = new TileGenerationList();
    //     foreach(EntropyData data in currentTile.possibleTiles){
    //         newGenerationList.tileGenerationList.Add(data.tile);
    //     }
    //     return newGenerationList;
    // }

    // public List<Vector3Int> GetPositionsWithLowestEntropy(){
    //     List<Vector3Int> newList = new List<Vector3Int>();
    //     int currentLowest = tiles.tileGenerationList.Count;
    //     for(int i = 0; i < maxWidth; i++){
    //         for(int j = 0; j < maxLength; j++){
    //             for(int k = 0; k < maxHeight; k++){
    //                 if(currentLowest > map[i, j, k] && map[i, j, k] != 0) currentLowest = map[i, j, k];
    //             }
    //         }
    //     }
    //     for(int i = 0; i < maxWidth; i++){
    //         for(int j = 0; j < maxLength; j++){
    //             for(int k = 0; k < maxHeight; k++){
    //                 if(map[i, j, k] == currentLowest) newList.Add(new Vector3Int(i, j, k));
    //             }
    //         }
    //     }
    //     return newList;
    // }

    // public void SelectNextTilePosition(){
    //     List<Vector3Int> LowestEntropyPositions = GetPositionsWithLowestEntropy();
    //     selectedPosition = LowestEntropyPositions[Random.Range(0, LowestEntropyPositions.Count - 1)];
    //     // from selectionPosition we need to get the tileGenerationDataAssociated with the tile in the selected position.
    //     // We need generate a new tileGenerationList from the tiles in the entropy data
    //     // 
    // }
    // public void CollapseCells(){
    //     for(int i = 0; i < maxWidth; i++){
    //         for(int j = 0; j < maxLength; j++){
    //             for(int k = 0; k < maxHeight; k++){
    //                 SelectNextTilePosition();
    //             }
    //         }
    //     }
    //     //assign one of the tiles from the list to the currentTile.
    //     GetRandomTile();
        // currentTile = tiles.tileGenerationList[3];
    // }
}

public class WFCGenerator{
    public TileGenerationList tiles { get; set;}
    public Tilemap tilemap { get; set;}
    public List<Cell> cells = new List<Cell>();
    public Cell currentCell;
    public int mapWidth;
    public int mapLength;
    public int mapHeight;

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
            for(int j = 0; j < mapWidth - 1; j++){
                for(int k = 0; k < mapLength - 1; k++){
                    cells.Add(new Cell(new Vector3Int(j, k, i), tiles));
                }
            }

        }
    }

    public void SetCellNeighbors(){
        foreach(var cell in cells){
            cell.leftCell = cells.Find(c => c.cellPosition == new Vector3Int(cell.cellPosition.x - 1, cell.cellPosition.y, cell.cellPosition.z));
            cell.rightCell = cells.Find(c => c.cellPosition == new Vector3Int(cell.cellPosition.x + 1, cell.cellPosition.y, cell.cellPosition.z));
            cell.upCell = cells.Find(c => c.cellPosition == new Vector3Int(cell.cellPosition.x, cell.cellPosition.y - 1, cell.cellPosition.z));
            cell.downCell = cells.Find(c => c.cellPosition == new Vector3Int(cell.cellPosition.x, cell.cellPosition.y + 1, cell.cellPosition.z));
            cell.aboveCell = cells.Find(c => c.cellPosition == new Vector3Int(cell.cellPosition.x, cell.cellPosition.y, cell.cellPosition.z + 1));
        }
    }


    public List<Cell> CellsWithLowestEntropy(){
        int minEntropy = cells.Count;
        foreach(Cell cell in cells){
            if(cell.entropy > 0 && cell.entropy < minEntropy) minEntropy = cell.entropy;
        }
        List<Cell> cellsWithLowestEntropy = cells.Where(cell => cell.entropy == minEntropy).ToList();
        return cellsWithLowestEntropy;
    }
    
    //Pick random cell from list of cells with lowest entropy as long as entropy is not 0;
    public void GetRandomCellWithLowestEntropy(){
        currentCell = CellsWithLowestEntropy()[Random.Range(0, CellsWithLowestEntropy().Count)];
    }
    // pick random tile from current tiles possibleTiles.
    public TileGenerationData GetRandomTile(){
        return currentCell.possibleTiles.tileGenerationList[Random.Range(0, currentCell.possibleTiles.tileGenerationList.Count - 1)];
    }
    //Set the random tile from the map
    public void SetTile(){
        currentCell.tile = GetRandomTile();
        tilemap.SetTile(currentCell.cellPosition, currentCell.tile.tileData.tileBase);
    }
    // recalculate the neighboring cells entropy.
    public void RecalculateEntropyData(){
        List<TileGenerationData> neighboringTiles = new List<TileGenerationData>();
        // find all neighboring cells that are not assigned
        foreach(Cell cell in currentCell.NeighboringCells()){
            // find the tileBase for this cell
                if(!cell.assigned){
                cell.tile = GetRandomTile();
                TileBase tile = cell.tile.tileData.tileBase;
                neighboringTiles.Add(tiles.tileGenerationList.Find(t => t.tileData.tileBase == tile));
            }
        };
        // get the lists of the possible tiles from all TileGenerationData in neighboring cells for each tile.
        List<List<EntropyData>> possibleTilesList = new List<List<EntropyData>>();
        foreach(TileGenerationData tile in neighboringTiles){
            possibleTilesList.Add(tile.possibleTiles);
        }
        // get list of all possible tiles that exist in all lists
        List<EntropyData> commonTiles = possibleTilesList[0];
        for(int i = 1; i < possibleTilesList.Count - 1; i++){
            commonTiles = commonTiles.Intersect(possibleTilesList[i]).ToList();
        }
        // get a list of all generationData from the common tiles and set in to the possibleTiles list for the currentCell;
        TileGenerationList newGenerationList = new TileGenerationList();
        newGenerationList.tileGenerationList = new List<TileGenerationData>();
        foreach(EntropyData entropyData in commonTiles){
            newGenerationList.tileGenerationList.Add(entropyData.tile);
        }
        currentCell.possibleTiles = newGenerationList;
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
        currentCell.tile = GetRandomTile();
        tilemap.SetTile(currentCell.cellPosition, currentCell.tile.tileData.tileBase);
        RecalculateEntropyData();
        currentCell.entropy = 0;
        currentCell.assigned = true;

    }
    // repeat cycle until all cells have an entropy of 0;
    public void collapseAllCells(){
        //
        collapseFirstCell();
        // iterate through the list until all tiles have been assigned
        while(!AllTilesAssigned()){
            GetRandomCellWithLowestEntropy();
            SetTile();
            RecalculateEntropyData();
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
 
public class Cell{
    public TileGenerationData tile = new TileGenerationData();
    public Vector3Int cellPosition;
    public int entropy;
    public TileGenerationList possibleTiles;
    public List<Cell> neighbors = new List<Cell>();
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
        possibleTiles = tileList;
        entropy = possibleTiles.tileGenerationList.Count;
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