using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace BTA
{
    public abstract class PlayerFighting : AFighting
    {
        protected InputManager m_inputMgr;
        protected CameraManager m_camManager;

        protected Player m_player;
        protected new Movement m_movement;

        protected SpecialAttackTrigger m_specialAttackTrigger = null;

        [SerializeField] protected MultiplierLadder m_comboLengthMultiplierLadder;

        protected AttackType m_superArmoredAttack = AttackType.None;
        public AttackType SuperArmoredAttack { get { return m_superArmoredAttack; } set { m_superArmoredAttack = value; } }

        [SerializeField] float m_inputBufferMemoryDuration = 0.4f;
        protected float m_inputBufferTimer;

        #region Chain & ChainGauge
        [Header("Chain")]

        [SerializeField] private float m_timeBeforeChainDrop = 1.5f;
        protected int m_chainValue = 0;
        protected float m_chainDropTimer = 0f;
        public float TimeBeforeChainDrop { get { return m_timeBeforeChainDrop; } set { m_timeBeforeChainDrop = value; } }

        public class ChainEvent : UnityEvent<int, float> { }
        public ChainEvent OnChainUpdateEvent = new ChainEvent();

        [Header("ChainGauge")]

        [SerializeField] protected MultiplierLadder m_chainGaugeMultiplierLadder;
        [SerializeField] protected int m_chainGaugefirstHitGain = 5;
        [SerializeField] protected int m_chainGaugeGain = 3;
        [SerializeField] protected int m_chainGaugeLossbyHit = 30;
        [SerializeField] protected List<int> m_maxChainGaugeValuePerLevel = new List<int>();
        [SerializeField] private int m_chainGaugeLossEverySecond = 3;
        [SerializeField] private float m_timeBetweenEachChainGaugeLoss = 1f;

        public int ChainGaugeLossEverySecond { get { return m_chainGaugeLossEverySecond;  } set { m_chainGaugeLossEverySecond = value; } }

        protected int m_chainGaugeValue = 0;
        protected float m_chainGaugeLossTimer = 0f;

        public class ChainGaugeEvent : UnityEvent<int, int, int> { }
        public ChainGaugeEvent OnChainGaugeUpdateEvent = new ChainGaugeEvent();

        float m_lifeRatioBeforeChainGaugeIncrease = 0f;
        public float LifeRatioChainGaugeIncrease {  get { return m_lifeRatioBeforeChainGaugeIncrease; } set { m_lifeRatioBeforeChainGaugeIncrease = value; } }
        #endregion

        #region Shake
        [Header("Shake")]

        [SerializeField] float m_shakeAmplitude = 0.15f;
        [SerializeField] float m_shakeFrequency = 5f;
        #endregion

        #region Sounds
        protected PlayerSounds m_sounds;
        #endregion

        protected override void Awake()
        {
            m_inputMgr = GameManager.Instance.GetInstanceOf<InputManager>();
            m_player = GetComponent<Player>();
            base.Awake();
            m_movement = GetComponent<Movement>();

            m_sounds = GetComponentInChildren<PlayerSounds>();
            if (!m_sounds)
                Debug.LogError("No Sounds on fighting " + gameObject.ToString());

            m_specialAttackTrigger = GetComponentInChildren<SpecialAttackTrigger>();
            AssignAttackEvent();
        }

        private void Start()
        {
            m_camManager = LevelManager.Instance.GetInstanceOf<CameraManager>();
        }

        virtual public void Update()
        {
            UpdateChainSystem();

            if (m_inputBufferTimer > 0f)
            {
                m_inputBufferTimer -= Time.deltaTime;
                if (m_inputBufferTimer <= 0f)
                    m_animator.ResetTrigger("InputAttack");
            }
        }

        #region Attack

        protected override void AssignAttackEvent()
        {
            PlayerAttackEventHandler handler = GetComponentInChildren<PlayerAttackEventHandler>();

            handler.OnAttackStateEvent.AddListener(m_movement.Freeze);
            handler.OnAttackBeginEvent.AddListener(OnAttackBegin);
            handler.OnAttackEndEvent.AddListener(OnAttackEnd);

            handler.OnSpecialAttackEvent.AddListener(OnSpecialBegin);
            handler.OnRecoveryStateEvent.AddListener(m_movement.SetRecoveringState);

            handler.OnConnectEvent.AddListener(OnAttackConnect);
            handler.OnRecoveryEvent.AddListener(OnAttackRecovery);
        }

        public override void AttackInput(AttackType type)
        {
            if (m_movement.IsDashing())
                return;

            base.AttackInput(type);
            m_inputBufferTimer = m_inputBufferMemoryDuration;
        }

        protected void OnAttackBegin(AttackType type)
        {
            if (m_superArmoredAttack == type)
                m_movement.HasSuperArmor = true;
        }

        protected override void OnAttackConnect(AttackType type , AttackData data, int comboLength) // maybe add force and duration ? & recup ComboLength here !
        {
            if (type == AttackType.SpecialAttack)
            {
                m_inputMgr.ShakeGamePad(m_player.GetGamePadID(), 1f, 1f);
                m_camManager.SetShakeCamera(m_shakeAmplitude, m_shakeFrequency);
            }

            ApplyComboLengthDamageMultiplier(ref data, comboLength);

            base.OnAttackConnect(type, data, comboLength);
        }

        protected override void OnAttackRecovery()
        {
            if (m_currUsedAttack == AttackType.SpecialAttack)
                SpecialAttackConsequences();

            base.OnAttackRecovery();
        }

        protected void OnAttackEnd(AttackType type)
        {
            if (m_superArmoredAttack == type)
                m_movement.HasSuperArmor = false;
        }

        protected void OnSpecialBegin(float range)
        {
            m_specialAttackTrigger.SetRadius(range);
            m_movement.HasSuperArmor = true;      
        }

        protected void SpecialAttackConsequences()
        {
            ResetChain();
            ResetChainGauge();
            m_movement.HasSuperArmor = false;
            m_inputMgr.ShakeGamePad(m_player.GetGamePadID(), 0f, 0f);
            m_camManager.ResetShakeCamera();
        }

        protected void ApplyComboLengthDamageMultiplier(ref AttackData data, int comboLength)
        {
            float damageFactor = m_comboLengthMultiplierLadder.GetMultiplierFromRequirement(comboLength);

            data.damage = (int)(data.damage * damageFactor);
        }

        #endregion

        #region ChainSystem

        public void OnSuccessfulHit(bool firstHit, int damage)
        {
            m_player.TryRegen(damage);


            //reset Both Timer when you hit something
            m_chainDropTimer = m_timeBeforeChainDrop;
            m_chainGaugeLossTimer = m_timeBetweenEachChainGaugeLoss;

            // Update Chain
            m_chainValue++;
            UpdateChain();

            // Update ChainGauge
            float chainGaugeFactor = m_chainGaugeMultiplierLadder.GetMultiplierFromRequirement(m_chainValue);

            if (firstHit)
                m_chainGaugeValue += (int)(m_chainGaugefirstHitGain * chainGaugeFactor);
            else
                m_chainGaugeValue += (int)(m_chainGaugeGain * chainGaugeFactor);

            UpdateChainGauge();
        }

        public void OnHitTaken()
        {
            if (m_movement.HasSuperArmor)
                return;

            ResetChain();
            m_chainGaugeValue -= m_chainGaugeLossbyHit;
            UpdateChainGauge();
        }

        private void UpdateChainSystem()
        {
            if (m_chainDropTimer > 0f)
                m_chainDropTimer -= Time.deltaTime;

            else // Drop Chain 
            { 
                    ResetChain();

                if (m_chainGaugeLossTimer > 0f)
                    m_chainGaugeLossTimer -= Time.deltaTime;

                else if (m_chainGaugeValue > 0) // if you got some ChainChauge Decrement
                {
                    m_chainGaugeLossTimer = m_timeBetweenEachChainGaugeLoss;

                    if (m_lifeRatioBeforeChainGaugeIncrease > 0f)
                    {
                        float lifeRatio = m_player.GetDameageable().Life / m_player.GetDameageable().MaxLife;
                        if (lifeRatio <= m_lifeRatioBeforeChainGaugeIncrease)
                            m_chainGaugeValue += m_chainGaugeLossEverySecond;
                    }
                    else
                        m_chainGaugeValue -= m_chainGaugeLossEverySecond;
                    UpdateChainGauge();
                }
            }
        }

        public void UpdateChain()
        {
            OnChainUpdateEvent.Invoke(m_chainValue, m_chainDropTimer);
        }

        public void ResetChain()
        {
            if (m_chainValue <= 0)
                return;

            m_chainValue = 0;
            m_chainDropTimer = 0f;
            UpdateChain();
        }

        public void UpdateChainGauge()
        {
            //restrict ChainGauge to 0 and upper
            m_chainGaugeValue = Mathf.Clamp(m_chainGaugeValue, 0, GetMaxGaugeValue());

            int chainGaugeValueOfCurrLevel;
            int chainGaugeLevel = GetChainGaugeLevel(out chainGaugeValueOfCurrLevel);

            //get Maximum in function of curr Gauge Level
            int maxChainGaugeValueValueOfCurrLevel;
            //Debug.Log(chainGaugeLevel + " < " + m_maxChainGaugeValuePerLevel.Count);
            if (chainGaugeLevel < m_maxChainGaugeValuePerLevel.Count)
                maxChainGaugeValueValueOfCurrLevel = m_maxChainGaugeValuePerLevel[chainGaugeLevel];
            else
                maxChainGaugeValueValueOfCurrLevel = chainGaugeValueOfCurrLevel;

            OnChainGaugeUpdateEvent.Invoke(chainGaugeLevel, chainGaugeValueOfCurrLevel, maxChainGaugeValueValueOfCurrLevel);
        }

        protected void ResetChainGauge()
        {
            if (m_chainGaugeValue <= 0)
                return;

            m_chainGaugeValue = 0;
            m_chainGaugeLossTimer = 0f;
            UpdateChainGauge();
        }

        public int GetChainGaugeLevel()
        {
            int currLevel = 0;
            int chainGaugeLeft = 0;
            int currChainGaugeValue = m_chainGaugeValue;
            foreach (int currLevelMaxGauge in m_maxChainGaugeValuePerLevel)
            {
                chainGaugeLeft = currChainGaugeValue - currLevelMaxGauge;
                if (chainGaugeLeft < 0)
                    break;

                currChainGaugeValue = chainGaugeLeft;
                currLevel++;
            }
            //Debug.Log("GetChainGaugeLevel -> level " + currLevel + " with " + currChainGaugeValue + " points !");
            return currLevel;
        }

        public int GetChainGaugeLevel(out int currChainGaugeValue)
        {
            int currLevel = 0;
            int chainGaugeLeft = 0;
            currChainGaugeValue = m_chainGaugeValue;
            foreach (int currLevelMaxGauge in m_maxChainGaugeValuePerLevel)
            {
                chainGaugeLeft = currChainGaugeValue - currLevelMaxGauge;
                if (chainGaugeLeft < 0)
                    break;

                currChainGaugeValue = chainGaugeLeft;
                currLevel++;
            }
            //Debug.Log("GetChainGaugeLevel -> level " + currLevel + " with " + currChainGaugeValue + " points !");
            return currLevel;
        }

        public int GetMaxGaugeValue()
        {
            int max = 0;
            foreach(int maxValue in m_maxChainGaugeValuePerLevel)
            {
                max += maxValue;
            }

            return max;
        }
        #endregion
    }
}
