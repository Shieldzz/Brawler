using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BTA
{

    public class NarrativeFlaskEmplacement : Button
    {

        [SerializeField] FlaskData m_narrativeFlask = null;
        private Sprite m_unequipSprite = null;
        [SerializeField] DescriptorPanelManager m_descriptor;
        [SerializeField] Image m_flaskImage = null;
        public GameObject FlaskDecorator { get { return m_flaskImage.gameObject; } }

        // Use this for initialization
        override protected void Start()
        {
            base.Start();
            m_unequipSprite = GetComponent<Image>().sprite;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnSelect(BaseEventData eventData)
        {
            m_descriptor.DescribeFlask(m_narrativeFlask);
            //m_flaskImage.GetComponent<Animator>().Play("Highlighted");
            m_flaskImage.GetComponent<Button>().OnSelect(null);
            base.OnSelect(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            //m_flaskImage.GetComponent<Animator>().Play("Normal");
            m_flaskImage.GetComponent<Button>().OnDeselect(null);

            base.OnDeselect(eventData);
        }

        protected override void OnDisable()
        {
            m_flaskImage.GetComponent<Button>().OnDeselect(null);
            base.OnDisable();
        }
    }

}