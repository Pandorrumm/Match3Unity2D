using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using System;

public class AdMobInterstitial : MonoBehaviour
{
    public string AppID;
    public string InterstitialAdUnitID;
    private InterstitialAd interstitialAd;
   // public string levelToLoad;

    void Start()
    {
        MobileAds.Initialize(AppID);
        interstitialAd = new InterstitialAd(InterstitialAdUnitID);
        interstitialAd.LoadAd(new AdRequest.Builder().Build());
        interstitialAd.OnAdClosed += HandlerOnClosed;
    }

    public void HandlerOnClosed(object sendler, EventArgs args)
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadSceneAsync(levelToLoad);
        SceneManager.LoadScene("Splash");
    }

    public void ShowAds(/*level*/)
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            // SceneManager.LoadSceneAsync(levelToLoad);
            SceneManager.LoadScene("Splash");
        }
       // levelToLoad = level;
    }
}
