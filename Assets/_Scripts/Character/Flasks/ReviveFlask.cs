using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class ReviveFlask : Flasks
    {

        private float m_flaskReviveTime = 0f;
        private int m_flaskHPBonus = 0;

        void Start()
        {

        }

        void Update()
        {

        }

        public override void OnUseFlask(Player player)
        {
            base.OnUseFlask(player);
            player.Reviver.m_reviveTime = m_flaskReviveTime;
            player.ReviveHealthRatio = (m_flaskHPBonus / 100f);
        }

        public override void OnUnuseFlask(Player player)
        {
            base.OnUnuseFlask(player);
            float[] backData = m_flaskUsers[player];
            player.Reviver.m_reviveTime = backData[0];
            player.ReviveHealthRatio = backData[1];
        }

        public override void SubscribeToFlask(Player player)
        {
            base.SubscribeToFlask(player);
            if(m_flaskUsers[player].Length == 0)
            {
                float[] data = new float[2];
                data[0] = player.Reviver.m_reviveTime;
                data[1] = player.ReviveHealthRatio;
                m_flaskUsers[player] = data;
            }
            else
            {
                m_flaskUsers[player][0] = player.Reviver.m_reviveTime;
                m_flaskUsers[player][1] = player.ReviveHealthRatio;
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

        public override void LoadFlaskFromData(FlaskData data)
        {
            base.LoadFlaskFromData(data);
            m_flaskReviveTime = data.m_reviveDuration;
            m_flaskHPBonus = data.m_hpBonus;
        }
    }

}