using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile Generation Data", menuName = "Tile Generation Data")]
public class TileGenerationData : ScriptableObject
{
    // Start is called before the first frame update
    public TileData tileData;
    public int weight;
    public List<EntropyData> possibleTiles = new List<EntropyData>();
    public List<GenerationType> generationTypes;
}

[Serializable]
public class EntropyData{
    public TileGenerationData tile;
    public int weight;

    public int neighborModifier;
    public  List<PlacementDirection> direction;
}

[Serializable]
public class GenerationConstraints{
    public Constraint constraint;

    public void EvalConstraint(){
        switch(constraint){
            case Constraint.Land: 
                LandEval();
                break;
            case Constraint.Tower: 
                TowerEval();
                break;
            case Constraint.Bridge: 
                BridgeEval();
                break;
            case Constraint.Road: 
                RoadEval();
                break;
        }
    }
    public void LandEval(){

    }
    public void TowerEval(){

    }
    public void RoadEval(){

    }
    public void BridgeEval(){

    }
}


public enum Constraint{
    Road,
    Tower,
    Bridge,
    Land,
    Water
}
public enum PlacementDirection {
    Left, Right, Up, Down, Above
}

public enum GenerationType{
    Land,
    Road,
    Tower,
    Bridge,
    River, 
    Lake
}

