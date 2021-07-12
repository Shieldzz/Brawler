using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BTA
{

    public class EssenceEquipedFlaskButton : MonoBehaviour, ISelectHandler
    {
        public DescriptorPanelManager m_descriptorPanel = null;

        void Start()
        {

        }

        void Update()
        {

        }

        public void OnSelect(BaseEventData eventData)
        {
            HelperUIManager helperManager = HelperUIManager.Instance;
            m_descriptorPanel.DescribeFlask(null);
            helperManager.DisableHelper(helperManager.m_helper3GameObject);
            //helperManager.ClearAction(ref helperManager.m_helper3Action);
        }
    }
}