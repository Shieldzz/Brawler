using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PositionDetector : MonoBehaviour
{
    Animator m_animator;

    bool m_isOnGround = true;
    bool m_ishittingCeiling = false;

    public bool IsOnGround { get { return m_isOnGround; } }
    public bool IsHittingCeilling { get { return m_ishittingCeiling; } }

    LayerMask m_groundCheckLayers;

    [SerializeField] BoxCollider m_collider;
    [SerializeField] float m_checkLength = 0.1f;
    [SerializeField] float m_safetyGap = 0.05f;

    public UnityEvent OnGroundEvent = new UnityEvent();
    public UnityEvent OnAirEvent = new UnityEvent();

    // Use this for initialization
    void Awake()
    {
        m_groundCheckLayers = (1 << LayerMask.NameToLayer("Solid")) | (1 << LayerMask.NameToLayer("Ground"));

        if (m_collider == null)
            GetComponent<BoxCollider>();

        m_animator = GetComponentInChildren<Animator>();
        //bool isOnGround = CheckGround();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isOnGround = CheckGround();
        if (m_isOnGround != isOnGround)
        {
            m_isOnGround = isOnGround;

            m_animator.SetBool("Airborn", !m_isOnGround);
            //m_animator.SetInteger("currComboStep", 0);

            if (m_isOnGround == true)
            {
                //Debug.Log("Become Grounded");
                OnGroundEvent.Invoke();
            }
            else
            {
                //Debug.Log("Become Airborn");
                OnAirEvent.Invoke();
            }
        }

        //m_ishittingCeiling = CheckBox(Vector3.up, m_checkLength, m_groundCheckLayers, true);
    }

    public bool CheckGround()
    {
        return CheckBox(Vector3.down, m_checkLength, m_groundCheckLayers, true);
    }

    // BoxCast is litteraly a RayCast but checking a BoxCollider
    public bool CheckBox(Vector3 direction, float distance, LayerMask layers, bool fromBorder = false)
    {
        //Security 
        direction.Normalize();

        //Set Origin of BoxCast at the center or at the border of the direction
        //Vector3 center = transform.position + m_collider.center;
        Vector3 origin = transform.position + m_collider.center;/*fromBorder ? center + Vector3.Scale(m_collider.size /2f, direction) : center;*/


        //Debug.DrawLine(origin, origin + direction * distance, Color.red);
        //Debug.Log("origin = " + origin + " box center = " + boxCenter);

        //Calc Box Data
        Vector3 boxCenter = origin + (direction * distance  / 2f);


        float width = m_collider.size.x - m_safetyGap;
        float height = m_collider.size.y - m_safetyGap;
        float length = m_collider.size.z - m_safetyGap;

        Vector3 boxHalfs = new Vector3(width / 2f, height / 2f, length / 2f);

        //Check Collision
        bool result = Physics.CheckBox(boxCenter, boxHalfs, Quaternion.identity, layers);

        //m_castDetect = result;
        //m_MaxDistance = distance;
        //m_Direction = direction;
        //m_CubeSize = boxHalfs;

        return result;
    }

    bool m_castDetect = false;

    public bool BoxCast(Vector3 direction, float distance, out RaycastHit hit, LayerMask layers)
    {
        //Security 
        direction.Normalize();

        //Set Origin of BoxCast at the center or at the border of the direction
        Vector3 center = transform.position + m_collider.center;

        //Debug.Log("Box Size = " + boxSize);
        //Debug.Log("reduced size = " + Vector3.Scale(boxSize, absDirection));

        Vector3 realColliderSize = new Vector3(m_collider.size.x * transform.localScale.x, m_collider.size.y * transform.localScale.y, m_collider.size.z * transform.localScale.z);

        Vector3 boxHalfs = (realColliderSize - m_safetyGap * Vector3.one) / 2f;

        //Check Collision
        bool result = Physics.BoxCast(center, boxHalfs, direction, out hit, Quaternion.identity, distance, layers);

        m_HitDetect = result;
        m_Hit = hit;
        m_MaxDistance = distance;
        m_center = center;
        m_Direction = direction;
        m_CubeSize = boxHalfs;

        return result;
    }

    public IEnumerator EnableAfterXSec(float time)
    {
        yield return new WaitForSeconds(time);
        enabled = true;
    }

    bool m_HitDetect;
    RaycastHit m_Hit;
    float m_MaxDistance;
    Vector3 m_Direction;
    Vector3 m_center;
    Vector3 m_CubeSize;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(m_center, m_Direction * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(m_center + m_Direction * m_Hit.distance, m_CubeSize * 2f);
            Gizmos.DrawSphere(m_Hit.point, 0.1f);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(m_center, m_Direction * m_MaxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(m_center + m_Direction * m_MaxDistance, m_CubeSize * 2f);
        }

        if (m_castDetect)
        {
            Gizmos.DrawWireCube(m_center + m_Direction * m_MaxDistance, m_CubeSize * 2f);
        }
    }
}
