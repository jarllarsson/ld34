using UnityEngine;
using System.Collections;

public class CamMover : MonoBehaviour 
{
    public float m_mouseMoveRotateMultiplier = 10.0f;
    public float m_mouseMovePlaceMultiplier = 10.0f;
    public float m_keyMovePlaceMultiplier = 10.0f;
    public float m_mouseMoveZoomMultiplier = 10.0f;
    public Vector2 m_xRotateLims = new Vector2(300, 70);
    public bool isDragging = false;
    private float m_dragStatCoolDown = 0.4f;
    private float m_dragStatCoolDownTick = 0.0f;

    public float m_rayCastOffset = 100.0f;
    public float m_rayCastBound = 100.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
        InputHandling();
        GroundCollideHandling();
	}


    void GroundCollideHandling()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(new Ray(transform.position + Vector3.up * m_rayCastOffset, Vector3.down), out rayHit))
        {
            if (transform.position.y < rayHit.point.y + m_rayCastBound)
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


    void InputHandling()
    {
        Vector3 mouseInput = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse ScrollWheel"));
        Vector3 keyInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        // Rotate
        if (Input.GetAxis("RightClick") > 0.0f)
        {
            if (Mathf.Abs(mouseInput.x) > 0.02f || Mathf.Abs(mouseInput.y) > 0.02f)
            {
                isDragging = true;
                m_dragStatCoolDownTick = m_dragStatCoolDown;
            }
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(-mouseInput.y, mouseInput.x, 0.0f) * m_mouseMoveRotateMultiplier);
            // Clamp
            if (transform.rotation.eulerAngles.x > m_xRotateLims.y && transform.rotation.eulerAngles.x < 180.0f)
                transform.rotation = Quaternion.Euler(m_xRotateLims.y, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            if (transform.rotation.eulerAngles.x < m_xRotateLims.x && transform.rotation.eulerAngles.x > 180.0f)
                transform.rotation = Quaternion.Euler(m_xRotateLims.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else
        {
            m_dragStatCoolDownTick -= Time.deltaTime;
            if (m_dragStatCoolDownTick < 0.0f)
            {
                isDragging = false;
            }
        }
        // Move
        if (Input.GetAxis("MiddleClick") > 0.0f)
        {
            transform.position += transform.right * -mouseInput.x * m_mouseMovePlaceMultiplier + transform.up * -mouseInput.y * m_mouseMovePlaceMultiplier;
        }
        transform.position += transform.right * keyInput.x * m_keyMovePlaceMultiplier + transform.forward * keyInput.y * m_keyMovePlaceMultiplier;
        // Zoom
        transform.position += transform.forward * mouseInput.z * m_mouseMoveZoomMultiplier;
    }
}
