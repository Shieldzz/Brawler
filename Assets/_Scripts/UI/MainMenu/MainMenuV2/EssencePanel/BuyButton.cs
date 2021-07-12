using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BTA
{

    public class BuyButton : Button
    {

        [SerializeField] private GameObject m_decoratorFeedback1 = null;
        [SerializeField] private GameObject m_decoratorFeedback2 = null;
        [SerializeField] private GameObject m_decoratorFeedback3 = null;
        [SerializeField] private GameObject m_decoratorFeedback4 = null;

        // Use this for initialization
        override protected void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CustonSubmitCaller()
        {
            OnSubmit(new BaseEventData(EventSystem.current));
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
        }
    }
}