using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{

    public class FlaskDescriptorPanel : MonoBehaviour
    {

        [SerializeField] private Text m_essenceNameText = null;
        [SerializeField] private Text m_essenceDescriptionText = null;
        [SerializeField] private Text m_essenceEffectText = null;
                                 
        [SerializeField] private Text m_warningText = null;    

        void Start()
        {

        }

        void Update()
        {

        }

        public void DescribeFlask(FlaskData data)
        {
            if (!data)
            {
                m_essenceNameText.text = "None";
                m_essenceDescriptionText.text = "No Essence Equipped";
                m_essenceEffectText.text = "";
                m_warningText.text = "";
                return;
            }
            m_essenceNameText.text = data.flaskName;
            m_essenceDescriptionText.text = data.flaskGeneralDescription;
            m_essenceEffectText.text = data.flaskEffectDescription;
            m_warningText.text = "C-9 toxicity : " + getWarningFromNocivity(data);
        }

        public string getWarningFromNocivity(FlaskData data)
        {
            if (data.nocivity <= 5f)
                return "LOW";

            else if (10f < data.nocivity && data.nocivity <= 10f)
                return "MEDIUM";
            else
                return "HIGH";
        }
    }
}