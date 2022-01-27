using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public ThirdPersonController controller;
    
    void OnTriggerEnter()
    {
        controller.passLine = true;
    }
}
