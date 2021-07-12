using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BTA
{

    public class Controller : MonoBehaviour
    {
        public class ControllerEvent : UnityEvent<GamePadID, CharacterEnum> { }
        public ControllerEvent m_selectedCharacter = new ControllerEvent();

        private RectTransform m_controlerTransform = null;
        private GamePadID m_currentGamePad = GamePadID.Controller1;
        public GamePadID CurrentGamePad { get { return m_currentGamePad; }  set { m_currentGamePad = value; } }
        public float m_moveRange = 100f;

        public bool m_isMelee = false;

        [SerializeField] private Controller m_otherController = null;
        private Vector3 m_standartPos = Vector3.zero;

        // Use this for initialization
        void Start()
        {
            m_controlerTransform = GetComponent<RectTransform>();
            m_standartPos = m_controlerTransform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private bool CanMove()
        {
            if (GameManager.Instance.GetGameMode() == GameMode.Solo)
                return false;

            //if (m_controlerTransform.localPosition.x > -m_moveRange && m_controlerTransform.localPosition.x < m_moveRange)
            //    return true;
            //return false;

            return true;
        }

        public void Reset()
        {
            m_controlerTransform.localPosition = m_standartPos; // new Vector3(0f, m_controlerTransform.localPosition.y, 0f);
            //SelectCharacter();
        }

        public void Swap()
        {
            Vector3 newPos = m_otherController.m_controlerTransform.localPosition;
            m_otherController.m_controlerTransform.localPosition = m_controlerTransform.localPosition;
            m_controlerTransform.localPosition = newPos;

            SwapCharacter();

            //m_otherController.SelectCharacter();
            //SelectCharacter();
        }

        private void SwapCharacter()
        {
            GameplayManager gameplayManager = GameManager.Instance.GetInstanceOf<GameplayManager>();
            if (gameplayManager.m_cacPlayerGamepad == CurrentGamePad)
            {
                gameplayManager.m_cacPlayerGamepad = m_otherController.CurrentGamePad;
                gameplayManager.m_distPlayerGamepad = CurrentGamePad;

            }
            else
            {
                gameplayManager.m_cacPlayerGamepad = CurrentGamePad;
                gameplayManager.m_distPlayerGamepad = m_otherController.CurrentGamePad;
            }
        }

        public void OnMoveLeft()
        {
            if (!CanMove())
                return;

            if (m_controlerTransform.localPosition.x <= -m_moveRange)
                return;

            m_controlerTransform.localPosition = new Vector3(m_controlerTransform.localPosition.x - m_moveRange, m_controlerTransform.localPosition.y, 0);

            SelectCharacter();
        }

        public void OnMoveRight()
        {
            if (!CanMove())
                return;

            if (m_controlerTransform.localPosition.x >= m_moveRange)
                return;

            m_controlerTransform.localPosition = new Vector3(m_controlerTransform.localPosition.x + m_moveRange, m_controlerTransform.localPosition.y, 0);

            SelectCharacter();
        }

        private CharacterEnum GetSelectedCharacter()
        {
            if (m_controlerTransform.localPosition.x <= -m_moveRange)
                return CharacterEnum.MELEE;
            if (m_controlerTransform.localPosition.x >= m_moveRange)
                return CharacterEnum.DISTANCE;
            return CharacterEnum.NONE;
        }

        private void SelectCharacter()
        {
            //GameplayManager gameplayManager = GameManager.Instance.GetInstanceOf<GameplayManager>();
            //
            //GamePadID otherControllerGamePad = m_otherController.CurrentGamePad;
            //
            //if (gameplayManager.m_cacPlayerGamepad == otherControllerGamePad)
            //    m_selectedCharacter.Invoke(m_currentGamePad, CharacterEnum.MELEE);
            //else if (gameplayManager.m_distPlayerGamepad == otherControllerGamePad)
            //    m_selectedCharacter.Invoke(m_currentGamePad, CharacterEnum.DISTANCE);

            //CharacterEnum character = GetSelectedCharacter();
            //Debug.Log(character);
            //m_selectedCharacter.Invoke(m_currentGamePad, character);
        }
    }
}