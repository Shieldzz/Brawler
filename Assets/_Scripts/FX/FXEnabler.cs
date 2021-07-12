using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXEnabler : AFXHandler
{

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public override void Proc()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
