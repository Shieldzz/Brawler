using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class JumpHeightBoostDrug : Flasks
    {


        [SerializeField] private int m_buffJumpHeightPercent = 20;
        private float m_basicJumpHeight = 0f;
        [SerializeField] private int m_buffJumpDurationPercent = 20;
        private float m_basicJumpDuration = 0f;
        [SerializeField] private int m_buffFallingSpeedPercent = -20;
        private float m_basicFallingSpeed = 0f;

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

            playerMovement.FallingSpeed += playerMovement.FallingSpeed* (m_buffFallingSpeedPercent / 100f);
            playerMovement.JumpDuration += playerMovement.JumpDuration * (m_buffJumpDurationPercent / 100f);
            playerMovement.JumpHeight += playerMovement.JumpHeight* (m_buffJumpHeightPercent / 100f);

        }

        public override void OnUnuseFlask(Player player)
        {
            base.OnUnuseFlask(player);
            Movement playerMovement = player.GetComponent<Movement>();
            float[] backData = m_flaskUsers[player];

            playerMovement.FallingSpeed = backData[0];
            playerMovement.JumpDuration = backData[1];
            playerMovement.JumpHeight = backData[2];
        }

        public override void SubscribeToFlask(Player player)
        {
            base.SubscribeToFlask(player);
            Movement playerMovement = player.GetComponent<Movement>();
            if (m_flaskUsers[player].Length == 0)
            {
                float[] data = new float[3];
                data[0] = playerMovement.FallingSpeed;
                data[1] = playerMovement.JumpDuration;
                data[2] = playerMovement.JumpHeight;
                m_flaskUsers[player] = data;

            }
            else
            {
                m_flaskUsers[player][0] = playerMovement.FallingSpeed;
                m_flaskUsers[player][1] = playerMovement.JumpDuration;
                m_flaskUsers[player][1] = playerMovement.JumpHeight;
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