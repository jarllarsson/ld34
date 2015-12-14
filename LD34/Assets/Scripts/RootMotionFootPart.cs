using UnityEngine;
using System.Collections;

public class RootMotionFootPart : MonoBehaviour 
{
    public Transform m_referenceFrame;
    public Vector2 m_currentDelta;
    private Vector3 m_oldPos;

    public float m_rayCastOffset = 100.0f;
    public float m_rayCastBound = 100.0f;
    public LayerMask m_ignoreLayer;
    public bool m_raycast = true;


	// Use this for initialization
	void Start () {
        m_oldPos = m_referenceFrame.InverseTransformPoint(transform.position);
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 delta = m_referenceFrame.InverseTransformPoint(transform.position) - m_oldPos;
        m_currentDelta = new Vector2(delta.x, Mathf.Max(0,-delta.z));

        if (m_raycast) GroundCollideHandling();

        //Debug.DrawLine(m_oldPos, transform.position, Color.green, 0.8f);
        Debug.DrawLine(transform.position, transform.position + new Vector3(m_currentDelta.x, 0.0f, m_currentDelta.y)*5.0f, Color.magenta, 0.8f);

        m_oldPos = m_referenceFrame.InverseTransformPoint(transform.position);
	}


    void GroundCollideHandling()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(new Ray(transform.position + Vector3.up * m_rayCastOffset, Vector3.down), out rayHit, Mathf.Infinity, ~m_ignoreLayer))
        {
            if (transform.position.y < rayHit.point.y + m_rayCastBound)
            {
                Debug.DrawLine(transform.position + Vector3.up * m_rayCastOffset, rayHit.point, Color.green);
                //transform.position = new Vector3(transform.position.x, rayHit.point.y + m_rayCastBound, transform.position.z);
            }
            else
            {
                Debug.DrawLine(transform.position + Vector3.up * m_rayCastOffset, rayHit.point, Color.grey);
            }
        }
    }

}
