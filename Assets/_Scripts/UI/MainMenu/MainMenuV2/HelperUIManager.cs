using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace BTA
{
    public class HelperUIManager : MonoBehaviour
    {

        static private HelperUIManager m_instance = null;
        static public HelperUIManager Instance
        {
            get
            {
                if (!m_instance)
                    m_instance = new HelperUIManager();
                return m_instance;
            }
        }

        private void Awake()
        {
            if (!m_instance)
                m_instance = this;
        }

        public GameObject m_helper1GameObject = null;
        public GameObject m_helper2GameObject = null;
        public GameObject m_helper3GameObject = null;
        public GameObject m_helper4GameObject = null;
        public GameObject m_helper5GameObject = null;

        public Sprite m_spriteIconA = null;
        public Sprite m_spriteIconB = null;
        public Sprite m_spriteIconY = null;
        public Sprite m_spriteIconX = null;

        public Sprite m_spriteIconStart = null;
        public Sprite m_spriteIconLbRb = null;

        public UnityAction m_controller1Helper1Action = null;
        public UnityAction m_controller1Helper2Action = null;
        public UnityAction m_controller1Helper3Action = null;
        public UnityAction m_controller1Helper4Action = null;
        public UnityAction m_controller1Helper5Action = null;

        public UnityAction m_controller2Helper1Action = null;
        public UnityAction m_controller2Helper2Action = null;
        public UnityAction m_controller2Helper3Action = null;
        public UnityAction m_controller2Helper4Action = null;
        public UnityAction m_controller2Helper5Action = null;

        private GamePadInput m_lastInput;
        private GamePadID m_lastController;


        void Start()
        {
        }

        public void DisableHelper(GameObject helper)
        {
            helper.SetActive(false);
        }

        public void EnableHelper(GameObject helper)
        {
            helper.SetActive(true);
        }

        public void EnableAllHelper()
        {
            m_helper1GameObject.SetActive(true);
            m_helper2GameObject.SetActive(true);
            m_helper3GameObject.SetActive(true);
            m_helper4GameObject.SetActive(true);
            m_helper5GameObject.SetActive(true);
        }

        public void DisableAllHelper()
        {
            m_helper1GameObject.SetActive(false);
            m_helper2GameObject.SetActive(false);
            m_helper3GameObject.SetActive(false);
            m_helper4GameObject.SetActive(false);
            m_helper5GameObject.SetActive(false);
        }

        public void ClearAllControllerActions()
        {
            InputManager inputManager = GameManager.Instance.GetInstanceOf<InputManager>();
            inputManager.CleanAllGamePadEvent();

            m_controller1Helper1Action = null;
            m_controller1Helper2Action = null;
            m_controller1Helper3Action = null;
            m_controller1Helper4Action = null;
            m_controller1Helper5Action = null;

            m_controller2Helper1Action = null;
            m_controller2Helper2Action = null;
            m_controller2Helper3Action = null;
            m_controller2Helper4Action = null;
            m_controller2Helper5Action = null;
        }

        public void ClearAllSpecificControllerActions(GamePadID controller)
        {
            InputManager inputManager = GameManager.Instance.GetInstanceOf<InputManager>();
            inputManager.CleanGamePadEvent(controller);


            if (controller == GamePadID.Controller1)
            {
                m_controller1Helper1Action = null;
                m_controller1Helper2Action = null;
                m_controller1Helper3Action = null;
                m_controller1Helper4Action = null;
                m_controller1Helper5Action = null;
            } 
            else if(controller == GamePadID.Controller2)
            {
                m_controller2Helper1Action = null;
                m_controller2Helper2Action = null;
                m_controller2Helper3Action = null;
                m_controller2Helper4Action = null;
                m_controller2Helper5Action = null;
            }
        }

        public void ClearAction(ref UnityAction action)
        {
            action = null;
            GameManager.Instance.GetInstanceOf<InputManager>().GetEvent(m_lastController, m_lastInput).RemoveAllListeners();
        }

        public void ClearAction(ref UnityAction action, GamePadID gamePad, GamePadInput input)
        {
            action = null;
            GameManager.Instance.GetInstanceOf<InputManager>().GetEvent(gamePad, input).RemoveAllListeners();
        }

        public void ClearAction(ref UnityAction action, GamePadID gamePad, GamePadInput input, UnityAction cb)
        {
            action = null;
            GameManager.Instance.GetInstanceOf<InputManager>().GetEvent(gamePad, input).RemoveListener(cb);
        }

        public void SetHelperInformation(GameObject helper, Sprite icon, string text, UnityAction cb = null, GamePadID controller = GamePadID.Controller1)
        {
            Image helperIcon = helper.GetComponent<Image>();
            Text helperText = helper.transform.GetChild(0).GetComponent<Text>();

            helperIcon.sprite = icon;
            helperText.text = text;

            if (cb == null)
                return;

            SetActionToInputManager(GetInputFromIcon(icon), cb, controller);
        }

        private void SetActionToInputManager(GamePadInput input, UnityAction cb, GamePadID controller = GamePadID.Controller1)
        {
            InputManager inputManager = GameManager.Instance.GetInstanceOf<InputManager>();
            inputManager.GetEvent(controller, input).AddListener(cb);
            m_lastController = controller;
            m_lastInput = input;

        }

        private GamePadInput GetInputFromIcon(Sprite icon)
        {
            if (icon == m_spriteIconA)
                return GamePadInput.ButtonA;
            else if (icon == m_spriteIconB)
                return GamePadInput.ButtonB;
            else if (icon == m_spriteIconX)
                return GamePadInput.ButtonX;
            else if (icon == m_spriteIconY)
                return GamePadInput.ButtonY;
            else if (icon == m_spriteIconStart)
                return GamePadInput.ButtonStart;
            else
                return GamePadInput.LeftBumper;

        }

    }

}