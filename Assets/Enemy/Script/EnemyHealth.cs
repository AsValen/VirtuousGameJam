using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health")]
    [SerializeField] private float startingHealth;
    [Header("Revive Duration")]
    [SerializeField] private float reviveTime = 5f;
    public float currentHealth { get; private set; }
    public bool IsDead { get;private set; }
    private Animator anim;
    private MeleeEnemy meleeEnemy;
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        meleeEnemy = GetComponent<MeleeEnemy>();
        if(meleeEnemy != null )
        {
            meleeEnemy.OnDamaged(attacker);
        }
        if(currentHealth > 0)
        {
            //enemy hurt
            anim.SetTrigger("hurt");
        }
        else
        {
            if(!IsDead)
            {
                StartCoroutine(ReviveEnemy());
            }
        }
    }
    private IEnumerator ReviveEnemy()
    {
        IsDead = true;
        anim.SetTrigger("down");

        if (GetComponentInParent<EnemyPatrol>() != null)
        {
            GetComponentInParent<EnemyPatrol>().enabled = false;
        }

        if (GetComponent<MeleeEnemy>() != null)
        {
            GetComponent<MeleeEnemy>().enabled = false;
        }

        yield return new WaitForSeconds(reviveTime);

        if (IsDead)
        {
            IsDead = false;
            currentHealth = startingHealth;

            anim.SetTrigger("revive");

            if (GetComponentInParent<EnemyPatrol>() != null)
            {
                GetComponentInParent<EnemyPatrol>().enabled = true;
            }

            if (GetComponent<MeleeEnemy>() != null)
            {
                GetComponent<MeleeEnemy>().enabled = true;
            }
        }
    }
    
}
