using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu()]
[Serializable]
public class LevelData : ScriptableObject{

    public string m_contractNumber;
    public string m_levelName;
    public string m_requester;
    public string m_levelDescription;
    public Sprite m_levelPreview;

    public string m_sceneName;
}
