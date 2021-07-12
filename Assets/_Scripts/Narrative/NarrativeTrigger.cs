using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class NarrativeTrigger : MonoBehaviour
    {
        NarrativeManager m_narrativeMgr;

        [SerializeField] NarrativeBlock m_narrativeBlock;

        private void Start()
        {
            m_narrativeMgr = LevelManager.Instance.GetInstanceOf<NarrativeManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_narrativeBlock)
            {
                m_narrativeMgr.OnNarrativeBlockStart.Invoke(m_narrativeBlock);
                gameObject.SetActive(false);
            }
        }
    }
}
