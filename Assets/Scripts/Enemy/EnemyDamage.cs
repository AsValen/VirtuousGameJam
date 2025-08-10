using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected int damage;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[EnemyDamage] Trigger with: {collision.name}, Tag: {collision.tag}");

        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                Debug.Log($"[EnemyDamage] DEALING {damage} DAMAGE to player!");
                playerStats.SetPlayerHP(damage);
            }
            else
            {
                Debug.LogError("[EnemyDamage] Player tag found but NO PlayerStats component!");
            }
        }
    }
}