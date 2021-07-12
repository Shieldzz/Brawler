using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BTA
{

    public class CustomEventSystem : EventSystem
    {

        [SerializeField] private GameObject[] m_availableGameObject = null;
        private StandaloneInputModule m_standaloneInputModel = null;


        // Use this for initialization
        override protected void Start()
        {
            m_standaloneInputModel = GetComponent<StandaloneInputModule>();
            base.Start();

            Debug.Log(gameObject.name + " EventSystem standalone Input");
            Debug.Log(m_standaloneInputModel.horizontalAxis.ToString());
            Debug.Log(m_standaloneInputModel.verticalAxis.ToString());
        }

        // Update is called once per frame
        override protected void Update()
        {
            EventSystem originalCurrent = current;
            current = this;
            Debug.Log(gameObject.name + " equiped ["+this+"]");
            base.Update();
            current = originalCurrent;

            Debug.Log(">>> >>> >>> <<< <<< <<< \n\n");
        }
    }
}