using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class PlayerSounds : MonoBehaviour
    {

        [FMODUnity.EventRef]
        public string m_lightAttackSound;
        [FMODUnity.EventRef]
        public string m_heavyAttackSound;
        [FMODUnity.EventRef]
        public string m_specialAttackSound;
        [FMODUnity.EventRef]
        public string m_jumpSound;
        [FMODUnity.EventRef]
        public string m_dashSound;
        [FMODUnity.EventRef]
        public string m_takeDamageSound;
        [FMODUnity.EventRef]
        public string m_deathSound;
        [FMODUnity.EventRef]
        public string m_reviveSound;
        [FMODUnity.EventRef]
        public string m_onGroundSound;

        public void LaunchLightAttackSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_lightAttackSound);
        }

        public void LaunchHeavyAttackSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_heavyAttackSound);
        }

        public void LaunchSpecialAttackSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_specialAttackSound);
        }

        public void LaunchJumpSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_jumpSound);
        }

        public void LaunchDashSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_dashSound);
        }

        public void LaunchTakeDamageSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_takeDamageSound);
        }

        public void LaunchDeathSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_deathSound);
        }

        public void LaunchReviveSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_reviveSound);
        }

        public void LaunchOnGroundSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_onGroundSound);
        }
    }
}
