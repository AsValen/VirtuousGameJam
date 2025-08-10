using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    //private Animator anim;
    private BoxCollider2D coll;
    private bool hit;
    private float direction; // Store the direction the projectile should move

    private void Awake()
    {
        //anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;

        // Get direction from the ProjectileHolder's scale
        // Since ProjectileHolder mirrors the enemy's scale via EnemyProjectileHolder script
        // Correct scale convention: scale.x > 0 (facing right), scale.x < 0 (facing left)
        direction = Mathf.Sign(transform.localScale.x);
        Debug.Log($"Projectile activated with localScale.x: {transform.localScale.x}, direction: {direction}");
    }

    private void Update()
    {
        if (hit) return;
        Debug.Log($"Projectile moving with direction: {direction}");
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed,0,0,Space.World);


        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision); //Execute logic from parent script first
        coll.enabled = false;
        //if (anim != null)
        //    anim.SetTrigger("explode"); 
        //else
        gameObject.SetActive(false); //When this hits any object deactivate arrow
    }
}