using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class ScalingAnimation : MonoBehaviour
{
    public float scaleRate;
    public UnityEngine.Vector3 minScale;
    public UnityEngine.Vector3 maxScale;
    private UnityEngine.Vector3 scale;
    private bool scaleUp;
    // Start is called before the first frame update
    void Start()
    {
        scale.x = Random.Range(minScale.x, maxScale.x);
        scale.y = Random.Range(minScale.y, maxScale.y);
        transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        if(scaleUp){
            if(scale.x < maxScale.x || scale.y < maxScale.y){
                scale.x += scaleRate * Time.deltaTime;
                scale.y += scaleRate * Time.deltaTime;
                transform.localScale = scale;
            }
            else{
                scaleUp = false;
            }

        }
        else{
            if(scale.x > minScale.x || scale.y > minScale.y){
                scale.x -= scaleRate * Time.deltaTime;
                scale.y -= scaleRate * Time.deltaTime;
                transform.localScale = scale;
            }
            else{
                scaleUp = true;
            }
        }
    }
}
