using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DuckHeadJoint : MonoBehaviour
{
    [Header("머리랑 이어지는 몸통")]
    public Rigidbody2D body;
    private HingeJoint2D joint;

    void Awake()
    {
        joint = gameObject.AddComponent<HingeJoint2D>();
        joint.connectedBody = body;

        joint.enableCollision = false;
        joint.useLimits = true;
        JointAngleLimits2D limits = new JointAngleLimits2D();
        limits.min = -5f;
        limits.max = 5f;
        joint.limits = limits;
    }
}
