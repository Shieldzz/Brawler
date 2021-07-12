using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class ManHole : MonoBehaviour
    {
        enum ManHoleState
        {
            Wait = 0,
            Anticipation = 1,
            Attack = 2,
            StateCount
        }

        ManHoleState m_currState = ManHoleState.Wait;
        AttackTrigger m_attackTrigger;

        bool m_isActive = false;
        public bool IsActive { set { m_isActive = value; } get { return m_isActive; } }

        [Header("Data")]
        [SerializeField] AttackStruct m_attackStruct;

        [Header("Timer")]
        [SerializeField] float m_waitDuration = 6f;
        [SerializeField] float m_anticipationDuration = 3f;
        [SerializeField] float m_attackDuration = 3f;

        float m_currTimer = 0f;

        [Header("Particle")]
        [SerializeField] ParticleSystem m_anticipationParticle;
        [SerializeField] ParticleSystem m_attackParticle;

        [Header("Sounds")]
        [FMODUnity.EventRef]
        public string m_attackSound;
        private FMOD.Studio.EventInstance m_attackSoundInstance;

        // Use this for initialization
        void Start()
        {
            m_attackTrigger = GetComponent<AttackTrigger>();
            m_anticipationParticle.Stop();
            m_attackSoundInstance = FMODUnity.RuntimeManager.CreateInstance(m_attackSound);
           //SwitchToState(ManHoleState.Wait);
        }

        // Update is called once per frame
        void Update()
        {
            if (m_currTimer >= 0f)
            {
                m_currTimer -= Time.deltaTime;
                if (m_currTimer <= 0f)
                    SwitchToNextState();
            }
        }

        //private void OnEnable()
        //{
        //    if (m_currState == ManHoleState.Attack)
        //        m_attackSoundInstance
        //}

        private void OnDisable()
        {
            m_attackSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        public void SwitchToNextState()
        {
            if (m_isActive)
            {
                m_currState++;
                if (m_currState == ManHoleState.StateCount)
                    m_currState = 0;

                SwitchToState(m_currState);
            }
            else
                SwitchToState(ManHoleState.Wait);
        }

        private void SwitchToState(ManHoleState state)
        {
            switch (state)
            {
                case ManHoleState.Wait:
                    OnWait();
                    break;
                case ManHoleState.Anticipation:
                    OnAnticipation();
                    break;
                case ManHoleState.Attack:
                    OnAttack();
                    break;
                default:
                    break;
            }
        }

        private void OnWait()
        {
            m_attackSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            m_attackParticle.Stop();
            m_attackTrigger.Disable();
            m_anticipationParticle.Stop();

            m_currState = ManHoleState.Wait;
            m_currTimer = m_waitDuration;
        }

        private void OnAnticipation()
        {
            m_anticipationParticle.Play();

            m_currState = ManHoleState.Anticipation;
            m_currTimer = m_anticipationDuration;
        }

        private void OnAttack()
        {
            m_attackSoundInstance.start();
            m_anticipationParticle.Stop();
            m_attackTrigger.Enable(m_attackStruct.attackDatas[0]);
            m_attackParticle.Play();

            m_currState = ManHoleState.Attack;
            m_currTimer = m_attackDuration;
        }
    }
}
