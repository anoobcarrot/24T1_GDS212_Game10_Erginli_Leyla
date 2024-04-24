using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class TestingAnimations : MonoBehaviour
{
    public Animator otherAnimator;
    public string castingAttackAnimationName = "Cast Spell Attack";
    private bool isCastingAttack = false;
    public KeyCode animationKey = KeyCode.Mouse1;
    public float castAttackCooldown = 1.0f;
    private float castAttackTimer = 0.0f;

    private ThirdPersonController thirdPersonController;

    private Animator myAnimator;

    public GameObject characterGameObject;

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
        if (!isCastingAttack && castAttackTimer <= 0.0f && Input.GetKeyDown(animationKey))
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
    public void SetCastingAttackFalse()
    {
        isCastingAttack = false;
        thirdPersonController.UnlockMovement();
        otherAnimator.SetBool("IsCastingAttack", isCastingAttack);

        myAnimator.enabled = true;
    }
}


