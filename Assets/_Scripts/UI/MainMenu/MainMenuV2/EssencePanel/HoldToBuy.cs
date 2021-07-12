using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class HoldToBuy : HoldTo
    {

        override protected void Start()
        {
            base.Start();
        }

        override protected void Update()
        {
            base.Update();
        }

        public override void HoldComplete()
        {
            base.HoldComplete();
            GetComponent<BuyButton>().CustonSubmitCaller();
        }
    }

}