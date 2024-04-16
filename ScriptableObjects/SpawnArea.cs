using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawn Area", menuName = "SpawnArea")]
public class SpawnArea : ScriptableObject
{
    public string spawnPointName;

    [System.Serializable]
    public class ResourceSpawn
    {
        public ResourceSpawn resource;
        public int weight;
    }

    // public List<ResourceSpawn> resources = new List<ResourceSpawn>();
    public List<ResourceSpawn> resources;
}
