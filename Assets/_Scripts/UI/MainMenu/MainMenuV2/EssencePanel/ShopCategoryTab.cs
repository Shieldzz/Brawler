using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCategoryTab : MonoBehaviour {

    [SerializeField] private Sprite m_basicSprite = null;
    [SerializeField] private Sprite m_selectedSprite = null;

    private Image m_image = null; 

	// Use this for initialization
	void Start () {
        m_image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTabSelected()
    {
        m_image.sprite = m_selectedSprite;
    }
    public void OnTabUnselected()
    {
        m_image.sprite = m_basicSprite;
    }
}
