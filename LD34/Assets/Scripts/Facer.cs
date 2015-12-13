using UnityEngine;
using System.Collections;

public class Facer : MonoBehaviour {
    public Mover m_mover;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_mover.m_dir.sqrMagnitude > 0.0f)
            transform.forward = new Vector3(m_mover.m_dir.x, 0.0f, m_mover.m_dir.y);
	}
}
