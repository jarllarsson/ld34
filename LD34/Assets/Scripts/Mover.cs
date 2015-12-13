using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
    public Vector2 m_dir;
    public float m_speed;
    public bool m_avoidWater = true;
    public LayerMask m_myLayer;
    public float m_avoidDist = 10.0f;
    public float m_waterAvoidTime = 1.0f;
    private float m_waterAvoidTimer = 0.0f;
    public bool m_loiter = false;
    public bool m_enabled = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_enabled)
        {
            if (m_loiter)
            {
                if (Random.Range(0, 1000) > 990)
                {
                    m_dir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                }
                else if (Random.Range(0, 2000) > 1990)
                {
                    m_dir = Vector2.zero;
                }
            }
            Move();
            Debug.DrawLine(transform.position, transform.position + new Vector3(m_dir.x, 0.0f, m_dir.y)*10.0f, Color.magenta);

            if (m_waterAvoidTimer > 0.0f) m_waterAvoidTimer -= Time.deltaTime;
            if (m_avoidWater && m_waterAvoidTimer <= 0.0f) AvoidWater();
        }
        else
        {
            m_dir = Vector3.zero;
        }
	}

    private void Move()
    {
        transform.position += new Vector3(m_dir.x, 0.0f, m_dir.y) * Time.deltaTime * m_speed;
    }

    public void SetTarget(Vector3 p_pos)
    {
        Vector3 cdir = p_pos - transform.position;
        m_dir = new Vector2(cdir.x, cdir.z);
        m_dir.Normalize();
    }

    public void SetTarget(Transform p_target)
    {
        SetTarget(p_target.position);
    }

    void AvoidWater()
    {
        RaycastHit rayHit;
        Vector3 dir = new Vector3(m_dir.x, 0.0f, m_dir.y);

        if (Physics.Raycast(new Ray(transform.position + Vector3.up + dir * m_avoidDist, Vector3.down + dir * m_avoidDist), out rayHit, Mathf.Infinity, ~m_myLayer))
        {
            if (rayHit.transform.tag == "Water")
            {
                Vector3 cdir = transform.position - rayHit.point;
                m_dir = new Vector2(cdir.x, cdir.z);
                m_dir.Normalize();
                Move();
                m_waterAvoidTimer = m_waterAvoidTime;
            }
        }
    }
}
