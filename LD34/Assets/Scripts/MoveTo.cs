using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {
    public float m_time;
    public float m_maxSpeed = 1.0f;
    public Transform m_target;
    private Vector3 m_velocity;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        if (m_time > 0.0f)
            transform.position = Vector3.SmoothDamp(transform.position, m_target.position, ref m_velocity, m_time, m_maxSpeed);
        else
            transform.position = m_target.position;
	}
}
