using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class EconomicData : MonoBehaviour
    {

        [SerializeField] private float m_slimValueChange = 5f;
        [SerializeField] private float m_moneyAccountAtStart = 100000f;

        // Use this for initialization
        void Start()
        {
            GameplayManager gameplayManager = GameManager.Instance.GetInstanceOf<GameplayManager>();
            gameplayManager.SetEconomicData(m_slimValueChange, m_moneyAccountAtStart);
        }
    }

}