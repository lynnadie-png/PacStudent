using UnityEngine;

public class IntroMusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip introClip;
    public AudioClip ghostNormalClip;

    private bool hasSwitched = false;

    void Start()
    {
        audioSource.clip = introClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    void Update()
    {
        if (hasSwitched) return;

        bool introFinished = !audioSource.isPlaying;
        bool threeSecondsPassed = audioSource.time >= 3f;

        if (introFinished || threeSecondsPassed)
        {
            SwitchToGhostNormal();
        }
    }

    void SwitchToGhostNormal()
    {
        hasSwitched = true;
        audioSource.clip = ghostNormalClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}