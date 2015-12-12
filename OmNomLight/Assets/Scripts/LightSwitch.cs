using UnityEngine;
using System.Collections;

public class LightSwitch : MonoBehaviour {
    public LightSource[] lightSources;
    public bool isOn;

	// Use this for initialization
	void Start () {
        updateLights();
	}

    public void toggle()
    {
        isOn = !isOn;
        updateLights();
    }

    private void updateLights()
    {
        foreach (LightSource l in lightSources)
        {
            l.SetIsOn(isOn);
        }
    }
}
