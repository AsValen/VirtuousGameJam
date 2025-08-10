using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
	[SerializeField] private Button mainMenu;

	private void Start()
	{
		// Set up button animations
		mainMenu.transform.localScale = Vector3.zero;
		mainMenu.transform.DOScale(3f, 0.5f).SetEase(Ease.OutBack);
	}

	public void MainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
