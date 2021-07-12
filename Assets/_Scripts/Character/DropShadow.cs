using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class DropShadow : MonoBehaviour
    {
        private GameObject m_shadowParent;
        private GameObject m_shadowPrefab;
        private GameObject m_shadow;
        //public GameObject GetShadowGameObject { get { return m_shadow; } }

        private SpriteRenderer m_spriteRenderer;
        [SerializeField] private bool m_update = false;
        LayerMask m_groundCheckLayers;
        [SerializeField] private Sprite m_sprite;
        [SerializeField, Range(0, 10)] float m_offset;

        private BoxCollider m_boxCollider;

        // Use this for initialization
        void Start()
        {
            m_shadowParent = GameObject.Find("DropShadowStock") as GameObject;
            m_shadowPrefab = Resources.Load("Entity/DropShadow") as GameObject;

            m_boxCollider = gameObject.GetComponent<BoxCollider>();

            //m_shadow = new GameObject(" Drop Shadow");
            /*m_shadow.transform.position = gameObject.transform.position;
            m_shadow.transform.rotation = gameObject.transform.rotation;

            m_spriteRenderer = m_shadow.AddComponent<SpriteRenderer>();
            m_spriteRenderer.sprite = m_sprite;
            
            m_shadow.transform.position = transform.position;
            */
            InstantiateDropShadow();

            m_groundCheckLayers = (1 << LayerMask.NameToLayer("Solid")) | (1 << LayerMask.NameToLayer("Ground"));

            RaycastHit hit;
            Vector3 position = transform.position;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, m_groundCheckLayers))
            {
                position.y = hit.point.y;
            }
            m_shadow.transform.position = position;

        }

        // Update is called once per frame
        void Update()
        {
            if (m_update && m_shadow)
            {
                Vector3 position = transform.position;
                RaycastHit hit;
                Vector3 raycast = transform.position;
                raycast.y -= 1;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, m_groundCheckLayers))
                {
                    position.y = hit.point.y;
                }
                m_shadow.transform.position = position;
                UpdateScale();
            }
        }

        private void InstantiateDropShadow()
        {
            m_shadow = Instantiate(m_shadowPrefab);
            m_shadow.transform.position = gameObject.transform.position;
            m_shadow.transform.rotation = gameObject.transform.rotation;
            UpdateScale();
            m_shadow.transform.parent = m_shadowParent.transform;
        }

        private void UpdateScale()
        {
            Vector3 size = transform.localScale + new Vector3(m_offset, m_offset, m_offset);
            m_shadow.transform.localScale = size;
        }

        public void OnDie(AttackTrigger trigger, Damageable damageable)
        {
            m_shadow.SetActive(false);
        }

        private void OnEnable()
        {
            if (m_shadow)
                m_shadow.SetActive(true);
        }

        private void OnDisable()
        {
            if (m_shadow)
                m_shadow.SetActive(false);
        }
    }
}
