using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarFader : MonoBehaviour 
{
    public Image m_image;
    public Color m_pos, m_neg;
    public RectTransform m_transform;
    public MonsterBehaviour m_monster;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_transform.localScale = new Vector3(m_monster.m_interactMeter * 200.0f, m_transform.localScale.y, m_transform.localScale.z);
	    m_image.color = Color.Lerp(m_neg, m_pos, (m_transform.localScale.x + 200.0f) / 400.0f);
	}
}
