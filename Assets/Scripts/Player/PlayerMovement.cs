using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 10f;

    private DashAbility dash;
    private float horizontalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        dash = GetComponentInChildren<DashAbility>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        //if (!dash.IsDashing)
        //{
        //    rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        //}

        if (horizontalInput > 0.01f) //Sprite turn right
        {
            sr.flipX = false;
        }
        else if(horizontalInput < -0.01f) //Sprite turn left
        {
            sr.flipX = true;
        }

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            Jump();
        }

        //Animation
        anim.SetBool("isRunning", horizontalInput != 0 && isGrounded());
        anim.SetBool("isGrounded", isGrounded());
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        anim.SetTrigger("Jump");
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        bool grounded = raycastHit.collider != null;
        Debug.Log($"Player is not on the ground: {grounded}");
        return grounded;
    }

    public bool CanAttack()
    {
        return horizontalInput == 0 && isGrounded();
    }
}
