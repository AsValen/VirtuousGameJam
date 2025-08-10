using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image title;
    [SerializeField] private Button play;
    [SerializeField] private Button quit;

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
    title.rectTransform.DOScale(4.2f, 0.5f).SetEase(Ease.OutBounce).SetDelay(0.2f).OnComplete(() =>
        {
            title.rectTransform.DOScale(4f, 0.5f).SetEase(Ease.InOutQuad);
        });
        // Set up button animations
        play.transform.localScale = Vector3.zero;
        quit.transform.localScale = Vector3.zero;
        play.transform.DOScale(3f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f);
        quit.transform.DOScale(3f, 0.5f).SetEase(Ease.OutBack).SetDelay(1f);
    }

    public void PlayGame()
    {
        sfxSource.PlayOneShot(buttonClick);
        SceneManager.LoadScene("Level1");
    }

    public void ExitGame()
    {
        sfxSource.PlayOneShot(buttonClick);
        Application.Quit();
    }
}
