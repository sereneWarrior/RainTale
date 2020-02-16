using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    public float minGrounNormalY = 0.65f;
    public float gravityModifier = 1f; // FixedUpdate


    protected Vector2 targetVelocity;
    protected bool isGrounded;
    protected Vector2 groundNormal;

    protected Rigidbody2D rigidbody2D; //general
    protected Vector2 velocity;//FixedUpdate

    //Movement(vector2 move)
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    protected const float minMoveDistance = 0.001f; 
    protected const float shellRadius = 0.01f; //avoid getting stuc in another collider
    protected  List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask (gameObject.layer));//uses physics 2D settings to determine what layers to check collision against
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        targetVelocity = Vector2.zero;//not use targetVelocity from the last frame
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity(){}
    void OnEnable()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity *Time.deltaTime;
        velocity.x = targetVelocity.x; //horizontal movement

        isGrounded = false;

        Vector2 deltaPostion = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x); //vector perpendicular to the groundNormal, works on slopes
        
        
        Vector2 move = moveAlongGround * deltaPostion.x;

        Movement(move,false);
        
        //vector2.up == Vector2(0,1)
        move = Vector2.up * deltaPostion.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        
        //if my distance is above the minvalue 
        //check for collision
        if(distance > minMoveDistance)
        {
            //public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, float distance = Mathf.Infinity); 
            int collisionCount = rigidbody2D.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            //insert indices of array that indicate an actual hit
            for ( int i = 0; i < collisionCount; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            //calculate angle of collision impact
            for (int i = 0; i < hitBufferList.Count; i++)
            {
                //compare normal with predefined min value = minGroundNormalY
                //can the other object be considered as ground
                //doesnt works on slopes
                Vector2 currentNormal = hitBufferList[i].normal;
          
                
                if (currentNormal.y > minGrounNormalY)
                {
                    isGrounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
               
                //get difference between velocty & currentnormal 
                //get if there needs to be an adjustment done for velocity
                float projection = Vector2.Dot(velocity, currentNormal);
                Debug.Log(velocity);
                if ( projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

        }
        rigidbody2D.position += move.normalized  * distance;
    }

    
}