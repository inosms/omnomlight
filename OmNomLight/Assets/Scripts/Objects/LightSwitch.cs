using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource), typeof(TriggerField))]
public class LightSwitch : MonoBehaviour 
{
    public LightSource[] lightSources;
    public bool isOn;

    private TriggerField triggerField;
    private AudioSource audiosource;
    void Start()
    {
        triggerField = GetComponent<TriggerField>();
        audiosource = GetComponent<AudioSource>();
    } 

	void Update () 
    {
        updateLights();

        if((triggerField.humanIsHere && Input.GetButtonDown("Pick Up"))
            || (triggerField.monsterIsHere && Input.GetButtonDown("Pick Up Controller")))
        {
            toggle();
        }
	}

    public void toggle()
    {
        isOn = !isOn;
        audiosource.Play();
    }

    private void updateLights()
    {
        foreach (LightSource l in lightSources)
        {
            l.SetIsOn(isOn);
        }
    }
}
