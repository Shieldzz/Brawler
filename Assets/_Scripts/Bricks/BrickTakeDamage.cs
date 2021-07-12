using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BTA
{
    public class BrickTakeDamage : MonoBehaviour
    {
        private Animator m_animator;

        // Use this for initialization
        void Start()
        {
            m_animator = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TakeDamage(AttackTrigger trigger, Damageable damageable)
        {
            m_animator.SetBool("Hit", true);
        }

        public void Death()
        {
            m_animator.SetBool("Dead", true);
        }

        public void DestroyItself()
        {
            Destroy(gameObject);
        }
    }
}