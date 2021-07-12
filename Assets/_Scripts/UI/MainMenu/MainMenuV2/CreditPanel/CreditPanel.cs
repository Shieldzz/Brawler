using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace BTA
{

    public class CreditPanel : MonoBehaviour
    {

        void Start()
        {

        }

        void Update()
        {

        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(null);
            LinkHelperBar();
        }

        private void LinkHelperBar()
        {
            MenuGUIManager menuGuiManager = MenuGUIManager.Instance;

            HelperUIManager helperManager = HelperUIManager.Instance;
            helperManager.ClearAllControllerActions();
            helperManager.DisableAllHelper();

            //helperManager.EnableHelper(helperManager.m_helper1GameObject);
            //helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "return");

            helperManager.EnableHelper(helperManager.m_helper1GameObject);
            helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconB, "Return", helperManager.m_controller1Helper1Action += menuGuiManager.EnableMainPanel);
            if(GameManager.Instance.GetGameMode() == GameMode.Duo)
                helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconB, "Return", helperManager.m_controller2Helper1Action += menuGuiManager.EnableMainPanel, GamePadID.Controller2);
        }
    }

}