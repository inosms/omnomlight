﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class Win : MonoBehaviour {

	AudioSource musicPlayer;
	public AudioClip clip;

	void Awake()
	{
		musicPlayer = gameObject.GetComponent<AudioSource>();

		musicPlayer.clip = clip;
		musicPlayer.loop = false;
		musicPlayer.Play();
	}

	void Update () 
	{
		if( Input.anyKeyDown )
			Application.LoadLevel("MainMenu");
	}
}
