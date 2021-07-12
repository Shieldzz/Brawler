using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class SoundManager : MonoBehaviour
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
            FMODUnity.RuntimeManager.GetBus("Bus:/").setVolume(0.5f);
            FMODUnity.RuntimeManager.GetBus("Bus:/Music").setVolume(0.5f);
            FMODUnity.RuntimeManager.GetBus("Bus:/SFX").setVolume(0.5f);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

} // namespace BTA

