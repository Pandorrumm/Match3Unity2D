using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobBanner : MonoBehaviour {

    public string AppID;
    public string BannerAdUnitID;

    private BannerView bannerView;

    void Start()
    {
        MobileAds.Initialize(AppID);

        bannerView = new BannerView(BannerAdUnitID, AdSize.Banner, AdPosition.Bottom);
        bannerView.LoadAd(new AdRequest.Builder().Build());

    }
}
