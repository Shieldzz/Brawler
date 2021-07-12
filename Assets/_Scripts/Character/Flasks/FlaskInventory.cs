using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class FlaskInventory : MonoBehaviour
    {

        private Flasks m_firstFlask = null;
        private Flasks m_secondFlask = null;
        private Flasks m_thirdFlask = null;

        public Flasks FirstFlask { get { return m_firstFlask; } set { ReplaceFlask(ref m_firstFlask, value); } }
        public Flasks SecondFlask { get { return m_secondFlask; } set { ReplaceFlask(ref m_secondFlask, value); } }
        public Flasks ThirdFlask { get { return m_thirdFlask; } set { ReplaceFlask(ref m_thirdFlask, value); } }

        private Player m_player = null;

        void Awake()
        {
            m_player = GetComponent<Player>();
        }

        void Update()
        {

        }

        public void EquipeFlaskFromList(Flasks[] flasksList, FlaskManager flaskManager)
        {
            foreach (Flasks flask in flasksList)
            {
                if (!flask)
                    continue;

                dynamic transformFlask = Convert.ChangeType(flask, flask.GetType());
                
                if (!FirstFlask)
                    FirstFlask = transformFlask;
                else if (!SecondFlask)
                    SecondFlask = transformFlask;
                else if (!ThirdFlask)
                    ThirdFlask = transformFlask;

                //if (!FirstFlask)
                //    FirstFlask = flask;
                //else if (!SecondFlask)
                //    SecondFlask = flask;
                //else if (!ThirdFlask)
                //    ThirdFlask = flask;
            }
        }

        public void RemoveFlask(Flasks flask)
        {
            if (!flask)
                return;

            dynamic convertedflask = Convert.ChangeType(flask, flask.GetType());

            //if(flask.CurrentFlaskType == FlaskData.FLASKTYPE.DASH)
            //{
            //    DashFlask dashFlask = flask as DashFlask;
            //    dashFlask.OnUnuseFlask(m_player);
            //}
            //else if (flask.CurrentFlaskType == FlaskData.FLASKTYPE.JUMP)
            //{
            //    JumpHeightBoostDrug jumpFlask = flask as JumpHeightBoostDrug;
            //    jumpFlask.OnUnuseFlask(m_player);
            //}
            //else if (flask.CurrentFlaskType == FlaskData.FLASKTYPE.SPEED)
            //{
            //    SpeedBoostDrug speedFlask = flask as SpeedBoostDrug;
            //    speedFlask.OnUnuseFlask(m_player);
            //}
            //else if (flask.CurrentFlaskType == FlaskData.FLASKTYPE.REVIVE)
            //{
            //    ReviveFlask reviveFlask = flask as ReviveFlask;
            //    reviveFlask.OnUnuseFlask(m_player);
            //}


            convertedflask.OnUnuseFlask(m_player);
            flask = null;
        }

        public void RemoveAllFlasks()
        {
            RemoveFlask(m_firstFlask);
            RemoveFlask(m_secondFlask);
            RemoveFlask(m_thirdFlask);
        }

        private void ReplaceFlask(ref Flasks oldFlask, Flasks newFlask)
        {
            RemoveFlask(oldFlask);
            oldFlask = AddNewFlask(newFlask);
        }

        private Flasks AddNewFlask(Flasks newFlask)
        {
            if (!newFlask)
                return null;

            dynamic flask = Convert.ChangeType(newFlask, newFlask.GetType());

            //if (newFlask.CurrentFlaskType == FlaskData.FLASKTYPE.DASH)
            //{
            //    DashFlask dashFlask = newFlask as DashFlask;
            //    dashFlask.SubscribeToFlask(m_player);
            //    return dashFlask;
            //}
            //else if (newFlask.CurrentFlaskType == FlaskData.FLASKTYPE.JUMP)
            //{
            //    JumpHeightBoostDrug jumpFlask = newFlask as JumpHeightBoostDrug;
            //    jumpFlask.SubscribeToFlask(m_player);
            //    return jumpFlask;
            //}
            //else if (newFlask.CurrentFlaskType == FlaskData.FLASKTYPE.SPEED)
            //{
            //    SpeedBoostDrug speedFlask = newFlask as SpeedBoostDrug;
            //    speedFlask.SubscribeToFlask(m_player);
            //    return speedFlask;
            //}
            //else if (newFlask.CurrentFlaskType == FlaskData.FLASKTYPE.REVIVE)
            //{
            //    ReviveFlask reviveFlask = newFlask as ReviveFlask;
            //    reviveFlask.SubscribeToFlask(m_player);
            //    return reviveFlask;
            //}

            flask.SubscribeToFlask(m_player);
            return newFlask;
        }

        public float GetFullNocivity()
        {
            float nocivity = 0f;

            if (m_firstFlask)
                nocivity += m_firstFlask.Nocivity;
            if (m_secondFlask)
                nocivity += m_secondFlask.Nocivity;
            if (m_thirdFlask)
                nocivity += m_thirdFlask.Nocivity;

            return nocivity;
        }
    }
}