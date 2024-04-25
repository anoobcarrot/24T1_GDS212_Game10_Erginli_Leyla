using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public float roamRadius = 10f;
    public float playerDetectionRadius = 20f;
    public int maxHealth = 100;
    public GameObject healthBarPrefab;

    // PROJECTILES

    public GameObject projectilePrefab; // Prefab of the projectile object
    public float projectileSpeed = 10f; // Speed of the projectile
    public float fireRate = 1f; // Rate at which the enemy fires projectiles (in seconds)
    public int projectileDamage = 10; // Damage inflicted by each projectile
    private float nextFireTime; // Time when the enemy can fire next

    private Transform player;
    private NavMeshAgent agent;
    private int currentHealth;
    private GameObject healthBar;
    private Image healthBarFill;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;

        Transform canvasTransform = transform.Find("Enemy Canvas");
        if (canvasTransform != null)
        {
            // Instantiate the health bar
            healthBar = Instantiate(healthBarPrefab, canvasTransform);

            // Set the health bar's position above the enemy's head
            Vector3 healthBarOffset = new Vector3(0f, 0f, 0f);
            healthBar.transform.localPosition = healthBarOffset;

            // Get the fill image component of the health bar
            healthBarFill = healthBar.GetComponentInChildren<Image>();
        }
        else
        {
            Debug.LogError("Canvas not found as a child of the enemy object.");
        }
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            // Player is in line of sight, chase the player
            ChasePlayer();
        }
        else
        {
            // Player is not in line of sight, roam around
            Roam();
        }

        // Rotate the health bar to face the player
        if (healthBar != null && player != null)
        {
            healthBar.transform.LookAt(player);
        }

        // Check if it's time to fire
        if (CanSeePlayer() && Time.time >= nextFireTime)
        {
            // Fire projectile at the player
            FireProjectile();
            // Set the next fire time
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void FireProjectile()
    {
        // Spawn projectile at the enemy's position
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Get the direction towards the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Get the rigidbody component of the projectile
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

        // Disable gravity for the projectile
        projectileRigidbody.useGravity = false;

        // Set the velocity of the projectile to shoot towards the player
        projectileRigidbody.velocity = directionToPlayer * projectileSpeed;

        // Set the damage value of the projectile
        projectile.GetComponent<Projectile>().damage = projectileDamage;
    }

    bool CanSeePlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, playerDetectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Vector3 direction = (player.position - transform.position).normalized;
                if (Vector3.Dot(transform.forward, direction) > 0.5f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void Roam()
    {
        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
            Vector3 finalPosition = hit.position;
            agent.SetDestination(finalPosition);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        float healthPercentage = (float)currentHealth / maxHealth;
        healthBarFill.fillAmount = healthPercentage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        Destroy(healthBar);
    }
}


