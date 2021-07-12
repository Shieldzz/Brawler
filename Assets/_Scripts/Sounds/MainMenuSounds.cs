using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BTA
{
    public class MainMenuSounds : MonoBehaviour
    {
        [FMODUnity.EventRef]
        public string m_changeGUIButtonSound;

        private GameObject m_gameObject;

        private void Start()
        {
            m_gameObject = EventSystem.current.currentSelectedGameObject;
        }

        private void Update()
        {
            if (m_gameObject != EventSystem.current.currentSelectedGameObject)
            {
                LaunchChangeGUIButtonSound();
                m_gameObject = EventSystem.current.currentSelectedGameObject;
            }
        }

        public void LaunchChangeGUIButtonSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_changeGUIButtonSound);
        }
    }
}
