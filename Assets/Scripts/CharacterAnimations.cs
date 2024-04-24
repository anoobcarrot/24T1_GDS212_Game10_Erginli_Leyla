using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    public Animator otherAnimator;

    // PUNCHING
    public KeyCode punchingAnimationKey = KeyCode.Mouse1;
    public string punchingAnimationName = "Punching";
    private bool isPunching = false;
    public float punchCooldown = 1.0f;
    private float punchTimer = 0.0f;

    // CASTING
    public KeyCode castingAnimationKey = KeyCode.Mouse1;
    public string castingAnimationName = "Cast Spell";
    private bool isCasting = false;
    public float castCooldown = 1.0f;
    private float castTimer = 0.0f;

    // CASTING ATTACK
    public KeyCode castingAttackAnimationKey = KeyCode.Mouse1;
    public string castingAttackAnimationName = "Cast Spell Attack";
    public float castAttackCooldown = 1.0f;
    private float castAttackTimer = 0.0f;
    private bool isCastingAttack = false;

    private ThirdPersonController thirdPersonController;

    private Animator myAnimator;

    void Start()
    {
        otherAnimator = GameObject.Find("Character").GetComponent<Animator>();
        myAnimator = GetComponent<Animator>();

        thirdPersonController = GameObject.FindObjectOfType<ThirdPersonController>();

        // Check if the reference is null
        if (thirdPersonController == null)
        {
            Debug.LogError("ThirdPersonController script not found in the scene.");
        }
    }

    void Update()
    {
        if (!isPunching && punchTimer <= 0.0f && Input.GetKeyDown(punchingAnimationKey) && !isCasting && !isCastingAttack)
        {
            myAnimator.enabled = false;
            isPunching = true;
            thirdPersonController.LockMovement();
            otherAnimator.SetBool("IsPunching", isPunching);
            otherAnimator.Play(punchingAnimationName);

            punchTimer = punchCooldown;
        }

        // Update the cooldown timer
        if (punchTimer > 0.0f)
        {
            punchTimer -= Time.deltaTime;
        }

        if (!isCasting && castTimer <= 0.0f && Input.GetKeyDown(castingAnimationKey) && !isPunching && !isCastingAttack)
        {
            myAnimator.enabled = false;
            isCasting = true;
            thirdPersonController.LockMovement();
            otherAnimator.SetBool("IsCasting", isCasting);
            otherAnimator.Play(castingAnimationName);

            castTimer = castCooldown;
        }

        // Update the cooldown timer
        if (castTimer > 0.0f)
        {
            castTimer -= Time.deltaTime;
        }

        if (!isCastingAttack && castAttackTimer <= 0.0f && Input.GetKeyDown(castingAttackAnimationKey) && !isPunching && !isCasting)
        {
            myAnimator.enabled = false;
            isCastingAttack = true;
            thirdPersonController.LockMovement();
            otherAnimator.SetBool("IsCastingAttack", isCastingAttack);

            otherAnimator.Play(castingAttackAnimationName);

            castAttackTimer = castAttackCooldown;
        }

        // Update the cooldown timer
        if (castAttackTimer > 0.0f)
        {
            castAttackTimer -= Time.deltaTime;
        }
    }

    // Method to be called from Animation Event to set isPunching to false
    public void SetPunchingFalse()
    {
        isPunching = false;
        thirdPersonController.UnlockMovement();
        otherAnimator.SetBool("IsPunching", isPunching);

        myAnimator.enabled = true;
    }
    public void SetCastingFalse()
    {
        isCasting = false;
        thirdPersonController.UnlockMovement();
        otherAnimator.SetBool("IsCasting", isCasting);

        // Re-enable the animator component attached to this GameObject
        myAnimator.enabled = true;
    }
    public void SetCastingAttackFalse()
    {
        isCastingAttack = false;
        thirdPersonController.UnlockMovement();
        otherAnimator.SetBool("IsCastingAttack", isCastingAttack);

        myAnimator.enabled = true;
    }
}

