using UnityEngine;
public class GameSceneAudio : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip bgm;

    [SerializeField] private AudioClip buttonClickSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource && bgm != null)
        {
            audioSource.clip = bgm;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}