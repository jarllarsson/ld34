using UnityEngine;
using System.Collections;

public class DayCycleSimulator : MonoBehaviour 
{
    public static System.DateTime s_dateTime = System.DateTime.Now;
    public float m_second = 60.0f; // 60s = 1 minute per second etc
    public bool m_realtime = false;
    public Light m_sunLight, m_moonLight;
    public float m_sunLightMaxPow = 1.0f, m_moonLightMaxPow = 2.3f;

    public Color[] m_dayFogColor = new Color[2];
    public Color m_currentFogColor;

    public Color[] m_dayLightColor = new Color[2];
    public Color m_currentLightColor;

    public Transform m_sun;
    public Renderer m_nightStarsOverlay;

	// Use this for initialization
	void Start () 
    {
        if (!m_realtime) ResetTime();
        m_nightStarsOverlay.material.SetColor("_TintColor", new Color(1.0f, 1.0f, 1.0f, 0.0f));
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_realtime)
            s_dateTime = s_dateTime.AddSeconds((double)m_second*(double)Time.deltaTime);
        else
            s_dateTime = System.DateTime.Now;

        Debug.Log(s_dateTime.ToLongTimeString());


        float day24HFrac = Get24HFrac();
        m_sun.rotation = Quaternion.Euler(new Vector3(day24HFrac * 360.0f, 0.0f, 0.0f));

        if (day24HFrac > 0.25f && day24HFrac < 0.75f) //day
        {
            float t = day24HFrac - 0.25f;
            t *= 2.0f;
            m_sunLight.intensity = m_sunLightMaxPow * Mathf.Lerp(0.0f,1.0f,Mathf.Sin(Mathf.PI*t));
        }
        else //night
        {
            float t = day24HFrac - 0.75f;
            if (day24HFrac <= 0.25f) t = day24HFrac + 0.25f;
            t *= 2.0f;
            t = Mathf.Sin(Mathf.PI * t);
            m_nightStarsOverlay.material.SetColor("_TintColor", new Color(1.0f, 1.0f, 1.0f, t));
            m_moonLight.intensity = m_moonLightMaxPow * Mathf.Lerp(0.0f, 1.0f, t);
        }

        UpdateSkyColor();
	}

    void UpdateSkyColor()
    {
        float day24HFrac = Get24HFrac();
        int fogColors = m_dayFogColor.Length;
        if (fogColors > 1 && fogColors == m_dayLightColor.Length)
        {
            float scale = day24HFrac * (float)fogColors;
            int low = Mathf.FloorToInt(scale);
            int high = Mathf.CeilToInt(scale) % fogColors;
            float step = scale - (float)low;
            m_currentFogColor = Color.Lerp(m_dayFogColor[low], m_dayFogColor[high], step);
            m_currentLightColor = Color.Lerp(m_dayLightColor[low], m_dayLightColor[high], step);

            RenderSettings.fogColor = m_currentFogColor;
            RenderSettings.fogDensity = m_currentFogColor.a * 0.006f;
            m_sunLight.color = m_currentLightColor;
            if (RenderSettings.skybox)
            {
                RenderSettings.skybox.SetColor("_GroundColor", m_currentFogColor);
            }

        }
    }

    void ResetTime()
    {
        s_dateTime = new System.DateTime(1, 5, 1, 12, 0, 0);
    }

    void OnDestroy()
    {
        ResetTime();
        UpdateSkyColor();
    }

    public float Get24HFrac()
    {
        return ((float)s_dateTime.Hour + ((float)s_dateTime.Minute + ((float)s_dateTime.Second + (float)s_dateTime.Millisecond / 1000.0f) / 60.0f) / 60.0f) / 24.0f;
    }
}
