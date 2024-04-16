
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// using System;

public class RandomMapGeneratorWindow : EditorWindow
{

    private List<TileGenerationData> tileList = new List<TileGenerationData>();
    private List<Vector3Int> existingTiles;
    private int seed;
    private TileGroupPrefab tileGroupPrefab;
    private Tilemap tilemap;
    private TileBase tile;

    private List<TileGenerationData> tiles;
    private int maxWidth;
    private int maxLength;
    private int maxHeight;
    
    [MenuItem("Window/RandomMapGenerator")]
    public static void ShowWindow(){
        EditorWindow.GetWindow<RandomMapGeneratorWindow>("RandomMapGenerator");
    }

    void OnGUI(){
        tileGroupPrefab = EditorGUILayout.ObjectField("TileGroupPrefabs", tileGroupPrefab, typeof(TileGroupPrefab), false) as TileGroupPrefab;
        maxLength = EditorGUILayout.IntField("MaxLength", maxLength);
        maxWidth = EditorGUILayout.IntField("MaxWidth", maxWidth);
        maxHeight = EditorGUILayout.IntField("MaxHeight", maxHeight);
        seed = EditorGUILayout.IntField("Seed", seed);
        if(GUILayout.Button("Generate")){
            GenerateMap();
        }
    }

    void GenerateMap(){
        Tilemap existingTilemap = FindObjectOfType<Tilemap>();
        if (existingTilemap != null)
        {
            // If a Tilemap GameObject exists, destroy it
            DestroyImmediate(existingTilemap.gameObject);
        }
        SetupTilemap();
        RunGenerationProcess();
    }

    void SetupTilemap(){
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

    void MakeCellList(TileGroup group){
        List<int> cellList = new List<int>();
        int count = 0;
        for(int i = 0; i < group.baseTiles.Count; i++){
            count += RNG.Int(group.baseTiles[i].minCellCount, group.baseTiles[i].maxCellCount);
            cellList.Add(count);
        }
        for(int i = 0; i < count; i++){
            int index = RNG.Int(0, cellList.Count - 1);
            tileList.Add(group.baseTiles[index]);
        }
    }

    void RunGenerationProcess(){
        Random.InitState(seed);
        List<TileGroup> tileGroups = tileGroupPrefab.tileGroups;
        tileList.Clear();
        foreach (TileGroup group in tileGroups){
            MakeCellList(group);
            tiles = group.baseTiles;
            GenerateTiles(tiles);
        }
    }

    void GenerateTiles(List<TileGenerationData> group){
        Vector3Int position = new Vector3Int(0, 0, 0);
        TileBase selectedTile = tilemap.GetTile(position);
        //iterate through all the tile types in the tile group

        for(int i = 0; group.Count > i; i++){
            position = new Vector3Int(0, 0, 0);
            //get the generation data for the selected tile
            tile = group[i].tile;
            TileGenerationData data = group[i];
            position.z = data.index; 
            int cellCount = RNG.Int(data.minCellCount, data.maxCellCount);
        
            generateCell(group[i], position);
        
            if(!selectedTile){
                selectedTile = tilemap.GetTile(position);
            }

        }

    }

    List<Vector3Int> createCoordinateList(Vector3Int position, int width, int length){
        List<Vector3Int> list = new List<Vector3Int>();
        //chose a position on the map. first if there are no cells in existingCells choose any point.
        position.x = RNG.Int(0, maxWidth);
        position.y = RNG.Int(0, maxLength);
        for(int i = position.x - width / 2; i <= position.x + width / 2; i++){
            for(int j = position.y - length / 2; j <= position.y + length / 2; j++){
                list.Add(new Vector3Int(i, j, 0));
            }
        }
        Debug.Log(list);
        // then choose only points on the map that have a missing neighboring tile.
        //fill the empty positions around the point chosen
        //find all existing tiles that posses at least one adjacent tile but do not have all four corners surrounded by tiles and 
        return list;

    }

    void generateCell(TileGenerationData group, Vector3Int position){
            // create random cell size to base the size of the tile cluster off of
            int width = RNG.Int(group.minCellWidth ,group.maxCellWidth);
            int height = RNG.Int(group.minCellLength ,group.maxCellLength);
            List<Vector3Int> coordinateList;
            coordinateList = createCoordinateList(position, width, height);
            int k = 0;
            while(k < coordinateList.Count){   
                    tilemap.SetTile(coordinateList[k], tile);
                    existingTiles.Add(coordinateList[k]);
                    k++;
            }
            coordinateList.Clear();
        }
    }

public static class RNG{
    public static int Int(int min, int max){
        return Random.Range(min, max);
    }
}

public enum Directions{
    None,
    Left,
    Right,
    Up,
    Down
}

public class Direction {
    public static Vector3Int Left = new Vector3Int(-1, 0, 0); 
    public static Vector3Int Right = new Vector3Int(1, 0, 0);
    public static Vector3Int Up  = new Vector3Int(0, 1, 0);
    public static Vector3Int Down = new Vector3Int(0, -1, 0);

    public static Vector3Int getDirection(Directions direction){
        if(direction == Directions.Left) return Left;
        if(direction == Directions.Right) return Right;
        if(direction == Directions.Up) return Up;
        if(direction == Directions.Down) return Down;
        return new Vector3Int(0, 0, 0);
    }

}

