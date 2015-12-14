using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MonsterBehaviour : MonoBehaviour 
{
	public Mover m_mover;
	public enum MonsterAction
	{
		Walk,
		Pickup,
		HoldWaitForInput,
		Eat,
		Throw,
		PutDown
	}

	public class ActionTreeNode
	{
		public MonsterAction m_actionType;
		public List<ActionTreeNode> m_children = new List<ActionTreeNode>();
		public List<float> m_weigths = new List<float>();
		public void AddWeigth(float p_w, int p_child)
		{
			float old = m_weigths[p_child];
			m_weigths[p_child] += p_w;
			m_weigths[p_child] = Mathf.Clamp01(m_weigths[p_child]);

			if (m_weigths.Count > 1)
			{
				float diff = m_weigths[p_child] - old;
				diff /= m_weigths.Count - 1;
				for (int i = 0; i < m_weigths.Count; i++ )
				{
					if (i != p_child)
					{
						m_weigths[i] -= diff;
						m_weigths[i] = Mathf.Clamp01(m_weigths[i]);
					}
				}
			}

		}
	}

	public Queue<MonsterAction> m_actionQueue = new Queue<MonsterAction>();
	private ActionTreeNode m_decisionTreeRoot = new ActionTreeNode();

	public Text m_dbgTextbox;
	public float m_inputWaitTime = 4.0f;
	private float m_inputWaitTimer = 0.0f;
	private Transform m_currentlyHolding;
    public MonsterAnimationController m_animationController;

    public float m_rootMotionControllerSpd = 1.0f;
    public Transform m_attachHand;

    public bool m_pickupDone = false;

	// Use this for initialization
	void Start () 
	{
		m_actionQueue.Enqueue(MonsterAction.Walk);
		SetupDecisionTree();
	}
	
	// Update is called once per frame
	void Update () 
	{
        m_rootMotionControllerSpd = 1.0f;
		if (m_actionQueue.Count > 0)
		{
			MonsterAction currentAction = m_actionQueue.Peek();
			bool dequeue = false;

			m_mover.m_enabled = false;


			switch (currentAction)
			{
				case MonsterAction.Walk:
					{
						dequeue = WalkAction();
						break;
					}
				case MonsterAction.Pickup:
					{
						dequeue = PickupAction();
						break;
					}
				case MonsterAction.HoldWaitForInput:
					{
						dequeue = HoldAndWait();
						break;
					}
				case MonsterAction.Eat:
					{
						if (m_currentlyHolding)
						{
							Debug.Log("EAT!!!! nomnom");
							Destroy(m_currentlyHolding.gameObject);
						}
						dequeue = true;
						break;
					}
				case MonsterAction.Throw:
					{
						if (m_currentlyHolding)
						{
							Debug.Log("Throw!!!!");
							Destroy(m_currentlyHolding.gameObject);
						}
						dequeue = true;
						break;
					}
				case MonsterAction.PutDown:
					{
						if (m_currentlyHolding)
						{
							Debug.Log("Away you go..");
							VillagerController controller = m_currentlyHolding.GetComponent<VillagerController>();
							if (controller)
							{
								controller.transform.parent = null;
								controller.PutDown();
							}
						}
						dequeue = true;
						break;
					}
			}



			if (dequeue)
			{
				m_actionQueue.Dequeue();
			}

			MonsterAction[] dbgList = m_actionQueue.ToArray();
			m_dbgTextbox.text = "";
			for (int i = 0; i < dbgList.Length; i++)
			{
				m_dbgTextbox.text += (i + " " + dbgList[i].ToString()+"\n");
			}
		}
		else
		{
            m_animationController.ResetAnimState();
			m_actionQueue.Enqueue(MonsterAction.Walk);
		}

	}

    bool PickupAction()
	{
		if (m_currentlyHolding)
		{
			VillagerController controller = m_currentlyHolding.GetComponent<VillagerController>();
            if (!controller.IsPickedUp())
            {
                if (controller) controller.PickedUp();
                m_animationController.PlayPickup(); // will trigger attach and next state
                m_actionQueue.Enqueue(MonsterAction.HoldWaitForInput);
                m_inputWaitTimer = m_inputWaitTime;
                Debug.Log("Picked up " + m_currentlyHolding.name + "successfully");
            }
            if (m_pickupDone)
                return true; // wait for animation here
            else
                return false;
		}
		else
		{
			return true;
		}
	}

    public void AttachCurrentlyHoldingToHand()
    {
        VillagerController controller = m_currentlyHolding.GetComponent<VillagerController>();
        if (controller.IsPickedUp())
        {
            controller.transform.parent = m_attachHand.transform;
            controller.transform.up = Vector3.up;
            controller.transform.localPosition = Vector3.zero;
        }
    }


	bool HoldAndWait()
	{
		if (m_currentlyHolding)
		{
			m_inputWaitTimer -= Time.deltaTime;
            Debug.Log("Is waiting...("+m_inputWaitTimer+")");
			if (m_inputWaitTimer < 0.0f)
			{
				DecideNextActionFrom(MonsterAction.HoldWaitForInput);
				return true;
			}
		}
		else
		{
			return true;
		}
		return false;
	}

	bool WalkAction()
	{
		Marker currentMarker = Marker.s_current;
		bool shouldDequeue = false;
		if (currentMarker)
		{
			if (currentMarker.IsActive())
			{
				Vector3 target = Vector3.zero;
				if (currentMarker.HasPosition())
				{
					target = currentMarker.GetPosition();
					m_mover.SetTarget(currentMarker.GetPosition());
				}
				else if (currentMarker.HasTarget())
				{
					target = currentMarker.GetTarget().position;
					m_mover.SetTarget(currentMarker.GetTarget());
				}
                m_rootMotionControllerSpd = Mathf.Clamp(Vector3.Magnitude(transform.position - target) * 0.08f,0.5f,2.3f);
				if (Vector3.SqrMagnitude(transform.position - target) < 1.0f)
				{
					m_currentlyHolding = currentMarker.GetTarget();
					currentMarker.Deactivate();
					if (m_currentlyHolding)
					{
						m_actionQueue.Enqueue(MonsterAction.Pickup);
                        m_pickupDone = false;
						Debug.Log("Arrived at " + m_currentlyHolding.name + " will try pickup");
					}
                    else
                    {
                        Debug.Log("Arrived at " + target.ToString() + " will stop");
                        m_mover.m_loiter = true;
                        m_mover.m_dir = Vector3.zero;
                    }
					shouldDequeue = true;
				}
				m_mover.m_enabled = true;
				m_mover.m_loiter = false;
			}
			else
			{
				m_mover.m_enabled = true;
				m_mover.m_loiter = true;
			}
		}
		return shouldDequeue;
	}

	public void SetNewWalkTarget()
	{
		m_actionQueue.Clear(); // will also remove input wait
		if (m_currentlyHolding != null)
		{
			// throw or set down?
			// Need instant decision
			DecideNextActionFrom(MonsterBehaviour.MonsterAction.HoldWaitForInput);
		}
		// Then walk
		m_actionQueue.Enqueue(MonsterBehaviour.MonsterAction.Walk);
	}

	void DecideNextActionFrom(MonsterAction p_currentAction)
	{
		// Find the decision weigths in tree:
		ActionTreeNode currentNode = FindNode(m_decisionTreeRoot, p_currentAction);
		if (currentNode!=null)
		{
			// Randomize decision based on weight
            float chanceSum = 1.0f;
            float rnd = Random.Range(0.0f, chanceSum);
			int result = 0;
            Debug.Log("Random is " + rnd);
			// Assume same size on child and weight list
            string actions = "Found actions: ";
			for (int i = 0; i < currentNode.m_weigths.Count; i++)
			{
                if (chanceSum > rnd)
                {
                    result = i;
                    chanceSum -= currentNode.m_weigths[i]; // weigthed random
                }
                actions += "(" + currentNode.m_children[i].m_actionType.ToString() + " " + currentNode.m_weigths[i] + ")  ";
			}
            Debug.Log(actions);
			// add decided action to queue
			Debug.Log("Decided " + currentNode.m_children[result].m_actionType.ToString());
			m_actionQueue.Enqueue(currentNode.m_children[result].m_actionType);
		}
	}

	// TODO This is recursive, maybe make it on stack instead
	ActionTreeNode FindNode(ActionTreeNode currentNode, MonsterAction p_break, int step = 0)
	{
		if (step < 20)
		{
			foreach (ActionTreeNode node in currentNode.m_children)
			{
				if (node.m_actionType == p_break)
				{
					return node;
				}
				else
				{
					return FindNode(node, p_break, step++);
				}
			}
		}
		return null;
	}

	private void SetupDecisionTree()
	{
		ActionTreeNode eatNode = new ActionTreeNode();
		eatNode.m_actionType = MonsterAction.Eat;

		ActionTreeNode throwNode = new ActionTreeNode();
		throwNode.m_actionType = MonsterAction.Throw;

		ActionTreeNode putdownNode = new ActionTreeNode();
		putdownNode.m_actionType = MonsterAction.PutDown;

		ActionTreeNode holdAndWaitNode = new ActionTreeNode();
		holdAndWaitNode.m_actionType = MonsterAction.HoldWaitForInput;
		holdAndWaitNode.m_children.Add(eatNode);
		holdAndWaitNode.m_weigths.Add(0.333f);
		holdAndWaitNode.m_children.Add(throwNode);
		holdAndWaitNode.m_weigths.Add(0.333f);
		holdAndWaitNode.m_children.Add(putdownNode);
		holdAndWaitNode.m_weigths.Add(0.333f);

        //holdAndWaitNode.AddWeigth(0.5f, 2); Make nicer for put down

		ActionTreeNode pickupNode = new ActionTreeNode();
		pickupNode.m_actionType = MonsterAction.Pickup;
		pickupNode.m_children.Add(holdAndWaitNode);
		// pickupNode.m_weigths.Add(1.0f);

		m_decisionTreeRoot.m_actionType = MonsterAction.Walk;
		m_decisionTreeRoot.m_children.Add(pickupNode);
		m_decisionTreeRoot.m_weigths.Add(1.0f);

	}

}
