using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFXHandler : MonoBehaviour
{
    [SerializeField] AFXHandler[] m_FX = new AFXHandler[0];

    private void ProcFX(int id)
    {
        if (id > m_FX.Length)
            return;

        AFXHandler currFX = m_FX[id];
        if (currFX)
        {
            currFX.Proc();
        }
    }

    private void DisableFX(int id)
    {
        if (id > m_FX.Length)
            return;

        if (m_FX[id])
            m_FX[id].gameObject.SetActive(false);
    }
}
