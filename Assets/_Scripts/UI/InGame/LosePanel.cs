using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BTA
{

    public class LosePanel : MonoBehaviour
    {

        [SerializeField] private GameObject m_retryGameObject;
        public GameObject Retry { get { return m_retryGameObject; } }

        [SerializeField] private GameObject m_menuGameObject;
        public GameObject Menu { get { return m_menuGameObject; } }


        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(Retry);
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }

}