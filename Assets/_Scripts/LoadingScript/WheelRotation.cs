using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class WheelRotation : MonoBehaviour
    {

        [SerializeField] private float m_speedRotation = 10f;
        [SerializeField] private float m_rotationOffset = 1f;

        private RectTransform m_rectTransform;

        // Use this for initialization
        void Start()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 currentRotation = m_rectTransform.localEulerAngles;
            currentRotation.z += m_rotationOffset * (m_speedRotation * Time.deltaTime);

            m_rectTransform.localEulerAngles = currentRotation;
        }
        
    }
}