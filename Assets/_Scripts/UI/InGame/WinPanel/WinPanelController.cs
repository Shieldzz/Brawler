using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class WinPanelController : MonoBehaviour
    {

        [SerializeField] private CharacterWinPanel m_charactersPanel = null;
        [SerializeField] private RewardsPanelController m_rewardPanel = null;
        [SerializeField] private ControllerIndicator m_controllerIndicatorPanel = null;

        [Header("ScriptedApparition")]

        [SerializeField] float m_playerTimer = 0f;
        [SerializeField] float m_playerKillTimer = 1f;
        [SerializeField] float m_totalKillTimer = 1f;
        [SerializeField] float m_transactionTimer = 1f;
        [SerializeField] float m_replyTimer = 1f;
        [SerializeField] float m_confirmationTimer = 1f;

        [Header("ConvertionSpeed")]

        [SerializeField] float m_playerKillDuration = 0.75f;
        [SerializeField] float m_totalKillDuration = 0.75f;
        [SerializeField] float m_conversionDuration = 0.75f;

        [SerializeField] NarrativeBlock m_randomAmaraReply;
        [SerializeField] NarrativeBlock m_randomJudeReply;
        private TextBlock m_currWinnerReply;

        public void Init()
        {
            CharacterEnum winningCharacter = m_charactersPanel.Init();
            m_rewardPanel.Init();

            NarrativeBlock randomWinnerReply = (winningCharacter == CharacterEnum.MELEE) ? m_randomAmaraReply : m_randomJudeReply;

            PickRandomReply(winningCharacter);

            StartCoroutine(ScriptedShow());
        }

        public void PickRandomReply(CharacterEnum character)
        {
            NarrativeBlock randomWinnerReply = (character == CharacterEnum.MELEE) ? m_randomAmaraReply : m_randomJudeReply;

            if (!randomWinnerReply)
                return;

            int replyCount = randomWinnerReply.TextBlockArray.Length;

            int rand = Random.Range(0, replyCount);

            m_currWinnerReply = randomWinnerReply.TextBlockArray[rand];
        }

        IEnumerator ScriptedShow()
        {
            yield return new WaitForSeconds(m_playerTimer);

            Debug.LogWarning("After Player Timer");

            m_charactersPanel.gameObject.SetActive(true);

            m_charactersPanel.WinPanel.ShowPlayerSprite();
            m_charactersPanel.WinPanel.ShowPlayerID();

            m_charactersPanel.LosePanel.ShowPlayerSprite();
            m_charactersPanel.LosePanel.ShowPlayerID();

            yield return new WaitForSeconds(m_playerKillTimer);

            Debug.LogWarning("After Player Kill Timer");

            m_charactersPanel.WinPanel.ShowSlimeCount(m_playerKillDuration);
            m_charactersPanel.LosePanel.ShowSlimeCount(m_playerKillDuration);

            yield return new WaitForSeconds(m_totalKillTimer);

            Debug.LogWarning("After Total Kill Timer");

            m_rewardPanel.ShowTotalSlimeBall(m_totalKillDuration);

            yield return new WaitForSeconds(m_transactionTimer);

            Debug.LogWarning("After transaction Timer");

            m_rewardPanel.ConversionToMoney(m_conversionDuration);

            yield return new WaitForSeconds(m_replyTimer);

            m_charactersPanel.WinPanel.ActiveNarration(m_currWinnerReply);

            yield return new WaitForSeconds(m_confirmationTimer);

            Debug.LogWarning("After Confirmation Timer");

            m_controllerIndicatorPanel.gameObject.SetActive(true);
            m_controllerIndicatorPanel.Init();
        }
    }
}