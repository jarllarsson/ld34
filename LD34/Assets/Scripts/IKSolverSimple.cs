using UnityEngine;
using System.Collections;

public class IKSolverSimple : MonoBehaviour 
{
    //[HideInInspector]
    public Transform m_owner;
    public Transform m_upperLeg,m_lowerLeg,m_foot;
    float uB = 2.5f; // the length of the legs
    float lB = 2.0f;
    float m_footOffset = 3.0f;
    
    public float m_hipAngle;
    public float m_kneeAngle;

    public Vector3 m_hipPos;
    public Vector3 m_kneePos;
    public Vector3 m_endPos;
    public Vector3 m_kneePosW;
    public int m_flip = 1;
    

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        calculate();
	}

    public void calculate()
    {
        int kneeFlip = m_flip;

        // Vector between foot and hip
        // This ik calc is in 2d, so eliminate rotation
        Vector3 footToHip = m_owner.InverseTransformDirection(m_upperLeg.localPosition - m_foot.localPosition - new Vector3(0.0f, m_footOffset, 0.0f));
        
        //Debug.DrawLine(m_upperLeg.position, m_upperLeg.position+topToFoot,Color.black);


        float toFootLen = footToHip.magnitude;
        //Debug.DrawLine(m_foot.position, m_foot.position + footToHip, Color.red);

        float upperLegAngle = 0.0f;
        float lowerLegAngle = 0.0f;

        //Debug.DrawLine(m_foot.position - Vector3.up * 0.1f, m_foot.position + (uB + lB) * footToHip.normalized - Vector3.up * 0.1f, Color.blue);
        //Debug.Log(uB);
        // first get offset angle beetween foot and axis
        float offsetAngle = Mathf.Atan2(footToHip.y, footToHip.z);
        // If dist to foot is shorter than combined leg length
        //bool straightLeg = false;
        if (toFootLen < uB + lB)
        {
            float uBS = uB * uB;
            float lBS = lB * lB;
            float hBS = toFootLen * toFootLen;
            // law of cosines for first angle
            upperLegAngle = (float)(kneeFlip) * Mathf.Acos((hBS + uBS - lBS) / (2.0f * uB * toFootLen)) + offsetAngle;
            // second angle
            Vector2 newLeg = new Vector2(uB * Mathf.Cos(upperLegAngle), uB * Mathf.Sin(upperLegAngle));

            m_kneePos = new Vector3(-newLeg.x, -newLeg.y, 0.0f);

            lowerLegAngle = Mathf.Atan2(footToHip.y - newLeg.y, footToHip.z - newLeg.x) - upperLegAngle;
        }
        else // otherwise, straight leg
        {
            float oldLen=uB+lB;
            // emergency stretch
            uB = lB = toFootLen * 0.5f;
           // m_upperLeg.localScale = new Vector3(oldLen / toFootLen, uB, 1.0f);
           // m_lowerLeg.localScale = new Vector3(oldLen / toFootLen, lB, 1.0f);
            upperLegAngle = offsetAngle;

            Vector2 newLeg = new Vector2(uB * Mathf.Cos(upperLegAngle), uB * Mathf.Sin(upperLegAngle));

            m_kneePos = new Vector3(-newLeg.x, -newLeg.y, 0.0f);


            lowerLegAngle = 0.0f;
        }
        float lowerAngleW = upperLegAngle + lowerLegAngle;


        m_hipAngle = upperLegAngle;
        m_kneeAngle = lowerLegAngle;



        //m_upperLeg.position = m_hipPos;
        //m_lowerLeg.position = m_hipPos+m_kneePos;
        m_upperLeg.localRotation = Quaternion.AngleAxis(Mathf.Rad2Deg * (upperLegAngle + Mathf.PI * 0.5f), Vector3.right);
        m_lowerLeg.localRotation = Quaternion.AngleAxis(Mathf.Rad2Deg * (m_kneeAngle + Mathf.PI * 0.5f), Vector3.right);



       

    }
}
