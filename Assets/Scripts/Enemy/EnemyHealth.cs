using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health")]
    [SerializeField] private float startingHealth;
    [field: SerializeField] public float currentHealth { get; private set; }
    [Header("Revive Duration")]
    [SerializeField] private float reviveTime = 5f;
    [Header("Revive")]
    [SerializeField] public bool isDead = false;
    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    private SpriteRenderer sr;

    public bool IsDead 
    {
        get => isDead;
    }

    //private Animator anim;
    private MeleeEnemy meleeEnemy;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Awake()
    {
        currentHealth = startingHealth;
        //anim = GetComponent<Animator>();
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
            //anim.SetTrigger("hurt");
        }
        else
        {
            if(!isDead)
            {
                StartCoroutine(ReviveEnemy());
            }
        }
    }
    private IEnumerator ReviveEnemy()
    {
        isDead = true;
        //anim.SetTrigger("down");

        foreach(Behaviour component in components) //disable MeleeEnemy and EnemyPatrol script
        {
            component.enabled = false;
        }

        yield return new WaitForSeconds(reviveTime);

        if (isDead)
        {
            isDead = false;
            currentHealth = startingHealth;

            //anim.SetTrigger("revive");

            foreach (Behaviour component in components)
            {
                component.enabled = true;
            }
        }
        if(isDead)
        {
            sr.color = new Color(0.6f, 0.65f, 1f, 1f);
        }
        else
        {
            sr.color = new Color(1f, 1f, 1f, 1f);
        }
    }
    
}
