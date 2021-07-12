using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXSpawner : AFXHandler
{
    [SerializeField] GameObject m_FX;

    public override void Proc()
    {
        GameObject gao = GameObject.Instantiate(m_FX);
        gao.SetActive(true);
        gao.transform.position = transform.position;
        gao.transform.rotation = transform.rotation;
    }
}
