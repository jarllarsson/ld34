using UnityEngine;
using System.Collections;

public class DayNightMusicPlayer : MonoBehaviour {


    public DayCycleSimulator m_music;

    public AudioClip[] m_dayMusic = new AudioClip[2];
    public AudioClip[] m_nightMusic = new AudioClip[2];
    public AudioSource m_dayPlayer, m_nightPlayer;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMusic();
	}

    void UpdateMusic()
    {
        float day24HFrac = m_music.Get24HFrac();
        if (day24HFrac > 0.25f && day24HFrac < 0.75f)
        {
            // DAY
            //Debug.Log("day");
            HandleMusic(m_dayPlayer, m_nightPlayer, m_dayMusic);
        }
        else
        {
            // NIGHT
            //Debug.Log("night");
            HandleMusic(m_nightPlayer, m_dayPlayer, m_nightMusic);
        }
    }

    void HandleMusic(AudioSource current, AudioSource old, AudioClip[] songs)
    {
        if (old.isPlaying) // fade way old music
        {
            old.volume -= Time.deltaTime;
            if (old.volume <= 0.0f)
            {
                old.volume = 0.0f;
                old.Stop();
            }
        }
        if (!current.isPlaying)
        {
            current.volume = 1.0f;
            current.clip = songs[Random.Range(0, songs.Length)];
            current.Play();
        }
    }
}
