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
    public Transform m_danceAroundTarget = null;
    public RootMotionController m_rootMotionController = null;
    private static float s_danceController;
    private static bool s_danceUpdated = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_enabled)
        {
            if (!m_danceAroundTarget)
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
            }
            else
            {
                // if dance target, go to and dance
                Vector3 diff = new Vector3(transform.position.x,0.0f,transform.position.z) - new Vector3(m_danceAroundTarget.position.x, 0.0f, m_danceAroundTarget.position.z);
                if (Vector3.Magnitude(diff) > 2.5f) // size of circle
                {
                    m_dir = -new Vector2(diff.x, diff.z).normalized;
                }
                else if (Vector3.Magnitude(diff) < 2.4f) // too far in
                {
                    m_dir = new Vector2(1.0f, 0.0f);
                }
                else
                {
                    // DANCE!
                    if (!s_danceUpdated)
                    {
                        s_danceUpdated = true;
                        s_danceController = Mathf.Sin(Time.time);
                    }
                    if (s_danceController < -0.2f)
                    {
                        m_dir = new Vector2(diff.z, -diff.x).normalized;
                    }
                    else if (s_danceController > 0.2f)
                    {
                        m_dir = -new Vector2(diff.z, -diff.x).normalized;
                    }
                    else
                    {
                        m_dir = Vector2.zero;
                    }
                }
            }
            if (m_dir.sqrMagnitude > 0)
            {
                Move();
            }



            Debug.DrawLine(transform.position, transform.position + new Vector3(m_dir.x, 0.0f, m_dir.y)*10.0f, Color.magenta);

            if (m_waterAvoidTimer > 0.0f) m_waterAvoidTimer -= Time.deltaTime;
            if (m_avoidWater && m_waterAvoidTimer <= 0.0f) AvoidWater();
        }
        else
        {
            m_dir = Vector3.zero;
        }
	}

    void LateUpdate()
    {
     s_danceUpdated = false;
    }

    private void Move()
    {
        if (!m_rootMotionController)
            transform.position += new Vector3(m_dir.x, 0.0f, m_dir.y) * Time.deltaTime * m_speed;
        else
        {
            transform.position += m_rootMotionController.m_deltaTotal;
        }
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
