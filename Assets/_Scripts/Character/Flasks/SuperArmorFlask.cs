using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class SuperArmorFlask : Flasks
    {
        private AttackType m_attackType = AttackType.None;


        public override void OnUseFlask(Player player)
        {
            base.OnUseFlask(player);

            PlayerFighting fighting = player.GetComponent<PlayerFighting>();

            fighting.SuperArmoredAttack = m_attackType;
        }

        public override void OnUnuseFlask(Player player)
        {
            base.OnUnuseFlask(player);

            PlayerFighting fighting = player.GetComponent<PlayerFighting>();

            fighting.SuperArmoredAttack = AttackType.None;
        }

        public override void LoadFlaskFromData(FlaskData data)
        {
            base.LoadFlaskFromData(data);
            m_attackType = data.m_attackGainingSuperArmor;
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
