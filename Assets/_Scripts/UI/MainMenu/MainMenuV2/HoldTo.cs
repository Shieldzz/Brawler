using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{

    public class HoldTo : MonoBehaviour
    {

        [SerializeField] protected float m_holdTime = 2f;
        [SerializeField] private Image m_holderImage = null;
        private float m_currentTimer = 0f;
        private bool m_isHolding = false;
        private bool m_isUnload = false;
        protected float m_holdProgess = 0f;

        virtual protected void Start()
        {
            SetupImage();
        }

        private void OnDisable()
        {
            ResetHolderData();
        }

        virtual protected void Update()
        {
            if (m_isHolding)
                Load();
            else if (m_isUnload)
                Unload();

            FillImage();
        }

        private void Load()
        {
            if (m_currentTimer < m_holdTime)
                m_currentTimer += Time.deltaTime;

            m_holdProgess = m_currentTimer / m_holdTime;

            if (m_currentTimer >= m_holdTime)
                HoldComplete();
        }

        private void Unload()
        {
            if (m_currentTimer > 0f)
                m_currentTimer -= Time.deltaTime;
            m_holdProgess = m_currentTimer / m_holdTime;

            if (m_currentTimer <= 0f)
                UnloadComplete();
        }

        virtual protected void SetupImage()
        {
            if (!m_holderImage)
                return;

            m_holderImage.type = Image.Type.Filled;
        }

        virtual protected void FillImage()
        {
            if (!m_holderImage)
                return;

            m_holderImage.fillAmount = m_holdProgess;
        }

        public void StartHolding()
        {
            m_isHolding = true;
            m_isUnload = false;
        }

        public void StopHolding()
        {
            m_isHolding = false;
            m_isUnload = true;
        }

        virtual public void UnloadComplete()
        {
            ResetHolderData();
        }

        virtual public void HoldComplete()
        {
            ResetHolderData();
        }

        private void ResetHolderData()
        {
            m_isHolding = false;
            m_isUnload = false;
            m_currentTimer = 0f;
            m_holdProgess = 0f;
            FillImage();
        }
    }

}