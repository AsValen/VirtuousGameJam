using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private Image title;
    [SerializeField] private Button restart;
    [SerializeField] private Button mainmenu;

    private void Start()
    {
        // Animate the title
        title.rectTransform.DOScale(4.2f, 0.5f).SetEase(Ease.OutBounce).SetDelay(0.2f).OnComplete(() =>
        {
            title.rectTransform.DOScale(4f, 0.5f).SetEase(Ease.InOutQuad);
        });
        // Set up button animations
        restart.transform.localScale = Vector3.zero;
        mainmenu.transform.localScale = Vector3.zero;
        restart.transform.DOScale(3f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f);
        mainmenu.transform.DOScale(3f, 0.5f).SetEase(Ease.OutBack).SetDelay(1f);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
