using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BTA
{
    public class BasicMovement : MonoBehaviour
    {
        protected Animator m_animator;
        protected Rigidbody m_rigidbody;

        protected bool m_superArmored;
        protected bool m_isFrozen;
        protected float m_afterHitFreezeTimer;

        public bool HasSuperArmor {  get { return m_superArmored; } set { m_superArmored = value; } }
        public bool IsFreeze { get { return m_isFrozen; } }


        [HideInInspector] public UnityEvent OnAttackInterrupt = new UnityEvent();

        [Header("Movement")]

        [SerializeField] private float m_speed = 175f;
        [SerializeField] private float m_zSpeed = 120f;
        [SerializeField] protected float m_afterHitFreezeDuration = 0.25f;

        public float Speed { get { return m_speed; } set { m_speed = value; } }
        public float ZSpeed { get { return m_zSpeed; } set { m_zSpeed = value; } }

        // FUNC 

        virtual protected void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();

            //TODO : Assign Event from animator here ? 
            m_animator = GetComponentInChildren<Animator>();
            if (!m_animator)
                Debug.LogError("No Animator on character " + gameObject.ToString());
        }

        virtual protected void OnDisable()
        {
            //Debug.Log("Disable BAsic Movement");
            Freeze(false);
        }

        virtual protected void FixedUpdate()
        {
            if (m_afterHitFreezeTimer > 0f)
            {
                m_afterHitFreezeTimer -= Time.deltaTime;

                //may need to check if player is attacking so that we don't unfreeze it
                if (m_afterHitFreezeTimer <= 0f)
                    Freeze(false);
            }
        }

        #region Movement

        virtual public void UpdateMovement(float xAxis, float zAxis)
        {
            //Debug.Log("Movement Call");
            if (m_isFrozen)
            {
                //Debug.Log("Movement Frozen");
                m_rigidbody.velocity = new Vector3(0f, 0f, 0f);
                m_animator.SetFloat("InputX", 0f);
                m_animator.SetFloat("InputY", 0f);
                return;
            }
            //if Freeze Don't update movement or Rotation

            //Debug.Log("After Freeze Movement Call");
            //Debug.Log("xAxis/yAxis = " + xAxis + " / " + zAxis);
            m_animator.SetFloat("InputX", xAxis);
            m_animator.SetFloat("InputY", zAxis);

            XFlip(xAxis);

            //Managing X and Y 

            float inputX = xAxis * Time.deltaTime * m_speed;
            float inputY = m_rigidbody.velocity.y;
            float inputZ = zAxis * Time.deltaTime * m_zSpeed;

            m_rigidbody.velocity = new Vector3(inputX, inputY, inputZ);
        }

        public void StopMovement()
        {
            m_animator.SetFloat("InputX", 0f);
            m_animator.SetFloat("InputY", 0f);
            m_rigidbody.velocity = Vector3.zero;
        }

        public void XFlip(float xAxis)
        {
            Vector3 eulerAngles = transform.localEulerAngles;
            if (xAxis < 0f)
                eulerAngles.y = 180f;

            else if (xAxis > 0f)
                eulerAngles.y = 0f;

            transform.localRotation = Quaternion.Euler(eulerAngles);
        }
        #endregion


        virtual public void Freeze(bool dofreeze)
        {
            //Debug.Log("Freeze");

            if (m_isFrozen == dofreeze)
                return;

            m_rigidbody.velocity = new Vector3(0f, 0f, 0f);
            m_isFrozen = dofreeze;
            m_rigidbody.useGravity = !dofreeze;
        }

        #region CrowdControl

        virtual public void Stagger()
        {
            Freeze(true);
            m_afterHitFreezeTimer = m_afterHitFreezeDuration;
            if (m_animator)
            {
                OnAttackInterrupt.Invoke();
                m_animator.SetTrigger("Stagger");
            }
        }

        virtual public void Knock(float force, Vector3 Direction)
        {
            OnAttackInterrupt.Invoke();

            if (m_isFrozen)
                Freeze(false);

            m_animator.SetTrigger("Stagger");

            m_rigidbody.velocity = new Vector3(0f, 0f, 0f);
            m_rigidbody.AddForce(Direction * force); // / m_rigidBody.mass;
        }
        #endregion
    }
}
