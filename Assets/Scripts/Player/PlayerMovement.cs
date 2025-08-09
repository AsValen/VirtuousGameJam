using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight =8f;

    private DashAbility dash;
    private float horizontalInput;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        dash = GetComponent<DashAbility>();
    }    

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        //if (!dash.IsDashing)
        //{
        //    rigidBody.linearVelocity = new Vector2(horizontalInput * moveSpeed, rigidBody.linearVelocity.y);
        //}
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if(horizontalInput > 0.01f) //Sprite turn right
        {
            sr.flipX = false;
        }
        else if(horizontalInput < -0.01f) //Sprite turn left
        {
            sr.flipX = true;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        //Animation
        anim.SetBool("isRunning", horizontalInput != 0);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        anim.SetTrigger("Jump");
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    //private bool isGrounded()
    //{
    //    return false;
    //}

    public bool CanAttack()
    {
        return horizontalInput == 0 && isGrounded;
    }
}
