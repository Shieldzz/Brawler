using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BTA
{
    public class SelectedTextUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField] Text m_linkedText;

        [SerializeField] Color m_baseColor;
        [SerializeField] Color m_selectedColor;

        public void OnEnable()
        {
           if (gameObject == EventSystem.current.currentSelectedGameObject)
                m_linkedText.color = m_selectedColor;
        }

        public void OnDisable()
        {
            m_linkedText.color = m_baseColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_linkedText.color = m_selectedColor;
        }
        public void OnSelect(BaseEventData eventData)
        {
            m_linkedText.color = m_selectedColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_linkedText.color = m_baseColor;
        }
        public void OnDeselect(BaseEventData eventData)
        {
            m_linkedText.color = m_baseColor;
        }
    }
}
