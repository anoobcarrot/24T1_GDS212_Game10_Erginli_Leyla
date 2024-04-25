using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10; // Damage inflicted by the projectile

    private void OnTriggerEnter(Collider other)
    {
        // Check if the projectile collides with a game object tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Get the health component of the player and apply damage
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            // Destroy the projectile when it hits the player
            Destroy(gameObject);
        }
    }
}

