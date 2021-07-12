using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{

    public class PlayerDecorator : MonoBehaviour
    {
        [SerializeField] RectTransform m_soloTransform;
        [SerializeField] RectTransform m_duoTransform;

        void OnEnable()
        { 
            if (GameManager.Instance.GetGameMode() == GameMode.Duo)
                SpreadDecorator();
            else
                CenterDecorator();
        }

        public void CenterDecorator()
        {
            transform.position = m_soloTransform.position;
        }

        public void SpreadDecorator()
        {
            transform.position = m_duoTransform.position;
        }
    }

}