using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEnabler : MonoBehaviour
{
    ParticleSystem m_particle;

    [SerializeField] bool m_disableAfterPlay = false;
    [SerializeField] bool m_destroyAfterPlay = false;
    [SerializeField] bool m_useTimerInsteadOfPlay = false;
    [SerializeField] float m_timerDuration = 2f;
    float m_timer = 0f;

    private void Awake()
    {
        m_particle = GetComponent<ParticleSystem>();
        if (!m_particle)
        {
            Debug.LogError(gameObject + " has no ParticleSystem => Disabling script " + this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (m_useTimerInsteadOfPlay)
        {
            m_timer -= Time.deltaTime;

            if (m_timer <= 0f)
                ManageParticleEnd();
        }
        else if (!m_particle.isPlaying)
            ManageParticleEnd();
    }

    private void OnEnable()
    {
        m_particle.Play();

        if (m_useTimerInsteadOfPlay)
            m_timer = m_timerDuration;
    }

    private void OnDisable()
    {
        m_particle.Stop();
    }

    private void ManageParticleEnd()
    {
        if (m_disableAfterPlay)
            gameObject.SetActive(false);

        else if (m_destroyAfterPlay)
            Destroy(gameObject);
    }
}
