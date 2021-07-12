using System.Collections;
using UnityEngine;

namespace BTA
{

    public class Movement : BasicMovement
    {
        CameraManager m_camMgr;
        PositionDetector m_positionDetector;

        #region Sounds
        private PlayerSounds m_sounds;
        #endregion

        bool m_isJumping = false;
        bool m_isDashing = false;

        bool m_isRecovering = false;

        [Header("Jump")]

        [SerializeField]
        AnimationCurve m_jumpCurve = new AnimationCurve();
        [SerializeField] float m_jumpHeight = 1.5f;
        [SerializeField] float m_jumpDuration = 1f;
        [SerializeField, Range(0f, 1f)] float m_airControl = 1f;

        [SerializeField] float m_fallingVelocity = 4f;

        public float JumpHeight { get { return m_jumpHeight; } set { m_jumpHeight = value; } }
        public float JumpDuration { get { return m_jumpDuration; } set { m_jumpDuration = value; } }
        public float FallingSpeed { get { return m_fallingVelocity; } set { m_fallingVelocity = value; } }

        bool m_jumpInterrupt = false;
        float m_beforeJumpYpos;

        [Header("Dash")]

        [SerializeField] AttackStruct m_dashAttack;
        [SerializeField] AttackTrigger m_dashTrigger;
        [SerializeField] float m_dashCooldown = 3f;
        [SerializeField] float m_dashLength = 2f;
        [SerializeField] float m_dashDuration = 0.25f;

        public float DashCooldown { get { return m_dashCooldown; } set { m_dashCooldown = value; } }
        public float DashLength { get { return m_dashLength; } set { m_dashLength = value; } }
        public float DashDuration { get { return m_dashDuration; } set { m_dashDuration = value; } }

        float m_dashTimer = 0f;
        float m_dashDir = 0f;
        bool m_dashInterrupt = false;
        float m_beforeDashXpos;

        [Header("SlamDown")]

        [SerializeField] FXEnabler m_slamDownFX;
        [SerializeField] float m_shakeAmplitude = 0.1f;
        [SerializeField] float m_shakeFrequency = 5f;
        [SerializeField] float m_shakeDuration = 0.1f;

        // Getter

        public bool IsJumping()
        {
            return m_isJumping;
        }

        public bool IsDashing()
        {
            return m_isDashing;
        }

        override protected void Awake()
        {
            base.Awake();
            m_sounds = GetComponentInChildren<PlayerSounds>();
            if (!m_sounds)
                Debug.LogError("No Sounds on character " + gameObject.ToString());

            m_positionDetector = GetComponent<PositionDetector>();
            if (!m_positionDetector)
                Debug.LogError("No Position Detector on character " + gameObject.ToString());

            PlayerFighting fighting = GetComponent<PlayerFighting>();
            m_dashTrigger.OnHitEvent.AddListener(fighting.OnSuccessfulHit);
        }

        private void Start()
        {
            m_camMgr = LevelManager.Instance.GetInstanceOf<CameraManager>();
            if (!m_camMgr)
                Debug.LogError("No Camera Manager found !");
        }

        protected void OnEnable()
        {
            m_animator.SetBool("Airborn", !m_positionDetector.IsOnGround);
        }

        override protected void OnDisable()
        {
            base.OnDisable();
            m_isJumping = false;
            m_jumpInterrupt = false;
            m_isDashing = false;
            m_dashInterrupt = false;
        }

        // Use this for initialization
        override protected void FixedUpdate()
        {
            base.FixedUpdate();

            if (m_dashTimer > 0f)
                m_dashTimer -= Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            //if (m_isDashing)
            //    if (collision.gameObject.layer == LayerMask.NameToLayer("Solid") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerRestricted"))
            //        m_dashInterrupt = true;

            //ensure that player jump is ending when player hit the ground on the Y Axis
            if (m_isJumping)
                if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Solid"))
                    if (m_positionDetector.IsOnGround)
                        m_jumpInterrupt = true;
                    
        }

        private void OnCollisionStay(Collision collision)
        {
            //if (m_isDashing)
            //    if (collision.gameObject.layer == LayerMask.NameToLayer("Solid") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerRestricted"))
            //        m_dashInterrupt = true;
        }

        public void SetRecoveringState(bool doRecover)
        {
            m_isRecovering = doRecover;
        }

        public override void Freeze(bool doFreeze)
        {
            base.Freeze(doFreeze);
            if (m_isJumping && doFreeze)
                m_jumpInterrupt = true;

            if (m_positionDetector.IsOnGround == false)
                m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, -m_fallingVelocity, m_rigidbody.velocity.z);
        }

        #region Movement

        override public void UpdateMovement(float xAxis, float zAxis)
        {
            m_dashDir = xAxis;
            if (m_isDashing /*|| !m_positionDetector.IsOnGround()*/)
                zAxis = 0f;
            if (m_isDashing)
                xAxis = 0f;

            // Need to chekc when "Airborn" & not just when Jumping
            float airControlRatio = (m_isJumping ? m_airControl : 1f);
            xAxis *= airControlRatio;
            zAxis *= airControlRatio;

            base.UpdateMovement(xAxis, zAxis);
        }
        #endregion

        #region Jump

        public void StartJumping()
        {
            if (!m_isJumping && m_positionDetector.IsOnGround && (!m_isFrozen || m_isRecovering))
            {
                OnAttackInterrupt.Invoke();
                if (m_isFrozen)
                    Freeze(false);

                m_animator.SetBool("Jump", true);
                m_animator.SetBool("Airborn", true);

                StartCoroutine(Jump());
            }
        }

        private IEnumerator Jump()
        {
            m_beforeJumpYpos = transform.position.y;
            m_isJumping = true;

            if (m_sounds)
                m_sounds.LaunchJumpSound();

            float t = 0f;
            while (t <= 1.0f)
            {
                float currCurveValue = m_jumpCurve.Evaluate(t);
                float height = currCurveValue * m_jumpHeight;

                transform.position = new Vector3(transform.position.x,
                                                    m_beforeJumpYpos + height,
                                                    transform.position.z);

                //Interrupt when feet are on the ground, or when hitting the ceiling
                if (m_jumpInterrupt)
                {
                    m_jumpInterrupt = false;
                    m_animator.SetBool("Jump", false);
                    break;
                }

                m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, 0f, m_rigidbody.velocity.z);

                t += Time.deltaTime / m_jumpDuration;

                if (m_jumpCurve.Evaluate(t) < currCurveValue)
                {
                    m_animator.SetBool("Jump", false);
                    m_jumpInterrupt = true;
                }

                yield return null;
            }

            m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, -m_fallingVelocity, m_rigidbody.velocity.z);

            m_jumpInterrupt = false;
            m_isJumping = false;
            

            yield return new WaitForEndOfFrame();
        }
        #endregion

        #region Dash

        public void StartDash()
        {
            if (!m_isDashing && m_dashTimer <= 0f && (!m_isFrozen || m_isRecovering))
            {
                //Ensure that player doesn't double Dash
                m_isDashing = true;
                OnAttackInterrupt.Invoke();

                if (m_isFrozen)
                    Freeze(false);

                m_animator.SetBool("Dash", true);

                StartCoroutine(Dash());
                m_sounds.LaunchDashSound();
            }
        }

        private IEnumerator Dash()
        {
            m_isDashing = true;
            m_dashTimer = m_dashDuration;
            m_jumpInterrupt = true;
            m_superArmored = true;

            m_dashTrigger.Enable(m_dashAttack.attackDatas[0]);

            float dir;
            if (m_dashDir == 0f) // No input -> player Direction
                dir = (transform.rotation.eulerAngles.y == 0f) ? 1f : -1f;
            else                   // input Direction
                dir = (m_dashDir > 0f) ? 1f : -1f;

            m_beforeDashXpos = transform.position.x;

            LayerMask dashLayers = (1 << LayerMask.NameToLayer("Solid")) | (1 << LayerMask.NameToLayer("PlayerRestricted"));

            RaycastHit hit;
            bool hasHit = m_positionDetector.BoxCast(dir * Vector3.right, m_dashLength, out hit, dashLayers);

            float afterDashXpos = hasHit ? hit.point.x : m_beforeDashXpos + (m_dashLength * dir);


            float t = 0f;
            while (t <= 1.0)
            {
                //Interrupt if touching any solid object/border screen
                if (m_dashInterrupt)
                {
                    m_dashInterrupt = false;
                    break;
                }

                //momentum
                m_rigidbody.velocity = Vector3.zero;

                float nextXpos = Mathf.Lerp(m_beforeDashXpos, afterDashXpos, t);

                transform.position = new Vector3(nextXpos,
                                                 transform.position.y,
                                                 transform.position.z);

                t += Time.deltaTime / m_dashDuration;
                yield return null;
            }

            m_dashTrigger.Disable();

            m_superArmored = false;
            m_dashTimer = m_dashCooldown;
            m_isDashing = false;
            m_dashInterrupt = false;
            m_jumpInterrupt = false;

            m_animator.SetBool("Dash", false);

            yield return new WaitForEndOfFrame();
        }
        #endregion

        public IEnumerator SlamDown(float speed)
        {
            m_slamDownFX.Proc();
            while (!m_positionDetector.IsOnGround)
            {
                transform.position = new Vector3(transform.position.x,
                                                 transform.position.y - speed * Time.deltaTime,
                                                 transform.position.z);

                yield return null;
            }

            m_slamDownFX.gameObject.SetActive(false);
            Freeze(false);
            m_superArmored = false;
            m_camMgr.ShakeCameraTemporary(m_shakeAmplitude, m_shakeFrequency, m_shakeDuration);
            OnAttackInterrupt.Invoke();
 
            yield return new WaitForEndOfFrame();
        }
    }
}
