using UnityEngine;
using System.Collections;

public class CamMover : MonoBehaviour 
{
    public float m_mouseMoveRotateMultiplier = 10.0f;
    public float m_mouseMovePlaceMultiplier = 10.0f;
    public float m_keyMovePlaceMultiplier = 10.0f;
    public float m_mouseMoveZoomMultiplier = 10.0f;
    public Vector2 m_xRotateLims = new Vector2(300, 70);
    public bool isRightMouseButtonDragging = false;
    private float m_dragStatCoolDown = 0.2f;
    private float m_dragStatCoolDownTick = 0.0f;
    public Camera m_camera;
    public MonsterBehaviour m_monster;
    public bool m_camMovingToTarget;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
        m_camMovingToTarget = false;
        if (!m_monster.m_interact)
            InputHandling();
        else
        {
            transform.position = Vector3.Lerp(transform.position, m_monster.m_cameraInteractTarget.position, 5.0f*Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, m_monster.m_cameraInteractTarget.rotation, 5.0f * Time.deltaTime);
            m_camMovingToTarget = Vector3.SqrMagnitude(transform.position - m_monster.m_cameraInteractTarget.position) > 0.1f;
            m_camMovingToTarget = m_camMovingToTarget && Quaternion.Angle(transform.rotation, m_monster.m_cameraInteractTarget.rotation) > 0.1f;
        }

        if (transform.position.y > 98.0f)
        {
            m_camera.nearClipPlane = 10.0f;
        }
        else
        {
            m_camera.nearClipPlane = 1.0f;
        }

        m_monster.m_waitForCamera = m_camMovingToTarget;
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
                isRightMouseButtonDragging = true;
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
                isRightMouseButtonDragging = false;
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
