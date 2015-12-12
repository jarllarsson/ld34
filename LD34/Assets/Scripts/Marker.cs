using UnityEngine;
using System.Collections;

public class Marker : MonoBehaviour {
    static Marker s_current;
    public Renderer m_renderer;
    private bool m_active;
    private Transform m_target;
    private Vector3 m_position;
	// Use this for initialization
	void Start () {
        s_current = this;
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_renderer.enabled = m_active;
	}

    public bool IsActive()
    {
        return m_active;
    }

    public void Activate(Transform p_transform)
    {
        m_position = Vector3.zero;
        m_target = p_transform;
        m_active = true;
    }

    public void Activate(Vector3 p_pos)
    {
        m_position = p_pos;
        m_target = null;
        m_active = true;
    }

    public void Deactivate()
    {
        m_position = Vector3.zero;
        m_target = null;
        m_active = false;
    }

    public bool HasTarget()
    {
        return m_target != null;
    }

    public bool HasPosition()
    {
        return m_position != Vector3.zero; // yeah, yeah...
    }

    public Transform GetTarget()
    {
        return m_target;
    }

    public Vector3 GetPosition()
    {
        return m_position;
    }

}

