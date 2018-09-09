using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {


	// Use this for initialization
    public AudioClip m_ballDeflect;
    public AudioClip m_ballLost;
    public AudioClip m_ballLaunch;
    public AudioClip m_brickExplode;
    public AudioClip m_powerupCatch;

    AudioSource m_audioSource;
    Dictionary<string,AudioClip> m_soundMap;

	void Start () 
    {
        m_audioSource = GetComponent<AudioSource>();
        m_soundMap = new Dictionary<string,AudioClip>()
        {
            {"ball_deflect", m_ballDeflect },
            {"ball_lost", m_ballLost },
            {"ball_launch", m_ballLaunch},
            {"brick_explode", m_brickExplode },
            {"powerup_catch", m_powerupCatch}
        };
	}
    public void PlaySound(string sound)
    {
        AudioClip clip = m_soundMap[sound];
        m_audioSource.PlayOneShot(clip, 1.0f);
    }
		
}
