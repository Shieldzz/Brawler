using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class ChainTimerFlask : Flasks
    {
        private float m_chainTimer = 0f;


        public override void OnUseFlask(Player player)
        {
            base.OnUseFlask(player);

            PlayerFighting fighting = player.GetComponent<PlayerFighting>();

            Debug.Log("Percent = " + m_chainTimer / 100f);
            Debug.Log("Base Value = " + fighting.TimeBeforeChainDrop);
            Debug.Log("adding = " + fighting.TimeBeforeChainDrop * (m_chainTimer / 100f));

            fighting.TimeBeforeChainDrop += fighting.TimeBeforeChainDrop * (m_chainTimer / 100f);

            Debug.Log("Result = " + fighting.TimeBeforeChainDrop);
        }

        public override void OnUnuseFlask(Player player)
        {
            base.OnUnuseFlask(player);

            //PlayerFighting fighting = player.GetComponent<PlayerFighting>();

            //fighting.ChainDropTimer -= fighting.ChainDropTimer * (m_chainTimer / 100f);
        }

        public override void LoadFlaskFromData(FlaskData data)
        {
            base.LoadFlaskFromData(data);
            m_chainTimer = data.m_chainTimer;
        }

        public override void SubscribeToFlask(Player player)
        {
            base.SubscribeToFlask(player);

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
