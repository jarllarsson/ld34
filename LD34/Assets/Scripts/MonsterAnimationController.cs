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


    // Pickup
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


    // Hold
    public void PlayHold()
    {
        Debug.Log("PlayHold!");
        m_animator.SetTrigger("monster_hold");
    }


    // Eat
    public void PlayEat()
    {
        Debug.Log("PlayEat!");
        m_animator.SetTrigger("monster_eat");
    }

    public void EventEatDone()
    {
        Debug.Log("EatDone!");
        m_mbehaviour.m_holdEndAnimDone = true;
    }

    public void EventKillPickupTarget()
    {
        Debug.Log("eatup kill!");
        m_mbehaviour.KillCurrentlyHoldingToHand();
    }

    // Throw
    public void PlayThrow()
    {
        Debug.Log("PlayThrow!");
        m_animator.SetTrigger("monster_throw");
    }

    public void EventThrowDone()
    {
        Debug.Log("ThrowDone!");
        m_mbehaviour.m_holdEndAnimDone = true;
    }

    public void EventThrowPickupTarget()
    {
        Debug.Log("throw!");
        m_mbehaviour.ThrowCurrentlyHoldingToHand();
    }


    // Putdown
    public void PlayPutdown()
    {
        Debug.Log("PlayPutdown!");
        m_animator.SetTrigger("monster_putdown");
    }

    public void EventPutdownDone()
    {
        Debug.Log("PutdownDone!");
        m_mbehaviour.m_holdEndAnimDone = true;
    }

    public void EventDetachPickupTarget()
    {
        Debug.Log("Detach!");
        m_mbehaviour.DetachCurrentlyHoldingToHand();
    }





    public void ResetAnimState()
    {
        m_animator.SetTrigger("monster_reset");
    }


}
