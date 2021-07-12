using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class ChainGaugeDecayFlask : Flasks
    {
        private float m_chainGaugeDecay = 0f;


        public override void OnUseFlask(Player player)
        {
            base.OnUseFlask(player);

            PlayerFighting fighting = player.GetComponent<PlayerFighting>();

            float currValue = fighting.ChainGaugeLossEverySecond;

            currValue += currValue * (m_chainGaugeDecay / 100f);

            fighting.ChainGaugeLossEverySecond = (int)Mathf.Clamp(currValue, 1f, Mathf.Infinity);
        }

        public override void OnUnuseFlask(Player player)
        {
            base.OnUnuseFlask(player);

            PlayerFighting fighting = player.GetComponent<PlayerFighting>();

            fighting.ChainGaugeLossEverySecond -= (int)Mathf.Clamp((m_chainGaugeDecay / 100f), 1f, Mathf.Infinity);
        }

        public override void LoadFlaskFromData(FlaskData data)
        {
            base.LoadFlaskFromData(data);
            m_chainGaugeDecay = data.m_chainGaugeLossOutOfFight;
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
