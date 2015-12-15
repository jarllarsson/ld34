using UnityEngine;
using System.Collections;

public class timekill : MonoBehaviour {
    public float m_lim;
    private float m_tick = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        m_tick += Time.deltaTime;
	    if (m_tick > m_lim)
        {
            Destroy(gameObject);
        }
	}
}
