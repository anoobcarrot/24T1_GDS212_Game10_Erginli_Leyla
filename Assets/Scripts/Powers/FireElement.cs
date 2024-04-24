using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElement : MonoBehaviour
{
    public float absorptionRadius = 20f;
    public ParticleSystem absorptionParticleEffectPrefab;
    public float attackCooldown = 1f;

    // KEYCODES
    public KeyCode absorptionKey = KeyCode.Mouse1;
    public KeyCode castingKey = KeyCode.Mouse0;
    public KeyCode projectileKey = KeyCode.Q;

    private bool absorbing = false;
    private bool absorbingTrigger = false;
    public float absorptionTimer = 0f; // ABSORPTION TIMER
    private GameObject player;
    private GameObject emissionPoint;
    private Quaternion emissionPointRotation;
    private ParticleSystem activeAbsorptionParticleEffect;

    private float attackDuration = 0f;
    public bool isCasting = false;

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
            absorptionTimer += Time.deltaTime; // Increment absorption timer while absorbing
        }

        if (absorbingTrigger)
        {
            if (Input.GetKeyDown(absorptionKey) && activeAbsorptionParticleEffect == null)
            {
                // Instantiate absorption particle effect if not already active
                if (absorptionParticleEffectPrefab != null)
                {
                    absorbing = true;
                    // Instantiate the absorption particle effect using the stored rotation of the emission point
                    activeAbsorptionParticleEffect = Instantiate(absorptionParticleEffectPrefab, emissionPoint.transform.position, emissionPointRotation, emissionPoint.transform);
                }
            }
            else if (Input.GetKeyUp(absorptionKey))
            {
                absorbing = false;
                // Stop active absorption particle effect
                if (activeAbsorptionParticleEffect != null)
                {
                    activeAbsorptionParticleEffect.Stop();
                    Destroy(activeAbsorptionParticleEffect.gameObject);
                    activeAbsorptionParticleEffect = null;
                }
                // Set attack duration to absorption timer value
                attackDuration = absorptionTimer;
                Debug.Log("Attack duration: " + attackDuration);
                absorptionTimer = 0f; // Reset absorption timer
            }

            if (attackDuration <= 0)
            {
                isCasting = false;
            }
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
        // Casting attack (pressing the casting key)
        if (player != null && Input.GetKeyDown(castingKey))
        {
            if (attackDuration > 0)
            {
                Debug.Log("Casting attack with duration: " + attackDuration);
                player.GetComponent<FirePower>().CastAttack(attackDuration);
                isCasting = true;
            }
        }

        // Continuously check if the projectile button is pressed while the casting key is held down
        if (player != null && Input.GetKey(castingKey) && Input.GetKeyDown(projectileKey))
        {
            if (isCasting && attackDuration > 0)
            {
                player.GetComponent<FirePower>().TurnAttackIntoProjectile(attackDuration);
            }
        }

        if (Input.GetKeyUp(castingKey))
        {
            attackDuration = 0f; // Reset attack duration
            player.GetComponent<FirePower>().CancelAttack();
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




