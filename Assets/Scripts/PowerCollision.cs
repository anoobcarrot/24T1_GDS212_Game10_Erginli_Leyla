using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCollision : MonoBehaviour
{
    public int damageAmount = 25;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");
            // Deal damage to the enemy
            EnemyAI enemyHealth = other.GetComponent<EnemyAI>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }
    }
}



