using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class PlayerUIContainer : MonoBehaviour
    {
        private HealthBar m_healthBar;
        private ChainHandler m_chainHandler;
        private ChainUI m_chainUI;
        private SlimeBallUI m_slimeBallUI;

        public HealthBar HealthBar { get { return m_healthBar; } }
        public ChainHandler ChainHandler { get { return m_chainHandler; } }
        public ChainUI ChainUI { get { return m_chainUI; } }
        public SlimeBallUI SlimeBallUI { get { return m_slimeBallUI; } }
       
        // Use this for initialization
        void Awake()
        {
            m_healthBar = GetComponentInChildren<HealthBar>();
            m_chainHandler = GetComponentInChildren<ChainHandler>();
            m_chainUI = GetComponentInChildren<ChainUI>();
            m_slimeBallUI = GetComponentInChildren<SlimeBallUI>();
        }
    }
}
