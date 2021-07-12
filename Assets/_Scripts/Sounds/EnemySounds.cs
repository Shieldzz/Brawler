using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BTA
{
    public class EnemySounds : MonoBehaviour
    {

        [FMODUnity.EventRef]
        public string m_attackSound;
        [FMODUnity.EventRef]
        public string m_hitSound;


        public void LaunchAttackSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_attackSound);
        }

        public void LauchHitSounds()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_hitSound);
        }
    }
}
