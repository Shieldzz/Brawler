using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace BTA
{

    public class Flasks : MonoBehaviour
    {
        public UnityEvent OnFlaskActivated = new UnityEvent();
        public UnityEvent OnFlaskDesactivated = new UnityEvent();

        protected FlaskData.FLASKTYPE m_currentFlaskType;
        public FlaskData.FLASKTYPE CurrentFlaskType { get { return m_currentFlaskType; } }

        protected bool m_isUsed = false;
        protected float m_nocivity = 0;

        public float Nocivity { get { return m_nocivity; } }

        protected Dictionary<Player, float[]> m_flaskUsers = new Dictionary<Player, float[]>();

        public virtual void OnUseFlask(Player player)
        {
            m_isUsed = true;
            OnFlaskActivated.Invoke();
        }
        public virtual void OnUnuseFlask(Player player)
        {
            m_isUsed = false;
            OnFlaskDesactivated.Invoke();
        }

        public virtual void LoadFlaskFromData(FlaskData data)
        {
            m_nocivity = data.nocivity;
            m_currentFlaskType = data.m_currentFlaskType;
        }

        public virtual void SubscribeToFlask(Player player)
        {
            if (player == null)
                Debug.LogError("There is no Player");

            if (!m_flaskUsers.ContainsKey(player))
                m_flaskUsers.Add(player, new float[0]);
        }
        public virtual void UnsubscribeToFlask(Player player)
        {
            if (!PlayerIsSuscribeToFlask(player))
                return;
            m_flaskUsers.Remove(player);
        }

        public virtual bool PlayerIsSuscribeToFlask(Player player)
        {
            if (m_flaskUsers.ContainsKey(player))
                return true;
            return false;
        }
    }
}