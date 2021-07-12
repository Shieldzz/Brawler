using UnityEngine;
using UnityEngine.Events;
using XInputDotNetPure;

namespace BTA
{
    public class InputManager : MonoBehaviour
    {
        #region Manager
        private bool m_created = false;

        private void Awake()
        {
            if (!m_created)
            {
                DontDestroyOnLoad(gameObject);
                m_created = true;
            }
        }
        #endregion

        GameManager m_gameMgr;

        // Do not set More than 4 GamePadData in this Array
        GamePadData[] m_gamePads = new GamePadData[2];

        public UnityEvent OnInitComplete = new UnityEvent();
        public bool init = false;

        // Use this for initialization
        void Start()
        {
            m_gameMgr = GameManager.Instance;

            //init all GamePads
            int gamePadNb = m_gamePads.Length;
            for (int i = 0; i < gamePadNb; i++)
            {
                m_gamePads[i] = new GamePadData();
                m_gamePads[i].padEvent = new GamePadEvent();
                m_gamePads[i].ID = i;
            }

            Debug.Log("InputMgr Init Done");
            init = true;
            OnInitComplete.Invoke();
        }

        void Update()
        {
            foreach (GamePadData gamePad in m_gamePads)
            {
                //Debug.Log("try to connect !");
                ManageGamePadConnection(gamePad);

                if (!gamePad.playerIndexSet)
                    return;

                gamePad.prevState = gamePad.state;
                gamePad.state = GamePad.GetState(gamePad.playerIndex);

                EventCall(gamePad);
            }
        }

        #region Connection/Deconnection
        void ManageGamePadConnection(GamePadData gamePad)
        {
            if (!gamePad.playerIndexSet || !gamePad.prevState.IsConnected)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)gamePad.ID;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    gamePad.state = testState;
                    gamePad.playerIndex = testPlayerIndex;
                    gamePad.playerIndexSet = true;
                    //GamePadConnection(gamePad);
                    return;
                }
            }
        }

        #endregion

        #region Event

        void EventCall(GamePadData gamePad)
        {
            if (gamePad.prevState.Buttons.A == ButtonState.Released && gamePad.state.Buttons.A == ButtonState.Pressed)
                gamePad.padEvent.OnButtonA.Invoke();

            if (gamePad.prevState.Buttons.B == ButtonState.Released && gamePad.state.Buttons.B == ButtonState.Pressed)
                gamePad.padEvent.OnButtonB.Invoke();

            if (gamePad.prevState.Buttons.X == ButtonState.Released && gamePad.state.Buttons.X == ButtonState.Pressed)
                gamePad.padEvent.OnButtonX.Invoke();

            if (gamePad.prevState.Buttons.Y == ButtonState.Released && gamePad.state.Buttons.Y == ButtonState.Pressed)
                gamePad.padEvent.OnButtonY.Invoke();

            if (gamePad.prevState.Buttons.A == ButtonState.Pressed && gamePad.state.Buttons.A == ButtonState.Released)
                gamePad.padEvent.OnButtonARelease.Invoke();

            if (gamePad.prevState.Buttons.Start == ButtonState.Released && gamePad.state.Buttons.Start == ButtonState.Pressed)
                gamePad.padEvent.OnButtonStart.Invoke();

            if (gamePad.prevState.Buttons.RightShoulder == ButtonState.Released && gamePad.state.Buttons.RightShoulder == ButtonState.Pressed)
                gamePad.padEvent.OnButtonRightBumper.Invoke();

            if (gamePad.prevState.Buttons.LeftShoulder == ButtonState.Released && gamePad.state.Buttons.LeftShoulder == ButtonState.Pressed)
                gamePad.padEvent.OnButtonLeftBumper.Invoke();

            if (gamePad.prevState.DPad.Down == ButtonState.Released && gamePad.state.DPad.Down == ButtonState.Pressed)
                gamePad.padEvent.OnDPadDown.Invoke();

            if (gamePad.prevState.DPad.Left == ButtonState.Released && gamePad.state.DPad.Left == ButtonState.Pressed)
                gamePad.padEvent.OnDPadLeft.Invoke();

            if (gamePad.prevState.DPad.Up == ButtonState.Released && gamePad.state.DPad.Up == ButtonState.Pressed)
                gamePad.padEvent.OnDPadUp.Invoke();

            if (gamePad.prevState.DPad.Right == ButtonState.Released && gamePad.state.DPad.Right == ButtonState.Pressed)
                gamePad.padEvent.OnDPadRight.Invoke();
        }

        public InputEvent GetEvent(GamePadID controllerID, GamePadInput input)
        {
            int id = (int)controllerID;
            switch (input)
            {
                case GamePadInput.ButtonA:
                    return m_gamePads[id].padEvent.OnButtonA;
                case GamePadInput.ButtonB:
                    return m_gamePads[id].padEvent.OnButtonB;
                case GamePadInput.ButtonX:
                    return m_gamePads[id].padEvent.OnButtonX;
                case GamePadInput.ButtonY:
                    return m_gamePads[id].padEvent.OnButtonY;
                case GamePadInput.ButtonARelease:
                    return m_gamePads[id].padEvent.OnButtonARelease;
                case GamePadInput.RightBumper:
                    return m_gamePads[id].padEvent.OnButtonRightBumper;
                case GamePadInput.LeftBumper:
                    return m_gamePads[id].padEvent.OnButtonLeftBumper;
                case GamePadInput.ButtonStart:
                    return m_gamePads[id].padEvent.OnButtonStart;
                case GamePadInput.DPadDown:
                    return m_gamePads[id].padEvent.OnDPadDown;
                case GamePadInput.DPadLeft:
                    return m_gamePads[id].padEvent.OnDPadLeft;
                case GamePadInput.DPadUp:
                    return m_gamePads[id].padEvent.OnDPadUp;
                case GamePadInput.DPadRight:
                    return m_gamePads[id].padEvent.OnDPadRight;
                default:
                    return null;
            }
        }

        public float GetAxis(GamePadID controllerID, GamePadAxis axis)
        {
            int id = (int)controllerID;

            if (!m_gamePads[id].playerIndexSet)
                return 0f;

            switch (axis)
            {
                case GamePadAxis.LeftJoystickX:
                    return m_gamePads[id].state.ThumbSticks.Left.X;
                case GamePadAxis.LeftJoystickY:
                    return m_gamePads[id].state.ThumbSticks.Left.Y;
                case GamePadAxis.RightJoystickX:
                    return m_gamePads[id].state.ThumbSticks.Right.X;
                case GamePadAxis.RightJoystickY:
                    return m_gamePads[id].state.ThumbSticks.Right.Y;
                case GamePadAxis.LeftTrigger:
                    return m_gamePads[id].state.Triggers.Left;
                case GamePadAxis.RightTrigger:
                    return m_gamePads[id].state.Triggers.Left;
                default:
                    return 0f;
            }
        }
        #endregion

        public void ShakeGamePad(GamePadID id, float left, float right)
        {
            GamePad.SetVibration((PlayerIndex)id, left, right);
        }


        // Not supposed to clean The entire GamePad Event, should be cleaning it from Behaviour that addListener :x
        public void CleanAllGamePadEvent()
        {
            CleanGamePadEvent(GamePadID.Controller1);
            CleanGamePadEvent(GamePadID.Controller2);
        }

        public void CleanGamePadEvent(GamePadID controllerID)
        {
            int id = (int)controllerID;
            m_gamePads[id].padEvent.OnButtonA.RemoveAllListeners();
            m_gamePads[id].padEvent.OnButtonB.RemoveAllListeners();
            m_gamePads[id].padEvent.OnButtonX.RemoveAllListeners();
            m_gamePads[id].padEvent.OnButtonY.RemoveAllListeners();

            m_gamePads[id].padEvent.OnButtonARelease.RemoveAllListeners();

            m_gamePads[id].padEvent.OnButtonRightBumper.RemoveAllListeners();
            m_gamePads[id].padEvent.OnButtonLeftBumper.RemoveAllListeners();

            m_gamePads[id].padEvent.OnButtonStart.RemoveAllListeners();

            m_gamePads[id].padEvent.OnDPadDown.RemoveAllListeners();
            m_gamePads[id].padEvent.OnDPadLeft.RemoveAllListeners();
            m_gamePads[id].padEvent.OnDPadRight.RemoveAllListeners();
            m_gamePads[id].padEvent.OnDPadUp.RemoveAllListeners();
        }

        private void OnDestroy()
        {
            foreach(GamePadData pad in m_gamePads)
            {
                GamePad.SetVibration(pad.playerIndex, 0f, 0f);
            }
        }
    }

} // namespace BTA

