using NUnit.Framework;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LayerMask layerToHit;  
    public float maxDistance = 50f;
    public GameObject EndVFX;

    private SpriteRenderer spriteRenderer;
    private float spriteUnitLength; // Width of the sprite in units (1 = 100 pixels at 100 PPU)

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found!");
            return;
        }

        // Calculate the original length of the sprite in world units
        spriteUnitLength = spriteRenderer.sprite.bounds.size.x;
        EndVFX.SetActive(false);
    }

    void Update()
    {
        Vector2 direction = transform.right;

        // Default to max distance
        float targetDistance = maxDistance;

        // Raycast to detect Wall/Player
        RaycastHit2D hit = Physics2D.Raycast(transform.parent.position, direction, maxDistance, layerToHit);
        if (hit.collider != null)
        {
            Debug.Log("Hit something");
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                targetDistance = hit.distance;
                EndVFX.SetActive(true);
                EndVFX.transform.position = hit.point;
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                targetDistance = hit.distance;
                EndVFX.SetActive(true);
                EndVFX.transform.position = hit.point;

                PlayerStats playerStats = hit.collider.gameObject.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    Debug.Log("Player hit by laser!");
                    playerStats.SetPlayerHP(10);
                }
            }
        }
        else
        {
            EndVFX.SetActive(false);
        }

        // Scale laser to match distance
        float newScaleX = targetDistance / spriteUnitLength;
        transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
    }

}
