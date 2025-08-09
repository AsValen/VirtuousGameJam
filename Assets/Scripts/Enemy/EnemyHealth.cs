using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health")]
    [SerializeField] private float startingHealth;
    [Header("Revive Duration")]
    [SerializeField] private float reviveTime = 5f;
    [Header("Revive")]
    [SerializeField] private bool isDead = false;
    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    public float currentHealth { get; private set; }
    public bool IsDead { get;private set; }

    private Animator anim;
    private MeleeEnemy meleeEnemy;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage,Transform attacker)
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

        foreach(Behaviour component in components) //disable MeleeEnemy and EnemyPatrol script
        {
            component.enabled = false;
        }

        yield return new WaitForSeconds(reviveTime);

        if (IsDead)
        {
            IsDead = false;
            currentHealth = startingHealth;

            anim.SetTrigger("revive");

            foreach (Behaviour component in components)
            {
                component.enabled = true;
            }
        }
    }
    
}
