using UnityEngine;
using System.Collections;

public class VillagerController : MonoBehaviour
{
    public Animator m_animator;

    public enum Sex
    {
        MAN = 0,
        WOMAN = 1
    }

    public Sex m_sex;
    public int m_age = 1;
    private float m_ager = 0.0f;
    private static float s_adultAgeSeconds = 120.0f;
    private static float s_oldAgeSeconds = 300.0f;
    private float m_dieAgeSeconds = 600.0f;
    private Mover m_mover;
    private bool m_isWalkingAnim = false;
    public Transform m_billboard;
    private bool m_isThrown = false;
    private Rigidbody m_opRb = null;
    public Transform m_house;

    public static int s_villagers = 0;
    private static int s_houseBuilts = 5;
    private static int s_houseSpace = 2;
    public LayerMask m_houseMask;
    public LayerMask m_terrainMask;

    private bool isPickedUp = false;
    bool isBorn = false;

	// Use this for initialization
	void Start () 
    {
        m_sex = Random.Range(0, 2) == 0 ? Sex.MAN : Sex.WOMAN;
        m_animator.SetInteger("villager_sex", (int)m_sex);
        m_dieAgeSeconds += Random.Range(-400.0f, 200.0f);
        s_villagers++;
        m_mover = GetComponent<Mover>();
	}
	
    void setup()
    {
        if (!isBorn)
        {
            if (m_age == 1) m_ager = s_adultAgeSeconds + Random.Range(0, 100);
            updateAnimAge();
            isBorn = true;
        }
    }

	// Update is called once per frame
	void Update () 
    {
        setup();

        //Debug.Log(s_villagers);
        if (Random.Range(0, 1000) > 990 && s_villagers - (s_houseBuilts*s_houseSpace) > s_houseSpace)
        {
            Debug.Log("TryTOBUild");
            bool notOnHouse = true;
            RaycastHit rayHit = new RaycastHit();
            if (Physics.SphereCast(new Ray(transform.position + Vector3.up * 100.0f, Vector3.down), 5.0f, out rayHit, Mathf.Infinity, m_houseMask.value))
            {
                notOnHouse = false;
                Debug.Log("Can't build on house, when on house");
            }

            if (notOnHouse)
            {
                Debug.Log("buildHouse");
                s_houseBuilts++;
                if (Physics.Raycast(new Ray(transform.position + Vector3.up * 100.0f, Vector3.down), out rayHit, Mathf.Infinity, m_terrainMask.value))
                {
                    Instantiate(m_house, rayHit.point, Quaternion.Euler(-90, Random.Range(0.0f, 360.0f), 0.0f));
                    Debug.Log("Put down on ground");
                }
                else
                    Instantiate(m_house, transform.position, Quaternion.Euler(-90, Random.Range(0.0f,360.0f), 0.0f));
            }
        }


        m_ager += Time.deltaTime;
        if (m_ager >= s_adultAgeSeconds && m_age == 0)
        {
            m_age++;
            updateAnimAge();
        }
        if (m_ager >= s_oldAgeSeconds & m_age == 1)
        {
            m_age++;
            updateAnimAge();
        }

        if (!m_isThrown)
        {
            if (m_ager >= m_dieAgeSeconds)
            {
                Die();
            }

            // animation for walk
            bool isWalking = m_mover.m_dir.SqrMagnitude() > 0.0f;
            if (isWalking)
            {
                if (!m_isWalkingAnim)
                {
                    m_animator.SetBool("villager_walk", true);
                    m_isWalkingAnim = true;
                }
                // facing
                if (m_billboard)
                {
                    Vector3 lDir = m_billboard.InverseTransformDirection(new Vector3(m_mover.m_dir.x, 0.0f, m_mover.m_dir.y));
                    Debug.DrawLine(transform.position, transform.position + lDir, Color.blue);
                    if (lDir.x < 0.0f)
                    {
                        m_billboard.localScale = Vector3.one;
                    }
                    else if (lDir.x > 0.0f)
                    {
                        m_billboard.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    }
                }
            }
            else
            {
                if (m_isWalkingAnim)
                {
                    m_animator.SetBool("villager_walk", false);
                    m_isWalkingAnim = false;
                }
            }
        }
        else
        {
            if (m_opRb)
            {
                if (m_opRb.IsSleeping() || m_opRb.velocity.magnitude < 0.3f)
                {
                    Debug.Log("GetUp!");
                    Destroy(m_opRb);
                    m_isThrown = false;
                    PutDown();
                    gameObject.tag = "Untagged";
                }
            }
            else
            {
                m_opRb = GetComponent<Rigidbody>();
            }
        }


	}

    private void updateAnimAge()
    {
        m_animator.SetInteger("villager_age", m_age);
    }

    public void Die()
    {
        s_villagers--;
        Destroy(gameObject);
    }

    public void PickedUp()
    {
        if (m_mover) m_mover.m_enabled = false;
        LandCollider collider = GetComponent<LandCollider>();
        if (collider) collider.m_enabled = false;
        isPickedUp = true;
    }
    public void Thrown()
    {
        m_isThrown = true;
        m_opRb = GetComponent<Rigidbody>();
        PickedUp();
    }

    public bool IsPickedUp()
    {
        return isPickedUp;
    }

    public void PutDown()
    {
        if (m_mover) m_mover.m_enabled = true;
        LandCollider collider = GetComponent<LandCollider>();
        if (collider) collider.m_enabled = true;
        isPickedUp = false;
    }
}
