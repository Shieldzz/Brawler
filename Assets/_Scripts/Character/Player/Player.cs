using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BTA
{

    public class Player : ACharacter
    {
        InputManager m_inputMgr;
        PlayerManager m_playerMgr;

        Movement m_movement;

        PlayerFighting m_fighting;

        private Damageable m_damageable;
        private FlaskInventory m_flaskInventory;

        public GameObject m_frontPlayer;
        public GameObject m_backPlayer;

        float m_moneyMultiplicator = 1f;
        public float MoneyMultiplicator { get { return m_moneyMultiplicator; } set { m_moneyMultiplicator = value; } }

        [SerializeField] private float m_switchCooldown = 2f;
        private float m_switchTimer = 0f;

        #region HealthSystem

        [Header("Health System")]

        [SerializeField]
        float m_timeBeforeRecoveryDrop = 5f;
        float m_recoveryTimer = 0f;

        [SerializeField] float m_nocivityCorruptionDuration = 20f;
        float m_corruptionTimer = 0f;

        [SerializeField] private float m_reviveHealthRatio = 1f / 3f;
        public float ReviveHealthRatio { get { return m_reviveHealthRatio; } set { m_reviveHealthRatio = value; } }
        [SerializeField] private Revive m_reviver = null;
        public Revive Reviver { get { return m_reviver; } }

        float m_recoveryLife = 0f;
        public float RecoveryLife { get { return m_recoveryLife; } }


        float m_corruptedLife = 0f;
        float m_corruptionPercent = 0f;
        float m_corruptionCeiling = 0f;
        float m_currNocivity = 0f;

        public float CorruptedLife { get { return m_corruptedLife; } }

        bool m_canRegen = true;
        public bool IsAbleToRegen { get { return m_canRegen; } set { m_canRegen = value; } }

        public class LifeEvent : UnityEvent<Damageable, float> { }
        public LifeEvent OnRecoveryLifeUpdate = new LifeEvent();
        public LifeEvent OnCorruptedLifeUpdate = new LifeEvent();

        #endregion

        #region SLIMBALLS
        private float m_slimeBallCount = 0f;
        private float m_slimeBallMultiplicator = 0f;

        public float SlimBallCount { get { return m_slimeBallCount; } }
        public float SlimBallMultiplicator { get { return m_slimeBallMultiplicator; } set { m_slimeBallMultiplicator = value; } }

        public class SlimeBallEvent : UnityEvent<float> { }
        public SlimeBallEvent OnSlimeBallUpdate = new SlimeBallEvent();
        #endregion

        #region ControllerInput

        GamePadID m_currGamePadID = GamePadID.None;

        [Header("Controls")]
        //[SerializeField]
        //GamePadID m_duoGamePadID;

        [Header("Movement Input")]
        [SerializeField]
        GamePadInput m_inputJump = GamePadInput.ButtonA;
        [SerializeField] GamePadInput m_inputDash = GamePadInput.RightBumper;
        [SerializeField] GamePadInput m_inputSwitchPlayer = GamePadInput.LeftBumper;

        [Header("Fighting Input")]
        [SerializeField]
        GamePadInput m_inputLightAttack = GamePadInput.ButtonX;
        [SerializeField] GamePadInput m_inputHeavyAttack = GamePadInput.ButtonY;
        [SerializeField] GamePadInput m_inputSpecialAttack = GamePadInput.ButtonB;

        [Header("Flask Input")]
        [SerializeField]
        GamePadInput m_inputEquipFlaskOne = GamePadInput.DPadLeft;
        [SerializeField] GamePadInput m_inputEquipFlaskTwo = GamePadInput.DPadUp;
        [SerializeField] GamePadInput m_inputEquipFlaskThree = GamePadInput.DPadRight;
        [SerializeField] GamePadInput m_inputUnequipFlask = GamePadInput.DPadDown;

        #endregion

        #region Sounds
        private PlayerSounds m_sounds;
        #endregion

        public void OnDestroy()
        {
            m_flaskInventory.RemoveAllFlasks();
        }

        // Use this for initialization
        public virtual void Awake()
        {
            GameManager gameMgr = GameManager.Instance;
            m_inputMgr = gameMgr.GetInstanceOf<InputManager>();

            m_playerMgr = GameObject.FindObjectOfType<PlayerManager>();
            m_animator = GetComponentInChildren<Animator>();

            m_movement = GetComponent<Movement>();
            m_fighting = GetComponent<PlayerFighting>();
            m_flaskInventory = GetComponent<FlaskInventory>();

            m_damageable = GetComponentInChildren<Damageable>();
            m_recoveryLife = m_damageable.Life;

            m_sounds = GetComponentInChildren<PlayerSounds>();
        }

        private void Start()
        {
            m_reviver.Player = this;
            EquipAllEquipedFlask();
        }

        private void OnEnable()
        {
            EnableInputEvent();

            m_switchTimer = m_switchCooldown;
        }

        private void OnDisable()
        {
            m_recoveryLife = m_damageable.Life;
            DisableInputEvent();
        }

        public void Pause(bool active)
        {
            if (!m_damageable.IsAlive())
                return;

            //Avoid running in Pause
            if (active == true)
                m_movement.StopMovement();

            paused = active;
            if (active)
                DisableInputEvent();
            else
                EnableInputEvent();
        }

        public void AssignGamePad(GamePadID newGamePad)
        {
            if (m_currGamePadID == newGamePad)
                return;

            if (m_currGamePadID != GamePadID.None)
                DisableInputEvent();

            m_currGamePadID = newGamePad;

            if (isActiveAndEnabled)
                EnableInputEvent();
        }

        // Update is called once per frame
        public virtual void FixedUpdate()
        {
            if (m_switchTimer > 0f)
                m_switchTimer -= Time.deltaTime;

            if (paused)
                return;

            if (m_damageable.IsAlive())
            {

                if (m_corruptionTimer < 1f)
                {
                    m_corruptionTimer += Time.deltaTime / m_nocivityCorruptionDuration;
                    UpdateCorruption();
                }

                UpdateRecovery();

                if (!m_inputMgr)
                    return;

                float axisX = m_inputMgr.GetAxis(m_currGamePadID, GamePadAxis.LeftJoystickX);
                float axisY = m_inputMgr.GetAxis(m_currGamePadID, GamePadAxis.LeftJoystickY);

                //DEBUG to use keyboard
                if (axisX == 0f) axisX = Input.GetAxis("HorizontalKey");
                if (axisY == 0f) axisY = Input.GetAxis("VerticalKey");
                DebugKeyboardInput();


                m_movement.UpdateMovement(axisX, axisY);
            }
        }

        #region Input

        private void DebugKeyboardInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                m_movement.StartJumping();

            if (Input.GetKeyDown(KeyCode.LeftShift))
                m_movement.StartDash();

            if (Input.GetKeyDown(KeyCode.G))
                LightAttack();

            if (Input.GetKeyDown(KeyCode.H))
                HeavyAttack();

            if (Input.GetKeyDown(KeyCode.J))
                SpecialAttack();

            if (Input.GetKeyDown(KeyCode.L))
                SwitchPlayer();
        }

        private void LightAttack()
        {
            m_fighting.AttackInput(AttackType.LightAttack);
        }

        private void HeavyAttack()
        {
            m_fighting.AttackInput(AttackType.HeavyAttack);
        }

        private void SpecialAttack()
        {
            if (m_fighting.GetChainGaugeLevel() == 0)
                return;

            m_fighting.AttackInput(AttackType.SpecialAttack);
        }

        private void SwitchPlayer()
        {
            //Debug.Log(gameObject + " Switch at " + m_switchTimer);
            if (m_switchTimer > 0f)
                return;
            if (m_animator.GetBool("Attacking") || m_animator.GetBool("Dash"))
                return;

            //Debug.Log(gameObject + " switch !");
            m_playerMgr.SwitchPlayer();
        }

        #endregion

        #region HealthSystem
        public void OnDamageTaken(AttackTrigger trigger, Damageable damageable)
        {
            if (m_recoveryTimer > 0f)
            {
                m_sounds.LaunchTakeDamageSound();
                m_recoveryLife = m_damageable.Life + trigger.Damage;
                OnRecoveryLifeUpdate.Invoke(damageable, m_recoveryLife);
            }

            m_recoveryTimer = m_timeBeforeRecoveryDrop;
        }

        public void TryRegen(int lifeToRegen)
        {
            if (!m_canRegen)
                return;

            float currLife = m_damageable.Life;
            if (currLife >= m_recoveryLife)
                return;

            float lifeGain = Mathf.Clamp(lifeToRegen, 0f, (m_recoveryLife - currLife));

            m_damageable.GainHealth(lifeGain);
        }

        void UpdateRecovery()
        {
            if (m_recoveryTimer > 0f)
            {
                m_recoveryTimer -= Time.deltaTime;
                if (m_recoveryTimer <= 0f)
                {
                    m_recoveryLife = m_damageable.Life;
                    OnRecoveryLifeUpdate.Invoke(m_damageable, m_recoveryLife);
                }
            }
        }

        void UpdateCorruption()
        {
            m_corruptionPercent = Mathf.Lerp(0, m_currNocivity, m_corruptionTimer);

            m_corruptedLife = m_corruptionPercent / 100f * m_damageable.MaxLife;
            m_corruptionCeiling = m_damageable.MaxLife - m_corruptedLife;

            //Debug.Log("Corruption State = " + m_corruptedLife + " at time " + m_corruptionTimer);

            if (m_damageable.Life > m_corruptionCeiling)
                m_damageable.Life = m_corruptionCeiling;

            OnCorruptedLifeUpdate.Invoke(m_damageable, m_corruptedLife);
        }

        public void OnDie()
        {
            Debug.Log("Death !");
            m_animator.SetBool("Dead", true);
            m_animator.Play("Death");
            m_sounds.LaunchDeathSound();

            m_movement.UpdateMovement(0f, 0f);
            DisableInputEvent();

            GameManager gameManager = GameManager.Instance;
            GameMode gameMode = gameManager.GetGameMode();
            if (gameMode == GameMode.Duo)
                m_reviver.gameObject.SetActive(true);

            gameManager.GetInstanceOf<GameplayManager>().OnPlayerDie();

            if (gameMode == GameMode.Solo && !m_playerMgr.ArePlayersDead())
                m_playerMgr.SwitchPlayer();
        }

        public void Revive()
        {
            m_animator.SetBool("Dead", false);
            Debug.Log("Player [" + name + "] is revive");

            float lifeGain = m_damageable.MaxLife * m_reviveHealthRatio;
            m_damageable.GainHealth(lifeGain);
            m_recoveryLife = lifeGain;
            OnRecoveryLifeUpdate.Invoke(m_damageable, m_recoveryLife);

            m_reviver.gameObject.SetActive(false);
            m_sounds.LaunchReviveSound();
            EnableInputEvent();
        }
        #endregion

        #region Flask

        private void EquipAllEquipedFlask()
        {
            FlaskManager flaskManager = FlaskManager.Instance;
            GameplayManager gameplayManager = GameManager.Instance.GetInstanceOf<GameplayManager>();

            if (GetComponent<PlayerCacFighting>())
                m_flaskInventory.EquipeFlaskFromList(flaskManager.m_meleeCharacterFlasks, flaskManager);
            else if (GetComponent<PlayerDistFighting>())
                m_flaskInventory.EquipeFlaskFromList(flaskManager.m_distanceCharacterFlasks, flaskManager);

            m_currNocivity = m_flaskInventory.GetFullNocivity();
        }

        #endregion

        #region SLIMBALLS

        public void AddSlimeBall(float ball)
        {
            float toAdd = (m_slimeBallMultiplicator == 0) ? ball : ball * m_slimeBallMultiplicator;
            m_slimeBallCount += toAdd;
            UpdateSlimeBall();
        }

        public void UpdateSlimeBall()
        {
            OnSlimeBallUpdate.Invoke(m_slimeBallCount);
        }

        public void ResetSlimBall()
        {
            m_slimeBallCount = 0f;
            UpdateSlimeBall();
        }

        #endregion

        #region ControllerInputEvent

        private void EnableInputEvent()
        {
            if (!m_inputMgr || m_currGamePadID == GamePadID.None)
                return;

            //Debug.Log("Enable input of " + m_currGamePadID.ToString() + " for " + gameObject.ToString());
            m_inputMgr.GetEvent(m_currGamePadID, m_inputJump).AddListener(m_movement.StartJumping);
            m_inputMgr.GetEvent(m_currGamePadID, m_inputDash).AddListener(m_movement.StartDash);
            m_inputMgr.GetEvent(m_currGamePadID, m_inputSwitchPlayer).AddListener(SwitchPlayer);

            m_inputMgr.GetEvent(m_currGamePadID, m_inputLightAttack).AddListener(LightAttack);
            m_inputMgr.GetEvent(m_currGamePadID, m_inputHeavyAttack).AddListener(HeavyAttack);
            m_inputMgr.GetEvent(m_currGamePadID, m_inputSpecialAttack).AddListener(SpecialAttack);
        }

        private void DisableInputEvent()
        {
            if (!m_inputMgr || m_currGamePadID == GamePadID.None)
                return;

            //Debug.Log("Disable input of " + m_currGamePadID.ToString() + " for " + gameObject.ToString());
            m_inputMgr.GetEvent(m_currGamePadID, m_inputJump).RemoveListener(m_movement.StartJumping);
            m_inputMgr.GetEvent(m_currGamePadID, m_inputDash).RemoveListener(m_movement.StartDash);
            m_inputMgr.GetEvent(m_currGamePadID, m_inputSwitchPlayer).RemoveListener(SwitchPlayer);

            m_inputMgr.GetEvent(m_currGamePadID, m_inputLightAttack).RemoveListener(LightAttack);
            m_inputMgr.GetEvent(m_currGamePadID, m_inputHeavyAttack).RemoveListener(HeavyAttack);
            m_inputMgr.GetEvent(m_currGamePadID, m_inputSpecialAttack).RemoveListener(SpecialAttack);
        }

        #endregion

        public Damageable GetDameageable()
        {
            return m_damageable;
        }

        public GamePadID GetGamePadID()
        {
            return m_currGamePadID;
        }
    }
}