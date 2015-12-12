using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    transform.Rotate(Vector3.back, 10 + Time.deltaTime);
	}

    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.CompareTag("Monster"))
        {
            OnPickup();    
        }
    }

    void OnPickup()
    {
        Destroy(this.gameObject, 0.3f);
    }
}
