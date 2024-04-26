
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
}

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
        foreach(EntropyData data in currentCell.possibleTiles){
            weightSum += data.weight;
        }
        int currentWeight = Random.Range(0, weightSum);
        for(int i = 0; i < currentCell.possibleTiles.Count; i++){
            currentWeight -= currentCell.possibleTiles[i].weight;
            if(currentWeight <= 0) return currentCell.possibleTiles[i];
        }
        return currentCell.possibleTiles[Random.Range(0, currentCell.possibleTiles.Count - 1)];
    }
    //Set the random tile from the map
    public void SetTile(){
        currentCell.tile = GetRandomTile().tile;
        tilemap.SetTile(currentCell.cellPosition, currentCell.tile.tileData.tileBase);
    }
    // recalculate the neighboring cells entropy.
    public void RecalculateEntropyData(){
        //set current cells entropy to 0;
        currentCell.entropy = 0;

        //get list of neighboring cells.
        // for each neighboring cell 
        List<Cell> neighboringCells = currentCell.NeighboringCells();
        foreach(Cell cell in currentCell.NeighboringCells()){
            // - get the possible tiles list for that tiles neighbors.
            List<List<EntropyData>> possibleTiles = new List<List<EntropyData>>();
            List<EntropyData> commonTiles = new List<EntropyData>();
            foreach(Cell neighborCell in cell.NeighboringCells()){
                // add the possibletiles of the neighborCell to the possibleTiles
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

    }
    // repeat cycle until all cells have an entropy of 0;
    public void collapseAllCells(){
        //
        collapseFirstCell();
        // iterate through the list until all tiles have been assigned
        while(!AllTilesAssigned() && loop){
            GetRandomCellWithLowestEntropy();
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
 
public class Cell{
    public TileGenerationData tile = new TileGenerationData();
    public Vector3Int cellPosition;
    public int entropy;
    public List<EntropyData> possibleTiles;
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
        possibleTiles = tileList.possibleTiles;
        tile = tileList.tileGenerationList[0];
        entropy = possibleTiles.Count;
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