using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 600f;
    public bool isFacingRight = true;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    public float health = 100;
    private Animator anim;
    private Rigidbody2D rigi;
    private bool isGrounded = true;    
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        //Runing
        float move = Input.GetAxis("Horizontal");
        if (Mathf.Abs(move) > 0.3) move = 1 * Mathf.Sign(move);
//        anim.SetFloat("hSpeed", Mathf.Abs(move));
        rigi.velocity = new Vector2(move * speed, rigi.velocity.y);
        if (move > 0 && !isFacingRight)
            Flip();
        else if (move < 0 && isFacingRight)
            Flip();
        //Jumping
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
//        anim.SetBool("Ground", isGrounded);
//        anim.SetFloat("vSpeed", rigi.velocity.y);
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
            }            
        }
        
    }
    private void Update()
    {

    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Jump()
    {
        rigi.velocity = new Vector2(rigi.velocity.x, 0);
        rigi.AddForce(new Vector2(0, jumpForce));
    }

    public void GetDamage(int damage)
    {
        health -= damage;
    }
}
