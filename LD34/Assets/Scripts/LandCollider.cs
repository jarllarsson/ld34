using UnityEngine;
using System.Collections;

public class LandCollider : MonoBehaviour 
{
    public float m_rayCastOffset = 100.0f;
    public float m_rayCastBound = 100.0f;
    public bool m_lockToGround = false;
    public LayerMask m_ignoreLayer;
    public bool m_enabled = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	
    }

    void LateUpdate()
    {
        if (m_enabled) GroundCollideHandling();
    }


    void GroundCollideHandling()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(new Ray(transform.position + Vector3.up * m_rayCastOffset, Vector3.down), out rayHit, Mathf.Infinity, ~m_ignoreLayer))
        {
            if (m_lockToGround || transform.position.y < rayHit.point.y + m_rayCastBound)
            {
                Debug.DrawLine(transform.position + Vector3.up * m_rayCastOffset, rayHit.point, Color.green);
                transform.position = new Vector3(transform.position.x, rayHit.point.y + m_rayCastBound, transform.position.z);
            }
            else
            {
                Debug.DrawLine(transform.position + Vector3.up * m_rayCastOffset, rayHit.point, Color.grey);
            }
        }
    }


}
