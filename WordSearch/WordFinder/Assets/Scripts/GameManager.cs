using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameOverScreen gameOverScreen;
    public ClockTimer timer;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);


        if (SoundManager.instance.IsBackgroundMusicMuted() == false)
        {
            SoundManager.instance.SilienceBackgroundMusic(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void HideBannerAd()
    {
        AdManager.Instance.HideBanner();
    }

    public void MuteToggleBackgroundMusic()
    {
        SoundManager.instance.ToggleBackgroundMusic();
    }

    public void MuteToggleSoundFX()
    {
        SoundManager.instance.ToggleSoundFX();
    }

    public void ShowAdsForExtraTime()
    {
        gameOverScreen.gameObject.SetActive(false);
        AdManager.Instance.ShowRewardedAd();
        timer.timeLeft = 60f;
        timer.timeFinish = false;
        timer.stopTimer = false;
    }
}
