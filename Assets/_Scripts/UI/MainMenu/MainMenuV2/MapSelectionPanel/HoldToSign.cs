using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BTA
{

    public class HoldToSign : HoldTo
    {

        // Use this for initialization
        override protected void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        override protected void Update()
        {
            base.Update();
        }

        public override void HoldComplete()
        {
            base.HoldComplete();
            GetComponent<Button>().OnSubmit(new BaseEventData(EventSystem.current));
        }
    }

}