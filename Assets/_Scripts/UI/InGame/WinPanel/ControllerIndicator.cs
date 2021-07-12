using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class ControllerIndicator : MonoBehaviour
    {
        public void Init()
        {
            InputManager inputManager = GameManager.Instance.GetInstanceOf<InputManager>();
            inputManager.GetEvent(GamePadID.Controller1, GamePadInput.ButtonA).AddListener(Next);
            inputManager.GetEvent(GamePadID.Controller2, GamePadInput.ButtonA).AddListener(Next);
        }

        private void Next()
        {
            InputManager inputManager = GameManager.Instance.GetInstanceOf<InputManager>();
            inputManager.GetEvent(GamePadID.Controller1, GamePadInput.ButtonA).RemoveAllListeners();
            inputManager.GetEvent(GamePadID.Controller2, GamePadInput.ButtonA).RemoveAllListeners();

            GameState gameState = GameManager.Instance.GetInstanceOf<GameState>();
            gameState.ChangeCurrentState(GAME_STATE.MAIN_MENU);
        }
    }
}