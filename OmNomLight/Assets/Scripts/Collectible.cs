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
		GameObject.FindObjectOfType<GameState>().MonsterEatsOneFood();
		this.gameObject.SetActive(false);
        Destroy(this.gameObject, 0.3f);
    }
}
