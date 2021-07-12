using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class FXTriggerHandler : MonoBehaviour
    {
        [SerializeField] AttackStruct m_attackStruct;
        [SerializeField] float m_startUpDuration = 0f;
        [SerializeField] float m_connectDuration = 0.2f;
        [SerializeField] float m_destroyDuration = 0f;

        AttackTrigger m_trigger;

        float m_startingTimer = 0f;
        float m_connectTimer = 0f;
        float m_destroyTimer = 0f;

        int m_currattackID = 0;

        private void Awake()
        {
            m_trigger = GetComponent<AttackTrigger>();
            if (!m_trigger)
                Debug.Log(gameObject + "'s FXHandler has no trigger !");
        }

        // Use this for initialization
        void OnEnable()
        {
            m_startingTimer = m_startUpDuration;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_startingTimer >= 0f)
            {
                m_startingTimer -= Time.deltaTime;
                if (m_startingTimer <= 0f)
                    StartConnect(0);
            }

            else if (m_connectTimer >= 0f)
            {
                m_connectTimer -= Time.deltaTime;
                if (m_connectTimer <= 0f)
                    EndConnect();
            }

            else if (m_destroyTimer >= 0f)
            {
                m_destroyTimer -= Time.deltaTime;
                if (m_destroyTimer <= 0f)
                    Destroy(gameObject);
            }
        }

        private void StartConnect(int id)
        {
            m_connectTimer = m_connectDuration;
            m_trigger.Enable(m_attackStruct.attackDatas[id]);
        }

        private void EndConnect()
        {
            m_trigger.Disable();
            m_currattackID++;

            // if there is more connect to Do 
            if (m_attackStruct.attackDatas.Length > m_currattackID)
                StartConnect(m_currattackID);
            else
                m_destroyTimer = m_destroyDuration;
        }
    }
}
