using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBox : MonoBehaviour
{
    public GameObject winBox;
    public AudioSource winAudio;

    // Start is called before the first frame update
    void Start()
    {
        winBox.SetActive(false);
        winAudio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameEvents.OnBoardCompleted += ShowWinBox;
        AdManager.OnInterstitialClosed += IntersitiialAdCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= ShowWinBox;
        AdManager.OnInterstitialClosed -= IntersitiialAdCompleted;
    }

    private void IntersitiialAdCompleted()
    {

    }

    private void ShowWinBox()
    {
        winAudio.Play();
        AdManager.Instance.HideBanner();
        SoundManager.instance.SilienceBackgroundMusic(true);

        if (winBox != null)
            winBox.SetActive(true);
    }

    public void LoadNextLevel()
    {
        if (winAudio.isPlaying)
            winAudio.Stop();

        GameEvents.LoadNextLevelMethod();
        SoundManager.instance.SilienceBackgroundMusic(false);
    }

}
