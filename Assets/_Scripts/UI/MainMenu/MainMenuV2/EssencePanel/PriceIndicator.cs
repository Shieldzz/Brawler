using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BTA
{

    public class PriceIndicator : MonoBehaviour
    {
        [SerializeField] Text m_lockText;
        [SerializeField] Text m_ownText;
        [SerializeField] Text m_priceText;

        public void SetLock()
        {
            m_lockText.gameObject.SetActive(true);
            m_ownText.gameObject.SetActive(false);
            m_priceText.gameObject.SetActive(false);
        }

        public void SetOwned()
        {
            m_lockText.gameObject.SetActive(false);
            m_ownText.gameObject.SetActive(true);
            m_priceText.gameObject.SetActive(false);
        }

        public void SetPrice(float price)
        {
            m_priceText.text = price.ToString();

            m_lockText.gameObject.SetActive(false);
            m_ownText.gameObject.SetActive(false);
            m_priceText.gameObject.SetActive(true);
        }

        public void SetNone()
        {
            m_lockText.gameObject.SetActive(false);
            m_ownText.gameObject.SetActive(false);
            m_priceText.gameObject.SetActive(false);
        }

    }
}
