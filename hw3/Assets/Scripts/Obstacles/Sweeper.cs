using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweeper : MonoBehaviour
{
    public string axis;
    public float min;
    public float max;
    public bool moveRightFirst;

    void Start () 
    {
        if(axis == "x")
        {
            min = min + transform.position.x;
            max = max + transform.position.x;
        }
        
        if(axis == "z")
        {
            min = min + transform.position.z;
            max = max + transform.position.z;
        }
   
    }
    void Update () 
    {
        if(axis == "x")
            transform.position = new Vector3(Mathf.PingPong(Time.time * 2, max - min) + min , transform.position.y, transform.position.z);
        
        int temp = 1;
        if(axis == "z"){
            if(moveRightFirst)
                temp = -1;
            transform.position = new Vector3(transform.position.x , transform.position.y, temp*Mathf.PingPong(Time.time * 2, max - min) + min);
        }   
    }
}
