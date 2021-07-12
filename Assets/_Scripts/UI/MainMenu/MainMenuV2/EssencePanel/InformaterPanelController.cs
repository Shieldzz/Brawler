using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{
    public class InformaterPanelController : MonoBehaviour
    {
        [SerializeField] private FlaskDescriptorPanel m_currFlaskDescriptor;
        [SerializeField] private FlaskDescriptorPanel m_selectedFlaskDescriptor;

        public void InspectEquipedFlask(FlaskData data)
        {
            //Debug.Log("Inspecting flask = " + data.name);
            m_currFlaskDescriptor.DescribeFlask(data);
        }

        public void InspectSelectedFlask(FlaskData data)
        {
            m_selectedFlaskDescriptor.DescribeFlask(data);
        }
    }
}