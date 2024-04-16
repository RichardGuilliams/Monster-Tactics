using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        position = player.transform.position;
        position.z = -1;
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        position = player.transform.position;
        position.z = -1;
        transform.position = position;
    }
}
