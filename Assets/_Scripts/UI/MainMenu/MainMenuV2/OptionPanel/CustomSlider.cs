using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace BTA
{

    public class CustomSlider : Slider
    {

        [SerializeField] private Image m_background = null;
        [SerializeField] private Sprite m_basicBackground = null;
        [SerializeField] private Sprite m_highlightBackground = null;

        override protected void Start()
        {
            base.Start();
        }

        void Update()
        {

        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            m_background.sprite = m_highlightBackground;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            m_background.sprite = m_basicBackground;
        }
    }

}