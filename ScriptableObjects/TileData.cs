using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile Data", menuName = "Tile Data")]
public class TileData : ScriptableObject
{
    public string tileName;
    public TileBase tile;
    public float movementCost;

    public bool canBuildOn;
    public Element element;
    
}

public enum Element{
    Fire, Water, Air, Earth, Light, Dark
}