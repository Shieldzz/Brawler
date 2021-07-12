using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class CameraScreenCollider : MonoBehaviour
    {
        [SerializeField] float m_colliderLengthX = 0.5f;
        [SerializeField] float m_colliderLengthZ = 10f;

        Camera m_camera;

        //Left & Righ Border Collider
        BoxCollider m_leftBorder;
        public BoxCollider GetLeftBorder { get { return m_leftBorder; } }
        BoxCollider m_rightBorder;
        public BoxCollider GetRightBorder { get { return m_rightBorder; } }

        // Use this for initialization
        void Start()
        {
            m_camera = GetComponent<Camera>();

            SetUpCollider();
        }

        void Update()
        {
            //TODO: Call this only when window size or orthographicSize is changed 
            UpdateCollider();
        }

        void SetUpCollider()
        {
            //Create Children to contain Collider
            GameObject childCollider = new GameObject();
            childCollider.transform.position = gameObject.transform.position;
            childCollider.transform.rotation = gameObject.transform.rotation;
            childCollider.transform.parent = gameObject.transform;

            //Set Up Both Collider
            childCollider.layer = LayerMask.NameToLayer("PlayerRestricted");

            m_leftBorder = childCollider.AddComponent<BoxCollider>();
            m_rightBorder = childCollider.AddComponent<BoxCollider>();

            UpdateCollider();
        }

        void UpdateCollider()
        {
            float halfSize = m_camera.orthographicSize;
            float size = halfSize * 2f;
            float far = m_camera.farClipPlane;
            float aspect = m_camera.aspect;

            m_leftBorder.center = new Vector3(-aspect * halfSize, 0f, -transform.position.z);
            m_leftBorder.size = new Vector3(m_colliderLengthX, size, m_colliderLengthZ);

            m_rightBorder.center = new Vector3(aspect * halfSize, 0f, -transform.position.z);
            m_rightBorder.size = new Vector3(m_colliderLengthX, size, m_colliderLengthZ);
        }

        public Vector3 GetRightBorderPosition()
        {
            Vector3 position = transform.position + m_rightBorder.center;
            position.x -= m_colliderLengthX;
            return position;
        }

        public Vector3 GetLeftBorderPosition()
        {
            Vector3 position = transform.position + m_leftBorder.center;
            position.x += m_colliderLengthX;
            return position;
        }
    }
}
