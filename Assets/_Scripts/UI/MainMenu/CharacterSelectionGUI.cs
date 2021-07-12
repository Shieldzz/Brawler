using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BTA
{
    public class CharacterSelectionGUI : MonoBehaviour
    {

        public GameObject m_p1Controller = null;
        public GameObject m_p2Controller = null;

        private bool m_isMulti = false;
        private bool m_hasFocus = false;

        private RectTransform m_p1ControllerTransform = null;
        private RectTransform m_p2ControllerTransform = null;

        void Start()
        {
            m_p1ControllerTransform = m_p1Controller.GetComponent<RectTransform>();
            m_p2ControllerTransform = m_p2Controller.GetComponent<RectTransform>();
        }

        void Update()
        {
            if (m_hasFocus)
            {
                if (m_isMulti)
                    CheckMultiCharacterSelectionInput();
                else
                    CheckSoloCharacterSelectionInput();
            }
        }

        private void CheckMultiCharacterSelectionInput()
        {
            //if (InputX.Down(InputCode.A) && !CheckIfCharacterIsUsed(InputCode.A) && m_p1ControllerTransform.localPosition.x > -200)
            //    m_p1ControllerTransform.localPosition = new Vector3(m_p1ControllerTransform.localPosition.x - 200, m_p1ControllerTransform.localPosition.y, 0);
            //else if (InputX.Down(InputCode.D) && !CheckIfCharacterIsUsed(InputCode.D) && m_p1ControllerTransform.localPosition.x < 200)
            //    m_p1ControllerTransform.localPosition = new Vector3(m_p1ControllerTransform.localPosition.x + 200, m_p1ControllerTransform.localPosition.y, 0);
        }

        private void CheckSoloCharacterSelectionInput()
        {
            //if (InputX.Down(InputCode.A) && m_p1ControllerTransform.localPosition.x > -200)
            //    m_p1ControllerTransform.localPosition = new Vector3(m_p1ControllerTransform.localPosition.x - 200, m_p1ControllerTransform.localPosition.y, 0);
            //else if (InputX.Down(InputCode.D) && m_p1ControllerTransform.transform.localPosition.x < 200)
            //    m_p1ControllerTransform.localPosition = new Vector3(m_p1ControllerTransform.localPosition.x + 200, m_p1ControllerTransform.localPosition.y, 0);
        }

        private bool CheckIfCharacterIsUsed(/*InputCode input*/)
        {
            //if (input == InputCode.A)
            //    if (m_p1ControllerTransform.localPosition.x == 0 || m_p2ControllerTransform.localPosition.x == 0)
            //        if (m_p1ControllerTransform.localPosition.x <= -200 || m_p2ControllerTransform.localPosition.x <= -200)
            //            return true;

            //if (input == InputCode.D)
            //    if (m_p1ControllerTransform.localPosition.x == 0 || m_p2ControllerTransform.localPosition.x == 0)
            //        if (m_p1ControllerTransform.localPosition.x >= 200 || m_p2ControllerTransform.localPosition.x >= 200)
            //            return true;

            return false;
        }


        public void onMultiplayerGame()
        {
            m_isMulti = true;
            m_p2Controller.GetComponent<Image>().enabled = true;
        }

        public void onSoloGame()
        {
            m_isMulti = false;
            m_p2Controller.GetComponent<Image>().enabled = false;
        }
    }
}