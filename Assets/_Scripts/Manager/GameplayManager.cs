using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BTA
{
    public enum CharacterEnum
    {
        NONE,
        MELEE,
        DISTANCE,
    }

    public class GameplayManager : MonoBehaviour
    {
        #region Manager
        private static GameplayManager m_instance = null;
        public static GameplayManager Instance
        {
            get
            {
                if (!m_instance)
                    m_instance = new GameplayManager();
                return m_instance;
            }
        }
        private static bool m_created = false;
        #endregion

        public class MoneyEvent : UnityEvent<float> { }
        public MoneyEvent m_onMoneyChange = new MoneyEvent();
        private bool m_economicSet = false;

        private GameManager m_gameManager = null;

        private float m_moneyAccount = 1500f;
        public float Money { get { return m_moneyAccount; } set { m_moneyAccount = value; m_onMoneyChange.Invoke(m_moneyAccount); } }

        private float m_currLevelMoney = 0f;
        public float LevelMoney { get { return m_currLevelMoney; } }

        private float m_slimChangeValue = 5f;

        public GamePadID m_cacPlayerGamepad = GamePadID.None;
        public GamePadID m_distPlayerGamepad = GamePadID.None;

        private bool m_shopVisited = false;
        public bool ShopVisited { get { return m_shopVisited; } set { m_shopVisited = value; } }

        void Awake()
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Use this for initialization
        void Start()
        {
            m_gameManager = GameManager.Instance;
        }

        public void SetEconomicData(float slimChangeValue, float startMoneyAcccount)
        {
            if (m_economicSet)
                return;

            m_slimChangeValue = slimChangeValue;
            m_moneyAccount = startMoneyAcccount;

            m_economicSet = true;
        }

        private void ConvertSlimToMoney()
        {
            PlayerManager playerManager = GameObject.FindObjectOfType<PlayerManager>();
            Player p1 = playerManager.GetPlayer(PlayerManager.PlayerInstance.PLAYER1);
            Player p2 = playerManager.GetPlayer(PlayerManager.PlayerInstance.PLAYER2);

            m_currLevelMoney += TradeSlimToWon(p1.SlimBallCount) * p1.MoneyMultiplicator;
            m_currLevelMoney += TradeSlimToWon(p2.SlimBallCount) * p2.MoneyMultiplicator;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public float TradeSlimToWon(float balls)
        {
            return balls * m_slimChangeValue;
        }

        public void OnPlayerDie()
        {
            if (OnLoseCondition())
                Lose();
        }

        public void OnEnemyDie()
        {
            //if (OnWinCondition())
            //    Win();
        }

        public void OnReachLevelEnd()
        {
            //m_narrativeMgr.EndingNarration();
            if (OnWinCondition())
                Win();
        }

        private bool OnWinCondition()
        {
            GameMode gameMode = m_gameManager.GetGameMode();
            PlayerManager playerManager = GameObject.FindObjectOfType<PlayerManager>();
            if (playerManager.ArePlayersAlive() && gameMode == GameMode.Duo)
                return true;
            else if (!playerManager.ArePlayersDead() && gameMode == GameMode.Solo)
                return true;

            return false;
        }

        private bool OnLoseCondition()
        {
            PlayerManager playerManager = GameObject.FindObjectOfType<PlayerManager>();
            if (playerManager.ArePlayersDead())
                return true;
            return false;
        }

        private void Win()
        {
            ConvertSlimToMoney();
            GameObject.FindObjectOfType<InGameUiManager>().OnWinEvent();

            m_moneyAccount += m_currLevelMoney;
            m_currLevelMoney = 0f;
            //foreach (GameObject go in gamesObject)
            //{
            //    if(go.name == "UISceneManager")
            //    {
            //        go.GetComponent<InGameUiManager>().OnWinEvent();
            //        return;
            //    }
            //}
        }

        private void Lose()
        {
            GameObject.FindObjectOfType<InGameUiManager>().OnLoseEvent();
            //GameObject[] gamesObject = FindObjectsOfType<GameObject>();

            //foreach (GameObject go in gamesObject)
            //{
            //    if (go.name == "UISceneManager")
            //    {
            //        go.GetComponent<InGameUiManager>().OnLoseEvent();
            //        return;
            //    }
            //}
        }

    }

} // namespace BTA

