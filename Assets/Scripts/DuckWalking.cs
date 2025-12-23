using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckWalking : MonoBehaviour
{
    [Header("게임 설정")]
    public bool gameStarted = false;
    
    [Header("걷기 설정")]
    public float walkSpeed = 2f;         
    public float stepHeight = 50f;        
    public float stepDuration = 0.8f;     
    public float forwardForce = 100f;      
    
    [Header("다리 레퍼런스")]
    public HingeJoint2D leftLeg;           
    public HingeJoint2D rightLeg;          
    public Rigidbody2D leftLegRb;        
    public Rigidbody2D rightLegRb;        
    
    private Rigidbody2D bodyRb;        
    private float stepTimer = 0f;         
    private bool isLeftStep = true;        
    
    void Start()
    {
        bodyRb = GetComponent<Rigidbody2D>();
        
        if (leftLeg == null || rightLeg == null)
        {
            Transform leftLegTransform = transform.Find("LeftLeg");
            Transform rightLegTransform = transform.Find("RightLeg");
            
            if (leftLegTransform != null)
            {
                leftLeg = leftLegTransform.GetComponent<HingeJoint2D>();
                leftLegRb = leftLegTransform.GetComponent<Rigidbody2D>();
                Debug.Log("왼쪽 다리 연결 완료");
            }
            
            if (rightLegTransform != null)
            {
                rightLeg = rightLegTransform.GetComponent<HingeJoint2D>();
                rightLegRb = rightLegTransform.GetComponent<Rigidbody2D>();
                Debug.Log("오른쪽 다리 연결 완료");
            }
            
            if (leftLeg == null || rightLeg == null)
            {
                HingeJoint2D[] joints = GetComponentsInChildren<HingeJoint2D>();
                foreach (HingeJoint2D joint in joints)
                {
                    if (joint.gameObject.name == "LeftLeg")
                    {
                        leftLeg = joint;
                        leftLegRb = joint.GetComponent<Rigidbody2D>();
                        Debug.Log("LeftLeg 연결");
                    }
                    else if (joint.gameObject.name == "RightLeg")
                    {
                        rightLeg = joint;
                        rightLegRb = joint.GetComponent<Rigidbody2D>();
                        Debug.Log("RightLeg 연결");
                    }
                }
            }
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !gameStarted)
        {
            StartGame();
        }
        
        if (gameStarted)
        {
            HandleWalking();
        }
    }
    
    void StartGame()
    {
        gameStarted = true;
        Debug.Log("게임 시작");
    }
    
    void HandleWalking()
    {
        bodyRb.AddForce(Vector2.right * forwardForce, ForceMode2D.Force);
        
        Debug.Log("앞으로 가는 힘 적용 됨: " + forwardForce);
        
        stepTimer += Time.deltaTime;
        
        if (stepTimer >= stepDuration)
        {
            TakeStep();
            stepTimer = 0f;
            isLeftStep = !isLeftStep;
        }
    }
    
    void TakeStep()
    {
        if (isLeftStep)
        {
            StepWithLeg(leftLegRb, leftLeg);
            Debug.Log("왼쪽 다리 걸음");
        }
        else
        {
            StepWithLeg(rightLegRb, rightLeg);
            Debug.Log("오른쪽 다리 걸음");
        }
    }
    
    void StepWithLeg(Rigidbody2D legRb, HingeJoint2D legJoint)
    {
        if (legRb != null && legJoint != null)
        {
            legRb.AddForce(Vector2.up * stepHeight, ForceMode2D.Impulse);
            

            legRb.AddForce(Vector2.right * forwardForce * 0.5f, ForceMode2D.Impulse);
            

            JointMotor2D motor = new JointMotor2D
            {
                maxMotorTorque = 200f
            };
            legJoint.motor = motor;
            legJoint.useMotor = true;
            
            Invoke(nameof(StopLegMotor), 0.3f);
        }
    }
    
    void StopLegMotor()
    {
        if (leftLeg != null) leftLeg.useMotor = false;
        if (rightLeg != null) rightLeg.useMotor = false;
    }
    
    void FixedUpdate()
    {
        if (gameStarted)
        {

            if (Mathf.Abs(transform.rotation.z) > 0.3f)
            {
                float stabilizeForce = -transform.rotation.z * 200f;
                bodyRb.AddTorque(stabilizeForce, ForceMode2D.Force);
            }
        }
    }
    
  
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "리셋 (R)") || Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
        
        if (!gameStarted)
        {
            GUI.Label(new Rect(Screen.width/2 - 100, Screen.height/2, 200, 30), "엔터: 게임 시작");
        }

        if (gameStarted)
        {
            GUI.Label(new Rect(10, 50, 300, 20), "속도: " + bodyRb.velocity.ToString());
        }
    }
    
    void ResetGame()
    {
        gameStarted = false;
        stepTimer = 0f;
        isLeftStep = true;
        

        transform.position = new Vector3(0, 2, 0);
        transform.rotation = Quaternion.identity;
        
 
        Rigidbody2D[] allRb = GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D rb in allRb)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}