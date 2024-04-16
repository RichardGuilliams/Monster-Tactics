using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource Node", menuName = "Resource Node")]
public class ResourceNode: ScriptableObject{
    public string resourceName;
    [System.Serializable]
    public class ResourceType{
        public Resource resource;
        public int min;
        public int max;
    }

    public List<ResourceType> resources = new List<ResourceType>();
    

}

