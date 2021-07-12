using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class AdrenalineFlask : Flasks
    {
        float m_lifePercentBeforeChainGaugeIncrease = 0f;

        public override void OnUseFlask(Player player)
        {
            base.OnUseFlask(player);

            PlayerFighting fighting = player.GetComponent<PlayerFighting>();

            fighting.LifeRatioChainGaugeIncrease = m_lifePercentBeforeChainGaugeIncrease / 100f;
        }

        public override void OnUnuseFlask(Player player)
        {
            base.OnUnuseFlask(player);

            PlayerFighting fighting = player.GetComponent<PlayerFighting>();

            fighting.LifeRatioChainGaugeIncrease = 0f;
        }

        public override void LoadFlaskFromData(FlaskData data)
        {
            base.LoadFlaskFromData(data);
            m_lifePercentBeforeChainGaugeIncrease = data.m_lifePercentBeforeChainGaugeIncrease;
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
