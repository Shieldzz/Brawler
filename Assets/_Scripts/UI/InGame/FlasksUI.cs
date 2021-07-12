using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{

    public class FlasksUI : MonoBehaviour
    {

        public Sprite m_noneSprite;
        public Sprite m_speedUpSprite;
        public Sprite m_jumpSprite;
        public Sprite m_dashSprite;

        private Image m_image;

        // Use this for initialization
        void Start()
        {
            m_image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnDesactivateFlask()
        {
            m_image.sprite = m_noneSprite;
        }

        public void OnActivateSpeedFlask()
        {
            m_image.sprite = m_speedUpSprite;
        }

        public void OnActivateJumpFlask()
        {
            m_image.sprite = m_jumpSprite;
        }

        public void OnActivateDashFlask()
        {
            m_image.sprite = m_dashSprite;
        }
    }

}