using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class HardcoreFlask : Flasks
    {
        float m_moneyMultiplicator = 0f;
        bool m_canRegen = true;

        public override void OnUseFlask(Player player)
        {
            base.OnUseFlask(player);

            player.IsAbleToRegen = m_canRegen;
            player.MoneyMultiplicator += (m_moneyMultiplicator / 100f);
        }

        public override void OnUnuseFlask(Player player)
        {
            base.OnUnuseFlask(player);

            player.IsAbleToRegen = !m_canRegen;
            player.MoneyMultiplicator -= (m_moneyMultiplicator / 100f);
        }

        public override void LoadFlaskFromData(FlaskData data)
        {
            base.LoadFlaskFromData(data);
            m_moneyMultiplicator = data.m_goldMultiplier;
            m_canRegen = !data.m_cannotRegenerateByHittingSlime;
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
