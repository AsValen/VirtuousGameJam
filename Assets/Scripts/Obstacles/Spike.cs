using UnityEngine;

public class Spike : MonoBehaviour
{
    //Add invulnerability
    //Add changing of opacity of player
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("spikeOn");
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if(playerStats != null)
            {
                //Debug.Log("spike");
                playerStats.SetPlayerHP(1); // Assuming spikes deal 1 damage
            }
        }
    }
}
