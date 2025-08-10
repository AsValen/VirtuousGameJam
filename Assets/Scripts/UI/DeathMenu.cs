using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private Image title;
    [SerializeField] private Button restart;
    [SerializeField] private Button mainmenu;

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

        // Animate the title
        title.rectTransform.DOScale(3.2f, 0.5f).SetEase(Ease.OutBounce).SetDelay(0.2f).OnComplete(() =>
        {
            title.rectTransform.DOScale(3f, 0.5f).SetEase(Ease.InOutQuad);
        });
        // Set up button animations
        restart.transform.localScale = Vector3.zero;
        mainmenu.transform.localScale = Vector3.zero;
        restart.transform.DOScale(3f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f);
        mainmenu.transform.DOScale(3f, 0.5f).SetEase(Ease.OutBack).SetDelay(1f);
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
