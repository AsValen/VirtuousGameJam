using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int playerHP = 10;
    private int maxHP;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image fillImage;

    private void Start()
    {
        maxHP = playerHP;
        hpSlider.maxValue = maxHP;
        hpSlider.value = playerHP;
        UpdateHPUI();
    }
    public int GetPlayerHP()
    {
        return playerHP;
    }

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
            Destroy(gameObject);
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
}
