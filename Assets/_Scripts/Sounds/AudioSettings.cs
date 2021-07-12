using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    FMOD.Studio.EventInstance SfxEvent;

    FMOD.Studio.Bus Master;
    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;

    float m_masterVolume = 0.5f;
    float m_musicVolume = 0.5f;
    float m_sfxVolutme = 0.3f;

    [SerializeField] Slider m_masterSlider;
    [SerializeField] Slider m_musicSlider;
    [SerializeField] Slider m_sfxSlider;

    [FMODUnity.EventRef]
    public string m_sfxVolumeTest;

    // Use this for initialization
    void Start()
    {
        Master = FMODUnity.RuntimeManager.GetBus("Bus:/");
        Music = FMODUnity.RuntimeManager.GetBus("Bus:/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("Bus:/SFX");

        SfxEvent = FMODUnity.RuntimeManager.CreateInstance(m_sfxVolumeTest);

        InitSlider();
    }



    void InitSlider()
    {
        float temp;
        Master.getVolume(out m_masterVolume, out temp);
        m_masterSlider.value = m_masterVolume;

        Music.getVolume(out m_masterVolume, out temp);
        m_musicSlider.value = m_masterVolume;

        SFX.getVolume(out m_masterVolume, out temp);
        m_sfxSlider.value = m_masterVolume;
    }

    public void UpdateMasterVolume(float newVolume)
    {
        m_masterVolume = newVolume;
        Master.setVolume(newVolume);
    }

    public void UpdateMusicVolume(float newVolume)
    {
        m_musicVolume = newVolume;
        Music.setVolume(newVolume);
    }

    public void UpdateSFXVolume(float newVolume)
    {
        m_sfxVolutme = newVolume;
        SFX.setVolume(newVolume);

        FMOD.Studio.PLAYBACK_STATE state;
        SfxEvent.getPlaybackState(out state);

        //if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            SfxEvent.start();
    }
}
