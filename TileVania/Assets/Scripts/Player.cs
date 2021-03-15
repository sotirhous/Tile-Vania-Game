using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);

    // State
    bool isAlive = true;
    
    // Cached component references
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;
    float gravityScaleAtStart;

    // Message them methods
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        myFeet = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }

        Run();
        Jump();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void Run()
    {
        float controlThrow = Input.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = (Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon);
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void ClimbLadder()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            myAnimator.SetBool("Climbing", false);
            myRigidbody.gravityScale = gravityScaleAtStart;
            return; 
        }

        float controlThrow = Input.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, controlThrow * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing",true);
    }

    private void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        


        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidbody.velocity += jumpVelocityToAdd;
        }
    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = (Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon);
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x),1f);
        }
    }

    
}
