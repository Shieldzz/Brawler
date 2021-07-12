using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BTA
{
    public class NarrativeUI : MonoBehaviour
    {
        NarrativeManager m_narrativeMgr;
        GameObject m_playerPanel;
        GameObject m_narrativePanel;

        InputEvent m_gamePad1InputA;
        InputEvent m_gamePad2InputA;

        InputEvent m_gamePad1InputY;
        InputEvent m_gamePad2InputY;

        [SerializeField] Image m_blackScreen;
        [SerializeField] Text m_textBlock;
        [SerializeField] GameObject m_girlUI;
        [SerializeField] GameObject m_boyUI;

        NarrativeBlock m_currNarration;
        int m_textBlockID = 0;

        float m_waitTimer = 0f;

        [HideInInspector] UnityEvent OnNarrationBlockEnd = new UnityEvent();

        private void Start()
        {
            //Narrative Event
            m_narrativeMgr = LevelManager.Instance.GetInstanceOf<NarrativeManager>();

            m_narrativeMgr.OnNarrativeBlockStart.AddListener(SetUpNarration);
            OnNarrationBlockEnd.AddListener(m_narrativeMgr.DisableNarrative);

            //Input Event
            InputManager inputMgr = GameManager.Instance.GetInstanceOf<InputManager>();

            m_gamePad1InputA = inputMgr.GetEvent(GamePadID.Controller1, GamePadInput.ButtonA);
            m_gamePad2InputA = inputMgr.GetEvent(GamePadID.Controller2, GamePadInput.ButtonA);

            m_gamePad1InputY = inputMgr.GetEvent(GamePadID.Controller1, GamePadInput.ButtonY);
            m_gamePad2InputY = inputMgr.GetEvent(GamePadID.Controller2, GamePadInput.ButtonY);

            //Panel
            m_playerPanel = transform.parent.GetComponentInChildren<PlayerUI>().gameObject;
            m_narrativePanel = transform.GetChild(0).gameObject;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
                NextBlock();

            if (m_waitTimer > 0f)
                m_waitTimer -= Time.deltaTime;
        }

        public void SetUpNarration(NarrativeBlock newBlock)
        {
            if (!newBlock)
                return;

            m_playerPanel.SetActive(false);
            m_narrativePanel.SetActive(true);

            m_currNarration = newBlock;
            m_textBlockID = 0;
            ShowBlock(m_textBlockID);

            m_gamePad1InputA.AddListener(NextBlock);
            m_gamePad2InputA.AddListener(NextBlock);

            m_gamePad1InputY.AddListener(DisableNarration);
            m_gamePad2InputY.AddListener(DisableNarration);
        }

        private void DisableNarration()
        {
            m_playerPanel.SetActive(true);
            m_narrativePanel.SetActive(false);

            m_gamePad1InputA.RemoveListener(NextBlock);
            m_gamePad2InputA.RemoveListener(NextBlock);

            m_gamePad1InputY.RemoveListener(DisableNarration);
            m_gamePad2InputY.RemoveListener(DisableNarration);

            OnNarrationBlockEnd.Invoke();
        }

        #region Block
        public void NextBlock()
        {
            if (!m_currNarration || m_waitTimer > 0f)
                return;

            m_textBlockID++;
            ShowBlock(m_textBlockID);
        }

        void ShowBlock(int id)
        {
            if (id >= m_currNarration.TextBlockArray.Length)
            {
                DisableNarration();
                m_waitTimer = m_narrativeMgr.NextBlockWaitTime;
                return;
            }

            TextBlock currBlock = m_currNarration.TextBlockArray[id];

            UpdateTalkingCharacter(currBlock.talkingCharacter);
            m_textBlock.text = currBlock.text;

            if (currBlock.hasSound)
            {
                FMOD.Studio.EventInstance inst = FMODUnity.RuntimeManager.CreateInstance(currBlock.sound);
                if (inst.isValid())
                    inst.start();
            }


            m_waitTimer = m_narrativeMgr.NextBlockWaitTime;
        }

        void UpdateTalkingCharacter(CharacterEnum talkingCharacter)
        {
            switch (talkingCharacter)
            {
                case CharacterEnum.NONE:
                    m_girlUI.SetActive(false);
                    m_boyUI.SetActive(false);
                    break;
                case CharacterEnum.MELEE:
                    m_girlUI.SetActive(true);
                    m_boyUI.SetActive(false);
                    break;
                case CharacterEnum.DISTANCE:
                    m_girlUI.SetActive(false);
                    m_boyUI.SetActive(true);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
