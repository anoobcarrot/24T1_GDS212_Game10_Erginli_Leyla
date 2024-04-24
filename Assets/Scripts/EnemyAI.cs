using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public float roamRadius = 10f;
    public float playerDetectionRadius = 20f;
    public int maxHealth = 100;
    public GameObject healthBarPrefab;

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
            healthBar = Instantiate(healthBarPrefab, canvasTransform);
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


