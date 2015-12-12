using UnityEngine;
using System.Collections;

public class MouseClicker : MonoBehaviour 
{
    public CamMover m_camMover;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Marker currentMarker = Marker.s_current;
        if (currentMarker)
	    {
            if (!m_camMover.isRightMouseButtonDragging && Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayHit;
                if (Physics.Raycast(ray, out rayHit))
                {
                    switch(rayHit.transform.tag)
                    {
                        case "Terrain":
                        {
                            print("Hit terrain at " + rayHit.point);
                            currentMarker.Activate(rayHit.point);
                        }
                        break;
                        case "Water":
                        {
                            print("Hit water");
                            currentMarker.Deactivate();
                        }
                        break;
                        case "Monster":
                        {
                            print("Hit monster");
                            currentMarker.Deactivate();
                        }
                        break;
                        default:
                        {
                            print("Hit " + rayHit.transform.name);
                            currentMarker.Activate(rayHit.transform);
                        }
                        break;
                    }
                }
            }
        }
	}
}
