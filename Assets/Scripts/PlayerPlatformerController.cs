using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    public float jumpSpeed = 7;
    public float maxSpeed = 7;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        // Just jump if on the ground.
        if(Input.GetButtonDown("Jump") && isGrounded)
        {   
            animator.SetTrigger("TakeOf");
        }

        if (isGrounded)
            animator.SetBool("IsJumping", false);
        else
            animator.SetBool("IsJumping", true);

        if (move.x == 0 )
        {
            animator.SetBool("IsWalking", false);
        }
        else
        {
            animator.SetBool("IsWalking", true);
        }

        //TODO: Create flipped sprite.
        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0f) : (move.x < 0f));
        if(flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("land"))
        {
            return;
        }

        targetVelocity = move * maxSpeed;
    }

    // Is called by the animation.
    private void Jump()
    {
        velocity.y = jumpSpeed;
    }

}
