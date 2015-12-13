using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicWithIntro : MonoBehaviour 
{
    public AudioClip intro;
    public AudioClip loop;

    private AudioSource musicPlayer;

    void Start()
    {
        musicPlayer = gameObject.GetComponent<AudioSource>();

        musicPlayer.clip = intro;
        musicPlayer.loop = false;
        musicPlayer.Play();
    }

	// Update is called once per frame
	void Update () 
    {
        if (!musicPlayer.isPlaying)
        {
            musicPlayer.clip = loop;
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
	
	}
}
