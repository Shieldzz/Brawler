using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BTA
{
    public class AttackTrigger : MonoBehaviour
    {

        Player m_owner;
        public Player Owner { get { return m_owner; } set { m_owner = value; } }

        protected int m_attackID;

        protected Collider m_collider;
        protected AttackData m_currData;

        // first hit or not
        public class HitEvent : UnityEvent<bool, int> { }
        protected bool m_firstHit = true;

        public HitEvent OnHitEvent = new HitEvent();

        public float Damage { get { return m_currData.damage; } }


        virtual protected void Awake()
        {
            m_owner = GetComponentInParent<Player>();

            m_collider = GetComponent<Collider>();
            if (!m_collider)
                Debug.LogError("Trigger " + gameObject.ToString() + "has no Collider on it ...");

            Disable();
        }

        virtual protected void OnTriggerEnter(Collider other)
        {
            Damageable oppDamageable = other.GetComponent<Damageable>();
            if (oppDamageable)
            {
                bool touched = oppDamageable.TakeDamage(this, m_currData.damage, m_attackID);
                if (!touched)
                    return;

                if (other.gameObject.layer == LayerMask.NameToLayer("EnemyHitBox"))
                    OnHitEvent.Invoke(m_firstHit, m_currData.damage);

                m_firstHit = false;

                ManageFXObject(other.gameObject);

                if (!oppDamageable.IsAlive())
                    return;

                BasicMovement mvmt = other.gameObject.GetComponentInParent<BasicMovement>();
                if (mvmt)
                    ManageCrowdControlEffect(mvmt);
            }
        }

        virtual public void Enable(AttackData data)
        {
            // force the collider to reset if already enable -> hard RESET 
            if (m_collider == enabled)
                m_collider.enabled = false;

            m_collider.enabled = true;

            //Generate Random ID for every Damage Input
            m_attackID = Random.Range(-100000000, 100000000);
            m_currData = data;

            //manage SlamDown Effect
            if (m_currData.CC == CrowdControl.SlamDown)
            {
                Movement mvmt = m_owner.GetComponent<Movement>();
                if (mvmt)
                    StartCoroutine(mvmt.SlamDown(10f));
            }
        }

        virtual public void Disable()
        {
            m_collider.enabled = false;
        }

        #region CrowdControl

        protected void ManageCrowdControlEffect(BasicMovement movingEntity)
        {
            if (movingEntity.HasSuperArmor)
                return;

            switch (m_currData.CC)
            {
                case CrowdControl.Stagger:
                    {
                        movingEntity.Stagger();
                        break;
                    }
                case CrowdControl.KnockBack:
                    {
                        float playerRotY = transform.rotation.eulerAngles.y;

                        if (playerRotY == 0f)
                            movingEntity.Knock(m_currData.CCForce, Vector3.right);
                        else
                            movingEntity.Knock(m_currData.CCForce, Vector3.left);
                        break;
                    }
                case CrowdControl.KnockDown:
                    {
                        movingEntity.Knock(m_currData.CCForce, Vector3.down);
                        break;
                    }
                case CrowdControl.KnockUp:
                    {
                        movingEntity.Knock(m_currData.CCForce, Vector3.up);
                        break;
                    }
                case CrowdControl.SlamDown:
                    {
                        movingEntity.Knock(m_currData.CCForce, Vector3.down);
                        break;
                    }
                case CrowdControl.Expell:
                    {
                        Vector3 dir = movingEntity.transform.position - transform.position;
                        movingEntity.Knock(m_currData.CCForce, dir);
                        break;
                    }
                default:
                    break;

            }
        }

        #endregion

        #region FX

        protected void ManageFXObject(GameObject opponent)
        {
            if (!m_currData.gaoFX)
                return;

            GameObject FX = GameObject.Instantiate(m_currData.gaoFX);

            FX.transform.position = transform.position; //opponent.transform.position;
            FX.transform.rotation = transform.rotation;

            AttackTrigger FxTrigger = FX.GetComponent<AttackTrigger>();
            if (FxTrigger)
                FxTrigger.Owner = m_owner;
        }

        #endregion
    }
}
