using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
	[SerializeField] private Button mainMenu;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    [SerializeField] private AudioClip bgm;
	[SerializeField] private AudioClip buttonClick;


	private void Start()
	{
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.clip = bgm;
        bgmSource.Play();

        sfxSource = gameObject.AddComponent<AudioSource>();

        // Set up button animations
        mainMenu.transform.localScale = Vector3.zero;
		mainMenu.transform.DOScale(3f, 0.5f).SetEase(Ease.OutBack);
	}

    public void RestartGame()
    {
        sfxSource.PlayOneShot(buttonClick);
        SceneManager.LoadScene("Level1");
    }

    public void MainMenu()
	{
        sfxSource.PlayOneShot(buttonClick);
        SceneManager.LoadScene("MainMenu");
	}
}
