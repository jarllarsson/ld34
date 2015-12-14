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
}
