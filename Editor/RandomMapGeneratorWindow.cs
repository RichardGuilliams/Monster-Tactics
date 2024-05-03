
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


 
