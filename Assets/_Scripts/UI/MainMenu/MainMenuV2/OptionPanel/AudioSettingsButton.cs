using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AudioSettingsButton : Button {

    [SerializeField] private Sprite m_basicSprite;
    [SerializeField] private Slider m_settingSlider;
    public Slider SettingSlider { get { return m_settingSlider; } }


    public class AudioSettingsEvent : UnityEvent<GameObject> { }

    public AudioSettingsEvent m_onClick = new AudioSettingsEvent();

	// Use this for initialization
	protected override void Start () {
		
	}

    public override void OnSubmit(BaseEventData eventData)
    {
        m_onClick.Invoke(m_settingSlider.gameObject);
        base.OnSubmit(eventData);
    }
}
