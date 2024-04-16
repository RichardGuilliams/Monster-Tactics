using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Movement2D360 : MonoBehaviour
{
    public bool rotates;
    public KeyCode sprintKey;
    public Stats stats;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
                    // Get input from horizontal and vertical axes
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3();
        if(Input.GetKey(sprintKey)){
            movement = new Vector3(moveHorizontal, moveVertical, 0) * (stats.speed * 2) * Time.deltaTime;

        }
        else movement = new Vector3(moveHorizontal, moveVertical, 0) * stats.speed * Time.deltaTime;

        transform.Translate(movement);
    }
}
