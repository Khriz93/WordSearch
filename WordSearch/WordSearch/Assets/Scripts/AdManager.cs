using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public string appId;
    public string adBannerId;
    public string adIntersitialId;
    public string rewardedIntersititialId;
    public AdPosition bannerPosition;

    public bool testDevice = false;
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewarded;
    private ClockTimer timer;
    private GameOverScreen gameOverScreen;

    public static AdManager Instance;
    public static Action OnInterstitialClosed;
    public static Action OnInterstitialRewardedClosed;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });

        this.CreateBanner(CreateRequest());
        RequestRewardedAd();

        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial = new InterstitialAd(adIntersitialId);
        this.interstitial.LoadAd(request);

        this.interstitial.OnAdClosed += InterstitialAdClosed;
    }

    private void OnDisable()
    {
        if (interstitial != null)
            this.interstitial.OnAdClosed -= InterstitialAdClosed;

/*        if (rewarded != null)
            this.rewarded.OnAdDidDismissFullScreenContent -= InterstitialRewardedAdClosed;*/
    }

   private void InterstitialRewardedAdClosed(object sender, EventArgs e)
    {
        if (OnInterstitialRewardedClosed != null)
            OnInterstitialRewardedClosed();

        SoundManager.instance.SilienceBackgroundMusic(false);
    }

    private void InterstitialAdClosed(object sender, EventArgs e)
    {
        if (OnInterstitialClosed != null)
            OnInterstitialClosed();

        SoundManager.instance.SilienceBackgroundMusic(false);
        ShowBanner();
    }

    private AdRequest CreateRequest()
    {
        AdRequest request = new AdRequest.Builder().Build();

        return request;
    }

    public void CreateIntersitialAd(AdRequest request)
    {
        this.interstitial = new InterstitialAd(adIntersitialId);
        this.interstitial.LoadAd(request);
    }

    public void ShowIterstitialAd()
    {
        if (this.interstitial.IsLoaded())
        {
            HideBanner();
            this.interstitial.Show();
            SoundManager.instance.SilienceBackgroundMusic(true);
        }

        this.interstitial.LoadAd(CreateRequest());
    }

    public void RequestRewardedAd()
    {
        rewarded = new RewardedAd(rewardedIntersititialId);
        rewarded.OnUserEarnedReward += HandleUserEarnedReward;
        rewarded.OnAdClosed += HandleRewardedAdClosed;
        rewarded.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        AdRequest request = new AdRequest.Builder().Build();
        rewarded.LoadAd(request);
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        RequestRewardedAd();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        RequestRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward reward)
    {
        SoundManager.instance.SilienceBackgroundMusic(false);
        RequestRewardedAd();
    }

   public void ShowRewardedAd()
    {
        if (rewarded.IsLoaded())
        {
            rewarded.Show();
            SoundManager.instance.SilienceBackgroundMusic(true);
        }
            
        RequestRewardedAd();
    }

    public void CreateBanner(AdRequest request)
    {
        this.bannerView = new BannerView(adBannerId, AdSize.SmartBanner, bannerPosition);
        this.bannerView.LoadAd(request);
        HideBanner();
    }

    public void HideBanner()
    {
        bannerView.Hide();
    }

    public void ShowBanner()
    {
        bannerView.Show();
    }
}
