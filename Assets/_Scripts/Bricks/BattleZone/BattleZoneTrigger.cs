using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

namespace BTA
{
    public class BattleZoneTrigger : MonoBehaviour
    {
        CameraManager m_camMgr;

        Collider m_triggerCollider;
        [SerializeField] CinemachineVirtualCamera m_battleZoneCamera;

        public UnityEvent OnBattleZoneActivate = new UnityEvent();

        // Use this for initialization
        void Start()
        {
            m_camMgr = LevelManager.Instance.GetInstanceOf<CameraManager>();
            if (!m_camMgr)
                Debug.LogError(this + " cannot find Camera Manager");

            m_triggerCollider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            BattleZoneActivation();
        }

        private void BattleZoneActivation()
        {
            m_battleZoneCamera.gameObject.SetActive(true);
            m_camMgr.EnableTemporaryCamera(m_battleZoneCamera);

            OnBattleZoneActivate.Invoke();
            m_triggerCollider.enabled = false;
        }

        public void OnBattleZoneDone()
        {
            m_battleZoneCamera.gameObject.SetActive(false);
            m_camMgr.ResetCamera();
        }
    }
}