using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{

    public class MoneyIndicator : MonoBehaviour
    {

        [SerializeField] private Text m_moneyText;

        private GameplayManager m_gameplayManager = null;

        private void Awake()
        {
            
        }

        // Use this for initialization
        void Start()
        {
            if (!m_gameplayManager)
                m_gameplayManager = GameManager.Instance.GetInstanceOf<GameplayManager>();
            UpdateMoney(m_gameplayManager.Money);
            m_gameplayManager.m_onMoneyChange.AddListener(UpdateMoney);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            if(m_gameplayManager)
                UpdateMoney(m_gameplayManager.Money);
        }

        private void UpdateMoney(float value)
        {
            m_moneyText.text = value.ToString();
        }
    }

}