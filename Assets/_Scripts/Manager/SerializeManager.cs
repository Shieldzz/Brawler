using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class SerializeManager : MonoBehaviour
    {
        private bool m_created = false;

        private void Awake()
        {
            if (!m_created)
            {
                DontDestroyOnLoad(gameObject);
                m_created = true;
            }
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

} // namespace BTA

