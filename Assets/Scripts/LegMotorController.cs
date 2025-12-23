using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMotorController : MonoBehaviour
{
    public HingeJoint2D leftLeg;
    public HingeJoint2D rightLeg;

    public float walkSpeed = 300f;
    public float torque = 1000f;

    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");

        if (input != 0)
        {
            SetMotor(leftLeg, -walkSpeed * input);
            SetMotor(rightLeg, walkSpeed * input);
        }
        else
        {
            SetMotor(leftLeg, 0);
            SetMotor(rightLeg, 0);
        }
    }

    void SetMotor(HingeJoint2D joint, float speed)
    {
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = speed;
        motor.maxMotorTorque = torque;
        joint.motor = motor;
    }
}
