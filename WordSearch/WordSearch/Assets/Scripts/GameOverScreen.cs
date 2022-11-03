using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public ClockTimer timer;
    public GameObject gameOverScreen;
    public GameObject continueGameAfterAdsButton;
    public AudioSource gameOverAudio;


    // Start is called before the first frame update
    void Start()
    {
        gameOverAudio = GetComponent<AudioSource>();
        continueGameAfterAdsButton.GetComponent<Button>().interactable = true;
        gameOverScreen.SetActive(false);
        GameEvents.OnGameOver += ShowGameOverScreen;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= ShowGameOverScreen;
    }

    public void ShowGameOverScreen()
    {
        gameOverAudio.Play();
        gameOverScreen.SetActive(true);
        AdManager.Instance.HideBanner();
        continueGameAfterAdsButton.GetComponent <Button>().interactable = true;
    }

    public void ShowAdForExtraTime()
    {
        if (gameOverAudio.isPlaying)
            gameOverAudio.Stop();

        //gameOverScreen.SetActive(false);
    }
}
