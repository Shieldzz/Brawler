using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

namespace BTA
{

    public class MoveScrollRect : ScrollRect, IMoveHandler, IPointerClickHandler
    {
        private const float m_speedMultiplier = 0.1f;
        public float m_xSpeed = 0;
        public float m_ySpeed = 0;

        private float m_hPos, m_vPos;

        public enum ScrollerPage
        {
            BOTTOM,
            MIDDLE,
            UPPER
        }

        public bool m_enableScrolling = false;
        private bool m_canScroll = true;
        public class ScrollerEvent : UnityEvent<ScrollerPage>
        { }
        public ScrollerEvent m_OnScroll = new ScrollerEvent();
        private InputManager m_inputManager = null;

        public float m_ScrollerTime = 0.5f;
        private float m_countDown = 0f;

        private void OnEnable()
        {
            if (!m_inputManager)
                m_inputManager = GameManager.Instance.GetInstanceOf<InputManager>();
            //normalizedPosition = new Vector2(0f, 0.f);
        }

        protected override void Start()
        {
            normalizedPosition = Vector3.up;
        }

        void IMoveHandler.OnMove(AxisEventData e)
        {
            m_xSpeed += e.moveVector.x * (Mathf.Abs(m_xSpeed) + 0.1f);
            m_ySpeed += e.moveVector.y * (Mathf.Abs(m_ySpeed) + 0.1f);
        }

        void Update()
        {
            m_hPos = horizontalNormalizedPosition + m_xSpeed * m_speedMultiplier;
            m_vPos = verticalNormalizedPosition + m_ySpeed * m_speedMultiplier;

            m_xSpeed = Mathf.Lerp(m_xSpeed, 0, 0.1f);
            m_ySpeed = Mathf.Lerp(m_ySpeed, 0, 0.1f);

            if (movementType == MovementType.Clamped)
            {
                m_hPos = Mathf.Clamp01(m_hPos);
                m_vPos = Mathf.Clamp01(m_vPos);
            }

            normalizedPosition = new Vector2(m_hPos, m_vPos);

            if (m_enableScrolling && m_canScroll)
                ManageScrolling();
        }

        private void ManageScrolling()
        {
            GamePadID gamePad = GetComponent<ShopPanelManager>().PlayerPanelController.CurrentGamePad;
            float axisY = m_inputManager.GetAxis(gamePad, GamePadAxis.LeftJoystickY);
            if (axisY >= 0.8f && normalizedPosition.y <= 0.9f)
                ScrollUp();
            else if (axisY <= -0.8f && normalizedPosition.y >= 0.1f)
                ScrollDown();
        }

        private void ScrollUp()
        {
            float oldYPos = normalizedPosition.y;
            normalizedPosition = new Vector2(0f, oldYPos + 0.5f);
            EmitScrollEvent();
            StartCountDown();
        }

        private void ScrollDown()
        {
            float oldYPos = normalizedPosition.y;
            normalizedPosition = new Vector2(0f, oldYPos - 0.5f);
            EmitScrollEvent();
            StartCountDown();
        }

        private void StartCountDown()
        {
            m_canScroll = false;
            m_countDown = 0f;
            StartCoroutine(ScrollCountDown());
        }

        private IEnumerator ScrollCountDown()
        {
            while(m_countDown < m_ScrollerTime)
            {
                m_countDown += Time.deltaTime;
                yield return null;
            }
            m_canScroll = true;
        }

        private void EmitScrollEvent()
        {
            if (normalizedPosition.y < 0.4f)
                m_OnScroll.Invoke(ScrollerPage.BOTTOM);
            else if (normalizedPosition.y > 0.6f)
                m_OnScroll.Invoke(ScrollerPage.UPPER);
            else
                m_OnScroll.Invoke(ScrollerPage.MIDDLE);
        }

        public void OnPointerClick(PointerEventData e)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
            base.OnBeginDrag(eventData);
        }
    }

}