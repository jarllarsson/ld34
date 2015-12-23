using UnityEngine;
using System.Collections;

public class RootMotionController : MonoBehaviour 
{
    public RootMotionFootPart[] m_footParts;
    public Vector3 m_deltaTotal;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 deltaTotal = Vector2.zero;
        foreach (RootMotionFootPart part in m_footParts)
        {
            deltaTotal += part.m_currentDelta;
        }
        m_deltaTotal = transform.TransformVector(new Vector3(/*deltaTotal.x*/0.0f, 0.0f, deltaTotal.y));
	}
}
