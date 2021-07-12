using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BTA
{
    public class MainMenuButton : Button
    {

        override protected void Start()
        {
        }

        void Update()
        {

        }

        override public void OnSelect(BaseEventData eventData)
        {
            HelperUIManager helperManager = HelperUIManager.Instance;

            helperManager.ClearAction(ref helperManager.m_controller1Helper1Action, GamePadID.Controller1, GamePadInput.ButtonA);
            helperManager.ClearAction(ref helperManager.m_controller2Helper1Action, GamePadID.Controller2, GamePadInput.ButtonA);

            helperManager.EnableHelper(helperManager.m_helper1GameObject);
            helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Confirm", helperManager.m_controller1Helper1Action += CustomSubmitCaller, GamePadID.Controller1);
            if(GameManager.Instance.GetGameMode() == GameMode.Duo)
                helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Confirm", helperManager.m_controller2Helper1Action += CustomSubmitCaller, GamePadID.Controller2);

            base.OnSelect(eventData);
        }


        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
        }

        private void CustomSubmitCaller()
        {
            OnSubmit(new BaseEventData(EventSystem.current));
        }
    }
}