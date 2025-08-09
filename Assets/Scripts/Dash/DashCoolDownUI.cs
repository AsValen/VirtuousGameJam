using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashCoolDownUI : MonoBehaviour
{
    [SerializeField] private DashAbility dashAbility;
    private Image cooldownImage;
    private TextMeshProUGUI cooldownTime;
    private float defaultCooldownDuration;
    private float currentCooldownDuration;

    void OnEnable()
    {
        DashAbility.OnPlayerDash += SetCooldownUI;
    }

    void OnDisable()
    {
        DashAbility.OnPlayerDash -= SetCooldownUI;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dashAbility = GameObject.FindGameObjectWithTag("Player").GetComponent<DashAbility>();
        cooldownImage = GameObject.FindGameObjectWithTag("CooldownLayer").GetComponent<Image>();
        cooldownImage.fillAmount = 0f;
        cooldownTime = GetComponentInChildren<TextMeshProUGUI>();
        defaultCooldownDuration = dashAbility.DashCooldownDuration;
        cooldownTime.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(dashAbility.IsDashCooldown)
        {
            if(currentCooldownDuration <= 0f)
            {
                currentCooldownDuration = defaultCooldownDuration;
            } 
            else
            {
                currentCooldownDuration -= Time.deltaTime;
                cooldownImage.fillAmount = currentCooldownDuration / defaultCooldownDuration;
            }

            cooldownTime.text = Mathf.Round(currentCooldownDuration).ToString();
        }
        else
        {
            ResetCooldownUI();
        }
    }

    void SetCooldownUI()
    {
        currentCooldownDuration = defaultCooldownDuration;
        cooldownTime.text = defaultCooldownDuration.ToString();
        cooldownImage.fillAmount = 1f;
    }

    void ResetCooldownUI()
    {
        cooldownTime.text = "";
        cooldownImage.fillAmount = 0f;
    }


}
