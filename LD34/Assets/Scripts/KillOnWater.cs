using UnityEngine;
using System.Collections;

public class KillOnWater : MonoBehaviour {
    public static float s_waterLevel = 5.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (transform.position.y < s_waterLevel)
        {
            Destroy(gameObject);
        }
	}
}
