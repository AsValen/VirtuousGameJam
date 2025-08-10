using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    private int damage;
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;
    }

    public void SetDamage(int value)
    {
        damage = value;
    }

    public void EnableHitbox()
    {
        col.enabled = true;
    }

    public void DisableHitbox()
    {
        col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, transform.root);
            }
        }
    }
}