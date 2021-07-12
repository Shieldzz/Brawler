using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{

    public class NarrativePanel : MonoBehaviour
    {

        [SerializeField] private Text m_titleText = null;
        [SerializeField] private Text m_narrativeText = null;
        [SerializeField] private Image m_essenceImage = null;

        [SerializeField] private Sprite m_flaskNone = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ExplainFlask(FlaskData data)
        {
            if (!data)
            {
                m_titleText.text = "No Flask";
                m_narrativeText.text = "No flask equipped";
                m_essenceImage.sprite = m_flaskNone;

                return;
            }

            m_titleText.text = data.flaskName;
            m_narrativeText.text = data.narrativeDescription;
            m_essenceImage.sprite = data.m_flaskSprite;
        }
    }
}