using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight =8f;

    [SerializeField] private int playerHP = 10;
    private int maxHP;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image fillImage;

    private bool isGrounded;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        maxHP = playerHP;
        hpSlider.maxValue = maxHP;
        hpSlider.value = playerHP;
        UpdateHPUI();
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
        SetPlayerHP(1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    public int GetPlayerHP()
    {
        return playerHP;
    }

    public void SetPlayerHP(int damage)
    {
        playerHP = Mathf.Clamp(playerHP - damage, 0, maxHP);
        hpSlider.value = playerHP;

        UpdateHPUI();
    }

    private void UpdateHPUI()
    {
        if (playerHP <= 0)
        {
            Destroy(gameObject);
        }
        else if (playerHP <= 3)
        {
            fillImage.color = Color.red;
        }
        else
        {
            fillImage.color = Color.green;
        }
    }
}
