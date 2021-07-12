using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlaskEquipementUI : MonoBehaviour {

    public GameObject m_flask1 = null;
    public GameObject m_flask2 = null;
    public GameObject m_flask3 = null;

    public GameObject m_buttonSample = null;
    public GameObject m_content = null;


    void Start () {
        GameObject button = Instantiate(m_buttonSample);
        button.transform.SetParent(m_content.transform);
	}
	
	void Update () {
	
        
	}
}
