using UnityEngine;
using System.Collections;

public class SpawnVillager : MonoBehaviour 
{
    public Transform m_villager;
    private float m_ticker = 120.0f;
    public bool m_spawnNow = false;
	// Use this for initialization
	void Start () 
    {
        m_ticker = Random.Range(20.0f, 240.0f);
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_ticker -= Time.deltaTime;
        if (m_spawnNow || m_ticker < 0.0f || VillagerController.s_villagers < 5)
        {
            m_ticker = Random.Range(40.0f,240.0f);
            Transform obj = Instantiate(m_villager, transform.position, Quaternion.identity) as Transform;
            VillagerController controller = obj.GetComponent<VillagerController>();
            if (controller)
            {
                controller.m_age = 0;
            }
            m_spawnNow = false;
        }
	}
}
