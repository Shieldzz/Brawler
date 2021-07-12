using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BTA
{

    public class RewardsPanelController : MonoBehaviour
    {
        [SerializeField] private Text m_contractID;
        [SerializeField] private Text m_levelName;

        [SerializeField] private GameObject m_transactionArrow;

        [SerializeField] private Text m_totalSlimeText;
        [SerializeField] private Text m_earnMoneyText;

        private int m_totalSlimeBall = 0;
        private int m_totalMoneyEarned = 0;

        public void Init()
        {
            //Counting SlimBall
            LevelManager levelManager = LevelManager.Instance;
            PlayerManager playerMgr = levelManager.GetInstanceOf<PlayerManager>();
            Player p1 = playerMgr.GetPlayer(PlayerManager.PlayerInstance.PLAYER1);
            Player p2 = playerMgr.GetPlayer(PlayerManager.PlayerInstance.PLAYER2);

            m_totalSlimeBall += (int)(p1.SlimBallCount + p2.SlimBallCount);

            //Counting Money
            GameplayManager gameplayMgr = GameManager.Instance.GetInstanceOf<GameplayManager>();
            m_totalMoneyEarned = (int)gameplayMgr.LevelMoney;

            //Setting Contract Data
            LevelData levelData = levelManager.GetLevelDataFromLevelName(SceneManager.GetActiveScene().name);

            m_contractID.text = levelData.m_contractNumber;
            m_levelName.text = levelData.m_levelName;
        }

        public void ShowTotalSlimeBall(float duration)
        {
            m_totalSlimeText.transform.parent.gameObject.SetActive(true);
            StartCoroutine(SmoothTranslation(m_totalSlimeText, 0, m_totalSlimeBall, duration));
        }

        public void ConversionToMoney(float duration)
        {
            m_transactionArrow.SetActive(true);

            m_earnMoneyText.transform.parent.gameObject.SetActive(true);
            StartCoroutine(SmoothTranslation(m_earnMoneyText, 0, m_totalMoneyEarned, duration));
        }


        IEnumerator SmoothTranslation(Text translatingText,  int start, int end, float duration)
        {
            float t = 0f;
            
            while (t <= 1)
            {
                translatingText.text = Mathf.Lerp(start, end, t).ToString("0");

                t += Time.deltaTime / duration;

                yield return null;
            }

            translatingText.text = end.ToString();
        }
    }
}