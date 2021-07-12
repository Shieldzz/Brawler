using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace BTA
{

    public class CityButton : Button
    {

        public bool m_isLock = false;
        [SerializeField] private Sprite m_lineSprite = null;
        [SerializeField] private Image m_glowLine = null;

        // Use this for initialization
        override protected void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnSelect(BaseEventData eventData)
        {
            LinkHelperBar();
            m_glowLine.sprite = m_lineSprite;
            base.OnSelect(eventData);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            if (m_isLock)
                return;
            base.OnSubmit(eventData);
        }

        private void CustomSubmitCaller()
        {
            OnSubmit(new BaseEventData(EventSystem.current));
        }

        private void LinkHelperBar()
        {
            bool isDuo = GameManager.Instance.GetGameMode() == GameMode.Duo;

            HelperUIManager helperManager = HelperUIManager.Instance;

            helperManager.ClearAction(ref helperManager.m_controller1Helper1Action, GamePadID.Controller1, GamePadInput.ButtonA);
            helperManager.ClearAction(ref helperManager.m_controller2Helper1Action, GamePadID.Controller2, GamePadInput.ButtonA);

            helperManager.EnableHelper(helperManager.m_helper1GameObject);

            helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Details", helperManager.m_controller1Helper1Action += CustomSubmitCaller, GamePadID.Controller1);
            if(isDuo)
                helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Details", helperManager.m_controller2Helper1Action += CustomSubmitCaller, GamePadID.Controller2);

        }
    }

}