using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    private PlayerMovement movement;
    private Animator animator;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform weaponHitBoxTransform;
    [SerializeField] private Vector3 weaponOffsetLeft;
    [SerializeField] private Vector3 weaponOffsetRight;
    private SpriteRenderer sr;
    private WeaponHitbox weapon;

    private InputAction baseAttackAction;
    private InputAction heavyAttackAction;

    [SerializeField] private int baseAtk = 5;
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private float maxChargeTime = 1f;

    private float attackTimer;
    private bool isCharging;
    private float chargeStartTime;

    private AudioSource audioSource;
    [SerializeField] private AudioClip swordSlashSound;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        weapon = weaponHitBoxTransform.GetComponent<WeaponHitbox>();
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        UpdateWeaponPositionAndFlip();
    }

    private void OnEnable()
    {

        baseAttackAction = InputSystem.actions.FindAction("BaseAttack");
        heavyAttackAction = InputSystem.actions.FindAction("HeavyAttack");

        if (baseAttackAction != null)
            baseAttackAction.performed += OnBaseAttackPerformed;

        if (heavyAttackAction != null)
        {
            heavyAttackAction.started += OnHeavyAttackStarted;
            heavyAttackAction.canceled += OnHeavyAttackReleased;
        }
    }

    private void OnDisable()
    {
        if (baseAttackAction != null)
        {
            baseAttackAction.performed -= OnBaseAttackPerformed;
            baseAttackAction = null;
        }

        if (heavyAttackAction != null)
        {
            heavyAttackAction.started -= OnHeavyAttackStarted;
            heavyAttackAction.canceled -= OnHeavyAttackReleased;
            heavyAttackAction = null;
        }
    }

    private void OnBaseAttackPerformed(InputAction.CallbackContext context)
    {
        if (movement.CanAttack() && attackTimer <= 0f)
        {
            animator.SetTrigger("Attack");
            attackTimer = attackCooldown;

            if (weapon != null)
            {
                weapon.SetDamage(baseAtk);
                weapon.EnableHitbox();
                audioSource.PlayOneShot(swordSlashSound);
                StartCoroutine(DisableHitboxAfterTime(0.2f));
            }
        }
    }

    private void OnHeavyAttackStarted(InputAction.CallbackContext context)
    {
        if (movement.CanAttack() && attackTimer <= 0f)
        {
            movement.enabled = false;
            isCharging = true;
            chargeStartTime = Time.time;
            animator.SetBool("isCharging", true);
            animator.SetBool("isChargeLooping", false);

            StartCoroutine(EnableChargeLoopAfterDelay(0.1f));
        }
    }

    private void OnHeavyAttackReleased(InputAction.CallbackContext context)
    {
        if (!isCharging)
        {
            return;
        }
        isCharging = false;

        animator.SetBool("isCharging", false);
        animator.SetBool("isChargeLooping", false);

        float heldTime = Time.time - chargeStartTime;
        float chargeRatio = Mathf.Clamp01(heldTime / maxChargeTime);

        int damage = Mathf.RoundToInt(baseAtk + (baseAtk * chargeRatio));
        animator.SetTrigger("ChargedAttack");

        if (weapon != null)
        {
            weapon.SetDamage(damage);
            weapon.EnableHitbox();
            audioSource.PlayOneShot(swordSlashSound);
            StartCoroutine(DisableHitboxAfterTime(0.3f));
        }

        StartCoroutine(EnableMovementAfterHeavyHit(1.5f));
        attackTimer = attackCooldown;
    }

    private void UpdateWeaponPositionAndFlip()
    {
        if (sr.flipX)
        {
            weaponHitBoxTransform.localPosition = weaponOffsetLeft;
            weaponHitBoxTransform.localScale = new Vector3(-1.5f, 1, 1);
        }
        else
        {
            weaponHitBoxTransform.localPosition = weaponOffsetRight;
            weaponHitBoxTransform.localScale = new Vector3(1.5f, 1, 1);
        }
    }

    private IEnumerator EnableChargeLoopAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isCharging)
        {
            animator.SetBool("isChargeLooping", true);
        }
    }

    private IEnumerator DisableHitboxAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (weapon != null)
        {
            weapon.DisableHitbox();
        }
    }

    private IEnumerator EnableMovementAfterHeavyHit(float delay)
    {
        yield return new WaitForSeconds(delay);
        movement.enabled = true;
    }
}
