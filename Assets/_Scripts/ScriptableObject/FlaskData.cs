using System;
using UnityEngine;

namespace BTA
{
    [CreateAssetMenu()]
    public class FlaskData : ScriptableObject
    {
        public enum FLASKTYPE
        {
            DASH,
            JUMP,
            SPEED,
            REVIVE,
            CHAINGAUGEDECAY,
            CHAINTIMER,
            SUPERARMOR,
            ADRENALINE,
            HARDCORE,
            NONE
        }

        #region GENERIC
        public FLASKTYPE m_currentFlaskType;
        public float price;
        [Range(0f, 30f)]public float nocivity;
        public string flaskName;
        public string flaskGeneralDescription;
        public string flaskEffectDescription;
        public string narrativeDescription;
        public Sprite m_flaskSprite;
        public Color m_fogColor;
        public bool m_isLock = false;
        #endregion

        #region DASH
        public int m_dashCooldown;
        public int m_dashLength;
        public int m_dashDuration;
        #endregion

        #region JUMP
        public int m_jumpHeight;
        public int m_jumpDuration;
        public int m_FallingSpeed;
        #endregion

        #region SPEED
        public int m_speed;
        #endregion

        #region REVIVE
        public float m_reviveDuration;
        public int m_hpBonus;
        #endregion

        #region CHAINGAUGEDECAY
        public float m_chainGaugeLossOutOfFight;
        #endregion

        #region CHAINTIMER
        public float m_chainTimer = 0f;
        #endregion

        #region SUPERAMOR
        public AttackType m_attackGainingSuperArmor = AttackType.HeavyAttack;
        #endregion

        #region ADRENALINEBOOST
        public int m_lifePercentBeforeChainGaugeIncrease;
        #endregion

        #region HARDCORE
        public bool m_cannotRegenerateByHittingSlime = false;
        public int m_goldMultiplier;
        #endregion


    }

}