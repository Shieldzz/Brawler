using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BTA
{
    [Serializable]
    public struct TextBlock
    {
        public CharacterEnum talkingCharacter;
        public string text;

        public bool hasSound;
        [FMODUnity.EventRef]
        public string sound;

#if UNITY_EDITOR
        public void Display()
        {
            GUILayout.BeginVertical("box");
            {
                talkingCharacter = (CharacterEnum)EditorGUILayout.EnumPopup("Talking Character", talkingCharacter);

                EditorGUILayout.LabelField("Dialogue :");
                text = EditorGUILayout.TextArea(text);

                hasSound = EditorGUILayout.Toggle("VoiceLine",hasSound);
                if (hasSound)
                    sound = EditorGUILayout.TextField("Sound Name", sound);
            }
            GUILayout.EndVertical();
        }
#endif
    }


    [CreateAssetMenu()]
    public class NarrativeBlock : ScriptableObject
    {

        public TextBlock[] TextBlockArray = new TextBlock[1];


#if UNITY_EDITOR
        public void Display()
        {

            System.Array.Resize<TextBlock>(ref TextBlockArray, EditorGUILayout.IntSlider("Number of Dialogues", TextBlockArray.Length, 1, 50));
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            for (int i = 0; i < TextBlockArray.Length; i++)
            {
                EditorGUILayout.LabelField("Dialogue Box n°" + (i + 1), EditorStyles.boldLabel);
                GUILayout.Space(10f);
                TextBlockArray[i].Display();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }
#endif
    }
}
