using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    private HingeJoint hinge;
    public float velocity = 90;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        var motor = hinge.motor;
        motor.force = 1000;
        motor.targetVelocity = velocity;
        motor.freeSpin = false;
        hinge.motor = motor;
        hinge.useMotor = true;
    }

}
