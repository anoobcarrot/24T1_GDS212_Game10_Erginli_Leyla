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
    private GameObject player;
    private GameObject emissionPoint;
    private Quaternion emissionPointRotation;
    private ParticleSystem activeAbsorptionParticleEffect;

    [SerializeField] private float attackDuration = 0f;
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
        if (Input.GetKeyDown(absorptionKey) && activeAbsorptionParticleEffect == null && !isCasting)
        {
            if (absorptionParticleEffectPrefab != null)
            {
                absorbing = true;
                activeAbsorptionParticleEffect = Instantiate(absorptionParticleEffectPrefab, emissionPoint.transform.position, emissionPointRotation, emissionPoint.transform);
            }
        }

        if (Input.GetKeyUp(absorptionKey))
        {
            absorbing = false;
            // Stop active absorption particle effect
            if (activeAbsorptionParticleEffect != null)
            {
                activeAbsorptionParticleEffect.Stop();
                Destroy(activeAbsorptionParticleEffect.gameObject);
                activeAbsorptionParticleEffect = null;
            }
        }

        if (absorbing && !isCasting)
        {
            // Increment absorption timer while absorbing
            attackDuration += Time.deltaTime;

            // Clamp attack duration between 0 and 10 seconds
            attackDuration = Mathf.Clamp(attackDuration, 0f, 10f);

            // Rotate the emission point to face towards the player
            Vector3 directionToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            directionToMouse.z = 0f; // Ensure the z-component is zero
            emissionPointRotation = Quaternion.LookRotation(directionToMouse);
            emissionPoint.transform.rotation = emissionPointRotation;
        }

        // Casting attack (pressing the casting key)
        if (Input.GetKeyDown(castingKey) && !absorbing && !isCasting)
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

        // Decrease attack duration while casting (holding down the casting key)
        if (Input.GetKey(castingKey))
        {
            if (attackDuration > 0 && !absorbing)
            {
                attackDuration -= Time.deltaTime;
                attackDuration = Mathf.Clamp(attackDuration, 0f, 10f); // Clamp attack duration between 0 and 10 seconds
            }
        }

        if (attackDuration <= 0)
        {
            GameObject waterBall = GameObject.Find("WaterBall(Clone)");
            attackDuration = 0f; // Reset attack duration
            player.GetComponent<WaterPower>().StopAllCoroutines();
            Destroy(waterBall);
            isCasting = false;
        }

        // Continuously check if the projectile button is pressed while the casting key is held down
        if (Input.GetKey(castingKey) && Input.GetKeyDown(projectileKey))
        {
            if (isCasting && attackDuration > 0 && !absorbing)
            {
                // player.GetComponent<WaterPower>().StopAllCoroutines();
                player.GetComponent<WaterPower>().AnimationCallback_ThrowBall();
            }
        }

        if (Input.GetKeyUp(castingKey) && !absorbing)
        {
            isCasting = false;
            attackDuration = 0f; // Reset attack duration
            GameObject waterBall = GameObject.Find("WaterBall(Clone)");
            player.GetComponent<WaterPower>().StopAllCoroutines();
            if (waterBall != null)
            {
                Destroy(waterBall);
            }
        }
    }
}

