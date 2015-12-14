using UnityEngine;
using System.Collections;

public class MonsterAnimationController : MonoBehaviour 
{
    public Animator m_animator;
    public Mover m_mover;
    public MonsterBehaviour m_mbehaviour;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_animator.SetFloat("monster_speed", m_mover.m_dir.sqrMagnitude);
        m_animator.SetFloat("ani_walk_speed", m_mbehaviour.m_rootMotionControllerSpd);
	}

    public void PlayPickup()
    {
        Debug.Log("PlayPickup!");
        m_animator.SetTrigger("monster_pickup");
    }

    public void EventPickupDone()
    {
        Debug.Log("PickupDone!");
        m_mbehaviour.m_pickupDone = true;
    }

    public void EventAttachPickupTarget()
    {
        Debug.Log("Attach!");
        m_mbehaviour.AttachCurrentlyHoldingToHand();
    }

    public void ResetAnimState()
    {
        m_animator.SetTrigger("monster_reset");
    }


}
