using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;


public class Volume : MonoBehaviour
{

    public Slider slider;
    public AudioMixer mixer;

    //private void Update()
    //{
    //   // valueCount.text = slider.value.ToString();
    //AudioListener.volume = slider.value;
    //    slider.value = PlayerPrefs.GetFloat("VolumeValue");

    //}

    public void masterVolumeChangeValue(float value)
    {
        AudioListener.volume = slider.value;

        mixer.SetFloat("volume", value);

        mixer.GetFloat("volume", out value);

        PlayerPrefs.SetFloat("volume", value);
        PlayerPrefs.Save();
    }
    void Start()
    {

        if (PlayerPrefs.HasKey("volume"))
        {
            slider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume");
        }
    }
}
