using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private GameObject m_resumeGameObject = null;
        public GameObject Resume { get { return m_resumeGameObject; } }
        [SerializeField] private GameObject m_QuitGameObject = null;
        public GameObject Quit { get { return m_QuitGameObject; } }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}