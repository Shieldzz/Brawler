using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BTA
{

    public class AudioSettingsController : MonoBehaviour
    {

        [SerializeField] private MenuGUIManager m_menuGuiManager;
        [SerializeField] private GameObject m_masterVolumeSlider;

        void Start()
        {
        }

        void Update()
        {

        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(m_masterVolumeSlider);
            m_masterVolumeSlider.GetComponent<Slider>().OnSelect(null);
            LinkHelperToSettingsButton();
        }


        private void LinkHelperToSettingsButton()
        {
            HelperUIManager helperManager = HelperUIManager.Instance;

            helperManager.DisableAllHelper();
            helperManager.ClearAllControllerActions();

            helperManager.EnableHelper(helperManager.m_helper1GameObject);
            helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconB, "Back", helperManager.m_controller1Helper1Action += m_menuGuiManager.EnableMainPanel, GamePadID.Controller1);
            if(GameManager.Instance.GetGameMode() == GameMode.Duo)
                helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconB, "Back", helperManager.m_controller2Helper1Action += m_menuGuiManager.EnableMainPanel, GamePadID.Controller2);
        }

    }
}