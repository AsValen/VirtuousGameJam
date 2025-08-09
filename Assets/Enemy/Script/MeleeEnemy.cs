using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Attack Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Chase Parameters")]
    [SerializeField] private float chaseSpeed = 3f;

    private float cooldownTimer = Mathf.Infinity;

    //References
    //private Animator anim;
    private PlayerStats playerHealth;
    private Transform playerTransform;
    private EnemyPatrol enemyPatrol;
    private bool provoked = false;

    private void Awake()
    {
        //anim  = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //Attack only when player is sight
        if(PlayerInSight())
        {
            provoked = true;
        }
        if(provoked)
        {
            ChasePlayer();
            if (cooldownTimer >= attackCooldown && PlayerInSight())
            {
                //Attack
                cooldownTimer = 0;
                DamagePlayer();  //set this event inside Animation event
                //anim.SetTrigger("basicAttack");
            }
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x *range,boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if(hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<PlayerStats>();
            playerTransform = hit.transform;
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void ChasePlayer()
    {
        if (playerTransform == null) return;

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();  //if see player, stop patrolling
        }
        //Chase player
        transform.position = Vector2.MoveTowards(transform.position,playerTransform.position,chaseSpeed * Time.deltaTime);

        //Face player
        Vector3 scale = transform.localScale;
        scale.x = playerTransform.position.x < transform.position.x ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
    public void OnDamaged(Transform attacker)
    {
        provoked = true;
        playerTransform = attacker;
    }
    private void DamagePlayer()
    {
        if(PlayerInSight())
        {
            //Damage player health
            playerHealth.SetPlayerHP(damage);
        }
    }
}
