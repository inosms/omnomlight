using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicWithIntro : MonoBehaviour 
{
    private AudioClip intro;
    private AudioClip loop;
    private AudioSource musicPlayer;

	public AudioClip humanIntro;
	public AudioClip humanLoop;
	public AudioClip monsterIntro;
	public AudioClip monsterLoop;

    void Start()
    {
		if( Random.Range(0,2) == 0 )
		{
			intro = humanIntro;
			loop = humanLoop;
		}
		else
		{
			intro = monsterIntro;
			loop = monsterLoop;
		}
	

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
