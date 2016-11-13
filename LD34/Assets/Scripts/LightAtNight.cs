using UnityEngine;
using System.Collections;

public class LightAtNight : MonoBehaviour 
{

    DayCycleSimulator m_daynight;
    public Light m_light;
    public ParticleSystem m_particles;
    private static bool hasUpdatedFlicker=false;
    private static float lightOrig = 1.78f;
    private float flicker = 10.0f;
    private float flickerOffset = 0.0f;
    private float flickerTarget = 10.0f;
    private float flickerSizeTarget = 10.0f;
    private float flickerSize = 10.0f;
	// Use this for initialization
	void Start () {
        m_daynight = DayCycleSimulator.instance;
        flickerOffset = Random.Range(0.0f, 1000.0f);
	}

    void FixedUpdate()
    {
        int rnd = Random.Range(0, 7);
        if (rnd == 5)
            flickerTarget = Random.Range(1.0f, 10.0f);
        if (rnd == 2)
            flickerSizeTarget = Random.Range(3.0f, 10.0f);
        flicker = Mathf.Lerp(flicker, flickerTarget, 5.0f*Time.deltaTime);
        flickerSize = Mathf.Lerp(flickerSize, flickerSizeTarget, 5.0f*Time.deltaTime);
    }
	
	// Update is called once per frame
	void Update () {
	    if (m_daynight)
        {
            float day24HFrac = m_daynight.Get24HFrac();
            if (day24HFrac > 0.25f && day24HFrac < 0.75f)
            {
                // DAY
                m_light.enabled = false;
                m_particles.enableEmission = false;
            }
            else
            {
                float flickerIntensity = /*(1.0f + Mathf.Sin(flickerOffset + Time.time * */flicker/*)) * 0.5f*/;
                m_light.intensity = 0.5f + flickerIntensity/* * lightOrig*/;
                m_light.range = flickerSize;
                // NIGHT
                m_light.enabled = true;
                m_particles.enableEmission = true;
            }
        }
	}

    void LateUpdate()
    {
        hasUpdatedFlicker = false;
        flicker = lightOrig;
    }
}
