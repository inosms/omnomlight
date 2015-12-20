using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.CompareTag("Monster"))
        {
            OnPickup();


        }
    }

    void OnPickup()
    {
		
    }
}
