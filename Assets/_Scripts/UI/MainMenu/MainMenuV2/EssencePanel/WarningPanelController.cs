using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{

    public class WarningPanelController : MonoBehaviour
    {

        [SerializeField] private GameObject m_warningMessageTitle = null;
        [SerializeField] private GameObject m_warningMessage = null;

        void Start()
        {

        }

        void Update()
        {

        }

        public void SetWarningText(string warningTitle, string warningMsg)
        {
            m_warningMessageTitle.GetComponent<Text>().text = warningTitle;
            m_warningMessage.GetComponent<Text>().text = warningMsg;
        }
    }
}