using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterElement : MonoBehaviour
{
    public float absorptionRadius = 20f;
    public ParticleSystem absorptionParticleEffectPrefab;
    public float attackCooldown = 1f;

    // KEYCODES
    public KeyCode absorptionKey = KeyCode.Mouse1;
    public KeyCode castingKey = KeyCode.Mouse0;
    public KeyCode projectileKey = KeyCode.Q;

    [SerializeField] private bool absorbing = false;
    [SerializeField] private bool absorbingTrigger = false;
    public float absorptionTimer = 0f; // ABSORPTION TIMER
    private GameObject player;
    private GameObject emissionPoint;
    private Quaternion emissionPointRotation;
    private ParticleSystem activeAbsorptionParticleEffect;

    private float attackDuration = 0f;
    public bool isCasting = false;

    public float emissionRadius = 10f;

    void Start()
    {
        // Create the emission point as a child GameObject
        emissionPoint = new GameObject("EmissionPoint");
        emissionPoint.transform.parent = transform;
        emissionPoint.transform.localPosition = Vector3.zero;
        emissionPointRotation = transform.rotation; // Initial rotation

        emissionPoint.transform.rotation = transform.rotation;

        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("player not found");
        }
    }

    void Update()
    {
        if (absorbing && absorbingTrigger)
        {
            absorptionTimer += Time.deltaTime;
        }

        if (absorbingTrigger)
        {
            if (Input.GetKeyDown(absorptionKey) && activeAbsorptionParticleEffect == null)
            {
                if (absorptionParticleEffectPrefab != null)
                {
                    absorbing = true;
                    activeAbsorptionParticleEffect = Instantiate(absorptionParticleEffectPrefab, emissionPoint.transform.position, emissionPointRotation, emissionPoint.transform);
                }
            }
        }

        if (player != null && emissionPoint != null && emissionPoint.transform.parent != null && absorbingTrigger && absorbing)
        {
            Vector3 directionToPlayer = player.transform.position - emissionPoint.transform.parent.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer <= emissionRadius)
            {
                emissionPoint.transform.position = player.transform.position;
            }
            else
            {
                Vector3 closestPoint = emissionPoint.transform.parent.position + directionToPlayer.normalized * emissionRadius;
                emissionPoint.transform.position = closestPoint;
            }
        }

        if (Input.GetKeyUp(absorptionKey))
        {
            absorbing = false;

            if (activeAbsorptionParticleEffect != null)
            {
                activeAbsorptionParticleEffect.Stop();
                Destroy(activeAbsorptionParticleEffect.gameObject);
                activeAbsorptionParticleEffect = null;
            }

            attackDuration = absorptionTimer;
            Debug.Log("Attack duration: " + attackDuration);
            absorptionTimer = 0f; // Reset absorption timer
        }

        if (attackDuration <= 0)
            {
                isCasting = false;
            }
        
        else
        {
            absorptionTimer = 0f; // Reset the absorption timer if not absorbing
        }

        Debug.Log("Current attack duration: " + attackDuration);
        if (player != null && Input.GetKey(castingKey))
        {
            if (attackDuration > 0)
            {
                attackDuration -= Time.deltaTime;
            }
        }

        if (player != null && Input.GetKeyDown(castingKey))
        {
            WaterPower waterPower = player.GetComponent<WaterPower>();
            if (attackDuration > 0)
            {
                Debug.Log("Casting attack with duration: " + attackDuration);
                waterPower.StartCoroutine(waterPower.Coroutine_WaterBall());
                player.GetComponent<WaterPower>().AnimationCallback_CreateWaterBall();
                isCasting = true;
            }
        }

        if (player != null && Input.GetKey(castingKey) && Input.GetKeyDown(projectileKey))
        {
            if (isCasting && attackDuration > 0)
            {
                // player.GetComponent<WaterPower>().StopAllCoroutines();
                player.GetComponent<WaterPower>().AnimationCallback_ThrowBall();
            }
        }

        if (Input.GetKeyUp(castingKey))
        {
            GameObject waterBall = GameObject.Find("WaterBall(Clone)");
            attackDuration = 0f; // Reset attack duration
            player.GetComponent<WaterPower>().StopAllCoroutines();
            if (waterBall != null)
            {
                Destroy(waterBall);
            }
        }

        if (attackDuration <= 0)
        {
            GameObject waterBall = GameObject.Find("WaterBall(Clone)");
            attackDuration = 0f; // Reset attack duration
            player.GetComponent<WaterPower>().StopAllCoroutines();
            Destroy(waterBall);
        }

        if (absorbingTrigger)
        {
            // Rotate the emission point to face towards the player
            if (player != null)
            {
                Vector3 directionToPlayer = player.transform.position - emissionPoint.transform.position;
                emissionPointRotation = Quaternion.LookRotation(directionToPlayer);
                emissionPoint.transform.rotation = emissionPointRotation;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            absorbingTrigger = true;
            player = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop absorbing when player exits absorption radius
            absorbingTrigger = false;
            // Stop active absorption particle effect
            if (activeAbsorptionParticleEffect != null)
            {
                activeAbsorptionParticleEffect.Stop();
                Destroy(activeAbsorptionParticleEffect.gameObject);
                activeAbsorptionParticleEffect = null;
            }
            player = null;
        }
    }
}
