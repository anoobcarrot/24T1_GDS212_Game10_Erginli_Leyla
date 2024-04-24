using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode attackKey = KeyCode.Mouse0;
    public float attackRange = 2f;
    public int attackDamage = 15;
    public float attackCooldown = 1.0f; // Cooldown
    private float lastAttackTime = 0f;

    void Update()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (Input.GetKeyDown(attackKey))
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyAI enemyHealth = collider.GetComponent<EnemyAI>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

