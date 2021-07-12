using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class DashFlask : Flasks
    {

        [SerializeField] int m_buffDashCooldownPercent = -50;
        [SerializeField] int m_buffDashLengthPercent = 50;
        [SerializeField] int m_buffDashDurationPercent = -50;

        private float m_basicDashCooldown = 0f;
        private float m_basicDashLength = 0f;
        private float m_basicDashDuration = 0f;

        void Start()
        {
        }

        void Update()
        {

        }

        public override void OnUseFlask(Player player)
        {
            base.OnUseFlask(player);

            Movement playerMovement = player.GetComponent<Movement>();

            playerMovement.DashCooldown+= playerMovement.DashCooldown * (m_buffDashCooldownPercent / 100f);
            playerMovement.DashDuration += playerMovement.DashDuration* (m_buffDashDurationPercent / 100f);
            playerMovement.DashLength += playerMovement.DashLength * (m_buffDashLengthPercent / 100f);
        }

        public override void OnUnuseFlask(Player player)
        {
            base.OnUnuseFlask(player);
            Movement playerMovement = player.GetComponent<Movement>();
            float[] backData = m_flaskUsers[player];
            playerMovement.DashCooldown = backData[0];
            playerMovement.DashDuration = backData[1];
            playerMovement.DashLength = backData[2];
        }

        public override void LoadFlaskFromData(FlaskData data)
        {
            base.LoadFlaskFromData(data);
            m_buffDashCooldownPercent = data.m_dashCooldown;
            m_buffDashDurationPercent = data.m_dashDuration;
            m_buffDashLengthPercent = data.m_dashLength;
        }

        public override void SubscribeToFlask(Player player)
        {
            //if (m_isUsed)
            //    return;
            base.SubscribeToFlask(player);
            Movement playerMovement = player.GetComponent<Movement>();
            if (m_flaskUsers[player].Length == 0)
            {
                float[] data = new float[3];
                data[0] = playerMovement.DashCooldown;
                data[1] = playerMovement.DashDuration;
                data[2] = playerMovement.DashLength;
                m_flaskUsers[player] = data;

            }
            else
            {
                m_flaskUsers[player][0] = playerMovement.DashCooldown;
                m_flaskUsers[player][1] = playerMovement.DashDuration;
                m_flaskUsers[player][2] = playerMovement.DashLength;
            }

            OnUseFlask(player);
        }

        public override void UnsubscribeToFlask(Player player)
        {
            if (!PlayerIsSuscribeToFlask(player))
                return;
            OnUnuseFlask(player);
            base.UnsubscribeToFlask(player);
        }
    }

}