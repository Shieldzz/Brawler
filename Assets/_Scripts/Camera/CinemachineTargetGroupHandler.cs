using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CinemachineTargetGroupHandler : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera m_vCam;
    CinemachineTargetGroup m_handledGroup;

    CinemachineTargetGroup.Target[] m_targets;
    CinemachineFramingTransposer m_framingTransposer;
    float m_borderOffset = 0f;

    [SerializeField] float m_targetBaseWeight = 1f;
    [SerializeField] float m_targetBorderWeight;

    // Use this for initialization
    void Start()
    {
        if (!m_vCam)
            m_vCam = GetComponent<CinemachineVirtualCamera>();

        Transform follow = m_vCam.Follow;
        m_handledGroup = follow.GetComponent<CinemachineTargetGroup>();
        if (!m_handledGroup)
        {
            Debug.LogError("This camera does not use TargetGroup, this script has no use and will disabled itself ...");
            enabled = false;
            return;
        }

        m_targets = m_handledGroup.m_Targets;
        m_framingTransposer = m_vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (!m_framingTransposer)
        {
            Debug.LogError("This Camera has no Body \"FramingTransposer\", this script has no use and will disable itself ...");
            enabled = false;
            return;
        }

        //float halfScreenSize = m_vCam.m_Lens.OrthographicSize * m_vCam.m_Lens.Aspect;
        //m_borderOffset = m_framingTransposer.m_DeadZoneWidth * halfScreenSize;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //TODO : Call only when screen is resize ...
        float halfScreenSize = m_vCam.m_Lens.OrthographicSize * m_vCam.m_Lens.Aspect;
        m_borderOffset = m_framingTransposer.m_DeadZoneWidth * halfScreenSize;

        int size = m_targets.Length;
        Vector3 camPosition = m_vCam.transform.position;

        for (int i = 0; i < size; i++)
        {
            Vector3 targetPosition = m_targets[i].target.transform.position;
            if (targetPosition.x >= camPosition.x + m_borderOffset
                || targetPosition.x <= camPosition.x - m_borderOffset)

                m_targets[i].weight = m_targetBorderWeight;
            else
                m_targets[i].weight = m_targetBaseWeight;
        }
	}
}
