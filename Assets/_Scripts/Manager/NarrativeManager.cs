using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BTA
{

    public class NarrativeManager : MonoBehaviour
    {
        GameplayManager m_gameplayMgr;
        PlayerManager m_playerMgr;

        public class NarrativeEvent : UnityEvent<NarrativeBlock> { }
        public NarrativeEvent OnNarrativeBlockStart = new NarrativeEvent();


        [SerializeField] NarrativeBlock m_startNarrativeBlock;
        [SerializeField] NarrativeBlock m_endNarrativeBlock;
        [SerializeField] float m_nextBlockWaitDuration = 2f;
        //[SerializeField] float m_textSpeed = 2f;

        bool m_start = true;
        bool m_ending = false;
        bool m_isInNarration = false;

        public bool IsInNarration { get { return m_isInNarration; } }
        public float NextBlockWaitTime { get { return m_nextBlockWaitDuration; } }

        public void Start()
        {
            m_gameplayMgr = GameManager.Instance.GetInstanceOf<GameplayManager>();
            m_playerMgr = LevelManager.Instance.GetInstanceOf<PlayerManager>();
        }

        public void Update()
        {
            if (m_start)
            {
                m_start = false;
                Debug.Log("Start Narration");
                if (m_startNarrativeBlock)
                    ProcNarrativeEvent(m_startNarrativeBlock);
            }
        }

        public void EndingNarration()
        {
            m_ending = true;
            ProcNarrativeEvent(m_endNarrativeBlock);
        }

        public void ProcNarrativeEvent(NarrativeBlock narrativeBlock)
        {
            m_playerMgr.Pause(true);
            m_isInNarration = true;

            OnNarrativeBlockStart.Invoke(narrativeBlock);
        }

        public void DisableNarrative()
        {

            if (m_ending)
            {
                m_gameplayMgr.OnReachLevelEnd();
                return;
            }

            m_isInNarration = false;
            m_playerMgr.Pause(false);
        }
    }

} // namespace BTA
