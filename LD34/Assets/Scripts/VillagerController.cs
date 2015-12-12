using UnityEngine;
using System.Collections;

public class VillagerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PickedUp()
    {
        Mover mover = GetComponent<Mover>();
        if (mover) mover.m_enabled = false;
        LandCollider collider = GetComponent<LandCollider>();
        if (collider) collider.m_enabled = false;
    }

    public void PutDown()
    {
        Mover mover = GetComponent<Mover>();
        if (mover) mover.m_enabled = true;
        LandCollider collider = GetComponent<LandCollider>();
        if (collider) collider.m_enabled = true;
    }
}
