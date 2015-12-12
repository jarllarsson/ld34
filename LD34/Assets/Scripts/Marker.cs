using UnityEngine;
using System.Collections;

public class Marker : MonoBehaviour {
    public static Marker s_current;
    public MonsterBehaviour m_monsterBehaviour;
    public Renderer m_renderer;
    private bool m_active = false;
    private Transform m_target;
    private Vector3 m_position;

    private Vector3 m_scale;
    private float m_scaling = 0.0f;

	// Use this for initialization
	void Start () {
        s_current = this;
        m_scale = transform.localScale;
        transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_active)
        {
            if (m_target)
            {
                transform.position = m_target.position;
            }
            else if (m_position != Vector3.zero)
            {
                transform.position = m_position;
            }
            if (m_scaling < 1.0f) m_scaling += Time.deltaTime * 8.0f; else m_scaling = 1.0f;
            transform.localScale = Vector3.Lerp(Vector3.zero, m_scale, m_scaling);
        }
        m_renderer.enabled = m_active;
        if (m_active && m_target == null && m_position == Vector3.zero)
        {
            m_active = false;
        }
        if (!m_active)
        {
            reset();
        }
	}

    void reset()
    {
        m_scaling = 0.0f;
        transform.localScale = Vector3.zero;
    }

    public bool IsActive()
    {
        return m_active;
    }

    public void Activate(Transform p_transform)
    {
        reset();
        m_position = Vector3.zero;
        m_target = p_transform;
        m_active = true;
        m_monsterBehaviour.SetNewWalkTarget();
    }

    public void Activate(Vector3 p_pos)
    {
        reset();
        m_position = p_pos;
        m_target = null;
        m_active = true;
        m_monsterBehaviour.SetNewWalkTarget();
    }

    public void Deactivate()
    {
        reset();
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

