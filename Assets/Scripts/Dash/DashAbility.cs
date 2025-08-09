using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Linq;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
public class DashAbility : MonoBehaviour
{
    private InputAction dash;

    private Rigidbody2D rb;

    [SerializeField] private float dashSpeed = 5f;
    private bool isDashCooldown = false;
    [SerializeField]  private float dashCooldownDuration = 3.0f; 
    private bool isInvulnerable = false;
    [SerializeField] private float invulnerabilityDuration = 1f;
    [SerializeField] private float gravityScale = 1f;

    private AudioSource audioSource;
    [SerializeField] private AudioClip dashSound;

    private bool isDashing = false;

    public static event Action OnPlayerDash;

    private LineRenderer dashLineRenderer;
    private Vector2 dashStartPosition;

    public float DashCooldownDuration
    {
        get => dashCooldownDuration;
    }

    public bool IsDashCooldown
    {
        get => isDashCooldown;
    }

    public bool IsDashing
    {
        get => isDashing;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        dashLineRenderer = GetComponent<LineRenderer>();
        dashLineRenderer.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        dash = InputSystem.actions.FindAction("Dash");

        if (dash != null)
        {
            dash.performed += dash_performed;
        }
    }

    private void OnDisable()
    {
        if (dash != null)
        {
            dash.performed -= dash_performed;
            dash = null;
        }
    }

    private void dash_performed(InputAction.CallbackContext context)
    {
        string bindingName = dash.activeControl.name;
        Debug.Log($"Dash triggered by: {bindingName}");

        if (bindingName == "a" || bindingName == "leftArrow")
        {
            Dash(Vector2.left);
        }
        else if (bindingName == "d" || bindingName == "rightArrow")
        {
            Dash(Vector2.right);
        }
    }

    private void Dash (Vector2 direction)
    {  
        if (!isDashCooldown)
        {

            if (audioSource != null && dashSound != null)
            {
                audioSource.PlayOneShot(dashSound);
            }

            dashStartPosition = transform.position;
            gravityScale = rb.gravityScale;
            rb.gravityScale = 0f;
            rb.AddForce(direction * dashSpeed, ForceMode2D.Impulse);
            //rb.linearVelocity = new Vector2(direction.x * dashSpeed, rb.linearVelocity.y);

            dashLineRenderer.positionCount = 2;
            dashLineRenderer.SetPosition(0, dashStartPosition);
            dashLineRenderer.enabled = true;

            OnPlayerDash?.Invoke();
            isDashing = true;
            isDashCooldown = true;
            isInvulnerable = true;
            StartCoroutine(InvulnerabilityCoroutine());
            StartCoroutine(CoolDownCoroutine());
        }
    }

    private void Update()
    {
        if (isDashCooldown && dashLineRenderer.enabled == true)
        {
            Debug.Log("Dash Line Renderer is enabled, updating position.");
            dashLineRenderer.SetPosition(1, transform.position);
        }
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        yield return new WaitForSeconds(invulnerabilityDuration);
        dashLineRenderer.enabled = false;
        rb.gravityScale = gravityScale;
        isDashing = false;
        isInvulnerable = false;
        Debug.Log("Dash invulnerability ended.");
    }

    private IEnumerator CoolDownCoroutine()
    {
        yield return new WaitForSeconds(dashCooldownDuration);
        isDashCooldown = false;
        Debug.Log("Cool Down ended");
    }
}
