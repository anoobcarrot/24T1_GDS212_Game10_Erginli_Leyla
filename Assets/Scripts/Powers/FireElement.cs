using UnityEngine;
using UnityEngine.UI;

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
    private GameObject emissionPoint;
    private Quaternion emissionPointRotation;
    private ParticleSystem activeAbsorptionParticleEffect;

    [SerializeField] private float attackDuration = 0f;
    public bool isCasting = false;

    public Image attackDurationFillBar;

    void Start()
    {
        // Create the emission point as a child GameObject
        emissionPoint = new GameObject("EmissionPoint");
        emissionPoint.transform.parent = transform;
        emissionPoint.transform.localPosition = Vector3.zero;
        emissionPointRotation = transform.rotation; // Initial rotation
    }

    void Update()
    {
        // Trigger absorption when the absorption key is pressed
        if (Input.GetKeyDown(absorptionKey) && activeAbsorptionParticleEffect == null && !isCasting)
        {
            // Instantiate absorption particle effect if not already active
            if (absorptionParticleEffectPrefab != null)
            {
                absorbing = true;
                // Instantiate the absorption particle effect using the stored rotation of the emission point
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

            // Update the fill amount of the UI image fill bar
            UpdateFillBar();

            // Rotate the emission point to face towards the player
            Vector3 directionToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            directionToMouse.z = 0f; // Ensure the z-component is zero
            emissionPointRotation = Quaternion.LookRotation(directionToMouse);
            emissionPoint.transform.rotation = emissionPointRotation;
        }

        // Casting attack (pressing the casting key)
        if (Input.GetKeyDown(castingKey) && !absorbing && !isCasting)
        {
            if (attackDuration > 0)
            {
                Debug.Log("Casting attack with duration: " + attackDuration);
                GetComponent<FirePower>().CastAttack(attackDuration);
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
                UpdateFillBar(); // Update the fill amount of the UI image fill bar
            }
        }

        if (attackDuration <= 0)
        {
            attackDuration = 0f; // Reset attack duration
            isCasting = false;
            GetComponent<FirePower>().CancelAttack();
            UpdateFillBar(); // Update the fill amount of the UI image fill bar
        }

        // Continuously check if the projectile button is pressed while the casting key is held down
        if (Input.GetKey(castingKey) && Input.GetKeyDown(projectileKey))
        {
            if (isCasting && attackDuration > 0 && !absorbing)
            {
                GetComponent<FirePower>().TurnAttackIntoProjectile(attackDuration);
            }
        }

        if (Input.GetKeyUp(castingKey) && !absorbing)
        {
            isCasting = false;
            attackDuration = 0f; // Reset attack duration
            GetComponent<FirePower>().CancelAttack();
            UpdateFillBar(); // Update the fill amount of the UI image fill bar
        }
    }

    // Method to update the fill amount of the UI image fill bar based on the current attack duration
    void UpdateFillBar()
    {
        if (attackDurationFillBar != null)
        {
            attackDurationFillBar.fillAmount = attackDuration / 10f;
        }
    }
}





