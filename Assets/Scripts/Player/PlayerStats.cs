using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int playerHP = 100;
    private int maxHP;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image fillImage;

    private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;

    private void Start()
    {
        maxHP = playerHP;
        hpSlider.maxValue = maxHP;
        hpSlider.value = playerHP;
        UpdateHPUI();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        ExorcistEnemy.OnExorcistEnemy += HealPlayer;
    }

    private void OnDisable()
    {
        ExorcistEnemy.OnExorcistEnemy -= HealPlayer;
    }

    private void HealPlayer(int healingAmount)
    {
        playerHP = Mathf.Clamp(playerHP + healingAmount, 0, maxHP);
        hpSlider.value = playerHP;
        UpdateHPUI();
    }

    public int GetPlayerHP() => playerHP;

    public void SetPlayerHP(int damage)
    {
        playerHP = Mathf.Clamp(playerHP - damage, 0, maxHP);
        hpSlider.value = playerHP;

        UpdateHPUI();
    }

    private void UpdateHPUI()
    {
        if (playerHP <= 0)
        {
            StartCoroutine(goToDeathMenu());
        }
        else if (playerHP <= 3)
        {
            fillImage.color = Color.red;
        }
        else
        {
            fillImage.color = Color.green;
        }
    }

    private IEnumerator goToDeathMenu() 
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("DeathMenu");
    }
}
