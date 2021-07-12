using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZScaling : MonoBehaviour
{
    [SerializeField] bool m_updatedEachFrame = false;
    [SerializeField] float m_scaleAtOrigin = 1f;
    [SerializeField] float m_scaleSpeed = 0.1f;

    private void Start()
    {
        UpdateScale(); 
    }

    private void Update()
    {
        if (m_updatedEachFrame)
            UpdateScale();
    }

    private void UpdateScale()
    {
        //invert Zpos so that sprite will grow when going closer (-Z)
        float scaleOffset = (-transform.position.z) * m_scaleSpeed;
        float newScale = m_scaleAtOrigin + scaleOffset;
        transform.localScale = new Vector3(newScale, newScale, m_scaleAtOrigin);
    }
}
