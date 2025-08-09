using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight =8f;

    private PlayerStats stats;

    private bool isGrounded;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
    }    

    private void Update()
    {
        rigidBody.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rigidBody.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpHeight);
        isGrounded = false;
        stats.SetPlayerHP(1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }    
}
