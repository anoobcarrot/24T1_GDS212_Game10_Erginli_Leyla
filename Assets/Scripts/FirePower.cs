using UnityEngine;
using DG.Tweening;

public class FirePower : MonoBehaviour
{
    public ParticleSystem castingParticleEffect;
    public float chestHeightOffset = 1.0f;
    public float projectileSpeed = 10f;

    public float projectileCooldownTimer = 0f;
    public bool isProjectileOnCooldown = false;

    private ParticleSystem activeEffect;

    void Update()
    {
        // Update cooldown timer
        if (isProjectileOnCooldown)
        {
            projectileCooldownTimer -= Time.deltaTime;
            if (projectileCooldownTimer <= 0)
            {
                isProjectileOnCooldown = false;
            }
        }
    }

    public void CastAttack(float attackDuration)
    {
        // Calculate the position for chest height instantiation
        Vector3 chestHeightPosition = transform.position + Vector3.up * chestHeightOffset;
        {
            // Instantiate the particle effect at chest height
            activeEffect = Instantiate(castingParticleEffect, chestHeightPosition, Quaternion.identity);

            // Set the parent of the instantiated particle system to be the player GameObject
            activeEffect.transform.SetParent(transform);

            // Get the direction the player is facing (forward vector of the player's transform)
            Vector3 direction = transform.forward;

            // Rotate the particle system to match the player's orientation
            activeEffect.transform.rotation = Quaternion.LookRotation(direction);

            // Destroy the particle effect after the attack duration
            Destroy(activeEffect.gameObject, attackDuration);

            Debug.Log("Casting Fire power!");
        }
    }

    public void TurnAttackIntoProjectile(float attackDuration)
    {
        if (!isProjectileOnCooldown)
        {
            Vector3 chestHeightPosition = transform.position + Vector3.up * chestHeightOffset;
            GameObject projectile = Instantiate(castingParticleEffect.gameObject, chestHeightPosition, Quaternion.identity);

            projectile.transform.SetParent(transform);

            Vector3 direction = transform.forward;

            Vector3 targetPosition = transform.position + direction * 20f;

            float distance = Vector3.Distance(projectile.transform.position, targetPosition);

            float travelTime = distance / projectileSpeed;

            projectile.transform.DOMove(targetPosition, travelTime).OnComplete(() =>
            {
                Destroy(projectile);
            });

            // Destroy the projectile after the attack duration
            Destroy(projectile, attackDuration);

            projectileCooldownTimer = 1f;
            isProjectileOnCooldown = true;

            Debug.Log("Casting Fire power as projectile!");
        }
        else
        {
            Debug.Log("Projectile is on cooldown!");
        }
    }

    public void CancelAttack()
    {
        if (activeEffect != null)
        {
            // Stop the particle effect
            activeEffect.Stop();
            // Destroy the particle effect object
            Destroy(activeEffect.gameObject);
        }
    }
}







