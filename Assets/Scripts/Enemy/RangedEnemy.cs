using UnityEngine;
using System.Collections;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Chase Parameters")]
    [SerializeField] private float chaseSpeed = 3f;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Forget Player Settings")]
    [SerializeField] private float forgetTime = 5f; // seconds before returning to patrol
    private float lastSeenTime;

    private float cooldownTimer = Mathf.Infinity;
    private Vector3 initScale;
    private bool provoked = false;

    //References
    //private Animator anim;
    private PlayerStats playerHealth;
    private Transform playerTransform;
    private EnemyPatrol enemyPatrol;
    private DashAbility dashAbility;
    private void Start()
    {
        dashAbility = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<DashAbility>();
    }
    private void Awake()
    {
        //anim  = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {

        if (playerHealth != null && playerHealth.GetPlayerHP() <= 0)
        {
            provoked = false;
            playerTransform = null;
            cooldownTimer = Mathf.Infinity;
            if (enemyPatrol != null)
                enemyPatrol.enabled = true;
            return;
        }
        bool playerVisible = PlayerInSight();
        cooldownTimer += Time.deltaTime;
        //Attack only when player is sight
        if (playerVisible)
        {
            provoked = true;
            lastSeenTime = Time.time;
        }
        if (provoked)
        {
            ChasePlayer(playerVisible);
            if (cooldownTimer >= attackCooldown && playerVisible)
            {
                //Attack
                cooldownTimer = 0;
                RangeAttack();  //set this event inside Animation event
                //anim.SetTrigger("basicAttack");
            }

            if (Time.time > lastSeenTime + forgetTime) //If hvnt seen player after sometime, stop chasing
            {
                provoked = false;
                playerTransform = null;
                cooldownTimer = Mathf.Infinity;

                if (enemyPatrol != null)
                {
                    enemyPatrol.enabled = true; // resume patrol
                    Debug.Log("Stopped chasing, resuming patrol");
                }
            }
        }
    }

    private void RangeAttack()
    {
        if (dashAbility == null || !dashAbility.IsInvulnerable)
        {
            cooldownTimer = 0;
            fireballs[FindFireball()].transform.position = firepoint.position;
            fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
        }
        else
        {
            Debug.Log("Player is invulerable, no damage applied");
        }
    }
    
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<PlayerStats>();
            playerTransform = hit.transform;
            return true;
        }
        return false;
    }

    private void OnDrawGizmos() //Indication of Enemy Detection Range (visual purposes)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void ChasePlayer(bool playerVisible)
    {
        if (playerTransform == null) return;

        if (enemyPatrol != null)
        {
            //enemyPatrol.enabled = !playerVisible;  //if see player, stop patrolling
            enemyPatrol.enabled = !provoked;
        }
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);
        if (distance > 1f)
        {
            //Chase player
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);
        }
        if (provoked)
        {
            //Face player
            //float direction = playerTransform.position.x > transform.position.x ? 1f : -1f;
            float direction = Mathf.Sign(targetPosition.x - transform.position.x);
            transform.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        }
    }
    public void OnDamaged(Transform attacker)
    {
        provoked = true;
        playerTransform = attacker;
        StartCoroutine(ResetBeingAttacked());
    }
    private IEnumerator ResetBeingAttacked()
    {
        yield return new WaitForSeconds(1f); // disable attack for second(s) after getting hit
    }

}
