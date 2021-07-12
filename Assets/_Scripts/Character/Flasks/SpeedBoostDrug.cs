using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class SpeedBoostDrug : Flasks
    {

        [SerializeField]
        private int m_buffSpeedPercent = 10;

        private float m_standardXSpeedValue = 0f;
        private float m_standardZSpeedValue = 0f;

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
            playerMovement.Speed += playerMovement.Speed * (m_buffSpeedPercent / 100f);
            playerMovement.ZSpeed += playerMovement.ZSpeed * (m_buffSpeedPercent / 100f);
        }

        public override void OnUnuseFlask(Player player)
        {
            base.OnUnuseFlask(player);
            Movement playerMovement = player.GetComponent<Movement>();
            float[] backData = m_flaskUsers[player];
            playerMovement.Speed = backData[0];
            playerMovement.ZSpeed = backData[1];
        }

        public override void LoadFlaskFromData(FlaskData data)
        {
            base.LoadFlaskFromData(data);
            m_buffSpeedPercent = data.m_speed;
        }

        public override void SubscribeToFlask(Player player)
        {
            base.SubscribeToFlask(player);
            Movement playerMovement = player.GetComponent<Movement>();
            if (m_flaskUsers[player].Length == 0)
            {
                float[] data = new float[2];
                data[0] = playerMovement.Speed;
                data[1] = playerMovement.ZSpeed;
                m_flaskUsers[player] = data;
            }
            else
            {
                m_flaskUsers[player][0] = playerMovement.Speed;
                m_flaskUsers[player][1] = playerMovement.ZSpeed;
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