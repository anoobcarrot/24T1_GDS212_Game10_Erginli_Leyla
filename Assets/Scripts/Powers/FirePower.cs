using UnityEngine;
using DG.Tweening;

public class FirePower : MonoBehaviour
{
    public ParticleSystem castingParticleEffect;
    public float chestHeightOffset = 1.0f;
    public float projectileSpeed = 10f;

    public float projectileCooldownTimer = 0f;
    public bool isProjectileOnCooldown = false;

    public float damageRadius = 1.0f; // Radius within which enemies take damage
    public int damageAmount = 25; // Damage to apply

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
        Vector3 direction = transform.forward;
        Vector3 spawnPosition = transform.position + transform.up * chestHeightOffset + direction;

        ParticleSystem activeEffect = Instantiate(castingParticleEffect, spawnPosition, Quaternion.identity);
        activeEffect.transform.SetParent(transform);

        // Rotate the particle system to match the player's orientation
        activeEffect.transform.rotation = Quaternion.LookRotation(direction);

        Destroy(activeEffect.gameObject, attackDuration);

        Debug.Log("Casting Fire power!");
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

            Destroy(projectile, attackDuration);
            projectileCooldownTimer = 2.5f;
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
            activeEffect.Stop();
            Destroy(activeEffect.gameObject);
        }
    }
}







