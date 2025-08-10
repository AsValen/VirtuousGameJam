using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private Image title;
    [SerializeField] private Button restart;
    [SerializeField] private Button mainMenu;

	private void Start()
	{
        title.rectTransform.DOScale(1.2f, 0.5f).SetEase(Ease.OutBounce).SetDelay(0.2f).OnComplete(() =>
        {
            title.rectTransform.DOScale(1f, 0.5f).SetEase(Ease.InOutQuad);
        });
        // Set up button animations
        mainMenu.transform.localScale = Vector3.zero;
		mainMenu.transform.DOScale(3f, 0.5f).SetEase(Ease.OutBack);
        restart.transform.localScale = Vector3.zero;
        restart.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f);
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
