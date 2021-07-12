
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BTA
{

    public enum CrowdControl
    {
        None = 0,
        Stagger = 1,
        KnockUp,
        KnockDown,
        KnockBack,
        SlamDown,
        Expell
    }

    [Serializable]
    public struct AttackData
    {
        //public int ID;
        public int damage;
        //public float connectDuration;
        public CrowdControl CC;
        public float CCForce;
        public bool hasFX;  // could be change to being a Enum, so that FX could spawn at the beginning or at the impact
        public GameObject gaoFX;

#if UNITY_EDITOR
        public void Display()
        {
            GUILayout.BeginVertical("box");
            {
                //ID = EditorGUILayout.IntField("ID", ID);
                damage = EditorGUILayout.IntField("Damage", damage);
                //connectDuration = EditorGUILayout.FloatField("Connect", connectDuration);

                CC = (CrowdControl)EditorGUILayout.EnumPopup("Control Effect", CC);
                if (CC > CrowdControl.Stagger)
                    CCForce = EditorGUILayout.FloatField("Control Force", CCForce);

                hasFX = EditorGUILayout.Toggle("Spawn Special Effect", hasFX);
                if (hasFX)
                    gaoFX = EditorGUILayout.ObjectField("Special Effect", gaoFX, typeof(GameObject), false) as GameObject;
            }
            GUILayout.EndVertical();
        }
#endif
    }


    [CreateAssetMenu()]
    public class AttackStruct : ScriptableObject
    {

        public AttackData[] attackDatas = new AttackData[1];


#if UNITY_EDITOR
        virtual public void Display()
        {
            //Manage Both Array -> Damage Line & Connect Duration
            GUILayout.Label("Damage", EditorStyles.boldLabel);
            System.Array.Resize<AttackData>(ref attackDatas, EditorGUILayout.IntSlider("Number of Damage Line", attackDatas.Length, 1, 10));

            // Damages
            int totalDamage = 0;

            int size = attackDatas.Length;
            {
                for (int i = 0; i < size; i++)
                {
                    attackDatas[i].Display();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    totalDamage += attackDatas[i].damage;
                }
            }
            GUILayout.Label("Total Damage => " + totalDamage, EditorStyles.boldLabel);
        }
#endif

    }
}

