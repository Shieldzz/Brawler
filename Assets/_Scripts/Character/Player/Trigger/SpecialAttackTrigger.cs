using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class SpecialAttackTrigger : AttackTrigger
    {
        ParticleSystem m_particle;
        [FMODUnity.EventRef]
        FMODUnity.StudioEventEmitter m_sfxEmitter;

        override protected void Awake()
        {
            m_particle = GetComponent<ParticleSystem>();
            m_sfxEmitter = GetComponent<FMODUnity.StudioEventEmitter>();
            base.Awake();
        }

        override public void Enable(AttackData data)
        {
            //if (m_collider.enabled == false)
            // RayCast to Set Collider Dist ??? but should only do that on Dist special ... 

            base.Enable(data);

            if (!m_particle.isPlaying)
                m_particle.Play();

            //if (!m_sfxEmitter.IsPlaying())
            //    m_sfxEmitter.Play();
        }

        override public void Disable()
        {
            base.Disable();
            m_particle.Stop();
            //m_sfxEmitter.Stop();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }

        public virtual void SetRadius(float radius)
        {
            ParticleSystem.ShapeModule shape = m_particle.shape;
            shape.radius = radius;
        }
    }
}
